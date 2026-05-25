using DAL.Data;
using DAL.Shared;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly TabibyDbContext _context;

    public GenaricRepository(TabibyDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByIdAsync(string id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}