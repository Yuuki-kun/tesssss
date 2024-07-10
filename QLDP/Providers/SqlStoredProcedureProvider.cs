using Newtonsoft.Json.Linq;
using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Providers
{
    public abstract class SqlStoredProcedureProvider<T> : IDataProvider<T> where T : class, new()
    {

        public static string _connectionString;

        public SqlStoredProcedureProvider()
        {
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            /*            _connectionString = ConfigurationManager.AppSettings["ConnectionString2"];
            */
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string is not provided.");
            }

        }

        public static void InitialDatabase()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand($"if object_id('MedicineCategory','U') is null create table MedicineCategory (Id int not null primary key identity(1,1), Name nvarchar(30))", connection);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"if object_id('Medicine','U') is null create table Medicine (Id int not null primary key identity(1,1), Name nvarchar(30), Description nvarchar(255), Price decimal(19,4), PrimaryImageId int, PopularityMedicine bit, CategoryId int foreign key references MedicineCategory(Id))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"if object_id('Image','U') is null create table Image (Id int not null primary key identity(1,1), Name nvarchar(30), URL nvarchar(255), MedicineId int foreign key references Medicine(Id))";

                cmd.ExecuteNonQuery();

            }
        }
        public List<T> FindAllByColumn(string column, object value)
        {

            string tableName = typeof(T).Name;
            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("LoadDataByColumn", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TableName", SqlDbType.VarChar, 50).Value = tableName;
                    cmd.Parameters.Add("@ColumnName", SqlDbType.VarChar, 50).Value = column;
                    cmd.Parameters.Add("@value", SqlDbType.VarChar, 50).Value = value;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        return CreateListObjectFromRows(reader);
                    }

                }
            }
        }
        public abstract T FindById(int id);
        public abstract List<T> FindByName(string name);
        public abstract List<T> LoadData();
        public void SaveAll(List<T> data)
        {
            foreach (var t in data)
            {
                SaveData(t);
            }
        }
        public abstract OperationResult SaveData(T data);

        public OperationResult UpdateColumn(int id, string column, string newValue)
        {
            string tableName = typeof(T).Name;
            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateColumn", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TableName", SqlDbType.VarChar, 50).Value = tableName;
                    cmd.Parameters.Add("@ColumnName", SqlDbType.VarChar, 50).Value = column;
                    cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                    cmd.Parameters.Add("@value", SqlDbType.VarChar, 50).Value = newValue;


                    //CHECK STATUS
                    SqlParameter errorMsgParam = new
                         SqlParameter("@message",
                                      SqlDbType.NVarChar,
                                      4000)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(errorMsgParam);

                    conn.Open();

                    int rowsAffacted = cmd.ExecuteNonQuery();


                    return new OperationResult { RowsAffected = rowsAffacted, ErrorMessage = errorMsgParam.Value.ToString() ?? "No message" };
                }
            }
        }
        protected abstract List<T> CreateListObjectFromRows(SqlDataReader reader);
        protected int IntConverter(object v)
        {
            int outp = 0;

            if (v is int)
            {
                outp = (int)v;
            }
            else
            {
                if (v is string)
                {
                    if (int.TryParse(v.ToString(), out outp))
                    {
                        return outp;
                    }
                }
            }

            return outp;
        }

        public abstract List<T> FindByPage(int p, int s, int d);

        public abstract OperationResult UpdateEntity(T entity);

        public abstract OperationResult Delete(int id);

    }


}
