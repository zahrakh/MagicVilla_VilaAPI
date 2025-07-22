using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogging _logget;

    public VillaApiController(ApplicationDbContext db,ILogging logger) 
    {
        _logget = logger;
        _db = db;
    }
    /*Endpoint to get Villa List*/
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        _logget.Log("Get All the Villas","Error");
        return Ok(_db.Villas.ToList());
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
            _logget.Log("Get villa error with id"+villaId,"Info");
            return BadRequest();
        }
    
        var villa = _db.Villas.FirstOrDefault(villa => villa.Id == villaId);
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
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO? villaDTO)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
    
        if (_db.Villas.FirstOrDefault(v => (v.Name).ToUpper() == villaDTO.Name.ToUpper()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa name already exists");
            return BadRequest(ModelState);
        }
    
        if (villaDTO == null)
        {
            return BadRequest(villaDTO);
        }
    
        if (villaDTO.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
            //add mapper manually here
            
        _db.Villas.Add(mapToVilla(villaDTO));
        _db.SaveChanges();
        return CreatedAtRoute("GetVilla", new { villaId = villaDTO.Id }, villaDTO);
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
    
        var vila = _db.Villas.FirstOrDefault(villa => villa.Id == villaId);
        if (vila == null)
        {
            return NotFound();
        }
    
        _db.Villas.Remove(vila);
        _db.SaveChanges();
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
        _db.Villas.Update(mapToVilla(villaDto));
        _db.SaveChanges();
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
    
        var villa = _db.Villas.FirstOrDefault(villa => villa.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }
        VillaDTO villaDto = mapToVilla(villa);
        patch.ApplyTo(villaDto, ModelState);
        _db.Update(mapToVilla(villaDto));
        _db.SaveChanges();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        return NoContent();
    }
    
    
    private Villa mapToVilla(VillaDTO villa)
    {
        Villa model = new()
        {
            Id= villa.Id,
            Name= villa.Name,
            Description= villa.Description,
            ImageUrl= villa.ImageUrl,
            Rate= villa.Rate,
            Amenity= villa.Amenity,
        }; 
       return model; 
    }
    
    private VillaDTO mapToVilla(Villa villa)
    {
        VillaDTO model = new()
        {
            Id= villa.Id,
            Name= villa.Name,
            Description= villa.Description,
            ImageUrl= villa.ImageUrl,
            Rate= villa.Rate,
            Amenity= villa.Amenity,
        }; 
        return model; 
    }
}