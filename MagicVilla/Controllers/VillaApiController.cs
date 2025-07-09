using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController(ILogging logger) : ControllerBase
{
    /*Endpoint to get Villa List*/
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        logger.Log("Get All the Villas","Error");
        return Ok(VillaStore.villaList);
    }


    /*Endpoint to get Villa by ID*/
    [HttpGet("{villaId:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult GetVilla(int villaId)
    {
        if (villaId == 0)
        {
            logger.Log("Get villa error with id"+villaId,"Info");
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(villa => villa.Id == villaId);
        if (villa == null)
        {
            return NotFound();
        }

        return Ok(villa);
    }


    /*Endpoint to create Villa */
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO? villa)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        if (VillaStore.villaList.FirstOrDefault(v => (v.Name).ToUpper() == villa.Name.ToUpper()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa name already exists");
            return BadRequest(ModelState);
        }

        if (villa == null)
        {
            return BadRequest(villa);
        }

        if (villa.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        villa.Id = VillaStore.villaList.OrderByDescending(villa => villa.Id).FirstOrDefault()?.Id + 1 ?? 0;
        VillaStore.villaList.Add(villa);
        return CreatedAtRoute("GetVilla", new { villaId = villa.Id }, villa);
    }


    /*Endpoint to Delete Villa */
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{villaId:int}", Name = "DeleteVilla")]
    public ActionResult DeleteVilla(int villaId)
    {
        if (villaId == 0)
        {
            return BadRequest();
        }

        var vila = VillaStore.villaList.FirstOrDefault(villa => villa.Id == villaId);
        if (vila == null)
        {
            return NotFound();
        }

        VillaStore.villaList.Remove(vila);
        return NoContent();
    }


    /*Endpoint to Update Villa */
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    public ActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDto)
    {
        if (villaDto.Id != id)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(villa => villa.Id == id);
        villa.Name = villaDto.Name;
        villa.Description = villaDto.Description;
        return NoContent();
    }


    /*Endpoint to update villa in patch*/
    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patch)
    {
        if (id == 0)
        {
            return BadRequest();
        }
    
        var villa = VillaStore.villaList.FirstOrDefault(villa => villa.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }
    
        patch.ApplyTo(villa, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        return NoContent();
    }
}