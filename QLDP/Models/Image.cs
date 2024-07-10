using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Models
{
    public class Image : IEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string URL { get; set; }
        public int MedicineId { get; set; }

        public Image() { }

        //using for list.Add((T)Activator.CreateInstance(typeof(T), [reader[reader.GetName(0)], reader[reader.GetName(1)], reader[reader.GetName(2)], reader[reader.GetName(3)]]));

        public Image(int id, string name, string url, int medicineId)
        {
            Id = id;
            Name = name;
            URL = url;
            MedicineId = medicineId;    
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
            return $"ID: {Id}, Name: {Name}, URL: {URL})";

        }
    }
}
