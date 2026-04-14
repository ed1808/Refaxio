using Backend.Dtos.Sale;

namespace Backend.Interfaces;

public interface ISaleService : IService<Guid, SaleCreateDto, SaleUpdateDto, SaleResponseDto>
{
    Task<IEnumerable<SaleResponseDto>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<SaleResponseDto>> GetByStatusAsync(string status);
}
