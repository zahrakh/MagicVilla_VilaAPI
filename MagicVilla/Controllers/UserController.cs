using System.Net;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.repository.InterfaceRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

[Route("api/UsersAuth")]
[ApiController]
public class UserController(IUserRepository userRepository) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
    {
        var response = new APIResponse();
        var loginResponse = await userRepository.Login(loginRequestDto);
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            response.ErrorMessages.Add("Username or Password is incorrect");
            response.IsSuccess = false;
            response.Status = HttpStatusCode.BadRequest;
            return BadRequest(response);
        }

        response.IsSuccess = true;
        response.Status = HttpStatusCode.OK;
        response.Result = loginResponse;
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
    {
        var response = new APIResponse();
        bool ifUserNameUnique = userRepository.IsUniqueUser(model.UserName);
        if (!ifUserNameUnique)
        {
            response.Status = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages.Add("Username already exists");
            return BadRequest(response);
        }

        var user = await userRepository.Register(model);
        if (user == null)
        {
            response.Status = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages.Add("Error while registering");
            return BadRequest(response);
        }

        response.Status = HttpStatusCode.OK;
        response.IsSuccess = true;
        return Ok(response);
    }
}
