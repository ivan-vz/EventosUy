using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.Interfaces;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }

        // GET ALL 

        [HttpGet]
        public async Task<IEnumerable<string>> GetAll() => await _categoryService.GetAllAsync();

        // GET BY ID

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<DTCategory>> GetById(Guid id) 
        {
            var dt = await _categoryService.GetByIdAsync(id);

            return  dt is null ? NotFound() : Ok(dt);
        }

        // CREATE

        [HttpPost]
        public async Task<ActionResult<DTCategory>> Create(string name) 
        {
            var validationResult = new ValidationResult();
            if (string.IsNullOrWhiteSpace(name)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Name", "Name cannot be empty.")
                    );

                return BadRequest(validationResult.Errors);
            }

            var (dt, validation) = await _categoryService.CreateAsync(name);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return CreatedAtAction(nameof(GetById), new { id = dt!.Id }, dt);
        }

        // Delete

        [HttpDelete("{id}")]
        public async Task<ActionResult<DTCategory>> Delete(Guid id)
        {
            var dt = await _categoryService.DeleteAsync(id);

            return dt is null ? NotFound() : Ok(dt);
        }
    }
}
