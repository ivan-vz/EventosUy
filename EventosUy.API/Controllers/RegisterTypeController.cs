using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterTypeController : ControllerBase
    {
        private readonly IRegisterTypeService _registerTypeService;
        private readonly RegisterTypeInsertValidator _registerTypeInsertValidator;

        public RegisterTypeController(IRegisterTypeService registerTypeService, RegisterTypeInsertValidator registerTypeInsertValidator) 
        {
            _registerTypeService = registerTypeService;
            _registerTypeInsertValidator = registerTypeInsertValidator;
        }

        // GET ALL

        [HttpGet("{id}")]
        public async Task<IEnumerable<RegisterTypeCard>> Get(Guid id) => await _registerTypeService.GetAllByEditionAsync(id);

        // GET BY ID

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<DTRegisterType>> GetById(Guid id) 
        {
            var dt = (await _registerTypeService.GetByIdAsync(id)).dt;

            return dt is null ? NotFound() : Ok(dt);
        }

        // CREATE

        [HttpPost]
        public async Task<ActionResult<DTRegisterType>> Create(DTInsertRegisterType dtInsert) 
        {
            var validationResult = await _registerTypeInsertValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _registerTypeService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<DTRegisterType>> Delete(Guid id) 
        {
            var dt = await _registerTypeService.DeleteAsync(id);

            return dt is null ? NotFound() : dt;
        }
    }
}
