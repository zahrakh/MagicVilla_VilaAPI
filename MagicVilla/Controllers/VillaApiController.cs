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
    private APIResponse _response; //todo--> is it allowed to have one global response?

    public VillaApiController(IVillaRepository villaRepository, ILogging logger, IMapper mapper)
    {
        _logget = logger;
        _repository = villaRepository;
        _imapper = mapper;
        _response = new APIResponse();
    }

    /*Endpoint to get Villa List*/
    [HttpGet(Name = "GetAllVillas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            IEnumerable<Villa>? villaList = await _repository.GetAllAsync();
            _logget.Log("Get All the Villas", "info");
            _response.Result = _imapper.Map<List<VillaDTO>>(villaList);
            _response.Status = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>()
            {
                e.Message
            };
        }

        return Ok(_response);
    }


    /*Endpoint to get Villa by ID*/
    [HttpGet("{villaId:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetVilla(int villaId)
    {
        try
        {
            if (villaId == 0)
            {
                _logget.Log("Get villa error with id" + villaId, "Info");
                _response.Status = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _repository.GetAsync(villa => villa.Id == villaId);
            if (villa == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _imapper.Map<VillaDTO>(villa);
            _response.Status = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>()
            {
                e.Message
            };
        }

        return _response;
    }


    /*Endpoint to create Villa */
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO? createDto)
    {
        try
        {
            if (await _repository.GetAsync(v => (v.Name).ToUpper() == createDto.Name.ToUpper()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa name already exists");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            var villa = _imapper.Map<Villa>(createDto);
            await _repository.CreateAsync(villa);
            _response.Result = _imapper.Map<VillaDTO>(villa);
            _response.Status = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { villaId = villa.Id }, villa);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>()
            {
                e.Message
            };
        }

        return _response;
    }


    /*Endpoint to Delete Villa */
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{villaId:int}", Name = "DeleteVilla")]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int villaId)
    {
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
            _response.Status = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = [e.Message];
        }

        return _response;
    }


    /*Endpoint to Update Villa */
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDto)
    {
        try
        {
            if (updateDto.Id != id)
            {
                _response.Status = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = _imapper.Map<Villa>(updateDto);
            await _repository.UpdateAsync(villa);
            _response.Status = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = [e.Message];
        }

        return _response;
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