using Backend.Dtos.PurchaseDetail;

namespace Backend.Interfaces;

public interface IPurchaseDetailRepository
{
    Task<IEnumerable<PurchaseDetailResponseDto>> GetAllByPurchaseIdAsync(Guid purchaseId);
    Task<PurchaseDetailResponseDto?> GetByIdAsync(Guid id);
    Task<PurchaseDetailResponseDto> CreateAsync(PurchaseDetailCreateDto dto);
}
