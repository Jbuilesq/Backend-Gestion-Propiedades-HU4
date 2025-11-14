using Microsoft.EntityFrameworkCore;
using property.Domain.Entities;
using property.Domain.Infrastructure;
using property.Infrastructure.Data;

namespace property.Infrastructure.Repositories;

public class PropertiesRepository : IRepository<Property>
{
    private readonly AppDbContext _propietyContext;

    public PropertiesRepository(AppDbContext dbContext)
    {
        _propietyContext = dbContext;
    }
    
    
    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _propietyContext.Properties.ToListAsync();
    }

    public async Task<Property> GetAByIdAsync(int id)
    {
        return await _propietyContext.Properties.FindAsync(id);
    }

    public async Task<Property> CreateAsync(Property entity)
    {
        _propietyContext.Properties.Add(entity);
        await _propietyContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Property> UpdateAsync(Property entity)
    {
        _propietyContext.Update(entity);
         await _propietyContext.SaveChangesAsync();
         return entity;
         
    }

    public async Task<bool> DeleteAsync(Property entity)
    {
        _propietyContext.Properties.Remove(entity);
        await _propietyContext.SaveChangesAsync();
        return true;
    }
}