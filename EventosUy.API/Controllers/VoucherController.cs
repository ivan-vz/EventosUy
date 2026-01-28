using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        private readonly VoucherInsertWithSponsorValidator _voucherInsertWithSponsorValidator;
        private readonly VoucherInsertWithoutSponsorValidator _voucherInsertWithoutSponsorValidator;

        public VoucherController(
            IVoucherService voucherService, 
            VoucherInsertWithSponsorValidator voucherInsertWithSponsorValidator, 
            VoucherInsertWithoutSponsorValidator voucherInsertWithoutSponsorValidator)
        {
            _voucherService = voucherService;
            _voucherInsertWithSponsorValidator = voucherInsertWithSponsorValidator;
            _voucherInsertWithoutSponsorValidator = voucherInsertWithoutSponsorValidator;
        }

        // GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<DTVoucher>> GetById(Guid id) 
        {
            if (id == Guid.Empty) { return BadRequest("Invalid Id"); }

            var dt = await _voucherService.GetByIdAsync(id);

            return dt is null ? NotFound() : Ok(dt);
        }

        // CREATE 

        [HttpPost]
        public async Task<ActionResult<DTVoucher>> Create(DTInsertVoucherWithSponsor dtInsert) 
        {
            var validationResult = await _voucherInsertWithSponsorValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _voucherService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        [HttpPost]
        public async Task<ActionResult<DTVoucher>> Create(DTInsertVoucherWithoutSponsor dtInsert)
        {
            var validationResult = await _voucherInsertWithoutSponsorValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _voucherService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }
    }
}
