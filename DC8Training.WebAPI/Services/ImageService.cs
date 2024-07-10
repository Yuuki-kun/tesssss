using QLDP.Exceptions;
using QLDP.Models;
using QLDP.Repositories;
using System.Xml.Linq;

namespace DC8Training.WebAPI.Services
{
    public class ImageService : IImageService
    {
        private IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public OperationResult Create(Image t)
        {
            throw new NotImplementedException();
        }

        public OperationResult CreateOrUpdate(Image t)
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(int id)
        {

            return _imageRepository.Delete(id);
        }

        public async Task DeleteImageFromDiskAsync(List<Image> images)
        {
            List<string> failedDeletions = new List<string>();

            string storedPath = System.Configuration.ConfigurationManager.AppSettings["SaveImagePath1"];

            foreach (var image in images)
            {
                var filePath = Path.Combine(storedPath, image.Name);

                try
                {
                    if (File.Exists(filePath))
                    {
                        await Task.Run(() => File.Delete(filePath));
                        Console.WriteLine($"Deleted: {filePath}");
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file {filePath}: {ex.Message}");
                }
            }

        }



        public Image FindById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"id = {id} is invalid");
            }
            return _imageRepository.FindById(id);
        }

        public List<Image> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Image> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Image> GetByPage(int page, int size, int d)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Image>> SaveImageToDiskAsync(List<IFormFile> UploadImages)
        {
            List<Image> returnList = new List<Image>();

            if (UploadImages != null)
            {
                try
                {
                    string storedPath = System.Configuration.ConfigurationManager.AppSettings["SaveImagePath1"];
                    //string storedPath = System.Configuration.ConfigurationManager.AppSettings["SaveImagePath2"];

                    foreach (var file in UploadImages)
                    {
                        string fname = Guid.NewGuid().ToString() + ".jpg";

                        var filePath = Path.Combine(storedPath, fname);

                        if (System.IO.File.Exists(filePath))
                        {
                            string tempFileName = Path.GetTempFileName();
                            string uniqueFileName = Path.GetFileName(tempFileName) + fname;
                            filePath = Path.Combine(storedPath, uniqueFileName);

                            System.IO.File.Delete(tempFileName);
                        }

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        returnList.Add(new Image { Name = fname, URL = $"https://localhost:7148/api/Image/{fname}" });
                    }

                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return returnList;
        }

        public OperationResult UpdateColumn(int id, string column, string value)
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdateEntity(Image t)
        {
            throw new NotImplementedException();
        }

        public ModelState Validate(Image t)
        {
            throw new NotImplementedException();
        }
    }
}
