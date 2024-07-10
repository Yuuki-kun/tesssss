using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Models
{
    internal readonly struct DataFilePath
    {
        static DataFilePath() {
            string dataFolder = "..\\..\\..\\Data";
            MedicinePath = Path.Combine(dataFolder, "Medicine.json");
            MedicineCategoriesPath = Path.Combine(dataFolder, "MedicineCate.json");
            ImagePath = Path.Combine(dataFolder, "MedicineImage.json");
        }
        public static string MedicinePath { get;  }
        public static string MedicineCategoriesPath { get;}
        public static string ImagePath { get;}

    }
}
