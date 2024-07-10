using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DC8Training.QLSV.Helpers
{
    internal class MyHelper
    {
        private static readonly Lazy<Dictionary<string, List<string>>> _names = new Lazy<Dictionary<string, List<string>>>(() =>
        {
            var fileContent = File.ReadAllText("./Data/names.json");
            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(fileContent);
        });

        public static string GenerateName(bool isMan = true)
        {
            var random = new Random();
            var boys = _names.Value["boys"];
            var girls = _names.Value["girls"];
            var lasts = _names.Value["last"];

            if (isMan)
                return boys[random.Next(0, boys.Count)] +" "+ lasts[random.Next(0, lasts.Count)];
            else
                return girls[random.Next(0, girls.Count)] + " " + lasts[random.Next(0, lasts.Count)];
        }
    }
}