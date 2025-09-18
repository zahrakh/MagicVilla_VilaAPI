using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration)
    : BaseService(clientFactory), IVillaNumberService
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private readonly string? _villaNumberUrl = configuration["ServiceUrls:VillaAPI"];

    public Task<T> GetAllSync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = _villaNumberUrl + "/api/VillaNumber"
        });
    }

    public Task<T> GetSync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = _villaNumberUrl + "/api/VillaNumber/" + id
        });
    }

    public Task<T> CreateSync<T>(VillaNumberCreateDTO villaNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = villaNumber,
            Url = _villaNumberUrl + "/api/VillaNumber"
        });
    }

    public Task<T> UpdateSync<T>(VillaNumberUpdateDTO villaNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = villaNumber,
            Url = _villaNumberUrl + "/api/VillaNumber/" + villaNumber.VillaNo
        });
    }

    public Task<T> DeleteSync<T>(int villaNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Data = villaNumber,
            Url = _villaNumberUrl + "/api/VillaNumber/" + villaNumber
        });
    }
}