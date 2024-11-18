using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.OrderDTO;

namespace Backend_Teamwork.src.Services.order
{
    public interface IOrderService
    {
        // Get all
        Task<List<OrderReadDto>> GetAllAsync();
        Task<List<OrderReadDto>> GetAllAsync(Guid id);

        // create
        Task<OrderReadDto> CreateOneAsync(Guid id, OrderCreateDto createDto);

        // Get by id
        Task<OrderReadDto> GetByIdAsync(Guid id);
        Task<OrderReadDto> GetByIdAsync(Guid id, Guid userId);

        // delete
        Task<bool> DeleteOneAsync(Guid id);
        Task<bool> UpdateOneAsync(Guid id, OrderUpdateDto updateDto);
        Task<List<OrderReadDto>> GetOrdersByPage(PaginationOptions paginationOptions);
        Task<List<OrderReadDto>> SortOrdersByDate();
    }
}
