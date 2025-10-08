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

public class UserRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper) : IUserRepository
{
    private readonly string? _secretKey = configuration.GetSection("ApiSettings:Secret").Value;

    public bool IsUniqueUser(string username)
    {
        var user = db.LocalUsers.FirstOrDefault(x => x.UserName == username);
        return user == null;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
    {
        var user = db.LocalUsers.FirstOrDefault(user =>
            user.UserName.ToLower() == loginRequestDto.UserName.ToLower() && user.Password == loginRequestDto.Password);
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
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            ]),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDTO loginResponseDto = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            User = mapper.Map<UserDTO>(user)
        };
        return loginResponseDto;
    }

    public async Task<LocalUser?> Register(RegistrationRequestDTO registrationRequestDto)
    {
        LocalUser user = new()
        {
            UserName = registrationRequestDto.UserName,
            Password = registrationRequestDto.Password,
            Name = registrationRequestDto.Name,
            Role = registrationRequestDto.Role,
        };
        db.LocalUsers.Add(user);
        await db.SaveChangesAsync();
        user.Password = ""; //todo check the all way to handle login system
        return user;
    }
}