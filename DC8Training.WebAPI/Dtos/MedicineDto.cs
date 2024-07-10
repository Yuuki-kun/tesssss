using QLDP.Models;

namespace DC8Training.WebAPI.Dtos
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; } = null;
        public int PrimaryImageId { get; set; } = 0;
        public bool? PopularityMedicine { get; set; } = false;
        public List<IFormFile>? Images { get; set; }

    }
}
