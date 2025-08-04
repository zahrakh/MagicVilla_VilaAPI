using AutoMapper;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web;

public class MappingConfig:Profile
{
    public MappingConfig()
    {
        //Villa 
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
        //VillaNumber
        CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
        CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
    }
}