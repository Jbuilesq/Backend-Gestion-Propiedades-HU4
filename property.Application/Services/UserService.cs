using property.Application.Interfaces;
using property.Domain.Entities;
using property.Domain.Infrastructure;


namespace property.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        if (id == null)
            throw new ArgumentNullException("El id del usuario es null");

        var user = await _repository.GetAByIdAsync(id);
        return user;
    }
}