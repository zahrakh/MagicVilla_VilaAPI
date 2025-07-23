using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;

namespace MagicVilla;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>();
        CreateMap<VillaDTO, Villa>();
        
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
    }
}