using AutoMapper;
using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogging _logget;
    private readonly IMapper _imapper;

    public VillaApiController(ApplicationDbContext db, ILogging logger, IMapper mapper)
    {
        _logget = logger;
        _db = db;
        _imapper = mapper;
    }

    /*Endpoint to get Villa List*/
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
        _logget.Log("Get All the Villas", "Error");
        return Ok(_imapper.Map<List<VillaDTO>>(villaList));
    }


    /*Endpoint to get Villa by ID*/
    [HttpGet("{villaId:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VillaDTO>> GetVilla(int villaId)
    {
        if (villaId == 0)
        {
            _logget.Log("Get villa error with id" + villaId, "Info");
            return BadRequest();
        }

        var villa = await _db.Villas.FirstOrDefaultAsync(villa => villa.Id == villaId);
        if (villa == null)
        {
            return NotFound();
        }

        return Ok(_imapper.Map<VillaDTO>(villa));
    }


    /*Endpoint to create Villa */
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO? createDto)
    {
        if (await _db.Villas.FirstOrDefaultAsync(v => (v.Name).ToUpper() == createDto.Name.ToUpper()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa name already exists");
            return BadRequest(ModelState);
        }

        if (createDto == null)
        {
            return BadRequest(createDto);
        }

        var villa = _imapper.Map<Villa>(createDto);
        await _db.Villas.AddAsync(villa);
        await _db.SaveChangesAsync();
        return CreatedAtRoute("GetVilla", new { villaId = villa.Id }, villa);
    }


    /*Endpoint to Delete Villa */
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{villaId:int}", Name = "DeleteVilla")]
    public async Task<ActionResult> DeleteVilla(int villaId)
    {
        if (villaId == 0)
        {
            return BadRequest();
        }

        var vila = await _db.Villas.FirstOrDefaultAsync(villa => villa.Id == villaId);
        if (vila == null)
        {
            return NotFound();
        }

        _db.Villas.Remove(vila);
        await _db.SaveChangesAsync();
        return NoContent();
    }


    /*Endpoint to Update Villa */
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDto)
    {
        if (updateDto.Id != id)
        {
            return BadRequest();
        }

        var villa = _imapper.Map<Villa>(updateDto);
        _db.Villas.Update(villa);
        await _db.SaveChangesAsync();
        return NoContent();
    }


    /*Endpoint to update villa in patch*/
    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patch)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(villa => villa.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }

        VillaUpdateDTO villaUpdateDTO = _imapper.Map<VillaUpdateDTO>(villa);

        patch.ApplyTo(villaUpdateDTO, ModelState);

        Villa model = _imapper.Map<Villa>(villaUpdateDTO);
        _db.Update(model);
        await _db.SaveChangesAsync();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}