using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace QLDP.Providers
{
    public class SPImageProvider : SqlStoredProcedureProvider<Image>
    {
        public SPImageProvider() : base() { }

        public override OperationResult Delete(int id)
        {
            var result = new OperationResult();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteImage", con))
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

        public override Image FindById(int id)
        {
            Image? image = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("FindImageById", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                image = new Image
                                {
                                    Id = (int)reader["Id"],
                                    Name = (string)reader["Name"],
                                    URL = (string)reader["URL"],
                                    MedicineId = reader["MedicineId"] == DBNull.Value ? 0 : (int)reader["MedicineId"],
                                };
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

            return image;
        }

        public override List<Image> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public override List<Image> FindByPage(int page, int size, int d)
        {
            throw new NotImplementedException();
        }

        public override List<Image> LoadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("LoadMedicineImage", con))
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public override OperationResult SaveData(Image data)
        {
            var result = new OperationResult();


            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddImage", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@name", SqlDbType.VarChar, 1000).Value = data.Name;
                        cmd.Parameters.Add("@url", SqlDbType.VarChar, 1000).Value = data.URL;
                        cmd.Parameters.Add("@medicineId", SqlDbType.Int).Value = data.MedicineId;

                        //CHECK STATUS
                        SqlParameter errorMsgParam = new
                             SqlParameter("@message",
                                          SqlDbType.NVarChar,
                                          4000)
                        {
                            Direction = ParameterDirection.Output
                        };

                        SqlParameter imageIdOutParam = new
                            SqlParameter("@imageId",
                                         SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(errorMsgParam);
                        cmd.Parameters.Add(imageIdOutParam);

                        con.Open();

                        cmd.ExecuteNonQuery();

                        int imageId = IntConverter(imageIdOutParam.Value);

                        string message = errorMsgParam.Value.ToString() ?? "No Message";

                        result.Success = imageId > 0;
                        result.ReturnId = imageId;
                    }
                }
            }
            catch (SqlException ex)
            {
                result.Success = false;
                result.ErrorMessage = "Database error: " + ex.Message;

            }
            catch (ArgumentException ex)
            {
                result.Success = false;
                result.ErrorMessage = "Input error: " + ex.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Unknown error: " + ex.Message;
            }

            return result;

        }

        public override OperationResult UpdateEntity(Image entity)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateImage", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", entity.Id);
                    cmd.Parameters.AddWithValue("@name", entity.Name);
                    cmd.Parameters.AddWithValue("@url", entity.URL);
                    cmd.Parameters.AddWithValue("@medicineID", entity.MedicineId);

                    int returnCode = cmd.ExecuteNonQuery();
                    string message = returnCode <= 0 ? "Update image failed" : "Update image successfully";
                    return new OperationResult { RowsAffected = returnCode, ErrorMessage = message };
                }
            }
        }

        protected override List<Image> CreateListObjectFromRows(SqlDataReader reader)
        {
            List<Image> list = new List<Image>();

            while (reader.Read())
            {
                Image obj = new Image { Id = (int)reader["Id"], Name = (string)reader["Name"], URL = (string)reader["URL"], MedicineId = (int)reader["MedicineId"] };
                list.Add(obj);
            }
            return list;
        }
    }
}
