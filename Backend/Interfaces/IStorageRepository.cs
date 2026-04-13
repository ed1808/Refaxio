using Backend.Dtos.Storage;

namespace Backend.Interfaces;

public interface IStorageRepository : IRepository<int, StorageCreateDto, StorageUpdateDto, StorageResponseDto>
{
}
