using Backend.Dtos.PurchaseDetail;
using Backend.Interfaces;

namespace Backend.Services;

public class PurchaseDetailService : IPurchaseDetailService
{
    private readonly IPurchaseDetailRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IStorageRepository _storageRepository;

    public PurchaseDetailService(
        IPurchaseDetailRepository repository,
        IProductRepository productRepository,
        IStorageRepository storageRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
        _storageRepository = storageRepository;
    }

    public Task<IEnumerable<PurchaseDetailResponseDto>> GetAllByPurchaseIdAsync(Guid purchaseId) =>
        _repository.GetAllByPurchaseIdAsync(purchaseId);

    public Task<PurchaseDetailResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public async Task<PurchaseDetailResponseDto> CreateAsync(PurchaseDetailCreateDto dto)
    {
        if (dto.Quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero.");

        if (await _productRepository.GetByIdAsync(dto.ProductSku) is null)
            throw new KeyNotFoundException($"Product with SKU '{dto.ProductSku}' not found.");

        if (await _storageRepository.GetByIdAsync(dto.StorageId) is null)
            throw new KeyNotFoundException($"Storage with id '{dto.StorageId}' not found.");

        return await _repository.CreateAsync(dto);
    }
}
