using Backend.Dtos.Customer;

namespace Backend.Interfaces;

public interface ICustomerService : IService<Guid, CustomerCreateDto, CustomerUpdateDto, CustomerResponseDto>
{
}
