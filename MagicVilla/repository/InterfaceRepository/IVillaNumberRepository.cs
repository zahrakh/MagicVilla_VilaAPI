using MagicVilla.Models;
using MagicVilla.Repository.InterfaceRepository;

namespace MagicVilla.repository.InterfaceRepository;

public interface IVillaNumberRepository: IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}