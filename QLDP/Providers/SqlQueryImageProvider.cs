using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Providers
{
    public class SqlQueryImageProvider : SqlQueryProvider<Image>
    {

        public override OperationResult SaveData(Image data)
        {
            string query = $"insert into Image (Name, URL, MedicineId) values ('{data.Name}', '{data.URL}', {data.MedicineId})";
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
