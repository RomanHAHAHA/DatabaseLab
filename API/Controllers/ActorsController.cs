using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController(
    IActorRepository actorRepository) : ControllerBase
{
    private readonly IActorRepository _actorRepository = actorRepository;

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

    #region Lab1 requests
    [HttpGet("get-by-prefix/{prefix}")]
    public async Task<IEnumerable<Actor>> GetByInitialsPrefix(string prefix)
        => await _actorRepository.GetBySurnamePrefix(prefix);

    [HttpGet("get-by-experience/{experience}")]
    public async Task<IEnumerable<Actor>> GetByExperience(int experience)
        => await _actorRepository.GetActorsWithExperience(experience);

    [HttpGet("get-by-rank/{rank}")]
    public async Task<IEnumerable<Actor>> GetByRank(ActorRank rank)
        => await _actorRepository.GetActorsWithRank(rank);
    #endregion

    #region Lab2 requests
    [HttpGet("with-contract-data/{minAverageConractPrice}")]
    public async Task<IEnumerable<ActorContractInfoDto>> GetActorsWithContractsData(
        decimal minAverageConractPrice)
        => await _actorRepository.GetActorWithContractsInfo(minAverageConractPrice);

    [HttpGet("with-private-data")]
    public async Task<IEnumerable<ActorDataDto>> GetActorsWithPrivateData()
        => await _actorRepository.GetActorsData();

    #endregion
}
