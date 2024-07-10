namespace DC8Training.WebAPI.Dtos
{
    public class ImageFileDto
    {
        public string FileName { get; set; }
        public IFormFile FileToSave { get; set; }
    }
}
