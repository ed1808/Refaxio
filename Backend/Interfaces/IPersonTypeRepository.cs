using Backend.Dtos.PersonType;

namespace Backend.Interfaces;

public interface IPersonTypeRepository : IRepository<Guid, PersonTypeCreateDto, PersonTypeUpdateDto, PersonTypeResponseDto>
{
}
