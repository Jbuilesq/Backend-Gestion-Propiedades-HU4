namespace property.Domain.Infrastructure;

public interface IRepository <T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}
