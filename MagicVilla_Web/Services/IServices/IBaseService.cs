using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services.IServices;

public interface IBaseService
{
    Task<T> SendAsync<T>(APIRequest request); //check syntax
}