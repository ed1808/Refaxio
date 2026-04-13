using Backend.Dtos.Customer;

namespace Backend.Interfaces;

public interface ICustomerRepository : IRepository<Guid, CustomerCreateDto, CustomerUpdateDto, CustomerResponseDto>
{
}
