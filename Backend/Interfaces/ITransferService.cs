using Backend.Dtos.Transfer;

namespace Backend.Interfaces;

public interface ITransferService
{
    Task<IEnumerable<TransferResponseDto>> GetAllAsync();
    Task<TransferResponseDto?> GetByIdAsync(Guid id);
    Task<TransferResponseDto> CreateAsync(TransferCreateDto dto);
    Task<IEnumerable<TransferResponseDto>> GetByStatusAsync(string status);
    Task<bool> DeleteAsync(Guid id);
}
