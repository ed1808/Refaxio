using Backend.Dtos.TransferDetail;
using Backend.Interfaces;

namespace Backend.Services;

public class TransferDetailService : ITransferDetailService
{
    private readonly ITransferDetailRepository _repository;
    private readonly IProductRepository _productRepository;

    public TransferDetailService(
        ITransferDetailRepository repository,
        IProductRepository productRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
    }

    public Task<IEnumerable<TransferDetailResponseDto>> GetAllByTransferIdAsync(Guid transferId) =>
        _repository.GetAllByTransferIdAsync(transferId);

    public Task<TransferDetailResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public async Task<TransferDetailResponseDto> CreateAsync(TransferDetailCreateDto dto)
    {
        if (dto.Quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero.");

        if (await _productRepository.GetByIdAsync(dto.ProductSku) is null)
            throw new KeyNotFoundException($"Product with SKU '{dto.ProductSku}' not found.");

        return await _repository.CreateAsync(dto);
    }
}
