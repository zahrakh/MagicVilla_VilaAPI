using System.Text;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Services;

public class BaseService : IBaseService
{
    public APIResponse ApiResponse { get; set; }
    public IHttpClientFactory ClientFactory { get; set; }

    BaseService(IHttpClientFactory clientFactory)
    {
        ClientFactory = clientFactory;
        ApiResponse = new APIResponse();
    }

    public async Task<T> SendAsync<T>(APIRequest request)
    {
        try
        {
            var client = ClientFactory.CreateClient("api");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("accept", "application/json");
            message.RequestUri = new Uri(request.Url);
            if (request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8,
                    "application/json");
            }

            switch (request.ApiType)
            {
                case SD.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                case SD.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case SD.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage response = null;
            response = await client.SendAsync(message);
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return apiResponse;
        }
        catch (Exception e)
        {
            var dto = new APIResponse()
            {
                ErrorMessages = [e.Message],
                IsSuccess = false,
            };
            var res= JsonConvert.SerializeObject(dto);
            var apiResponse = JsonConvert.DeserializeObject<T>(res);
            return apiResponse;
        }
        
    }
}