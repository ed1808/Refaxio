using Backend.Dtos.Customer;
using Backend.Interfaces;

namespace Backend.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IDocumentIdTypeRepository _docTypeRepository;
    private readonly IPersonTypeRepository _personTypeRepository;

    public CustomerService(
        ICustomerRepository repository,
        IDocumentIdTypeRepository docTypeRepository,
        IPersonTypeRepository personTypeRepository)
    {
        _repository = repository;
        _docTypeRepository = docTypeRepository;
        _personTypeRepository = personTypeRepository;
    }

    public Task<IEnumerable<CustomerResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<CustomerResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public async Task<CustomerResponseDto> CreateAsync(CustomerCreateDto dto)
    {
        await ValidateForeignKeysAsync(dto.DocTypeId, dto.PersonTypeId);
        return await _repository.CreateAsync(dto);
    }

    public async Task<CustomerResponseDto?> UpdateAsync(Guid id, CustomerUpdateDto dto)
    {
        await ValidateForeignKeysAsync(dto.DocTypeId, dto.PersonTypeId);
        return await _repository.UpdateAsync(id, dto);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        _repository.DeleteAsync(id);

    private async Task ValidateForeignKeysAsync(Guid docTypeId, Guid personTypeId)
    {
        if (await _docTypeRepository.GetByIdAsync(docTypeId) is null)
            throw new KeyNotFoundException($"DocumentIdType with id '{docTypeId}' not found.");

        if (await _personTypeRepository.GetByIdAsync(personTypeId) is null)
            throw new KeyNotFoundException($"PersonType with id '{personTypeId}' not found.");
    }
}
