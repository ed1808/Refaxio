using Backend.Dtos.TransferDetail;

namespace Backend.Interfaces;

public interface ITransferDetailRepository
{
    Task<IEnumerable<TransferDetailResponseDto>> GetAllByTransferIdAsync(Guid transferId);
    Task<TransferDetailResponseDto?> GetByIdAsync(Guid id);
    Task<TransferDetailResponseDto> CreateAsync(TransferDetailCreateDto dto);
}
