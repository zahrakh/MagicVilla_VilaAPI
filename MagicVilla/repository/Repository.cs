using System.Linq.Expressions;
using MagicVilla.Data;
using MagicVilla.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    private DbSet<T> _dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public async Task<List<T>?> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool track = true)
    {
        IQueryable<T> query = _dbSet;
        if (track)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();
    }
    

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}