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
    private APIResponse _response;

    public VillaNumberController(IVillaNumberRepository repository, ILogging log, IMapper imapper)
    {
        _repository = repository;
        _logget = log;
        _imapper = imapper;
        _response = new APIResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> VillaNumbers()
    {
        try
        {
            IEnumerable<VillaNumber>? villaNumbers = await _repository.GetAllAsync(includeProperty:"Villa");
            _response.Result= _imapper.Map<IEnumerable<VillaNumberDTO>>(villaNumbers);
            _response.Status = HttpStatusCode.OK;
            _response.IsSuccess=true;
            return Ok(_response);
        }
        catch (Exception e)
        {
           _response.IsSuccess = false;
           _response.ErrorMessages = [e.Message];
        }
        return Ok(_response);
    }


    [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaNumberDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
    {
        try
        {
            if (villaNo == 0)
            {
                _response.Status = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villa = await _repository.GetAsync(villaNumber=> villaNumber.VillaNo==villaNo);
            if (villa == null)
            {
                _response.Status = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = _imapper.Map<VillaNumberDTO>(villa);
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
        return Ok(_response);
    }
    
   
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO? createDto)
    {
        try
        {
            if (await _repository.GetAsync(v => v.VillaNo == createDto.VillaNo) != null)
            {
                ModelState.AddModelError("ErrorMessages", "item already exists");
                return BadRequest(ModelState);
                //todo:
                //what is the best way to handle the return response 
                //1-return BadRequest(ModelState);
                //2-return BadRequest(response(isSuccess=false)); how to return error message and wrong validation
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            var villaNumber = _imapper.Map<VillaNumber>(createDto);
            await _repository.CreateAsync(villaNumber);
            _response.Result = _imapper.Map<VillaDTO>(villaNumber);
            _response.Status = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { villaId = villaNumber.VillaNo }, villaNumber);
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
    
    
    [HttpPut("{villaNo:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, [FromBody] VillaNumberUpdateDTO? updateDto)
    {
        try
        {
            if (updateDto.VillaNo != villaNo)
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var villa= _imapper.Map<VillaNumber>(updateDto);
            await _repository.UpdateAsync(villa);
            _response.Status = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages =[e.Message];
        }
        return Ok(_response);
    }
    
}