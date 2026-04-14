using Backend.Dtos.PersonType;
using Backend.Interfaces;

namespace Backend.Services;

public class PersonTypeService : IPersonTypeService
{
    private readonly IPersonTypeRepository _repository;

    public PersonTypeService(IPersonTypeRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonTypeResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<PersonTypeResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<PersonTypeResponseDto> CreateAsync(PersonTypeCreateDto dto) =>
        _repository.CreateAsync(dto);

    public Task<PersonTypeResponseDto?> UpdateAsync(Guid id, PersonTypeUpdateDto dto) =>
        _repository.UpdateAsync(id, dto);

    public Task<bool> DeleteAsync(Guid id) =>
        _repository.DeleteAsync(id);
}
