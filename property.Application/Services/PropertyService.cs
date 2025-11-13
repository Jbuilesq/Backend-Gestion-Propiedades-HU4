using property.Application.Interfaces;
using property.Domain.Entities;
using property.Domain.Infrastructure;

namespace property.Application.Services;

public class PropertyService : IPropertyService
{
    
    private IRepository<Property> _repository;

    public PropertyService(IRepository<Property> propertyService)
    {
        _repository = propertyService;
    }
    
    
    public async Task<IEnumerable<Property>> GetAll()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Property?> GetById(int id)
    {
        return  await _repository.GetAByIdAsync(id);
    }

    public async Task<Property> Create(Property property)
    {
        if (string.IsNullOrWhiteSpace(property.Title)) 
            throw new ArgumentNullException("The property title cannot be null or empty.");
        if (property.Price <= 0)
            throw new ArgumentException("El precio debe ser mayor a 0.");
        
        return await _repository.CreateAsync(property);
        
    }

    public async Task<Property> Update(Property property)
    {
        var existing = await _repository.GetAByIdAsync(property.Id);
        if (existing == null)
            throw new KeyNotFoundException("Property not found.");

        existing.Title = property.Title;
        existing.Description = property.Description;
        existing.Price = property.Price;
        existing.Localization = property.Localization;
        existing.ImagesJson = property.ImagesJson;

        return await _repository.UpdateAsync(existing);
    }

    public async Task Delete(int id)
    {
        var existing = await _repository.GetAByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException("Property not found.");

        await _repository.DeleteAsync(existing);
    }
}