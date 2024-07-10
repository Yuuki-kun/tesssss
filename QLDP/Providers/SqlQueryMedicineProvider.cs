using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Providers
{
    public class SqlQueryMedicineProvider : SqlQueryProvider<Medicine>
    {

        public override OperationResult SaveData(Medicine data)
        {
            string query = $"insert into Medicine (Name, Description, Price, PrimaryImageId, PopularityMedicine, CategoryId) values ('{data.Name}', '{data.Description}', {data.Price},{data.PrimaryImageId}, '{data.PopularityMedicine}', null)";
            Console.WriteLine("insert query = " + query);
            int result;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    result = cmd.ExecuteNonQuery();

                }
            }
            string mess = result > 0 ? "Success" : "Failed";
            return new OperationResult() { RowsAffected = result, ErrorMessage = mess };

        }
    }
}
