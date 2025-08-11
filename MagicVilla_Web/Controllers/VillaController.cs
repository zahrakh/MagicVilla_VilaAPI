using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaService villaService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public VillaController(IVillaService villaService, IMapper mapper, ILogger<VillaController> logger)
    {
        this.villaService = villaService;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDTO> list = new List<VillaDTO>();
        var response = await villaService.GetAllSync<APIResponse>();
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
            var response = await villaService.CrateSync<APIResponse>(model);
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
        var response = await villaService.GetSync<APIResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(mapper.Map<VillaUpdateDTO>(model));
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await villaService.UpdateSync<APIResponse>(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }

        return View(model);
    }

    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await villaService.GetSync<APIResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result)!);
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDTO model)
    {
        {
            var response = await villaService.DeleteSync<APIResponse>(model.Id);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
            return View(model);
        }
    }
}