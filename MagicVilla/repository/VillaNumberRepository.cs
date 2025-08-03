using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.repository.InterfaceRepository;

namespace MagicVilla.repository;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private readonly ApplicationDbContext _db;
    public VillaNumberRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdatedTime= DateTime.UtcNow;
        _db.VillaNumbers.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
} 