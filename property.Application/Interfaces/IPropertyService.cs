using property.Application.DTOs;
using property.Domain.Entities;

namespace property.Application.Interfaces;

public interface IPropertyService
{
    Task<IEnumerable<Property>> GetAll();
    Task<Property?> GetById(int id);
    Task<Property> Create(Property property,UploadFileDto? image);
    Task<Property> Update(Property property);
    Task Delete(int id);
}