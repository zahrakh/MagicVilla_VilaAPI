using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

interface IVillaNumberService
{
    Task<T> GetAllSync<T>();
    Task<T> GetSync<T>(int id);
    Task<T> CreateSync<T>(VillaNumberCreateDTO villaNumber);
    Task<T> UpdateSync<T>(VillaNumberUpdateDTO villaNumber);
    Task<T> DeleteSync<T>(int villaNumber);
}