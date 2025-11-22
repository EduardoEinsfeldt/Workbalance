using Microsoft.AspNetCore.Mvc;
using Workbalance.Application.Services.MoodEntries;
using Workbalance.Application.Dtos;
using Workbalance.Hateoas;

namespace Workbalance.Controllers
{
    //  V1 - CRUD normal via EF Core

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/mood-entries")]
    [Produces("application/json")]
    public class MoodEntryV1Controller : ControllerBase
    {
        private readonly IMoodEntryService _service;
        private readonly LinkBuilder _links;

        public MoodEntryV1Controller(MoodEntryServiceV1 service, LinkBuilder links)
        {
            _service = service;
            _links = links;
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetAllByUser(Guid userId)
        {
            var result = await _service.GetAllByUserAsync(userId);
            var version = _links.GetApiVersion();

            var list = result.Select(m =>
            {
                var res = new Resource<MoodEntryResponseDto>(m);
                res.Links.Add(_links.Self($"/api/{version}/mood-entries/{m.Id}"));
                res.Links.Add(_links.Action("update", $"/api/{version}/mood-entries/{m.Id}", "PUT"));
                res.Links.Add(_links.Action("delete", $"/api/{version}/mood-entries/{m.Id}", "DELETE"));
                return res;
            });

            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            var version = _links.GetApiVersion();

            var resource = new Resource<MoodEntryResponseDto>(item);
            resource.Links.Add(_links.Self($"/api/{version}/mood-entries/{id}"));
            resource.Links.Add(_links.Action("update", $"/api/{version}/mood-entries/{id}", "PUT"));
            resource.Links.Add(_links.Action("delete", $"/api/{version}/mood-entries/{id}", "DELETE"));

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MoodEntryCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            var version = _links.GetApiVersion();

            var resource = new Resource<MoodEntryResponseDto>(created);
            resource.Links.Add(_links.Self($"/api/{version}/mood-entries/{created.Id}"));

            return Created($"/api/{version}/mood-entries/{created.Id}", resource);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MoodEntryUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var version = _links.GetApiVersion();

            var resource = new Resource<MoodEntryResponseDto>(updated);
            resource.Links.Add(_links.Self($"/api/{version}/mood-entries/{id}"));

            return Ok(resource);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }


    //  V2 - INSERT via Procedure Oracle

    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/{version:apiVersion}/mood-entries")]
    [Produces("application/json")]
    public class MoodEntryV2Controller : ControllerBase
    {
        private readonly IMoodEntryService _service;
        private readonly LinkBuilder _links;

        public MoodEntryV2Controller(MoodEntryServiceV2 service, LinkBuilder links)
        {
            _service = service;
            _links = links;
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetAllByUser(Guid userId)
        {
            var result = await _service.GetAllByUserAsync(userId);
            var version = _links.GetApiVersion();

            var list = result.Select(m =>
            {
                var res = new Resource<MoodEntryResponseDto>(m);
                res.Links.Add(_links.Self($"/api/{version}/mood-entries/{m.Id}"));
                res.Links.Add(_links.Action("update", $"/api/{version}/mood-entries/{m.Id}", "PUT"));
                res.Links.Add(_links.Action("delete", $"/api/{version}/mood-entries/{m.Id}", "DELETE"));
                return res;
            });

            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            var version = _links.GetApiVersion();

            var resource = new Resource<MoodEntryResponseDto>(item);
            resource.Links.Add(_links.Self($"/api/{version}/mood-entries/{id}"));
            resource.Links.Add(_links.Action("update", $"/api/{version}/mood-entries/{id}", "PUT"));
            resource.Links.Add(_links.Action("delete", $"/api/{version}/mood-entries/{id}", "DELETE"));

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MoodEntryCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            var version = _links.GetApiVersion();

            var resource = new Resource<MoodEntryResponseDto>(created);
            resource.Links.Add(_links.Self($"/api/{version}/mood-entries/{created.Id}"));

            return Created($"/api/{version}/mood-entries/{created.Id}", resource);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MoodEntryUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var version = _links.GetApiVersion();

            var resource = new Resource<MoodEntryResponseDto>(updated);
            resource.Links.Add(_links.Self($"/api/{version}/mood-entries/{id}"));

            return Ok(resource);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
