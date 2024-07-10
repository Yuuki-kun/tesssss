using DC8Training.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using QLDP.Exceptions;
using QLDP.Models;

namespace DC8Training.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private IImageService _imageService { get; set; }
        public ImageController(IImageService imageService) {
            _imageService = imageService;
        }

        /*[HttpGet("/id/{id}")]
        public IActionResult GetImage(int id)
        {
            var image = _imageService.FindById(id);
            return image == null ? NotFound() : Ok(image);
        }*/

        [Route("{name}")]
        [HttpGet]
        public IActionResult GetImage(string name)
        {
            string? storedPath = System.Configuration.ConfigurationManager.AppSettings["SaveImagePath1"];
            //string? storedPath = System.Configuration.ConfigurationManager.AppSettings["SaveImagePath2"];

            var imagePath = Path.Combine(storedPath, name);

            if(storedPath == null || string.IsNullOrEmpty(storedPath) || string.IsNullOrWhiteSpace(storedPath) ||
               imagePath == null || string.IsNullOrEmpty(imagePath) || string.IsNullOrWhiteSpace(imagePath)) {

                Console.WriteLine("get image api, folder dsn't exist");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            try
            {
                if (!System.IO.File.Exists(imagePath))
                {

                    //if img not found -> return sample.jpeg
                    imagePath = Path.Combine(storedPath, "sample.jpeg");

                    if (!System.IO.File.Exists(imagePath))
                    {
                        //sample dsn't exist -> 404
                        return NotFound("image not found, name: sample.jpeg");
                    }
                    var sampleImageBytes = System.IO.File.ReadAllBytes(imagePath);
                    return File(sampleImageBytes, "image/jpeg");
                }

                var imageBytes = System.IO.File.ReadAllBytes(imagePath);

                return File(imageBytes, "image/jpeg");
            }catch(Exception ex)
            {
                Console.WriteLine("Get image api, ",ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || id < 0)
            {
                return BadRequest("id is not valid");
            }
            try
            {
                var image = _imageService.FindById(id);
                if (image == null)
                {
                    return BadRequest();
                }

                var result = _imageService.Delete(id);
                
                if (result.Success)
                {
                    List<Image> img = [];
                    img.Add(image);
                    await _imageService.DeleteImageFromDiskAsync(img);
                    return NoContent();
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (MedicineNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest($"Not Found {e.Message}");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }

}
