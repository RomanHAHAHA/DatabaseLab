using DatabaseLab.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab.Domain.Dtos.SpectacleDtos;

public class SpectacleCreateDto
{
    [Required(ErrorMessage = $"{nameof(Name)} is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(ProductionDate)} is required.")]
    [Range(0, 2050)]
    public int? ProductionDate { get; set; }

    [Required(ErrorMessage = $"{nameof(Budget)} is required.")]
    [Range(0, 1000000)]
    public decimal? Budget { get; set; }

    public Spectacle ToEntity()
    {
        if (ProductionDate is null)
            throw new ArgumentNullException(nameof(ProductionDate));

        if (Budget is null)
            throw new ArgumentNullException(nameof(Budget));

        return new Spectacle
        {
            Name = Name,
            ProductionDate = (int)ProductionDate,
            Budget = (decimal)Budget
        };
    }

    public Spectacle ToEntity(long id)
    {
        var spectacle = ToEntity(); 
        spectacle.Id = id;

        return spectacle;
    }
}
