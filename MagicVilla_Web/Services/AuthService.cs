using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private string? baseUrl;

    public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) :
        base(clientFactory) //todo--> how do we manage different url?
    {
        _clientFactory = clientFactory;
        baseUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
    }


    public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = loginRequestDto,
            Url = baseUrl+"/api/UsersAuth/login"
        });
    }

    public Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = registrationRequestDto,
            Url = baseUrl+"/api/UsersAuth/register"
        });
    }
}