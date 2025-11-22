using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Workbalance.Application.Services.Users;
using Workbalance.Application.Dtos;
using Workbalance.Hateoas;

namespace Workbalance.Controllers;

[ApiController]
[Route("api/{version:apiVersion}/users")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly UserServiceV1 _v1;
    private readonly UserServiceV2 _v2;
    private readonly LinkBuilder _links;

    public UserController(UserServiceV1 v1, UserServiceV2 v2, LinkBuilder links)
    {
        _v1 = v1;
        _v2 = v2;
        _links = links;
    }

    //               VERSION 1.0

    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAllV1()
    {
        var list = await _v1.GetAllAsync();
        var version = _links.GetApiVersion();

        var response = list.Select(u =>
        {
            var res = new Resource<UserResponseDto>(u);
            res.Links.Add(_links.Self($"/api/{version}/users/{u.Id}"));
            res.Links.Add(_links.Action("update", $"/api/{version}/users/{u.Id}", "PUT"));
            res.Links.Add(_links.Action("delete", $"/api/{version}/users/{u.Id}", "DELETE"));
            return res;
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetByIdV1(Guid id)
    {
        var user = await _v1.GetByIdAsync(id);
        if (user is null) return NotFound();

        var version = _links.GetApiVersion();

        var resource = new Resource<UserResponseDto>(user);
        resource.Links.Add(_links.Self($"/api/{version}/users/{id}"));
        resource.Links.Add(_links.Action("update", $"/api/{version}/users/{id}", "PUT"));
        resource.Links.Add(_links.Action("delete", $"/api/{version}/users/{id}", "DELETE"));

        return Ok(resource);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> CreateV1([FromBody] UserCreateDto dto)
    {
        var created = await _v1.CreateAsync(dto);
        var version = _links.GetApiVersion();

        var resource = new Resource<UserResponseDto>(created);
        resource.Links.Add(_links.Self($"/api/{version}/users/{created.Id}"));

        return Created($"/api/{version}/users/{created.Id}", resource);
    }

    [HttpPut("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateV1(Guid id, [FromBody] UserUpdateDto dto)
    {
        var updated = await _v1.UpdateAsync(id, dto);
        if (updated is null) return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteV1(Guid id)
    {
        var ok = await _v1.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    //               VERSION 2.0

    [HttpGet]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetAllV2()
    {
        var list = await _v2.GetAllAsync();
        var version = _links.GetApiVersion();

        var response = list.Select(u =>
        {
            var res = new Resource<UserResponseDto>(u);
            res.Links.Add(_links.Self($"/api/{version}/users/{u.Id}"));
            res.Links.Add(_links.Action("update", $"/api/{version}/users/{u.Id}", "PUT"));
            res.Links.Add(_links.Action("delete", $"/api/{version}/users/{u.Id}", "DELETE"));
            return res;
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetByIdV2(Guid id)
    {
        var user = await _v2.GetByIdAsync(id);
        if (user is null) return NotFound();

        var version = _links.GetApiVersion();

        var resource = new Resource<UserResponseDto>(user);
        resource.Links.Add(_links.Self($"/api/{version}/users/{id}"));
        resource.Links.Add(_links.Action("update", $"/api/{version}/users/{id}", "PUT"));
        resource.Links.Add(_links.Action("delete", $"/api/{version}/users/{id}", "DELETE"));

        return Ok(resource);
    }

    [HttpPost]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> CreateV2([FromBody] UserCreateDto dto)
    {
        var created = await _v2.CreateAsync(dto);
        var version = _links.GetApiVersion();

        var resource = new Resource<UserResponseDto>(created);
        resource.Links.Add(_links.Self($"/api/{version}/users/{created.Id}"));

        return Created($"/api/{version}/users/{created.Id}", resource);
    }

    [HttpPut("{id:guid}")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> UpdateV2(Guid id, [FromBody] UserUpdateDto dto)
    {
        var updated = await _v2.UpdateAsync(id, dto);
        if (updated == null) return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> DeleteV2(Guid id)
    {
        var ok = await _v2.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
