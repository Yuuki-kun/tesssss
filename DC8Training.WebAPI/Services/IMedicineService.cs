using DC8Training.WebAPI.Dtos;
using QLDP.Models;

namespace DC8Training.WebAPI.Services
{
    public interface IMedicineService : IService<Medicine>
    {
        List<Medicine> GetMedicinesByCategory(int categoryId);
    }
}
