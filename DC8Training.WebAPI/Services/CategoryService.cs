using QLDP.Exceptions;
using QLDP.Models;
using QLDP.Repositories;

namespace DC8Training.WebAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public OperationResult Create(MedicineCategory category)
        {
            
            return _categoryRepository.Save(category);

        }

        public OperationResult CreateOrUpdate(MedicineCategory category)
        {
            ModelState modelState = Validate(category);
            if (!modelState.IsValid)
            {
                return new OperationResult { Success = false, ErrorMessage = "invalid entity" };
            }
            if (category.Id <= 0)
            {
                var createActionResult = Create(category);
                return createActionResult;
            }
            else
            {
                var updateActionResult = UpdateEntity(category);
                return updateActionResult;
            }
        }

        public MedicineCategory FindById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"id = {id} is invalid");
            }
            return _categoryRepository.FindById(id);
        }

        public List<MedicineCategory> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"name = {name} is not valid");
            }
            return _categoryRepository.FindByName(name);
        }

        public List<MedicineCategory> GetAll()
        {
            return _categoryRepository.FindAll();
        }

        public List<MedicineCategory> GetByPage(int page, int size, int d)
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdateEntity(MedicineCategory t)
        {
            var category = FindById(t.Id);

            if (category == null)
            {
                throw new MedicineNotFoundException("");
            }
            category.Name = t.Name;

            return _categoryRepository.UpdateEntity(category);
        }

        public OperationResult UpdateColumn(int id, string column, string value)
        {
            throw new NotImplementedException();
        }

      

        public ModelState Validate(MedicineCategory category)
        {
            ModelState modelState = new ModelState();
            if (category == null)
            {
                modelState.ModelError = "category is null";
                return modelState;
            }
            if (category.Name.Trim().Length == 0)
            {
                modelState.ModelError = "Name is required";
                return modelState;
            }
            modelState.IsValid = true;
            return modelState;
        }

        public OperationResult Delete(int id)
        {
            var category = FindById(id);

            if (category == null)
            {
                throw new MedicineNotFoundException($"category with id {id} not found");
            }

            if(category.Medicines != null && category.Medicines.Count() > 0)
            {
                return new OperationResult { 
                    Success = false, RowsAffected = 0, ErrorMessage = "has 1-N medicine" };
            }

            return _categoryRepository.Delete(id);
        }
    }
}
