using DC8Training.WebAPI.Dtos;
using DC8Training.WebAPI.Mappers;
using DC8Training.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using QLDP.Exceptions;
using QLDP.Models;
using QLDP.Repositories;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DC8Training.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private IMedicineService _medicneService { get; set; }
        private IImageService _imageService { get; set; }
        public MedicineController(IMedicineService medicneService, IImageService imageService)
        {
            _medicneService = medicneService;
            _imageService = imageService;
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<Medicine> GetMedicineById(int id)
        {

            try
            {
                var medicine = _medicneService.FindById(id);
                if (medicine == null)
                {
                    return NotFound();

                }
                return Ok(medicine);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("get-by-name")]
        [HttpGet]
        public ActionResult<List<Medicine>> GetMedicinesByName(string name)
        {
            try
            {
                var medicine = _medicneService.FindByName(name);
                if (medicine == null)
                {
                    return NotFound();
                }
                return Ok(medicine);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("get-by-category/{id}")]
        [HttpGet]
        public ActionResult<List<Medicine>> GetMedicinesByCategory(int id)
        {

            try
            {
                var categories = _medicneService.GetMedicinesByCategory(id);

                if (categories == null || categories.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(categories);
                }
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("medicines")]
        [HttpGet]
        public ActionResult<List<Medicine>> GetMedicinesByPage(int page = 0, int size = 10, int sort = 0)
        {
            try
            {
                return _medicneService.GetByPage(page, size, sort);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region Create or Update Medicine
        /// <summary>
        ///     Inserts a Medicine object into the database
        ///     or updates an existing Medicine in the database based on the provided DTO
        /// </summary>
        /// <param name="medicineDto">The DTO containing Medicine data and images to save or update</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromForm] MedicineDto medicineDto)
        {
            //get Entity to save or update from DTO
            var medicineToSave = new MedicineMapperImpl().MapFrom(medicineDto);

            //get image files from DTO
            var imageFilesToSave = medicineDto.Images;

            //save images to disk, returns list of Image that saved successfully
            List<Image> savedImagesToDisk = new List<Image>();
            if (medicineDto.Images != null && medicineDto.Images.Count > 0)
            {
                try {
                    savedImagesToDisk = await _imageService.SaveImageToDiskAsync(imageFilesToSave);
                    medicineToSave.Images = savedImagesToDisk;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
/*            else return BadRequest("No images uploaded.");
*/
            try
            {
                var result = _medicneService.CreateOrUpdate(medicineToSave);

                if (result.Success)
                {
                    //create
                    if (result.ReturnId > 0)
                    {
                        return CreatedAtAction(nameof(CreateOrUpdate), new { id = result.ReturnId }, medicineToSave);
                    }

                    //update
                    else
                    {   
                        return NoContent();
                    }
                }
                else
                {
                    //create failed
                    if (medicineDto.Id <= 0)
                    {
                        //delete saved images
                        await _imageService.DeleteImageFromDiskAsync(savedImagesToDisk);

                    }
                    return BadRequest(result.ErrorMessage);
                }
            }
            catch (MedicineNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e) when (e is ArgumentException || e is SqlException)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion


        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            if(id <=0 )
            {
                return BadRequest();
            }
            try
            {
                var medicine = _medicneService.FindById(id);

                if (medicine == null)
                {
                    return BadRequest();
                }

                var result = _medicneService.Delete(id);

                if (result.Success)
                {

                    await _imageService.DeleteImageFromDiskAsync(medicine.Images);

                    return NoContent();
                }
                return BadRequest();
            } catch (MedicineNotFoundException e)
            {
                return NotFound(e.Message);
            } catch (ArgumentException e)
            {
                return BadRequest($"MedicineNotFound {e.Message}");
            }catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
