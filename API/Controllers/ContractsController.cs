using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ContractDtos;
using DatabaseLab.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContractsController(
    IContractRepository contractRepository) : ControllerBase
{
    private readonly IContractRepository _contractRepository = contractRepository;

    [HttpPost("create")]
    public async Task<IActionResult> Create(ContractCreateDto contractDto)
    {
        var contract = contractDto.ToEntity();
        var createResult = await _contractRepository.CreateAsync(contract);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Contract>> GetAll()
        => await _contractRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);

        return contract is null ? NotFound() : Ok(contract);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);

        if (contract is null)
        {
            return NotFound();
        }

        var removeResult = await _contractRepository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(long id, ContractCreateDto contractDto)
    {
        var contract = await _contractRepository.GetByIdAsync(id);

        if (contract is null)
        {
            return NotFound();
        }

        var updatedActorData = contractDto.ToEntity(id);
        var updateResult = await _contractRepository.UpdateAsync(updatedActorData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("with-price/{price}")]
    public async Task<IEnumerable<Contract>> GetByPrice(decimal price)
        => await _contractRepository.GetConractsWithPrice(price);

    [HttpGet("with-role/{role}")]
    public async Task<IEnumerable<Contract>> GetByRole(string role)
        => await _contractRepository.GetContractsWithRolePrefix(role);

    [HttpGet("of-author/{actorId}")]
    public async Task<IEnumerable<Contract>> GetByActor(long actorId)
        => await _contractRepository.GetContractsOfActor(actorId);

    [HttpGet("of-spectacle/{spectacleId}")]
    public async Task<IEnumerable<Contract>> GetBySpectacle(long spectacleId)
        => await _contractRepository.GetContractsOfSpectacle(spectacleId);
}
