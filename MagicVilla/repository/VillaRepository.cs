using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.repository.InterfaceRepository;

namespace MagicVilla.repository;

public class VillaRepository:  Repository<Villa>, IVillaRepository
{
    private readonly ApplicationDbContext _db;

    public VillaRepository(ApplicationDbContext db):base(db)// to pass it to parent constructor
    {
        _db = db;
    }

    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _db.Villas.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}