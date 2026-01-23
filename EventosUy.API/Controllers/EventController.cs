using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.DTOs.Records;
using Microsoft.AspNetCore.Mvc;

namespace EventosUy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly EventInsertValidator _eventInsertValidator;
        private readonly EventUpdateValidator _eventUpdateValidator;

        public EventController(IEventService eventService, EventInsertValidator eventInsertValidator, EventUpdateValidator eventUpdateValidator)
        {
            _eventService = eventService;
            _eventInsertValidator = eventInsertValidator;
            _eventUpdateValidator = eventUpdateValidator;
        }

        // GET ALL

        [HttpGet]
        public async Task<IEnumerable<ActivityCard>> GetAll() => await _eventService.GetAllAsync();

        // GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<DTEvent>> GetById(Guid id) 
        {
            var (dt, validation) = await _eventService.GetByIdAsync(id);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return Ok(dt);
        }

        // CREATE   

        [HttpPost]
        public async Task<ActionResult<DTEvent>> Create(DTInsertEvent dtInsert) 
        {
            var validationResult = await _eventInsertValidator.ValidateAsync(dtInsert);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _eventService.CreateAsync(dtInsert);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return CreatedAtAction(nameof(GetById), new { id = dt!.Id }, dt);
        }

        // UPDATE

        [HttpPut]
        public async Task<ActionResult<DTEvent>> Update(DTUpdateEvent dtUpdate) 
        {
            var validationResult = await _eventUpdateValidator.ValidateAsync(dtUpdate);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

            var (dt, validation) = await _eventService.UpdateAsync(dtUpdate);

            if (!validation.IsValid) { return BadRequest(validation.Errors); }

            return CreatedAtAction(nameof(GetById), new { id = dt!.Id }, dt);
        }

        // DELETE

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id) 
        {
            var dt = await _eventService.DeleteAsync(id);

            return dt is null ? NotFound() : Ok(dt);
        }
    }
}
