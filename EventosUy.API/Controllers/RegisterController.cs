using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService) 
        {
            _registerService = registerService;
        }

        // GET ALL

        [HttpGet("client/{id}")]
        public async Task<IEnumerable<RegisterCardByClient>> GetAllByClient(Guid id) => await _registerService.GetAllByClientAsync(id);

        [HttpGet("edition/{id}")]
        public async Task<IEnumerable<RegisterCardByEdition>> GetAllByEdition(Guid id) => await _registerService.GetAllByEditionAsync(id);

        // CREATE

        [HttpPost]
        public async Task<ActionResult<DTRegister>> Create(
            DTInsertRegisterWithVoucher dtInsert,
            [FromServices] IValidator<DTInsertRegisterWithVoucher> validator
            ) 
        {
            var validationResult = await validator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _registerService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        [HttpPost]
        public async Task<ActionResult<DTRegister>> Create(
            DTInsertRegisterWithoutVoucher dtInsert,
            [FromServices] IValidator<DTInsertRegisterWithoutVoucher> validator
            )
        {
            var validationResult = await validator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _registerService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }
    }
}
