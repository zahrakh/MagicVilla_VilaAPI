using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services.IServices;

public interface IBaseService
{
    public APIResponse ApiResponse { get; set; }
    Task<T> SendAsync<T>(APIRequest request); //check syntax
}