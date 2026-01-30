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
    public class SponsorshipController : ControllerBase
    {
        private readonly ISponsorshipService _sponsorshipService;

        public SponsorshipController(ISponsorshipService sponsorshipService)
        {
            _sponsorshipService = sponsorshipService;
        }

        // GET ALL

        [HttpGet("editions/{id}")]
        public async Task<IEnumerable<SponsorshipCard>> GetAllByEdition(Guid id) => await _sponsorshipService.GetAllByEditionAsync(id);

        [HttpGet("institutions/{id}")]
        public async Task<IEnumerable<SponsorshipCard>> GetAllByInstitution(Guid id) => await _sponsorshipService.GetAllByInstitutionAsync(id);

        // GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<DTSponsorship?>> GetById(Guid id)
        {
            var dt = (await _sponsorshipService.GetByIdAsync(id)).dt;

            return dt is null ? NotFound() : dt;
        }

        // CREATE

        [HttpPost]
        public async Task<ActionResult<DTSponsorship>> Create(
            DTInsertSponsorship dtInsert,
            [FromServices] IValidator<DTInsertSponsorship> validator)
        {
            var validationResult = await validator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _sponsorshipService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        // DELETE

        [HttpDelete("{id}")]
        public async Task<ActionResult<DTSponsorship?>> Delete(Guid id) 
        {
            var dt = await _sponsorshipService.DeleteAsync(id);

            return dt is null ? NotFound() : dt;
        }
    }
}
