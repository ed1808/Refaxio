namespace Backend.Interfaces;

public interface IService<TId, TCreate, TUpdate, TResponse>
{
    Task<IEnumerable<TResponse>> GetAllAsync();
    Task<TResponse?> GetByIdAsync(TId id);
    Task<TResponse> CreateAsync(TCreate dto);
    Task<TResponse?> UpdateAsync(TId id, TUpdate dto);
    Task<bool> DeleteAsync(TId id);
}
