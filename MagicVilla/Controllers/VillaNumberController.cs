using System.Net;
using AutoMapper;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.repository.InterfaceRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

[Route("api/VillaNumber")]
[ApiController]
public class VillaNumberController:ControllerBase
{
    private readonly IVillaNumberRepository _repository;
    private readonly ILogging _logget;
    private readonly IMapper _imapper;

    public VillaNumberController(IVillaNumberRepository repository, ILogging log, IMapper imapper)
    {
        _repository = repository;
        _logget = log;
        _imapper = imapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> VillaNumbers()
    {
        var response = new APIResponse();
        try
        {
            IEnumerable<VillaNumber>? villaNumbers = await _repository.GetAllAsync(includeProperty:"Villa");
            response.Result= _imapper.Map<IEnumerable<VillaNumberDTO>>(villaNumbers);
            response.Status = HttpStatusCode.OK;
            response.IsSuccess=true;
            return Ok(response);
        }
        catch (Exception e)
        {
           response.IsSuccess = false;
           response.ErrorMessages = [e.Message];
        }
        return Ok(response);
    }


    [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaNumberDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
    {
        var response = new APIResponse();
        try
        {
            if (villaNo == 0)
            {
                response.Status = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            var villa = await _repository.GetAsync(villaNumber=> villaNumber.VillaNo==villaNo);
            if (villa == null)
            {
                response.Status = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            response.Result = _imapper.Map<VillaNumberDTO>(villa);
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
        return Ok(response);
    }
    
   
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO? createDto)
    {
        var response = new APIResponse();
        try
        {
            if (await _repository.GetAsync(v => v.VillaNo == createDto.VillaNo) != null)
            {
                ModelState.AddModelError("ErrorMessages", "item already exists");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            var villaNumber = _imapper.Map<VillaNumber>(createDto);
            await _repository.CreateAsync(villaNumber);
            response.Result = _imapper.Map<VillaDTO>(villaNumber);
            response.Status = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { villaId = villaNumber.VillaNo }, villaNumber);
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
    
    
    [HttpPut("{villaNo:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, [FromBody] VillaNumberUpdateDTO? updateDto)
    {
        var response = new APIResponse();
        try
        {
            if (updateDto.VillaNo != villaNo)
            {
                response.Status = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }
            var villa= _imapper.Map<VillaNumber>(updateDto);
            await _repository.UpdateAsync(villa);
            response.Status = HttpStatusCode.NoContent;
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages =[e.Message];
        }
        return Ok(response);
    }
    
}
