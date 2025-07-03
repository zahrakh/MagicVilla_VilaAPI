using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto;

public class VillaDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CreatedDate { get; set; }
}