using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Models
{
    public class MedicineCategory : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> MedicineIds { get; set; } = new List<int>();
        public List<Medicine>? Medicines { get; set; }
        public MedicineCategory() { }

     

        public void Display()
        {
            Console.WriteLine(this);
        }

        public int GetId()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public override string ToString()
        {
            string medicines = "";
            for(int i = 0; i < MedicineIds.Count;i++)
            {
                medicines += i + (i != MedicineIds.Count - 1 ? ", " : "");
            }
            return $"ID: {Id}, Name: {Name}, Medicinces [{medicines}]";
        }

    }
}
