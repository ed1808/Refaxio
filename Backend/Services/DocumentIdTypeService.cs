using Backend.Dtos.DocumentIdType;
using Backend.Interfaces;

namespace Backend.Services;

public class DocumentIdTypeService : IDocumentIdTypeService
{
    private readonly IDocumentIdTypeRepository _repository;

    public DocumentIdTypeService(IDocumentIdTypeRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<DocumentIdTypeResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<DocumentIdTypeResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<DocumentIdTypeResponseDto> CreateAsync(DocumentIdTypeCreateDto dto) =>
        _repository.CreateAsync(dto);

    public Task<DocumentIdTypeResponseDto?> UpdateAsync(Guid id, DocumentIdTypeUpdateDto dto) =>
        _repository.UpdateAsync(id, dto);

    public Task<bool> DeleteAsync(Guid id) =>
        _repository.DeleteAsync(id);
}
