using System;


namespace QLDP.Exceptions
{
    public class MedicineNotFoundException : Exception
    {
        public MedicineNotFoundException() { }
        public MedicineNotFoundException(string message) : base(message) { }
        public MedicineNotFoundException(string message,
                                         Exception innerException) : base(message, innerException) { }
    }
}
