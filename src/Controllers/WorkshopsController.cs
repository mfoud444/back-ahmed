using System.Security.Claims;
using Backend_Teamwork.src.Services.workshop;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.WorkshopDTO;

namespace sda_3_online_Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkshopsController : ControllerBase
    {
        private readonly IWorkshopService _workshopService;

        public WorkshopsController(IWorkshopService service)
        {
            _workshopService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkshopReadDTO>>> GetWorkshop()
        {
            var workshops = await _workshopService.GetAllAsync();
            return Ok(workshops);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkshopReadDTO>> GetWorkshopById([FromRoute] Guid id)
        {
            var workshop = await _workshopService.GetByIdAsync(id);
            return Ok(workshop);
        }

        [HttpGet("page")]
        public async Task<ActionResult<WorkshopReadDTO>> GetWorkShopByPage(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var workshops = await _workshopService.GetAllAsync(paginationOptions);
            return Ok(workshops);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult<WorkshopReadDTO>> CreateWorkshop(
            [FromBody] WorkshopCreateDTO createDto
        )
        {
            // extract user information
            var authenticateClaims = HttpContext.User;
            // get user id from claim
            var userId = authenticateClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            // string => guid
            var userGuid = new Guid(userId);

            var workshopCreated = await _workshopService.CreateOneAsync(userGuid, createDto);
            return CreatedAtAction(
                nameof(GetWorkshopById),
                new { id = workshopCreated.Id },
                workshopCreated
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult<bool>> DeleteWorkshop([FromRoute] Guid id)
        {
            var isDeleted = await _workshopService.DeleteOneAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult<bool>> UpdateWorkshop(
            Guid id,
            [FromBody] WorkshopUpdateDTO updateDto
        )
        {
            var updateWorkshop = await _workshopService.UpdateOneAsync(id, updateDto);
            return Ok(updateWorkshop);
        }
    }
}
