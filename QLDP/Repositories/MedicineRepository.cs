using QLDP.Models;
using QLDP.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace QLDP.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly IDataProvider<Medicine> _medicineProvider;
        private readonly IDataProvider<Image> _imageProvider;
        private readonly IDataProvider<MedicineCategory> _categoryProvider;
        public MedicineRepository(IDataProvider<Medicine> medicineProvider, 
            IDataProvider<Image> imageProvider,
            IDataProvider<MedicineCategory> categoryProvider
            ) {
            _medicineProvider = medicineProvider;
            _imageProvider = imageProvider;
            _categoryProvider = categoryProvider;
        }

        public OperationResult Delete(int id)
        {
            if(id  <= 0) throw new ArgumentOutOfRangeException("id");
            return _medicineProvider.Delete(id);
        }

        public List<Medicine> FindAll()
        {
            return _medicineProvider.LoadData() ?? [];
        }

        public List<Medicine> FindAllByColumn(string column, object value)
        {
            var medicines =  _medicineProvider.FindAllByColumn(column, value);

            foreach (var m in medicines)
            {
                if (m != null)
                {
                    m.Images = _imageProvider.FindAllByColumn("MedicineId", m.Id);
                }
            }

            return medicines;
        }

        public Medicine FindById(int id)
        {
            
            var medicine = _medicineProvider.FindById(id);
            if(medicine!=null) medicine.Images = _imageProvider.FindAllByColumn("MedicineId", id);
            return medicine;
        }

        public List<Medicine> FindByName(string name)
        {
            var medicines = _medicineProvider.FindByName(name);

            foreach (var m in medicines)
            {
                if (m != null)
                {
                    m.Images = _imageProvider.FindAllByColumn("MedicineId", m.Id);
                }
            }

            return medicines;
        }

        public List<Medicine> GetByPage(int page, int size, int d)
        {
            var medicines = _medicineProvider.FindByPage(page, size, d);
            
            foreach(var m in medicines)
            {
                if (m != null)
                {
                    m.Images = _imageProvider.FindAllByColumn("MedicineId", m.Id);
                }
            }

            return medicines;
        }

        public OperationResult Save(Medicine medicineToSave)
        {
            Console.WriteLine("upload medicine's Id = "+medicineToSave.CategoryId);

            var result = _medicineProvider.SaveData(medicineToSave);
            
                if (result.Success)
                {
                    medicineToSave.Id = result.ReturnId;

                    List<Image> imageSaveSuccess = new List<Image>();

                    if (medicineToSave.Images != null && medicineToSave.Images.Count > 0)
                    {
                        foreach (var img in medicineToSave.Images)
                        {
                            if (img != null)
                            {
                                img.MedicineId = result.ReturnId;
                                
                                var savedImage = _imageProvider.SaveData(img);
                                if (savedImage.Success)
                                {
                                    img.Id = savedImage.ReturnId;
                                    imageSaveSuccess.Add(img);
                                    Console.WriteLine("Save image success");
                                }
                                else
                                {
                                    Console.WriteLine("Save image failed");
                                }
                            }
                        }

                        medicineToSave.Images = imageSaveSuccess;
                      
                    }
                }
                else
                {
                    Console.WriteLine($"Save medicine failed. {result.ErrorMessage}");
                }
            return result;
        }

        public void SaveAll(List<Medicine> data)
        {
            _medicineProvider.SaveAll(data);
        }

        public OperationResult UpdateColumn(int id, string column, string newValue)
        {
            return _medicineProvider.UpdateColumn(id, column, newValue);
        }

        public OperationResult UpdateEntity(Medicine medicineToUpdate)
        {

            var result = _medicineProvider.UpdateEntity(medicineToUpdate);

            if (result.Success)
            {


                List<Image> imageSaveSuccess = new List<Image>();

                if (medicineToUpdate.Images != null && medicineToUpdate.Images.Count > 0)
                {
                    foreach (var img in medicineToUpdate.Images)
                    {
                        if (img != null)
                        {
                            img.MedicineId = medicineToUpdate.Id;

                            var savedImage = _imageProvider.SaveData(img);
                            if (savedImage.Success)
                            {
                                img.Id = savedImage.ReturnId;
                                imageSaveSuccess.Add(img);
                                Console.WriteLine("Save image success");
                            }
                            else
                            {
                                Console.WriteLine("Save image failed");
                            }
                        }
                    }

                    medicineToUpdate.Images = imageSaveSuccess;

                }
            }
            else
            {
                Console.WriteLine($"Save medicine failed. {result.ErrorMessage}");
            }

            return result;
        }
    }
}
