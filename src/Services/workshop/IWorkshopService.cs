using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.WorkshopDTO;

namespace Backend_Teamwork.src.Services.workshop
{
    public interface IWorkshopService
    {
        Task<WorkshopReadDTO?> CreateOneAsync(Guid artistId, WorkshopCreateDTO createworkshopDto);
        Task<List<WorkshopReadDTO?>> GetAllAsync();
        Task<List<WorkshopReadDTO>> GetAllAsync(PaginationOptions paginationOptions);
        Task<WorkshopReadDTO?> GetByIdAsync(Guid id);
        Task<bool> DeleteOneAsync(Guid id);
        Task<bool> UpdateOneAsync(Guid id, WorkshopUpdateDTO updateworkshopDto);
    }
}
