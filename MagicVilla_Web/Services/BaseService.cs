using System.Net;
using System.Net.Http.Headers;
using System.Text;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _clientFactory;

    public BaseService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<T> SendAsync<T>(APIRequest request)
    {
        try
        {
            var client = _clientFactory.CreateClient("api");

            using var message = new HttpRequestMessage
            {
                RequestUri = new Uri(request.Url),
                Method = request.ApiType switch
                {
                    SD.ApiType.DELETE => HttpMethod.Delete,
                    SD.ApiType.PUT    => HttpMethod.Put,
                    SD.ApiType.POST   => HttpMethod.Post,
                    _                 => HttpMethod.Get
                }
            };

            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (request.Data is not null)
            {
                message.Content = new StringContent(
                    JsonConvert.SerializeObject(request.Data),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            using var response = await client.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();

            // Try to parse as APIResponse first
            if (typeof(T) == typeof(APIResponse))
            {
                return JsonConvert.DeserializeObject<T>(content)!;
            }

            // Otherwise, try to parse directly into the requested type
            return JsonConvert.DeserializeObject<T>(content)!;
        }
        catch (Exception ex)
        {
            // Handle exceptions gracefully with a default APIResponse
            if (typeof(T) == typeof(APIResponse))
            {
                var errorResponse = new APIResponse
                {
                    ErrorMessages = new List<string> { ex.Message },
                    IsSuccess = false,
                    Status = HttpStatusCode.InternalServerError
                };
                return (T)(object)errorResponse;
            }

            throw; // rethrow if caller expected a custom DTO
        }
    }
}
