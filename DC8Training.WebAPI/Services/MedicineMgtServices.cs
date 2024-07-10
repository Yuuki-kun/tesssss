
using DC8Training.WebAPI.Dtos;
using QLDP.Exceptions;
using QLDP.Models;
using QLDP.Repositories;
using Image = QLDP.Models.Image;

namespace DC8Training.WebAPI.Services
{
    public class MedicineMgtServices : IMedicineService
    {
        private IMedicineRepository _medicineRepository;
        public MedicineMgtServices(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public OperationResult Create(Medicine medicineToSave)
        {
            return _medicineRepository.Save(medicineToSave);
        }

        public OperationResult CreateOrUpdate(Medicine medicine)
        {
            ModelState modelState = Validate(medicine);
            if (!modelState.IsValid)
            {
                return new OperationResult { Success = false, ErrorMessage = "invalid entity" };
            }
            if (medicine.Id <= 0)
            {
                var createActionResult = Create(medicine);
                return createActionResult;
            }
            else
            {
                var updateActionResult = UpdateEntity(medicine);
                return updateActionResult;
            }
        }

        public OperationResult Delete(int id)
        {
            var medicine = FindById(id);

            if (medicine == null)
            {
                throw new MedicineNotFoundException($"medicine with id {id} not found");
            }

            return _medicineRepository.Delete(id);
        }

        public Medicine FindById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"id = {id} is invalid");
            }
            return _medicineRepository.FindById(id);
        }

        public List<Medicine> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"name = {name} is not valid");
            }
            return _medicineRepository.FindByName(name);
        }

        public List<Medicine> GetAll()
        {
            return _medicineRepository.FindAll();
        }

        public List<Medicine> GetByPage(int page, int size, int direction)
        {
            return _medicineRepository.GetByPage(page, size, direction);
        }

        public List<Medicine> GetMedicinesByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentException($"categoryId = {categoryId} is not valid");
            }
            return _medicineRepository.FindAllByColumn("CategoryId", categoryId);
        }

        public OperationResult UpdateEntity(Medicine m)
        {

            var medicine = FindById(m.Id);

            if (medicine == null)
            {
                throw new MedicineNotFoundException("");
            }

            medicine.Name = m.Name;
            medicine.Description = m.Description;
            medicine.Price = m.Price;
            medicine.PrimaryImageId = m.PrimaryImageId;
            medicine.PopularityMedicine = m.PopularityMedicine;
            medicine.CategoryId = m.CategoryId;
            medicine.Images = m.Images;

            return _medicineRepository.UpdateEntity(medicine);
        }


        public OperationResult UpdateColumn(int id, string column, string value)
        {
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value)
                || string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(value)
                )
            {
                throw new ArgumentNullException("invalid param");
            }

            return _medicineRepository.UpdateColumn(id, column, value);
        }




        //validate medicine
        public ModelState Validate(Medicine medicine)
        {
            ModelState modelState = new ModelState();
            if (medicine == null)
            {
                modelState.ModelError = "Medicine is null";
                return modelState;
            }
            if (string.IsNullOrWhiteSpace(medicine.Name) || medicine.Name.Trim().Length == 0)
            {
                modelState.ModelError = "Name is required";
                return modelState;
            }
            if (medicine.Price == null || medicine.Price <= 0)
            {
                modelState.ModelError = "Price is not valid";
                return modelState;
            }
            modelState.IsValid = true;
            return modelState;
        }



      
    }
}
