using Backend.Dtos.Storage;

namespace Backend.Interfaces;

public interface IStorageService : IService<int, StorageCreateDto, StorageUpdateDto, StorageResponseDto>
{
}
