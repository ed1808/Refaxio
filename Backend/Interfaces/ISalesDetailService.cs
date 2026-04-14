using Backend.Dtos.SalesDetail;

namespace Backend.Interfaces;

public interface ISalesDetailService
{
    Task<IEnumerable<SalesDetailResponseDto>> GetAllBySaleIdAsync(Guid saleId);
    Task<SalesDetailResponseDto?> GetByIdAsync(Guid id);
    Task<SalesDetailResponseDto> CreateAsync(SalesDetailCreateDto dto);
}
