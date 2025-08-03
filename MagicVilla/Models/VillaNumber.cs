using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Models;

public class VillaNumber
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int VillaNo { get; set; }
    [ForeignKey("Villa")]
    public int VillaID { get; set; }
    public Villa Villa { get; set; }
    public string? SpecialDetails { get; set; }
    public DateTime UpdatedTime { get; set; }
    public DateTime CreatedTime { get; set; }
}