using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.DTOs.Records;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IInstitutionService _institutionService;
        private readonly IClientService _clientService;
        private readonly IValidator<DTInsertClient> _clientInsertValidator;
        private readonly IValidator<DTUpdateClient> _clientUpdateValidator;
        private readonly IValidator<DTInsertInstitution> _institutionInsertValidator;
        private readonly IValidator<DTUpdateInstitution> _institutionUpdateValidator;

        public UserController(
            IInstitutionService institutionService, 
            IClientService clientService, 
            IValidator<DTInsertClient> clientInsertValidator,
            IValidator<DTUpdateClient> clientUpdateValidator,
            IValidator<DTInsertInstitution> institutionInsertValidator,
            IValidator<DTUpdateInstitution> institutionUpdateValidator
            )
        {
            _institutionService = institutionService;
            _clientService = clientService;
            _clientInsertValidator = clientInsertValidator;
            _clientUpdateValidator = clientUpdateValidator; 
            _institutionInsertValidator = institutionInsertValidator;
            _institutionUpdateValidator = institutionUpdateValidator;
        }

        // GET ALL

        [HttpGet("clients")]
        public async Task<IEnumerable<UserCard>> GetClient() => await _clientService.GetAllAsync();

        [HttpGet("institution")]
        public async Task<IEnumerable<UserCard>> GetInstitutions() => await _institutionService.GetAllAsync();

        // GET BY ID

        [HttpGet("client/detail/{id}")]
        public async Task<ActionResult<DTClient>> GetClientById(Guid id)
        {
            DTClient? dtClient = await _clientService.GetByIdAsync(id);

            return dtClient == null ? NotFound() : Ok(dtClient);
        }

        [HttpGet("institution/detail/{id}")]
        public async Task<ActionResult<DTInstitution>> GetInstitutionById(Guid id)
        {
            DTInstitution? dtInstitution = await _institutionService.GetByIdAsync(id);

            return dtInstitution == null ? NotFound() : Ok(dtInstitution);
        }

        // CREATE

        [HttpPost("client")]
        public async Task<ActionResult<DTClient>> CreateClient(DTInsertClient dtInsert) 
        {
            var validationResult = await _clientInsertValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _clientService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return CreatedAtAction(nameof(GetClientById), new { id = dt!.Id }, dt);
        }

        [HttpPost("institution")]
        public async Task<ActionResult<DTInstitution>> CreateInstitution(DTInsertInstitution dtInsert)
        {
            var validationResult = await _institutionInsertValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _institutionService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return CreatedAtAction(nameof(GetInstitutionById), new { id = dt!.Id }, dt);
        }

        // UPDATE

        [HttpPut("client")]
        public async Task<ActionResult<DTClient>> UpdateClient(DTUpdateClient dtUpdate)
        {
            var validationResult = await _clientUpdateValidator.ValidateAsync(dtUpdate);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _clientService.UpdateAsync(dtUpdate);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        [HttpPut("institution")]
        public async Task<ActionResult<DTInstitution>> UpdateInstitution(DTUpdateInstitution dtUpdate)
        {
            var validationResult = await _institutionUpdateValidator.ValidateAsync(dtUpdate);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _institutionService.UpdateAsync(dtUpdate);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        // DELETE

        [HttpDelete("client{id}")]
        public async Task<ActionResult<DTClient>> DeleteClient(Guid id) 
        {
            var dt = await _clientService.DeleteAsync(id);

            return dt == null ? NotFound() : Ok(dt);
        }

        [HttpDelete("institution{id}")]
        public async Task<ActionResult<DTInstitution>> DeleteInstitution(Guid id)
        {
            var dt = await _institutionService.DeleteAsync(id);

            return dt == null ? NotFound() : Ok(dt);
        }
    }
}
