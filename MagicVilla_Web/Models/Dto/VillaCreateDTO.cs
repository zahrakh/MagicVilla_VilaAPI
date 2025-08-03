using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto;

public class VillaCreateDTO
{
    
    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public double Rate { get; set; }
    public double Amenity { get; set; }
}