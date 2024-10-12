using DatabaseLab.API.ValidationAttributes;
using DatabaseLab.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab.Domain.Dtos.ActorDetailsDtos;

public class ActorDetailsCreateDto
{
    [Required(ErrorMessage = $"{nameof(ActorId)} is required.")]
    [Range(0, long.MaxValue, ErrorMessage = $"{nameof(ActorId)} must be greater then 0.")]
    public long? ActorId { get; set; }

    [Required(ErrorMessage = $"{nameof(Phone)} is required.")]
    [PhoneNumber]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(Email)} is required.")]
    [Email(10)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(Birthday)} is required.")]
    [Date]
    public string Birthday { get; set; } = string.Empty;

    public ActorDetail ToEntity()
    {
        if (ActorId is null)
            throw new ArgumentNullException(nameof(ActorId));
        
        return new ActorDetail
        {
            ActorId = (long)ActorId,
            Phone = Phone,
            Email = Email,
            Birthday = DateTime.Parse(Birthday)
        };
    }
}
