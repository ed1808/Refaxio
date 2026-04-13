using Backend.Dtos.DocumentIdType;

namespace Backend.Interfaces;

public interface IDocumentIdTypeRepository : IRepository<Guid, DocumentIdTypeCreateDto, DocumentIdTypeUpdateDto, DocumentIdTypeResponseDto>
{
}
