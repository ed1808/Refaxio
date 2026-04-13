using Backend.Dtos.Purchase;

namespace Backend.Interfaces;

public interface IPurchaseRepository : IRepository<Guid, PurchaseCreateDto, PurchaseUpdateDto, PurchaseResponseDto>
{
    Task<IEnumerable<PurchaseResponseDto>> GetByProviderIdAsync(Guid providerId);
    Task<IEnumerable<PurchaseResponseDto>> GetByStatusAsync(string status);
}
