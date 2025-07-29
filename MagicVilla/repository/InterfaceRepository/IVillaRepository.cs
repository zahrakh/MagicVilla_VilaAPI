using System.Linq.Expressions;
using MagicVilla.Models;
using MagicVilla.Repository.InterfaceRepository;

namespace MagicVilla.repository.InterfaceRepository;

public interface IVillaRepository: IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}