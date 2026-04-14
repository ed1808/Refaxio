using Backend.Dtos.InventoryMovement;

namespace Backend.Interfaces;

public interface IInventoryMovementService
{
    Task<IEnumerable<InventoryMovementResponseDto>> GetAllAsync();
    Task<InventoryMovementResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<InventoryMovementResponseDto>> GetByProductSkuAsync(string productSku);
    Task<IEnumerable<InventoryMovementResponseDto>> GetByStorageIdAsync(int storageId);
}
