using Backend.Dtos.Inventory;

namespace Backend.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<InventoryResponseDto>> GetAllAsync();
    Task<InventoryResponseDto?> GetByKeyAsync(string productSku, int storageId);
    Task<IEnumerable<InventoryResponseDto>> GetByProductSkuAsync(string productSku);
    Task<IEnumerable<InventoryResponseDto>> GetByStorageIdAsync(int storageId);
    Task<InventoryResponseDto?> UpdateAsync(string productSku, int storageId, InventoryUpdateDto dto);
}
