using Backend.Dtos.Inventory;
using Backend.Interfaces;

namespace Backend.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<InventoryResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<InventoryResponseDto?> GetByKeyAsync(string productSku, int storageId) =>
        _repository.GetByKeyAsync(productSku, storageId);

    public Task<IEnumerable<InventoryResponseDto>> GetByProductSkuAsync(string productSku) =>
        _repository.GetByProductSkuAsync(productSku);

    public Task<IEnumerable<InventoryResponseDto>> GetByStorageIdAsync(int storageId) =>
        _repository.GetByStorageIdAsync(storageId);

    public Task<InventoryResponseDto?> UpdateAsync(string productSku, int storageId, InventoryUpdateDto dto) =>
        _repository.UpdateAsync(productSku, storageId, dto);
}
