using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DC8Training.WebAPI.Dtos
{
    public class UploadDtoTest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IFormFile formFile { get; set; }
    }
}
