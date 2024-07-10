using QLDP.Models;
using System.Data.SqlClient;
using System.Data;

namespace QLDP.Providers
{
    public class SPCategoryProvider : SqlStoredProcedureProvider<MedicineCategory>
    {

        public SPCategoryProvider() : base()
        { }

        public override OperationResult Delete(int id)
        {
            var result = new OperationResult();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteCategory", con))
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

        public override MedicineCategory FindById(int id)
        {
            MedicineCategory? m = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("FindCategoryById", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                m = new MedicineCategory
                                {
                                    Id = (int)reader["Id"],
                                    Name = (string)reader["Name"],
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

            return m;
        }

        public override List<MedicineCategory> FindByName(string name)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SearchCategoryByName", con))
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

        public override List<MedicineCategory> FindByPage(int page, int size, int direction)
        {
            throw new NotImplementedException();
        }

        public override List<MedicineCategory> LoadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("LoadMedicineCategory", con))
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

        public override OperationResult SaveData(MedicineCategory data)
        {
            var result = new OperationResult();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddMedicineCategory", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@name", data.Name);

                        SqlParameter errorMsgParam = new
                             SqlParameter("@message",
                                          SqlDbType.NVarChar,
                                          4000)
                        {
                            Direction = ParameterDirection.Output
                        };

                        SqlParameter categoryIdOutParam = new
                            SqlParameter("@categoryId",
                                         SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(errorMsgParam);
                        cmd.Parameters.Add(categoryIdOutParam);

                        con.Open();

                        cmd.ExecuteNonQuery();

                        int returnId = IntConverter(categoryIdOutParam.Value);

                        result.Success = returnId > 0;
                        result.ReturnId = returnId;

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

        public override OperationResult UpdateEntity(MedicineCategory entity)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", entity.Id);
                    cmd.Parameters.AddWithValue("@value", entity.Name);
;

                    int rowsAffacted = cmd.ExecuteNonQuery();
                    string message = rowsAffacted <= 0 ? "Update category failed" : "Update category successfully";
                    return new OperationResult { Success = rowsAffacted > 0, ErrorMessage = message };
                }
            }
        }


        protected override List<MedicineCategory> CreateListObjectFromRows(SqlDataReader reader)
        {
            List<MedicineCategory> list = new List<MedicineCategory>();

            while (reader.Read())
            {
                MedicineCategory obj = new MedicineCategory { Id = (int)reader["Id"], Name = (string)reader["Name"] };
                list.Add(obj);
            }
            return list;
        }
    }
}
