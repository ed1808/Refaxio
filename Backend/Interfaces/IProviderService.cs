using Backend.Dtos.Provider;

namespace Backend.Interfaces;

public interface IProviderService : IService<Guid, ProviderCreateDto, ProviderUpdateDto, ProviderResponseDto>
{
}
