using Backend.Dtos.Sale;
using Backend.Interfaces;

namespace Backend.Services;

public class SaleService : ISaleService
{
    private static readonly HashSet<string> ValidStatuses = ["COMPLETED", "VOIDED"];

    private readonly ISaleRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStorageRepository _storageRepository;

    public SaleService(
        ISaleRepository repository,
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IProductRepository productRepository,
        IStorageRepository storageRepository)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _storageRepository = storageRepository;
    }

    public Task<IEnumerable<SaleResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<SaleResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<IEnumerable<SaleResponseDto>> GetByCustomerIdAsync(Guid customerId) =>
        _repository.GetByCustomerIdAsync(customerId);

    public Task<IEnumerable<SaleResponseDto>> GetByStatusAsync(string status) =>
        _repository.GetByStatusAsync(status);

    public async Task<SaleResponseDto> CreateAsync(SaleCreateDto dto)
    {
        dto.Status = "COMPLETED";

        if (await _customerRepository.GetByIdAsync(dto.CustomerId) is null)
            throw new KeyNotFoundException($"Customer with id '{dto.CustomerId}' not found.");

        if (await _userRepository.GetByIdAsync(dto.UserId) is null)
            throw new KeyNotFoundException($"User with id '{dto.UserId}' not found.");

        if (dto.Details.Count == 0)
            throw new InvalidOperationException("A sale must have at least one detail.");

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

    public async Task<SaleResponseDto?> UpdateAsync(Guid id, SaleUpdateDto dto)
    {
        if (!ValidStatuses.Contains(dto.Status))
            throw new InvalidOperationException($"Invalid status '{dto.Status}'. Valid values are: {string.Join(", ", ValidStatuses)}.");

        var sale = await _repository.GetByIdAsync(id);
        if (sale is null) return null;

        if (sale.Status == "VOIDED")
            throw new InvalidOperationException("A voided sale cannot be modified.");

        return await _repository.UpdateAsync(id, dto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sale = await _repository.GetByIdAsync(id);
        if (sale is null) return false;

        if (sale.Status != "VOIDED")
            throw new InvalidOperationException("Only sales with status 'VOIDED' can be deleted.");

        return await _repository.DeleteAsync(id);
    }
}
