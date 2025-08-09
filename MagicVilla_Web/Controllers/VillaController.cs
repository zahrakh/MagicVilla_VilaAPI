using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers;

public class VillaController:Controller
{
    public IActionResult Index()
    {
        return View();
    }
}