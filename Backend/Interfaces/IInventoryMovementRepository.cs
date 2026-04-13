using Backend.Dtos.InventoryMovement;

namespace Backend.Interfaces;

public interface IInventoryMovementRepository
{
    Task<IEnumerable<InventoryMovementResponseDto>> GetAllAsync();
    Task<InventoryMovementResponseDto?> GetByIdAsync(Guid id);
    Task<InventoryMovementResponseDto> CreateAsync(InventoryMovementCreateDto dto);
    Task<IEnumerable<InventoryMovementResponseDto>> GetByProductSkuAsync(string productSku);
    Task<IEnumerable<InventoryMovementResponseDto>> GetByStorageIdAsync(int storageId);
}
