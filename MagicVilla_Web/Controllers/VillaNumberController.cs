using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public VillaNumberController(
        IVillaNumberService villaNumberService,
        IVillaService villaService,
        ILogger<VillaNumberController> logger,
        IMapper mapper
    )
    {
        _villaNumberService = villaNumberService;
        _villaService = villaService;
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

    [HttpGet]
    public async Task<IActionResult> CreateVillaNumber()
    {
        var vm = new VillaNumberCreateVM();
        var response = await _villaService.GetAllSync<APIResponse>();
        if (response.IsSuccess && response.Result != null)
        {
            vm.VillaList = JsonConvert
                .DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                .Select(it => new SelectListItem
                {
                    Text = it.Name,
                    Value = it.Id.ToString()
                });
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateSync<APIResponse>(model.VillaNumber);
            if (response.IsSuccess && response.ErrorMessages.Count == 0)
            {
                return RedirectToAction(nameof(IndexVillaNumber)); //Todo alternative syntax
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }

        var villaListResponse = await _villaService.GetAllSync<APIResponse>();
        if (villaListResponse.IsSuccess)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(villaListResponse.Result))
                .Select(it => new SelectListItem
                {
                    Text = it.Name,
                    Value = it.Id.ToString()
                });
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        _logger.Log(LogLevel.Information, villaNo.ToString(), "Updating item");
        VillaNumberUpdateVM vm = new();

        var response = await _villaNumberService.GetSync<APIResponse>(villaNo);
        if (response.IsSuccess && response != null)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            vm.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);

            response = await _villaService.GetAllSync<APIResponse>();
            if (response.IsSuccess)
            {
                vm.VillaList = JsonConvert
                    .DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(it => new SelectListItem
                    {
                        Text = it.Name,
                        Value = it.Id.ToString()
                    });
            }

            return View(vm);
        }

        return NotFound();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.UpdateSync<APIResponse>(model.VillaNumber);
            if (response.IsSuccess && (response.ErrorMessages != null && response.ErrorMessages.Count == 0))
            {
                return RedirectToAction(nameof(IndexVillaNumber)); //Todo alternative syntax
            }
            else
            {
                if (response.ErrorMessages != null && response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }

        var villaListResponse = await _villaService.GetAllSync<APIResponse>();
        if (villaListResponse != null && villaListResponse.IsSuccess)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(villaListResponse.Result))
                .Select(it => new SelectListItem
                {
                    Text = it.Name,
                    Value = it.Id.ToString()
                });
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        _logger.Log(LogLevel.Information, villaNo.ToString(), "Updating item");
        VillaNumberDeleteVM vm = new();

        var response = await _villaNumberService.GetSync<APIResponse>(villaNo);
        if (response.IsSuccess && response != null)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            vm.VillaNumber = model;

            response = await _villaService.GetAllSync<APIResponse>();
            if (response.IsSuccess)
            {
                vm.VillaList = JsonConvert
                    .DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(it => new SelectListItem
                    {
                        Text = it.Name,
                        Value = it.Id.ToString()
                    });
            }

            return View(vm);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model) 
    {//todo Implement delete Api 
        var response = await _villaNumberService.DeleteSync<APIResponse>(model.VillaNumber.VillaNo);
        if (response!=null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }

        return View(model);
    }
}