using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Models
{
    public class Medicine : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Image> Images { get; set; } = [];
        public MedicineCategory? MedicineCategory { get; set; }
        public int PrimaryImageId { get; set; } = 0;
        public int? CategoryId { get; set; } = null;

        public Medicine() { }
        public bool? PopularityMedicine { get; set; } = false;

        public void display()
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
            string images = "";
            for (int i = 0; i < Images.Count; i++)
            {
                images += Images[i].URL + (Images[i].Id == PrimaryImageId ? " (primary)" : "") + (i != Images.Count - 1 ? ", " : "");
                          
            }
            return $"ID: {Id}, Name: {Name}, Descriptions: {Description}, Price: {Price}, Images [{images}]";

        }

    }
}
