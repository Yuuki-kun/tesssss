using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using QLDP.Models;
using System.Data;
using System.Reflection;
using System.Configuration;
namespace QLDP.Providers
{
    public abstract class SqlQueryProvider<T> : IDataProvider<T> where T : class, new()
    {
        public static string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];

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
            List<T> list = new List<T>();
            string query = $"select * from {typeof(T).Name} where {column}={value.ToString()}";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        list = CreateListObjectFromRows(reader);
                    }
                }
            }
            return list;
        }

        public T FindById(int id)
        {
            string tableName = typeof(T).Name;
            string findQuery = $"select * from {tableName} where Id={id}";
            T obj = new T();
            Console.WriteLine(findQuery);
            Thread.Sleep(200);
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(findQuery, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var properties = typeof(T).GetProperties();

                        while (reader.Read())
                        {
                            foreach (var prop in properties)
                            {
                                if (!reader.HasColumn(prop.Name) || reader[prop.Name] == DBNull.Value)
                                    continue;

                                prop.SetValue(obj, reader[prop.Name]);
                            }

                        }
                    }

                }
            }
            return obj;

        }

        public List<T> FindByName(string name)
        {
            string tableName = typeof(T).Name;
            string findQuery = $"select * from {tableName} where Name like '%{name}%'";
            List<T> objs = new List<T>();
            Console.WriteLine(findQuery);
            Thread.Sleep(200);
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(findQuery, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        objs = CreateListObjectFromRows(reader);
                    }

                }
            }
            return objs;
        }

        public List<T> LoadData()
        {
            List<T> objs = new List<T>();
            Console.WriteLine("Loading data = " + typeof(T).Name);
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"select * from {typeof(T).Name}", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        objs = CreateListObjectFromRows(reader);
                    }
                }
            }
            return objs;

            /*            return list;
            */
        }

        private List<T> CreateListObjectFromRows(SqlDataReader reader)
        {
            List<T> list = new List<T>();
            var properties = typeof(T).GetProperties();

            while (reader.Read())
            {
                T obj = new T();

                foreach (var prop in properties)
                {
                    if (!reader.HasColumn(prop.Name) || reader[prop.Name] == DBNull.Value)
                        continue;

                    prop.SetValue(obj, reader[prop.Name]);
                }

                list.Add(obj);
            }
            return list;
        }

        public void SaveAll(List<T> data)
        {
            throw new NotImplementedException();
        }

        /*        public SqlActionStatus SaveData(T data)
                {
                    var properties = typeof(T).GetProperties();

                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    foreach (var prop in properties)
                    {

                        PropertyInfo propertyInfo = data.GetType().GetProperty(prop.Name);
                        object itemv = propertyInfo.GetValue(data, null);
                        //if list => ignore
                        if ( itemv!=null )
                        {
                            if(!itemv.GetType().IsGenericType && (prop.Name != "Id"))
                                keyValues.Add(prop.Name, itemv.ToString());
                        }
                        else
                        {
                            keyValues.Add(prop.Name, "NULL");
                        }


                    }

                    string columns = "";
                    string values = "";

                    for(int i=0; i<keyValues.Count; i++)
                    {
                        var item = keyValues.ElementAt(i);
                        columns += item.Key + (i < keyValues.Count - 1 ? ", " : "");
                        values += (item.Value=="NULL" ? $"{item.Value}":$"'{item.Value}'") + (i < keyValues.Count - 1 ? ", " : "");
                    }

                    string insertQuery = $"insert into {typeof(T).Name} ({columns}) values ({values})";
                    Console.WriteLine($"Execute {insertQuery}");
                    Thread.Sleep(200);
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return new SqlActionStatus { Id=1, Message="Success"};
                }*/

        public abstract OperationResult SaveData(T data);
        public OperationResult UpdateColumn(int id, string column, string newValue)
        {
            string tableName = typeof(T).Name;
            string updateQuery = $"update {tableName} set {column} = '{newValue}' where Id={id}";
            Console.WriteLine(updateQuery);
            Thread.Sleep(200);
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    con.Open();
                    int rowAffects = cmd.ExecuteNonQuery();
                    Console.WriteLine($"row affects = {rowAffects}");
                    Console.ReadKey();
                }
            }
            return new OperationResult { RowsAffected = id, ErrorMessage = "Success" };
        }

        public List<T> FindByPage(int p, int s, int d)
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}