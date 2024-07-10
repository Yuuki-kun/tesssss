using QLDP.Models;

namespace DC8Training.WebAPI.Services
{
    public interface IImageService : IService<Image>
    {
        Task DeleteImageFromDiskAsync(List<Image> images);
        Task<List<Image>> SaveImageToDiskAsync(List<IFormFile> UploadImages);

    }
}
