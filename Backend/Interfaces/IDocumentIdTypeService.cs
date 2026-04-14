using Backend.Dtos.DocumentIdType;

namespace Backend.Interfaces;

public interface IDocumentIdTypeService : IService<Guid, DocumentIdTypeCreateDto, DocumentIdTypeUpdateDto, DocumentIdTypeResponseDto>
{
}
