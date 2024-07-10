using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLDP.Providers
{
    public class SPMedicineProvider : SqlStoredProcedureProvider<Medicine>
    {

        public SPMedicineProvider() : base() { }

        public override OperationResult Delete(int id)
        {
            var result = new OperationResult();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteMedicine", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);

                        con.Open();

                        result.Success = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false; result.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                throw;
            }
            return result;
        }

        public override Medicine FindById(int id)
        {
            Medicine? m = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("FindMedicineById", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                m = new Medicine
                                {
                                    Id = (int)reader["Id"],
                                    Name = (string)reader["Name"],
                                    Description = (string)reader["Description"],
                                    Price = (decimal)reader["Price"],
                                    PrimaryImageId = (int)reader["PrimaryImageId"],
                                    PopularityMedicine = (bool)reader["PopularityMedicine"],
                                    CategoryId = reader["CategoryId"] == DBNull.Value ? null : (int)reader["CategoryId"],
                                };
                                if (m.CategoryId != null && m.CategoryId > 0)
                                {
                                    m.MedicineCategory = new MedicineCategory { Id = m.CategoryId ?? -1, Name = (string)reader["CategoryName"] };

                                }
                            }
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return m;
        }

        public override List<Medicine> FindByName(string name)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SearchMedicineByName", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchValue", name);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            return CreateListObjectFromRows(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public override List<Medicine> FindByPage(int page, int size, int direction)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("LoadMedicineByPage", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@page", page);
                        cmd.Parameters.AddWithValue("@size", size);
                        cmd.Parameters.AddWithValue("@direction", direction);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            return CreateListObjectFromRows(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine(ex.Message); throw;
                }
            }

        }

        public override List<Medicine> LoadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("LoadMedicine", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            return CreateListObjectFromRows(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); throw;
            }
        }

        public override OperationResult SaveData(Medicine data)
        {
            var result = new OperationResult();

            try
            {

                using (SqlConnection con = new SqlConnection(_connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("AddMedicine", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@name", data.Name);
                        cmd.Parameters.AddWithValue("@description", data.Description);
                        cmd.Parameters.AddWithValue("@price", data.Price);
                        cmd.Parameters.AddWithValue("@primaryImageId", data.PrimaryImageId);
                        cmd.Parameters.AddWithValue("@popularityMedicine", data.PopularityMedicine);
                        cmd.Parameters.AddWithValue("@categoryId", data.CategoryId > 0 ? data.CategoryId : null);

                        //CHECK STATUS
                        SqlParameter errorMsgParam = new
                             SqlParameter("@message",
                                          SqlDbType.NVarChar,
                                          4000)
                        {
                            Direction = ParameterDirection.Output
                        };

                        SqlParameter medicineIdOutParam = new
                            SqlParameter("@medicineId",
                                         SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(medicineIdOutParam);
                        cmd.Parameters.Add(errorMsgParam);

                        con.Open();

                        cmd.ExecuteNonQuery();

                        string message = errorMsgParam.Value.ToString() ?? "No message";

                        int returnId = IntConverter(medicineIdOutParam.Value);

                        result.Success = returnId > 0;
                        result.ReturnId = returnId;
                    }
                }
            }
            catch (SqlException ex)
            {
                result.Success = false;
                result.ErrorMessage = "Database error: " + ex.Message;
                throw;

            }
            catch (ArgumentException ex)
            {
                result.Success = false;
                result.ErrorMessage = "Input error: " + ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Unknown error: " + ex.Message;
                throw;
            }
            return result;
        }

        public override OperationResult UpdateEntity(Medicine entity)
        {

            var result = new OperationResult();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateMedicine", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", entity.Id);
                        cmd.Parameters.AddWithValue("@name", entity.Name);
                        cmd.Parameters.AddWithValue("@description", entity.Description);
                        cmd.Parameters.AddWithValue("@price", entity.Price);
                        cmd.Parameters.AddWithValue("@primaryImageId", entity.PrimaryImageId);
                        cmd.Parameters.AddWithValue("@popularityMedicine", entity.PopularityMedicine);
                        cmd.Parameters.AddWithValue("@categoryId", entity.CategoryId);

                        con.Open();

                        result.RowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine("Row affedcted = " + result.RowsAffected);
                        result.Success = result.RowsAffected > 0;

                    }
                }
            }
            catch (SqlException ex)
            {
                result.Success = false;
                result.ErrorMessage = "Database error: " + ex.Message;
                Console.WriteLine(result.ErrorMessage);
                throw;

            }
            catch (ArgumentException ex)
            {
                result.Success = false;
                result.ErrorMessage = "Input error: " + ex.Message;
                Console.WriteLine(result.ErrorMessage);

                throw;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Unknown error: " + ex.Message;
                Console.WriteLine(result.ErrorMessage);

                throw;

            }

            return result;

        }

        protected override List<Medicine> CreateListObjectFromRows(SqlDataReader reader)
        {
            List<Medicine> list = new List<Medicine>();

            while (reader.Read())
            {
                Medicine obj = new Medicine
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Price = (decimal)reader["Price"],
                    PrimaryImageId = (int)reader["PrimaryImageId"],
                    PopularityMedicine = (bool)reader["PopularityMedicine"],
                    CategoryId = reader["CategoryId"].Equals(DBNull.Value) ? -1 : (int)reader["CategoryId"],
                };
                list.Add(obj);
            }
            return list;
        }
    }
}
