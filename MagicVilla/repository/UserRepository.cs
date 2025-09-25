
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.repository.InterfaceRepository;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla.repository;

public class UserRepository:IUserRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly string _secretKey;

    public UserRepository(ApplicationDbContext db, IConfiguration configuration,IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
        _secretKey= configuration.GetSection("ApiSettings:Secret").Value;
    }
    public bool IsUniqueUser(string username)
    {
         var user= _db.LocalUsers.FirstOrDefault(x=>x.UserName == username);
         return user == null;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        var user= _db.LocalUsers.FirstOrDefault(u=>u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() && u.Password == loginRequestDTO.Password);
        if (user == null)
        {
            return new LoginResponseDTO()
            {
                User = null,
                Token = ""
            };
        }
        //todo
        //check real Senarios!
        //check syntax
        
        
        //generate JWT
        var tokenHandler = new JwtSecurityTokenHandler(); 
        var key= Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            ]),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            User = _mapper.Map<UserDTO>(user)
        };
        return loginResponseDTO;
    }

    public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
    {
        LocalUser user = new()
        {
             UserName = registerationRequestDTO.UserName,
             Password = registerationRequestDTO.Password,
             Name = registerationRequestDTO.Name,
             Role = registerationRequestDTO.Role,
        };
        _db.LocalUsers.Add(user);
        await _db.SaveChangesAsync();
        user.Password = ""; //todo check the all way to handle login system
        return user;
    }
}