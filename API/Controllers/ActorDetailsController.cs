using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDetailsDtos;
using DatabaseLab.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab.API.Controllers;

[Route("api/actor-details")]
[ApiController]
public class ActorDetailsController(
    IRepository<ActorDetail> actorDetailsRepository) : ControllerBase
{
    private readonly IRepository<ActorDetail> _actorDetailsRepository = actorDetailsRepository;

    [HttpPost("create")]
    public async Task<IActionResult> Create(ActorDetailsCreateDto actorDetailsDto)
    {
        var actorDetails = actorDetailsDto.ToEntity();
        var createResult = await _actorDetailsRepository.CreateAsync(actorDetails);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<ActorDetail>> GetAll()
        => await _actorDetailsRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var actor = await _actorDetailsRepository.GetByIdAsync(id);

        return actor is null ? NotFound() : Ok(actor);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var actor = await _actorDetailsRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var removeResult = await _actorDetailsRepository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(
        long id,
        ActorDetailsCreateDto actorDetailsDto)
    {
        var actor = await _actorDetailsRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var updatedActorData = actorDetailsDto.ToEntity();
        var updateResult = await _actorDetailsRepository
            .UpdateAsync(updatedActorData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
}
