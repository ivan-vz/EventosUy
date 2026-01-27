using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.DTOs.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly IRegisterService _registerService;
        private readonly RegisterInsertWithVoucherValidator _registerInsertWithVoucherValidator;
        private readonly RegisterInsertWithoutVoucherValidator _registerInsertWithoutVoucherValidator;

        public RegisterController(
            IRegisterService registerService, 
            RegisterInsertWithVoucherValidator registerInsertWithVoucherValidator, 
            RegisterInsertWithoutVoucherValidator registerInsertWithoutVoucherValidator
            ) 
        {
            _registerService = registerService;
            _registerInsertWithVoucherValidator = registerInsertWithVoucherValidator;
            _registerInsertWithoutVoucherValidator = registerInsertWithoutVoucherValidator;
        }

        // GET ALL

        [HttpGet("client/{id}")]
        public async Task<IEnumerable<RegisterCardByClient>> GetAllByClient(Guid id) => await _registerService.GetAllByClientAsync(id);

        [HttpGet("edition/{id}")]
        public async Task<IEnumerable<RegisterCardByEdition>> GetAllByEdition(Guid id) => await _registerService.GetAllByEditionAsync(id);

        // CREATE

        [HttpPost]
        public async Task<ActionResult<DTRegister>> Create(DTInsertRegisterWithVoucher dtInsert) 
        {
            var validationResult = await _registerInsertWithVoucherValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _registerService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        [HttpPost]
        public async Task<ActionResult<DTRegister>> Create(DTInsertRegisterWithoutVoucher dtInsert)
        {
            var validationResult = await _registerInsertWithoutVoucherValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _registerService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }
    }
}
