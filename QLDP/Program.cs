using QLDP.Models;
using System.Configuration;


namespace QLDP
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string conf = ConfigurationManager.AppSettings["Environment"];
            Console.WriteLine(conf);
            EnvironmentType e;
            if (conf.Equals("DATABASE"))
            {
                e = EnvironmentType.DATABASE;

            }
            else
            {
                e = EnvironmentType.JSON;
            }

            //string conn = @"Data Source = TNGCNGMINHA7C2; Initial Catalog = TestDatabase; Integrated Security = True";

            ConsoleHelper consoleHelper = new ConsoleHelper(e);

            string option = "D0";

            do
            {
                switch (option)
                {
                    case "D0":
                        consoleHelper.ShowMenu();
                        break;
                    case "D1":
                        consoleHelper.AddMedicineCategory();
                        option = "D0";
                        continue;
                    case "D2":
                        consoleHelper.AddMedicine();
                        break;
                    case "D3":
                        consoleHelper.AddMedicineToCategory();
                        break;
                    case "D4":
                        consoleHelper.FindMedicineByID();
                        break;
                    case "D5":
                        consoleHelper.FindMedicineByName();
                        break;
                    case "D6":
                        consoleHelper.ShowMedicines();
                        break;
                    case "D7":
                        consoleHelper.ShowMedicineByCategory();
                        break;
                    default:
                        consoleHelper.ShowMenu();
                        break;
                }
                if (option != "D0") Console.WriteLine("\t0. Show menu");
                Console.WriteLine("\tEsc. Exit");

                Console.Write("/> ");

                ConsoleKeyInfo ck = Console.ReadKey(true);
                option = Convert.ToString(ck.Key) ?? "D0";

                Console.WriteLine(option);
            } while (option != "Escape");

        }

    }
}
