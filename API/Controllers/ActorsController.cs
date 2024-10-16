using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace DatabaseLab.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController(
    IActorRepository actorRepository,
    IAgencyRepository agencyRepository) : ControllerBase
{
    private readonly IActorRepository _actorRepository = actorRepository;
    private readonly IAgencyRepository _agencyRepository = agencyRepository;

    #region CRUD operations
    [HttpPost("create")]
    public async Task<IActionResult> Create(ActorCreateDto actorDto)
    {
        var actorEntity = actorDto.ToEntity();
        var createResult = await _actorRepository.CreateAsync(actorEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Actor>> GetAll()
        => await _actorRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);

        return actor is null ? NotFound() : Ok(actor);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var removeResult = await _actorRepository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(long id, ActorCreateDto actorDto)
    {
        var actor = await _actorRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var updatedActorData = actorDto.ToEntity(id);
        var updateResult = await _actorRepository.UpdateAsync(updatedActorData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
    #endregion

    [HttpPatch("add-to-agency/{actorId}/{agencyId}")]
    public async Task<IActionResult> AddToAgency(long actorId, long agencyId)
    {
        var actorTask = _actorRepository.GetByIdAsync(actorId);
        var agencyTask = _agencyRepository.GetByIdAsync(agencyId);

        await Task.WhenAll(actorTask, agencyTask);
        
        var actor = await actorTask;
        var agency = await agencyTask;

        if (actor is null)
        {
            return NotFound();
        }

        if (agency is null)
        {
            return NotFound();
        }

        actor.AgencyId = agencyId;
        var result = await _actorRepository.UpdateAsync(actor);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("with-contract-data/{minAverageConractPrice}")]
    public async Task<IEnumerable<ActorContractInfoDto>> GetActorsWithContractsData(
        decimal minAverageConractPrice)
        => await _actorRepository.GetActorWithContractsInfo(minAverageConractPrice);

    [HttpGet("with-private-data")]
    public async Task<IEnumerable<ActorDataDto>> GetActorsWithPrivateData()
        => await _actorRepository.GetActorsData();
}
