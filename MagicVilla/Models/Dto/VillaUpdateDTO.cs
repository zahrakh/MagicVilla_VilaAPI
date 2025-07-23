using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto;

public class VillaUpdateDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public string? ImageUrl { get; set; }
    [Required]
    public double Rate { get; set; }
    [Required]
    public double Amenity { get; set; }
}