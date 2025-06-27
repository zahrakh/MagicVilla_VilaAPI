using MagicVilla.Data;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController: ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        return Ok(VillaStore.villaList);
    }

    [HttpGet("{villaId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult GetVilla(int villaId)
    {
        if (villaId == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(villa => villa.Id == villaId);
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(villa);
    }
}