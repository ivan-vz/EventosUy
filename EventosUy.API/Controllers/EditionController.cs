using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditionController : ControllerBase
    {
        private readonly IEditionService _editionService;
        private readonly EditionInsertValidator _editionInsertValidator;
        private readonly EditionUpdateValidator _editionUpdateValidator;

        public EditionController(IEditionService editionService, EditionInsertValidator editionInsertValidator, EditionUpdateValidator editionUpdateValidator)
        {
            _editionService = editionService;
            _editionInsertValidator = editionInsertValidator;
            _editionUpdateValidator = editionUpdateValidator;
        }

        // GET ALL

        [HttpGet]
        public async Task<IEnumerable<EditionCard>> GetAll() => await _editionService.GetAllAsync();

        // GET BY ID

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<DTEdition>> GetById(Guid id)
        {
            var dt = await _editionService.GetByIdAsync(id);

            return dt is null ? NotFound() : Ok(dt);
        }

        // CREATE

        [HttpPost]
        public async Task<ActionResult<DTEdition>> Create(DTInsertEdition dtInsert)
        {
            var validationResult = await _editionInsertValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _editionService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        // UPDATE

        [HttpPut]
        public async Task<ActionResult<DTEdition>> Update(DTUpdateEdition dtUpdate)
        {
            var validationResult = await _editionUpdateValidator.ValidateAsync(dtUpdate);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _editionService.UpdateAsync(dtUpdate);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        // DELETE

        [HttpDelete("{id}")]
        public async Task<ActionResult<DTEdition>> Delete(Guid id)
        {
            var dt = await _editionService.DeleteAsync(id);

            return dt is null ? NotFound() : Ok(dt);
        }

        // Approve 

        [HttpPut("approve/{id}")]
        public async Task<ActionResult<DTEdition>> Approve(Guid id) 
        {
            var approved = await _editionService.ApproveAsync(id);

            return approved ? NotFound() : Ok();
        }

        // Reject 

        [HttpPut("reject/{id}")]
        public async Task<ActionResult<DTEdition>> Reject(Guid id)
        {
            var rejected = await _editionService.RejectAsync(id);

            return rejected ? NotFound() : Ok();
        }
    }
}
