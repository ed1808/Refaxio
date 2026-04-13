using Backend.Dtos.Provider;

namespace Backend.Interfaces;

public interface IProviderRepository : IRepository<Guid, ProviderCreateDto, ProviderUpdateDto, ProviderResponseDto>
{
}
