using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Dtos.SpectacleDtos;
using DatabaseLab.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpectaclesController(
    ISpectacleRepository spectacleRepository) : ControllerBase
{
    private readonly ISpectacleRepository _spectacleRepository = spectacleRepository;

    #region CRUD operations
    [HttpPost("create")]
    public async Task<IActionResult> Create(SpectacleCreateDto spectacleDto)
    {
        var actorEntity = spectacleDto.ToEntity();
        await _spectacleRepository.CreateAsync(actorEntity);

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Spectacle>> GetAll()
        => await _spectacleRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var actor = await _spectacleRepository.GetByIdAsync(id);

        return actor is null ? NotFound() : Ok(actor);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var actor = await _spectacleRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        await _spectacleRepository.RemoveAsync(id);

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(long id, SpectacleCreateDto spectacleDto)
    {
        var actor = await _spectacleRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var updatedActorData = spectacleDto.ToEntity(id);

        await _spectacleRepository.UpdateAsync(updatedActorData);

        return Ok();
    }

    #endregion

    [HttpGet("with-total-info")]
    public async Task<IEnumerable<SpectacleTotalDto>> GetTotalSpectaclesInfo()
        => await _spectacleRepository.GetWithTotalContractPrice();

    [HttpGet("with-actor-agency-name/{spectacleId}")]
    public async Task<IEnumerable<ActorWithAgencyInfo>> GetActorsOfSpecta5cle(long spectacleId)
     => await _spectacleRepository.GetActorsWithAgencyName(spectacleId);
    
    [HttpGet("with-actors-experience")]
    public async Task<IEnumerable<SpectacleWithActors>> GetSpectaclesWithActorsExperience()
        => await _spectacleRepository.GetSpectacleActors();
    
    [HttpGet("with-total-budget")]
    public async Task<IEnumerable<TotalSpectaclesBudget>> GetTotalSpectaclesbudget()
        => await _spectacleRepository.GetSpectaclesWithTotalBudget();
}
