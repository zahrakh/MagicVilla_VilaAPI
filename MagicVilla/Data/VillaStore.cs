using MagicVilla.Models.Dto;

namespace MagicVilla.Data;

public static class VillaStore
{
    public static List<VillaDTO> villaList =
       [
        new VillaDTO { Id = 1, Name = "Villa 1", Description = "Sea View" },
        new VillaDTO { Id = 2, Name = "Villa 2", Description = "Jungle View" },
        new VillaDTO { Id = 3, Name = "Villa 3", Description = "Without window" }
    ];
}