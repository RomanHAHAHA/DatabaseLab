using DatabaseLab.API.ValidationAttributes;
using DatabaseLab.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab.Domain.Dtos.AgencyDtos;

public class AgencyCreateDto
{
    [Required(ErrorMessage = $"{nameof(Name)} is required.")]
    [MaxLength(255, ErrorMessage = "Max length is 255 signs")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(Email)} is required.")]
    [MaxLength(255, ErrorMessage = "Max length is 255 signs")]
    [Email]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(Phone)} is required.")]
    [MaxLength(50, ErrorMessage = "Max length is 50 signs")]
    [PhoneNumber]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(Address)} is required.")]
    [MaxLength(255, ErrorMessage = "Max length is 255 signs")]
    public string Address { get; set; } = string.Empty;

    public Agency ToEntity()
    {
        return new Agency
        {
            Name = Name,
            Email = Email,
            Phone = Phone,
            Address = Address
        };
    }

    public Agency ToEntity(long id)
    {
        var agency = ToEntity();
        agency.Id = id;

        return agency;
    }
}
