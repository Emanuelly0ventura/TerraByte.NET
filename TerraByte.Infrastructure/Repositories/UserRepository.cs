using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class UserRepository(TerraByteContext context) : IUserRepository
{
    public IReadOnlyCollection<User> FetchAll()
    {
        return context.Users
            .AsNoTracking()
            .ToList();
    }

    public User? FetchById(Guid id)
    {
        return context.Users
            .FirstOrDefault(u => u.Id == id);
    }

    public User? FetchByEmail(string email)
    {
        return context.Users
            .FirstOrDefault(u => u.Email == email);
    }

    public void Create(User user)
    {
        context.Users.Add(user);
    }

    public void Patch(User entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(User user)
    {
        context.Users.Remove(user);
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }
}