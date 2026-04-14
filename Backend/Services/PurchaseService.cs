using Backend.Dtos.Purchase;
using Backend.Interfaces;

namespace Backend.Services;

public class PurchaseService : IPurchaseService
{
    private static readonly HashSet<string> ValidStatuses = ["RECEIVED", "CANCELLED"];

    private readonly IPurchaseRepository _repository;
    private readonly IProviderRepository _providerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStorageRepository _storageRepository;

    public PurchaseService(
        IPurchaseRepository repository,
        IProviderRepository providerRepository,
        IUserRepository userRepository,
        IProductRepository productRepository,
        IStorageRepository storageRepository)
    {
        _repository = repository;
        _providerRepository = providerRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _storageRepository = storageRepository;
    }

    public Task<IEnumerable<PurchaseResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<PurchaseResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<IEnumerable<PurchaseResponseDto>> GetByProviderIdAsync(Guid providerId) =>
        _repository.GetByProviderIdAsync(providerId);

    public Task<IEnumerable<PurchaseResponseDto>> GetByStatusAsync(string status) =>
        _repository.GetByStatusAsync(status);

    public async Task<PurchaseResponseDto> CreateAsync(PurchaseCreateDto dto)
    {
        if (await _providerRepository.GetByIdAsync(dto.ProviderId) is null)
            throw new KeyNotFoundException($"Provider with id '{dto.ProviderId}' not found.");

        if (await _userRepository.GetByIdAsync(dto.UserId) is null)
            throw new KeyNotFoundException($"User with id '{dto.UserId}' not found.");

        if (dto.Details.Count == 0)
            throw new InvalidOperationException("A purchase must have at least one detail.");

        foreach (var detail in dto.Details)
        {
            if (detail.Quantity <= 0)
                throw new InvalidOperationException($"Quantity for product '{detail.ProductSku}' must be greater than zero.");

            if (await _productRepository.GetByIdAsync(detail.ProductSku) is null)
                throw new KeyNotFoundException($"Product with SKU '{detail.ProductSku}' not found.");

            if (await _storageRepository.GetByIdAsync(detail.StorageId) is null)
                throw new KeyNotFoundException($"Storage with id '{detail.StorageId}' not found.");
        }

        return await _repository.CreateAsync(dto);
    }

    public async Task<PurchaseResponseDto?> UpdateAsync(Guid id, PurchaseUpdateDto dto)
    {
        if (!ValidStatuses.Contains(dto.Status))
            throw new InvalidOperationException($"Invalid status '{dto.Status}'. Valid values are: {string.Join(", ", ValidStatuses)}.");

        return await _repository.UpdateAsync(id, dto);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        _repository.DeleteAsync(id);
}
