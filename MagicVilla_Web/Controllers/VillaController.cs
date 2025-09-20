using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaService _VillaService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public VillaController(IVillaService villaService, IMapper mapper, ILogger<VillaController> logger)
    {
        this._VillaService = villaService;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDTO> list = new List<VillaDTO>();
        var response = await _VillaService.GetAllSync<APIResponse>();
        if (response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }

    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _VillaService.CrateSync<APIResponse>(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }

        return View(model);
    }

    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        logger.Log(LogLevel.Information, villaId.ToString(), "Updating villa");
        var response = await _VillaService.GetSync<APIResponse>(villaId);
        if (!response.IsSuccess) return NotFound();
        VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
        return View(mapper.Map<VillaUpdateDTO>(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _VillaService.UpdateSync<APIResponse>(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }

        return View(model);
    }

    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _VillaService.GetSync<APIResponse>(villaId);
        if (!response.IsSuccess) return NotFound();
        VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result)!);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDTO model)
    {
        var response = await _VillaService.DeleteSync<APIResponse>(model.Id);
        if (response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVilla));
        }

        return View(model);
    }
}