using Backend.Dtos.PersonType;

namespace Backend.Interfaces;

public interface IPersonTypeService : IService<Guid, PersonTypeCreateDto, PersonTypeUpdateDto, PersonTypeResponseDto>
{
}
