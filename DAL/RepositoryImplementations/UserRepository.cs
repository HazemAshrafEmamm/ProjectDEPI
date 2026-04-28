using DAL.Data;
using DAL.Models;
using DAL.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.RepositoryImplementations;

public class UserRepository : IUserRepository
{
    private readonly TabibyDbContext _context;

    public UserRepository(TabibyDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        user.IsActive = false;
        _context.Users.Update(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}