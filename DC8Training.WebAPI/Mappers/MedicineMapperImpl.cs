using DC8Training.WebAPI.Dtos;
using QLDP.Models;

namespace DC8Training.WebAPI.Mappers
{
    public class MedicineMapperImpl : IMapper<Medicine, MedicineDto>
    {
        public Medicine MapFrom(MedicineDto b)
        {
            if(b == null)
                throw new ArgumentNullException(nameof(b));
            return new Medicine { 
                Id = b.Id, 
                Name = b.Name, 
                Description = b.Description,
                Price = b.Price,
                PrimaryImageId = b.PrimaryImageId,
                CategoryId = b.CategoryId,
                PopularityMedicine = b.PopularityMedicine
            };
        }

        public MedicineDto MapTo(Medicine a)
        {
            throw new NotImplementedException();
        }
    }
}
