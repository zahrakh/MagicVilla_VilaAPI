using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    
    VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper,
        ILogger<VillaNumberController> logger) {
        _villaNumberService = villaNumberService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNumberDTO> list = new List<VillaNumberDTO>();
        var response = await _villaNumberService.GetAllSync<APIResponse>();
        if (response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result)); 
            //Todo check the syntax
        }
        return View(list);
    }
    
}