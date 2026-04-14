using Backend.Dtos.Storage;
using Backend.Interfaces;

namespace Backend.Services;

public class StorageService : IStorageService
{
    private readonly IStorageRepository _repository;

    public StorageService(IStorageRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<StorageResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<StorageResponseDto?> GetByIdAsync(int id) =>
        _repository.GetByIdAsync(id);

    public Task<StorageResponseDto> CreateAsync(StorageCreateDto dto) =>
        _repository.CreateAsync(dto);

    public Task<StorageResponseDto?> UpdateAsync(int id, StorageUpdateDto dto) =>
        _repository.UpdateAsync(id, dto);

    public Task<bool> DeleteAsync(int id) =>
        _repository.DeleteAsync(id);
}
