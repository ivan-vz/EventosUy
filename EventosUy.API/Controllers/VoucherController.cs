using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        // GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<DTVoucher>> GetById(Guid id) 
        {
            if (id == Guid.Empty) { return BadRequest("Invalid Id"); }

            var dt = (await _voucherService.GetByIdAsync(id)).dt;

            return dt is null ? NotFound() : Ok(dt);
        }

        // CREATE 

        [HttpPost]
        public async Task<ActionResult<DTVoucher>> Create(
            DTInsertVoucherWithSponsor dtInsert,
            [FromServices] IValidator<DTInsertVoucherWithSponsor> validator
            ) 
        {
            var validationResult = await validator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _voucherService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        [HttpPost]
        public async Task<ActionResult<DTVoucher>> Create(
            DTInsertVoucherWithoutSponsor dtInsert,
            [FromServices] IValidator<DTInsertVoucherWithoutSponsor> validator)
        {
            var validationResult = await validator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _voucherService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }
    }
}
