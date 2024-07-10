using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Providers
{
    public class SqlQueryCategoryProvider : SqlQueryProvider<MedicineCategory>
    {

        public override OperationResult SaveData(MedicineCategory data)
        {
            string query = $"insert into MedicineCategory (Name) values ('{data.Name}')";
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
