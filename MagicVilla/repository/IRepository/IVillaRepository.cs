using System.Linq.Expressions;
using MagicVilla.Models;

namespace MagicVilla.repository.IRepository;

public interface IVillaRepository
{
    Task<List<Villa>?> GetAllAsync(Expression<Func<Villa, bool>>? filter = null);
    Task<Villa?> GetAsync(Expression<Func<Villa,bool>>? filter = null, bool track = true);
    Task CreateAsync(Villa entity);
    Task UpdateAsync(Villa entity);
    Task RemoveAsync(Villa entity);
    Task SaveAsync();
}