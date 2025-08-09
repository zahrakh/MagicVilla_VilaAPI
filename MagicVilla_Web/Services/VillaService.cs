using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaService : BaseService, IVillaService
{
    private readonly IHttpClientFactory _clientFactory;
    private string? villaUrl;

    public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) :
        base(clientFactory) //how do we manage different url?
    {
        _clientFactory = clientFactory;
        villaUrl = configuration["ServiceUrls:VillaAPI"];
    }

    public Task<T> GetAllSync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = villaUrl + "/api/villaAPI"
        });
    }

    public Task<T> GetSync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = villaUrl + "/api/villaAPI" + id
        });
    }

    public Task<T> CrateSync<T>(VillaCreateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/api/villaAPI"
        });
    }

    public Task<T> UpdateSync<T>(VillaUpdateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/api/VillaAPI/" + dto.Id
        });
    }

    public Task<T> DeleteSync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = villaUrl + "/api/VillaAPI/" + id
        });
    }
}