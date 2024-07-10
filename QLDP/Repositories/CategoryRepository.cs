using QLDP.Models;
using QLDP.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLDP.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly IDataProvider<MedicineCategory> _categoryProvider;
        private readonly IDataProvider<Medicine> _medicineProvider;
        public CategoryRepository(IDataProvider<MedicineCategory> categoryProvider,
                IDataProvider<Medicine> medicineProvider
            ) {
            _categoryProvider = categoryProvider;
            _medicineProvider = medicineProvider;
        }

        public OperationResult Delete(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException("id");
            return _categoryProvider.Delete(id);
        }

        public List<MedicineCategory> FindAll()
        {
            var categories = _categoryProvider.LoadData();
            if (categories != null && categories.Count > 0)
            {
                foreach (var category in categories)
                {
                    category.Medicines = _medicineProvider.FindAllByColumn("CategoryId", category.Id);
                }
            }
            return categories;
        }

        public List<MedicineCategory> FindAllByColumn(string column, object value)
        {
            return _categoryProvider.FindAllByColumn(column, value);
        }

        public MedicineCategory FindById(int id)
        {
            var categories = _categoryProvider.FindById(id);
            if(categories != null)
            {
                categories.Medicines = _medicineProvider.FindAllByColumn("CategoryId", id);
            }
            return categories;
        }

        public List<MedicineCategory> FindByName(string name)
        {
            var categories = _categoryProvider.FindByName(name);
            if(categories != null  && categories.Count>0 )
            {
                foreach( var category in categories)
                {
                    category.Medicines = _medicineProvider.FindAllByColumn("CategoryId", category.Id);
                }
            }
            return categories;
        }

        public OperationResult Save(MedicineCategory category)
        {
            var result = _categoryProvider.SaveData(category);
          
            return result;
        }

        public void SaveAll(List<MedicineCategory> data)
        {
            _categoryProvider.SaveAll(data);
        }

        public OperationResult UpdateColumn(int id, string column, string newValue)
        {
            return _categoryProvider.UpdateColumn(id, column, newValue);
        }

        public OperationResult UpdateEntity(MedicineCategory t)
        {
           return _categoryProvider.UpdateEntity(t);
        }
    }
}
