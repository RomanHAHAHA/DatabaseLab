using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class ActorCreateDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string MiddleName { get; set; } = string.Empty;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActorRank Rank { get; set; }

    [Required]
    [Range(0, 100)]
    public int Experience { get; set; }

    public long? AgencyId { get; set; }

    public Actor ToEntity()
    {
        return new Actor
        {
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            Rank = Rank,
            Experience = Experience,
            AgencyId = AgencyId
        };
    }

    public Actor ToEntity(long id)
    {
        var actor = ToEntity();
        actor.Id = id;

        return actor;
    }
}
