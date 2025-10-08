using MagicVilla.Models;
using MagicVilla.Models.Dto;

namespace MagicVilla.repository.InterfaceRepository;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto);
    Task<LocalUser?> Register(RegistrationRequestDTO registrationRequestDto);
} 