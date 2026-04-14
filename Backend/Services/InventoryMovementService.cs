using Backend.Dtos.InventoryMovement;
using Backend.Interfaces;

namespace Backend.Services;

public class InventoryMovementService : IInventoryMovementService
{
    private readonly IInventoryMovementRepository _repository;

    public InventoryMovementService(IInventoryMovementRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<InventoryMovementResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<InventoryMovementResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<IEnumerable<InventoryMovementResponseDto>> GetByProductSkuAsync(string productSku) =>
        _repository.GetByProductSkuAsync(productSku);

    public Task<IEnumerable<InventoryMovementResponseDto>> GetByStorageIdAsync(int storageId) =>
        _repository.GetByStorageIdAsync(storageId);
}
