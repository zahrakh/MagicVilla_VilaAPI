using System.Net;
using AutoMapper;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.repository.InterfaceRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController : ControllerBase
{
    private readonly IVillaRepository _repository;
    private readonly ILogging _logget;
    private readonly IMapper _imapper;

    public VillaApiController(IVillaRepository villaRepository, ILogging logger, IMapper mapper)
    {
        _logget = logger;
        _repository = villaRepository;
        _imapper = mapper;
    }

    /*Endpoint to get Villa List*/
    [HttpGet(Name = "GetAllVillas")]
    [ResponseCache(Duration = 30)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        var response = new APIResponse();
        try
        {
            IEnumerable<Villa>? villaList = await _repository.GetAllAsync();
            _logget.Log("Get All the Villas", "info");
            response.Result = _imapper.Map<List<VillaDTO>>(villaList);
            response.Status = HttpStatusCode.OK;
            response.IsSuccess = true;
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>()
            {
                e.Message
            };
        }

        return Ok(response);
    }


    /*Endpoint to get Villa by ID*/
    [HttpGet("{villaId:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetVilla(int villaId)
    {
        var response = new APIResponse();
        try
        {
            if (villaId == 0)
            {
                _logget.Log("Get villa error with id" + villaId, "Info");
                response.Status = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            var villa = await _repository.GetAsync(villa => villa.Id == villaId);
            if (villa == null)
            {
                response.Status = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            response.Result = _imapper.Map<VillaDTO>(villa);
            response.Status = HttpStatusCode.OK;
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>()
            {
                e.Message
            };
        }

        return response;
    }


    /*Endpoint to create Villa */
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO? createDto)
    {
        var response = new APIResponse();
        try
        {
            if (await _repository.GetAsync(v => (v.Name).ToUpper() == createDto.Name.ToUpper()) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Place name already exists");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            var villa = _imapper.Map<Villa>(createDto);
            await _repository.CreateAsync(villa);
            response.Result = _imapper.Map<VillaDTO>(villa);
            response.Status = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { villaId = villa.Id }, villa);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>()
            {
                e.Message
            };
        }

        return response;
    }


    /*Endpoint to Delete Villa */
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{villaId:int}", Name = "DeleteVilla")]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int villaId)
    {
        var response = new APIResponse();
        try
        {
            if (villaId == 0)
            {
                return BadRequest();
            }

            var vila = await _repository.GetAsync(villa => villa.Id == villaId);
            if (vila == null)
            {
                return NotFound();
            }

            await _repository.RemoveAsync(vila);
            response.Status = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = [e.Message];
        }

        return response;
    }


    /*Endpoint to Update Villa */
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDto)
    {
        var response = new APIResponse();
        try
        {
            if (updateDto.Id != id)
            {
                response.Status = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            var villa = _imapper.Map<Villa>(updateDto);
            await _repository.UpdateAsync(villa);
            response.Status = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = [e.Message];
        }

        return response;
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

        var villa = await _repository.GetAsync(villa => villa.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }

        VillaUpdateDTO villaUpdateDTO = _imapper.Map<VillaUpdateDTO>(villa);

        patch.ApplyTo(villaUpdateDTO, ModelState);

        var model = _imapper.Map<Villa>(villaUpdateDTO);
        await _repository.UpdateAsync(model);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}
