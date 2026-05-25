using DAL.Models;
using DAL.Shared;

namespace DAL.Repository;

public interface IGenaricRepository<TEntity> where TEntity : BaseEntity , new()
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity user);
    void Update(TEntity user);
    void Delete(TEntity user);
}