using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaService
{
    Task<T> GetAllSync<T>();
    Task<T> GetSync<T>(int id);
    Task<T> CrateSync<T>(VillaCreateDTO dto);
    Task<T> UpdateSync<T>(VillaUpdateDTO dto);
    Task<T> DeleteSync<T>(int id);
}