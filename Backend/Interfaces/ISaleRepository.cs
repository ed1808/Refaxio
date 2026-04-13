using Backend.Dtos.Sale;

namespace Backend.Interfaces;

public interface ISaleRepository : IRepository<Guid, SaleCreateDto, SaleUpdateDto, SaleResponseDto>
{
    Task<IEnumerable<SaleResponseDto>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<SaleResponseDto>> GetByStatusAsync(string status);
}
