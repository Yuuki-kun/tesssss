using DC8Training.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using QLDP.Exceptions;
using QLDP.Models;
using System.Data.SqlClient;

namespace DC8Training.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService {  get; set; }
        public CategoryController(ICategoryService categoryService) { 
            _categoryService = categoryService;
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetCategoryById(int id)
        {
            Console.WriteLine("get category id = " +id);
            try
            {
                var category = _categoryService.FindById(id);
                if (category == null)
                {
                    return NotFound();

                }
                return Ok(category);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("get-by-name")]
        [HttpGet]
        public ActionResult<List<MedicineCategory>> GetCategoriesByName(string name)
        {
            try
            {
                var categories = _categoryService.FindByName(name);
                if (categories == null)
                {
                    return NotFound();
                }
                return Ok(categories);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("categories")]
        [HttpGet]
        public ActionResult<List<MedicineCategory>> GetAll()
        {
            try
            {
                return Ok(_categoryService.GetAll());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpPost]
        public ActionResult<MedicineCategory> CreateOrUpdate([FromBody] MedicineCategory category)
        {

            try
            {
                var result = _categoryService.CreateOrUpdate(category);

                if (result.Success)
                {
                    if (result.ReturnId > 0)
                    {
                        category.Id = result.ReturnId;
                        return CreatedAtAction(nameof(CreateOrUpdate), new { id = result.ReturnId }, category);
                    }
                    return NoContent();
                }
                return BadRequest();
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

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            if (id == null || id < 0)
            {
                return BadRequest("id is not valid");
            }
            try
            {
                var result = _categoryService.Delete(id);
                if (result.Success)
                {
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
                return BadRequest($"{e.Message}");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
    }
}
