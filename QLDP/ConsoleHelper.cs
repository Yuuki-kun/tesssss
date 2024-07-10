using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Globalization;
using QLDP.Exceptions;
using QLDP.Repositories;
using System.Data.SqlClient;
using QLDP.Providers;

namespace QLDP.Models
{
    internal class ConsoleHelper
    {
        public static int CurrentMedicineId { get; set; } = 0;
        public static int CurrentCategoryId { get; set; } = 0;
        public static int CurrentImageId { get; set; } = 0;

        private readonly MedicineRepository _medicineRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CategoryRepository _categoryRepository;

        private readonly EnvironmentType _environment;
        public ConsoleHelper(EnvironmentType environmentType)
        {

            _environment = environmentType;
            if (_environment.Equals(EnvironmentType.JSON))
            {
                
                _medicineRepository = new MedicineRepository(new JsonDataProvider<Medicine>(DataFilePath.MedicinePath),
                                                             new JsonDataProvider<Image>(DataFilePath.ImagePath),
                                                             new JsonDataProvider<MedicineCategory>(DataFilePath.MedicineCategoriesPath));

                _categoryRepository = new CategoryRepository(new JsonDataProvider<MedicineCategory>(DataFilePath.MedicineCategoriesPath),
                    new JsonDataProvider<Medicine>(DataFilePath.MedicinePath));
                _imageRepository = new ImageRepository(new JsonDataProvider<Image>(DataFilePath.ImagePath));
            }
            else
            {
                string conf = ConfigurationManager.AppSettings["dbProviderType"] ?? "";
                IDataProvider<Medicine> medicineProvider;
                IDataProvider<Image> imageProvider;
                IDataProvider<MedicineCategory> categoryProvider;

                Console.WriteLine(conf);
                Console.ReadKey();

                if (conf.Equals("QUERY"))
                {
                    medicineProvider = new SqlQueryMedicineProvider();
                    imageProvider = new SqlQueryImageProvider();
                    categoryProvider = new SqlQueryCategoryProvider();
                }
                else
                {
                    medicineProvider = new SPMedicineProvider();
                    imageProvider = new SPImageProvider();
                    categoryProvider = new SPCategoryProvider();
                }

                _medicineRepository = new MedicineRepository(medicineProvider, imageProvider, categoryProvider);
                _categoryRepository = new CategoryRepository(categoryProvider, medicineProvider);
                _imageRepository = new ImageRepository(imageProvider);
            }

            CurrentImageId = _imageRepository.FindAll().Count != 0 ? _imageRepository.FindAll().Max(m => m.Id) : 0;
            CurrentCategoryId = _categoryRepository.FindAll().Count != 0 ? _categoryRepository.FindAll().Max(m => m.Id) : 0;
            CurrentMedicineId = _medicineRepository.FindAll().Count != 0 ? _medicineRepository.FindAll().Max(m => m.Id) : 0;

        }
        private void printHeader(string header)
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - header.Length) / 2, Console.CursorTop + 2);
            Console.WriteLine(header.ToUpper());
        }
        public void ShowMenu()
        {
            string header = "Medicines Management";
            printHeader(header);

            Console.WriteLine("\t1. Add Medicine Category.");
            Console.WriteLine("\t2. Add Medicine.");
            Console.WriteLine("\t3. Add Medicine to Category.");
            Console.WriteLine("\t4. Get Medicine by ID.");
            Console.WriteLine("\t5. Search Medicines by name.");
            Console.WriteLine("\t6. Show medicines.");
            Console.WriteLine("\t7. Show medicines by category.");

        }

        public void AddMedicineCategory() {
            printHeader("Add Medicine Category");
            string name = string.Empty;
            while (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {    
                Console.WriteLine("Enter name: ");
                name = Console.ReadLine();
            }

            var newMedicineType = new MedicineCategory() { Id = ++CurrentCategoryId, Name = name };
           
            try
            {
                var actionStatus = _categoryRepository.Save(newMedicineType);
                /*if (actionStatus.RowsAffected <= 0)
                {
                    //fail
                    Console.WriteLine(actionStatus.ErrorMessage);
                    Console.ReadKey();
                    return;
                }
                else
                {
                    //success
                    Console.WriteLine(actionStatus.RowsAffected);
                    Console.WriteLine(actionStatus.ErrorMessage);
                    Console.ReadKey();
                }*/
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                
            }

            //write object to json file
            Console.WriteLine($"Add {name} successfully.");
        }

        public void AddMedicine()
        {
            printHeader("Add Medicine");
            string name = string.Empty;

            while (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Enter name: ");
                name = Console.ReadLine();
            }

            Console.WriteLine("Enter description: ");
            string description = Console.ReadLine() ?? "";

            bool convertPriceSuccess;
            decimal price;

            do
            {
                Console.WriteLine("Enter price: ");
                string inputPrice = Console.ReadLine();
                convertPriceSuccess = decimal.TryParse(inputPrice, out price);
            } while (!convertPriceSuccess);

            var newMedicine = new Medicine()
            {
                Id = ++CurrentMedicineId,
                Name = name,
                Description = description,
                Price = price,
                PrimaryImageId = 0,
                PopularityMedicine = false,
                CategoryId = null
            };

            
            Console.WriteLine($"Do you want to add Image? y/any.");

            string addImageOption = Console.ReadLine() ?? "";
            if (addImageOption.ToLower() == "y")
            {
                newMedicine.Images = AddImage();
            }

            //status check
            try
            {
                var actionStatus = _medicineRepository.Save(newMedicine);

              /*  if (actionStatus.RowsAffected <= 0)
                {
                    //fail
                    Console.WriteLine(actionStatus.ErrorMessage);
                    Console.ReadKey();
                    return;
                }
                else
                {
                    //success
                    Console.WriteLine(actionStatus.ErrorMessage);
                    newMedicine.Id = actionStatus.RowsAffected;
                    Console.ReadKey();
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"Add Medicine {name} completed.");
        }

        public List<Image> AddImage()
        {
            List<Image> images = new List<Image>();

            string name = string.Empty;
            string url = string.Empty;
            string optionKey;
            do
            {
                Console.WriteLine("Add Image");

                while (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Enter name: ");
                    name = Console.ReadLine();
                }

                while (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
                {
                    Console.WriteLine("Enter URL: ");
                    url = Console.ReadLine();
                }

                var image = new Image() { Id = ++CurrentImageId, Name = name, URL = url };

                images.Add(image);

                 name = string.Empty;
                url = string.Empty;
                Console.WriteLine("Press o to add more or any to complete.");
                optionKey = Console.ReadLine();
            } while (optionKey == "o");
            return images;
        }

        public void AddMedicineToCategory()
        {
            printHeader("Add Medicine To Category");

            Console.WriteLine("Select category:");
            displayMCategories();

            int medicineCateId = InputIDHandle();

            Console.WriteLine("Select Medicine:");
            displayMedicines();
            int medicineId = InputIDHandle();

            List<MedicineCategory> medicineCategories = _categoryRepository.FindAll();
            var category = medicineCategories.Find(m => m.Id == medicineCateId);

            var actionStatus = _medicineRepository.UpdateColumn(medicineId, "CategoryId", medicineCateId.ToString());

            if (actionStatus.RowsAffected > 0)
            {
                Console.WriteLine("Update ssf");
                Console.WriteLine($"Msg: {actionStatus.ErrorMessage}");
            }
            else if (actionStatus.RowsAffected == 0)
            {
                Console.WriteLine("Nothing to update");
                Console.WriteLine($"Mgs: {actionStatus.ErrorMessage}");
            }
            else
            {
                Console.WriteLine("Update failed");
                Console.WriteLine($"Error msg: {actionStatus.ErrorMessage}");
            }

            if (_environment.Equals(EnvironmentType.JSON))
                _categoryRepository.SaveAll(medicineCategories);
        }

        public void FindMedicineByID()
        {
            printHeader("Find Medicine By ID");

            int id = InputIDHandle();

            try
            {
                var medicine = _medicineRepository.FindById(id);
                Console.WriteLine(medicine);
            }
            catch (MedicineNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static int InputIDHandle()
        {
            int ID = -1;
            do
            {
                Console.WriteLine("Enter ID:");
                Console.Write("/>");
                if (!int.TryParse(Console.ReadLine(), out ID) || ID < 0)
                {
                    Console.WriteLine("ID must be an integer and greater than or equals 0.");
                    continue;
                }
                break;
            } while (true);
            return ID;
        }

        public void FindMedicineByName()
        {
            printHeader("Search Medicine By Name");
            Console.WriteLine("Enter name:");
            string searchValue = Console.ReadLine() ?? "";
            var medicineSearchResults = _medicineRepository.FindByName(searchValue);
            foreach (var m in medicineSearchResults)
            {
                Console.WriteLine(m);
            }
        }

        public void ShowMedicines()
        {
            printHeader("Show medicines");

            //need to add selection (sort by, sort direction ....)
            List<Medicine> medicines = _medicineRepository.FindAll();
            int page = 0;
            int size = 10;
            int totalPages = medicines.Count / (size * (page + 1));
            bool hasSelectSortType = false;
            string inputSortBy = "";
            string inputSortDirection = "";
            SortBy sortBy = SortBy.PRICE;
            SortDirection sortDirection = SortDirection.DESCENDING;
            do
            {
                if (!hasSelectSortType)
                {
                    Console.WriteLine("Sort by: p (price), n (name)");
                    Console.Write(" Your choose: ");
                    inputSortBy = Console.ReadLine();

                    if (inputSortBy.ToLower().Equals("p"))
                    {
                        sortBy = SortBy.PRICE;
                    }
                    else if (inputSortBy.ToLower().Equals("n"))
                    {
                        sortBy = SortBy.NAME;

                    }
                    else continue;

                    Console.WriteLine("Sort direction: a (asc), d (des)");
                    Console.Write(" Your choose: ");
                    inputSortDirection = Console.ReadLine();

                    if (inputSortDirection.ToLower().Equals("a"))
                    {
                        sortDirection = SortDirection.ASCENDING;
                    }
                    else if (inputSortDirection.ToLower().Equals("d"))
                    {
                        sortDirection = SortDirection.DESCENDING;

                    }
                    else continue;
                }
                hasSelectSortType = true;



                foreach (var m in GetMedicinesByPage(medicines, page, size, sortBy, sortDirection))
                {
                    Console.WriteLine(m);
                }
                Console.WriteLine();
                Console.Write("Enter -> load more\t");
                Console.Write("s -> change size\t");
                Console.WriteLine("\tp -> jump");
                Console.WriteLine($"Current page: {page + 1}, Current size: {size}");
                ConsoleKeyInfo keypress = Console.ReadKey(true);
                Console.WriteLine(keypress.Key.ToString());
                if (keypress.Key.ToString().Equals("Enter"))
                {
                    ++page;
                }
                else if (keypress.Key.ToString().Equals("S"))
                {
                    Console.WriteLine("enter size: ");
                    int newSize = int.Parse(Console.ReadLine());
                    size = newSize;
                }
                else if (keypress.Key.ToString().Equals("P"))
                {
                    Console.WriteLine("enter page: ");
                    int newPage = int.Parse(Console.ReadLine());
                    page = newPage - 1;
                }
            } while (page < totalPages);

        }

        public List<Medicine> GetMedicinesByPage(List<Medicine> medicines, int page, int size, SortBy sortBy, SortDirection sortDirection)
        {
            int startAt = page * size;
            int endAt = startAt + size;

            int totalElements = _medicineRepository.FindAll().Count;
            
            if (totalElements == 0) return [];

            endAt = endAt > totalElements ? totalElements : endAt;
            
            List<Medicine> sortedList = [];

            Console.WriteLine($"Total Elements: {totalElements}");

            try
            {
                if (sortBy.Equals(SortBy.PRICE))
                {
                    sortedList = sortDirection == SortDirection.DESCENDING ?
                         medicines.OrderByDescending(m => m.Price).ToList().GetRange(startAt, endAt) :
                         medicines.OrderBy(m => m.Price).ToList().GetRange(startAt, endAt);
                }
                else if (sortBy.Equals(SortBy.NAME))
                {
                    sortedList = sortDirection == SortDirection.DESCENDING ?
                        medicines.OrderByDescending(m => m.Name).ToList().GetRange(startAt, endAt) :
                        medicines.OrderBy(m => m.Name).ToList().GetRange(startAt, endAt);
                }
            }
            catch (IndexOutOfRangeException ie)
            {
                Console.WriteLine(ie.Message);
            }
            return sortedList;

        }

        public void ShowMedicineByCategory()
        {
            printHeader("show medicine by category");
            Console.WriteLine("Select category:");
            displayMCategories();

            int medicineCateId = InputIDHandle();

            List<Medicine> medicines = _medicineRepository.FindAllByColumn("CategoryId",medicineCateId);
            foreach(var m  in medicines)
            {
                Console.WriteLine(m);
            }
        }

        public void displayMCategories()
        {
            foreach (var mc in _categoryRepository.FindAll())
            {
                Console.WriteLine(mc);
            }
        }

        public void displayMedicines()
        {
            foreach (var m in _medicineRepository.FindAll())
            {
                Console.WriteLine(m);
            }
        }
    }
}