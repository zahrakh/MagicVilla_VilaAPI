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
    private readonly IMapper _Mapper;
    private readonly ILogger _Logger;

    public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, ILogger<VillaNumberController> logger) {
        _villaNumberService = villaNumberService;
        _Mapper = mapper;
        _Logger = logger;
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

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateSync<APIResponse>(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber)); //Todo alternative syntax
            }
        }
        return View(model);
    }

    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        _Logger.Log(LogLevel.Information, villaNo.ToString(), "Updating villa number");
        var response = await _villaNumberService.GetSync<APIResponse>(villaNo);
        if (response.IsSuccess&& response!=null)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            return View(_Mapper.Map<VillaUpdateDTO>(model));
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.UpdateSync<APIResponse>(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
        }
        return View(model);//todo add break point and debug why we should return the UI?
    }

    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        var response = await _villaNumberService.GetSync<APIResponse>(villaNo);
        if (response.IsSuccess)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            return View(model);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDTO model)
    {
        var response = await _villaNumberService.DeleteSync<APIResponse>(model.VillaNo);
        if (response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        return View(model);
    }


}