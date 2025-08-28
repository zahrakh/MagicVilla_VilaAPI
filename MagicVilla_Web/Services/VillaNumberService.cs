using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaNumberService : BaseService, IVillaNumberService
{
    private readonly IHttpClientFactory _clientFactory;
    private string? villaNumberUrl;

    public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        villaNumberUrl = configuration["ServiceUrls:VillaAPI"];
    }

    public Task<T> GetAllSync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaNumberUrl + "api/VillaNumber/"
        });
    }

    public Task<T> GetSync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaNumberUrl + "api/VillaNumber/" + id
        });
    }

    public Task<T> CreateSync<T>(VillaNumberCreateDTO villaNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = villaNumber,
            Url = villaNumberUrl + "api/VillaNumber/"
        });
    }

    public Task<T> UpdateSync<T>(VillaNumberUpdateDTO villaNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = villaNumber,
            Url = villaNumberUrl + "api/VillaNumber/" + villaNumber.VillaNo
        });
    }

    public Task<T> DeleteSync<T>(int villaNumber)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Data = villaNumber,
            Url = villaNumberUrl + "api/VillaNumber/" + villaNumber
        });
    }
}