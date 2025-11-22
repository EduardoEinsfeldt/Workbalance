using Microsoft.AspNetCore.Mvc;
using Workbalance.Application.Services.Recommendations;
using Workbalance.Application.Dtos;
using Workbalance.Hateoas;

namespace Workbalance.Controllers
{
    //  V1 - CRUD via EF Core

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/recommendations")]
    [Produces("application/json")]
    public class RecommendationV1Controller : ControllerBase
    {
        private readonly IRecommendationService _service;
        private readonly LinkBuilder _links;

        public RecommendationV1Controller(RecommendationServiceV1 service, LinkBuilder links)
        {
            _service = service;
            _links = links;
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetAllByUser(Guid userId)
        {
            var result = await _service.GetAllByUserAsync(userId);
            var version = _links.GetApiVersion();

            var list = result.Select(r =>
            {
                var res = new Resource<RecommendationResponseDto>(r);
                res.Links.Add(_links.Self($"/api/{version}/recommendations/{r.Id}"));
                res.Links.Add(_links.Action("update", $"/api/{version}/recommendations/{r.Id}", "PUT"));
                res.Links.Add(_links.Action("delete", $"/api/{version}/recommendations/{r.Id}", "DELETE"));
                return res;
            });

            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();

            var version = _links.GetApiVersion();

            var resource = new Resource<RecommendationResponseDto>(result);
            resource.Links.Add(_links.Self($"/api/{version}/recommendations/{id}"));
            resource.Links.Add(_links.Action("update", $"/api/{version}/recommendations/{id}", "PUT"));
            resource.Links.Add(_links.Action("delete", $"/api/{version}/recommendations/{id}", "DELETE"));

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecommendationCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            var version = _links.GetApiVersion();

            var resource = new Resource<RecommendationResponseDto>(created);
            resource.Links.Add(_links.Self($"/api/{version}/recommendations/{created.Id}"));

            return Created($"/api/{version}/recommendations/{created.Id}", resource);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecommendationUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var version = _links.GetApiVersion();

            var resource = new Resource<RecommendationResponseDto>(updated);
            resource.Links.Add(_links.Self($"/api/{version}/recommendations/{id}"));

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


    //  V2 - INSERT via PROCEDURE Oracle

    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/{version:apiVersion}/recommendations")]
    [Produces("application/json")]
    public class RecommendationV2Controller : ControllerBase
    {
        private readonly IRecommendationService _service;
        private readonly LinkBuilder _links;

        public RecommendationV2Controller(RecommendationServiceV2 service, LinkBuilder links)
        {
            _service = service;
            _links = links;
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetAllByUser(Guid userId)
        {
            var result = await _service.GetAllByUserAsync(userId);
            var version = _links.GetApiVersion();

            var list = result.Select(r =>
            {
                var res = new Resource<RecommendationResponseDto>(r);
                res.Links.Add(_links.Self($"/api/{version}/recommendations/{r.Id}"));
                res.Links.Add(_links.Action("update", $"/api/{version}/recommendations/{r.Id}", "PUT"));
                res.Links.Add(_links.Action("delete", $"/api/{version}/recommendations/{r.Id}", "DELETE"));
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

            var resource = new Resource<RecommendationResponseDto>(item);
            resource.Links.Add(_links.Self($"/api/{version}/recommendations/{id}"));
            resource.Links.Add(_links.Action("update", $"/api/{version}/recommendations/{id}", "PUT"));
            resource.Links.Add(_links.Action("delete", $"/api/{version}/recommendations/{id}", "DELETE"));

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecommendationCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            var version = _links.GetApiVersion();

            var resource = new Resource<RecommendationResponseDto>(created);
            resource.Links.Add(_links.Self($"/api/{version}/recommendations/{created.Id}"));

            return Created($"/api/{version}/recommendations/{created.Id}", resource);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecommendationUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var version = _links.GetApiVersion();

            var resource = new Resource<RecommendationResponseDto>(updated);
            resource.Links.Add(_links.Self($"/api/{version}/recommendations/{id}"));

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
