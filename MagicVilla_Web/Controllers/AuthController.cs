using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public AuthController(IAuthService authService, IMapper mapper, ILogger<AuthController> logger)
    {
        this._authService = authService;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDTO loginRequestDto = new LoginRequestDTO();
        return View(loginRequestDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] //todo--> what is it for?
    public async Task<IActionResult> Login(LoginRequestDTO loginRequestDto)
    {
        APIResponse response = await _authService.LoginAsync<APIResponse>(loginRequestDto);
        if (response != null && response.IsSuccess)
        {
            LoginResponseDTO loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result)); 
            HttpContext.Session.SetString(SD.SessionToken,loginResponseDto?.Token); 
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("ErrorMessages", "Invalid login attempt.");
            return View(loginRequestDto);
        }
      
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegistrationRequestDTO registrationRequestDto)
    {
        APIResponse result = await _authService.RegisterAsync<APIResponse>(registrationRequestDto);
        if (result.IsSuccess && result != null)
        {
            RedirectToAction("Login");
        }

        return View(registrationRequestDto);
    }

    public async Task<IActionResult> Logout()
    {
       await HttpContext.SignOutAsync();
       HttpContext.Session.SetString(SD.SessionToken,""); 
       return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}