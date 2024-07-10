
namespace QLDP.Models
{
    public class OperationResult
    {
       
        public bool Success { get; set; }
        public int RowsAffected { get; set; }
        public string ErrorMessage { get; set; }
        public int ReturnId { get; set; }
    }
}
