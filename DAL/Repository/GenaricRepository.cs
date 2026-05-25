using DAL.Data;
using DAL.Models;
using DAL.Shared;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity , new()
{
    private readonly TabibyDbContext _context;

    public GenaricRepository(TabibyDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity> GetByIdAsync(int id)
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

    public void Update(TEntity Entity)
    {
        _context.Set<TEntity>().Update(Entity);
    }

    public void Delete(TEntity Entity)
    {
        _context.Set<TEntity>().Remove(Entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}