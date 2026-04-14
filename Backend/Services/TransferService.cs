using Backend.Dtos.Transfer;
using Backend.Interfaces;

namespace Backend.Services;

public class TransferService : ITransferService
{
    private readonly ITransferRepository _repository;
    private readonly IStorageRepository _storageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public TransferService(
        ITransferRepository repository,
        IStorageRepository storageRepository,
        IUserRepository userRepository,
        IProductRepository productRepository)
    {
        _repository = repository;
        _storageRepository = storageRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public Task<IEnumerable<TransferResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<TransferResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<IEnumerable<TransferResponseDto>> GetByStatusAsync(string status) =>
        _repository.GetByStatusAsync(status);

    public async Task<TransferResponseDto> CreateAsync(TransferCreateDto dto)
    {
        if (dto.OriginStorageId == dto.DestinationStorageId)
            throw new InvalidOperationException("Origin and destination storage must be different.");

        if (await _storageRepository.GetByIdAsync(dto.OriginStorageId) is null)
            throw new KeyNotFoundException($"Storage with id '{dto.OriginStorageId}' not found.");

        if (await _storageRepository.GetByIdAsync(dto.DestinationStorageId) is null)
            throw new KeyNotFoundException($"Storage with id '{dto.DestinationStorageId}' not found.");

        if (await _userRepository.GetByIdAsync(dto.UserId) is null)
            throw new KeyNotFoundException($"User with id '{dto.UserId}' not found.");

        if (dto.Details.Count == 0)
            throw new InvalidOperationException("A transfer must have at least one detail.");

        foreach (var detail in dto.Details)
        {
            if (detail.Quantity <= 0)
                throw new InvalidOperationException($"Quantity for product '{detail.ProductSku}' must be greater than zero.");

            if (await _productRepository.GetByIdAsync(detail.ProductSku) is null)
                throw new KeyNotFoundException($"Product with SKU '{detail.ProductSku}' not found.");
        }

        return await _repository.CreateAsync(dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var transfer = await _repository.GetByIdAsync(id);
        if (transfer is null) return false;

        throw new InvalidOperationException("Transfers cannot be deleted because stock movements cannot be reversed. Consider creating a reverse transfer instead.");
    }
}
