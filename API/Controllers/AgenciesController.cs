using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDetailsDtos;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Dtos.AgencyDtos;
using DatabaseLab.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgenciesController(
    IAgencyRepository agencyRepository) : ControllerBase
{
    private readonly IAgencyRepository _agencyRepository = agencyRepository;

    #region CRUD operations
    [HttpPost("create")]
    public async Task<IActionResult> Create(AgencyCreateDto agencyDto)
    {
        var actorEntity = agencyDto.ToEntity();
        var createResult = await _agencyRepository
            .CreateAsync(actorEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Agency>> GetAll()
        => await _agencyRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var actor = await _agencyRepository.GetByIdAsync(id);

        return actor is null ? NotFound() : Ok(actor);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var actor = await _agencyRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var removeResult = await _agencyRepository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(long id, AgencyCreateDto agencyDto)
    {
        var oldAgency = await _agencyRepository.GetByIdAsync(id);

        if (oldAgency is null)
        {
            return NotFound();
        }

        var newAgency = agencyDto.ToEntity(id);
        var updateResult = await _agencyRepository.UpdateAsync(newAgency);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
    #endregion

    [HttpGet("with-actor-groups/{agencyId}")]
    public async Task<IEnumerable<CountOfActorsInRank>> GetActorGroups(long agencyId)
        => await _agencyRepository.GetActorGroups(agencyId);

    [HttpGet("with-max-min-spectacle-budget")]
    public async Task<IEnumerable<AgencyWithSpectacleBudget>> GetMaxMinSpectaclesBudget()
        => await _agencyRepository.GetMaxMinSpectacleBudget();
}
