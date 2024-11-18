using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.BookingDTO;

namespace Backend_Teamwork.src.Services.booking
{
    public interface IBookingService
    {
        Task<List<BookingReadDto>> GetAllAsync();
        Task<BookingReadDto> GetByIdAsync(Guid id, Guid userId, string userRole);
        Task<List<BookingReadDto>> GetByUserIdAsync(Guid userId);
        Task<List<BookingReadDto>> GetByStatusAsync(string status);
        Task<List<BookingReadDto>> GetByUserIdAndStatusAsync(Guid userId, string status);
        Task<List<BookingReadDto>> GetWithPaginationAsync(PaginationOptions paginationOptions);
        Task<List<BookingReadDto>> GetByUserIdWithPaginationAsync(
            PaginationOptions paginationOptions
        );
        Task<BookingReadDto> CreateAsync(BookingCreateDto booking, Guid userId);
        Task<BookingReadDto> ConfirmAsync(Guid id);
        Task<List<BookingReadDto>> RejectAsync(Guid workshopId);
        Task<BookingReadDto> CancelAsync(Guid id, Guid userId);
    }
}
