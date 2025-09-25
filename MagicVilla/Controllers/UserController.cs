using System.Net;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.repository.InterfaceRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

[Route("api/UserAuth")]
[ApiController]
public class UserController(IUserRepository userRepository) : Controller
{
    private APIResponse _response = new();


    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
    {
        var loginResponse = await userRepository.Login(loginRequestDto);
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _response.ErrorMessages.Add("Username or Password is incorrect");
            _response.IsSuccess = false;
            _response.Status = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.Status = HttpStatusCode.OK;
        _response.Result = loginResponse;
        return Ok(_response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
    {
        bool ifUserNameUnique = userRepository.IsUniqueUser(model.UserName);
        if (!ifUserNameUnique)
        {
            _response.Status = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Username already exists");
            return BadRequest(_response);
        }

        var user = await userRepository.Register(model);
        if (user == null)
        {
            _response.Status = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Error while registering");
            return BadRequest(_response);
        }

        _response.Status = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}