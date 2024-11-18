using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.WorkshopDTO;

namespace Backend_Teamwork.src.Services.workshop
{
    public class WorkshopService : IWorkshopService
    {
        protected readonly WorkshopRepository _workshopRepo;
        protected readonly IMapper _mapper;

        public WorkshopService(WorkshopRepository workshopRepo, IMapper mapper)
        {
            _workshopRepo = workshopRepo;
            _mapper = mapper;
        }

        public async Task<WorkshopReadDTO> CreateOneAsync(
            Guid artistId,
            WorkshopCreateDTO createworkshopDto
        )
        {
            var workshop = _mapper.Map<WorkshopCreateDTO, Workshop>(createworkshopDto);
            workshop.UserId = artistId;
            var workshopCreated = await _workshopRepo.CreateOneAsync(workshop);
            return _mapper.Map<Workshop, WorkshopReadDTO>(workshopCreated);
        }

        public async Task<List<WorkshopReadDTO>> GetAllAsync()
        {
            var workshopList = await _workshopRepo.GetAllAsync();
            if (workshopList == null || !workshopList.Any())
            {
                throw CustomException.NotFound("Workshops not found");
            }
            return _mapper.Map<List<Workshop>, List<WorkshopReadDTO>>(workshopList);
        }

        public async Task<List<WorkshopReadDTO>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Validate pagination options
            if (paginationOptions.PageSize <= 0)
            {
                throw CustomException.BadRequest("Page Size should be greater than 0.");
            }

            if (paginationOptions.PageNumber < 0)
            {
                throw CustomException.BadRequest("Page Number should be 0 or greater.");
            }
            var workshopList = await _workshopRepo.GetAllAsync(paginationOptions);
            if (workshopList == null || !workshopList.Any())
            {
                throw CustomException.NotFound("Workshops not found");
            }
            return _mapper.Map<List<Workshop>, List<WorkshopReadDTO>>(workshopList);
        }

        public async Task<WorkshopReadDTO> GetByIdAsync(Guid id)
        {
            var foundworkshop = await _workshopRepo.GetByIdAsync(id);
            if (foundworkshop == null)
            {
                throw CustomException.NotFound($"Workshop with ID {id} not found.");
            }
            return _mapper.Map<Workshop, WorkshopReadDTO>(foundworkshop);
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var foundworkshop = await _workshopRepo.GetByIdAsync(id);
            if (foundworkshop == null)
            {
                throw CustomException.NotFound($"Workshop with ID {id} not found.");
            }
            return await _workshopRepo.DeleteOneAsync(foundworkshop);
            ;
        }

        public async Task<bool> UpdateOneAsync(Guid id, WorkshopUpdateDTO workshopupdateDto)
        {
            var foundworkshop = await _workshopRepo.GetByIdAsync(id);
            if (foundworkshop == null)
            {
                throw CustomException.NotFound($"Workshop with ID {id} not found.");
            }
            _mapper.Map(workshopupdateDto, foundworkshop);
            return await _workshopRepo.UpdateOneAsync(foundworkshop);
        }
    }
}
