using System.Security.Claims;
using Backend_Teamwork.src.DTO;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.artwork;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.ArtworkDTO;

namespace Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ArtworksController : ControllerBase
    {
        private readonly IArtworkService _artworkService;

        // Constructor
        public ArtworksController(IArtworkService service)
        {
            _artworkService = service;
        }

        // Create
        // End-Point: api/v1/artworks
        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<ActionResult<ArtworkReadDto>> CreateOne(
            [FromBody] ArtworkCreateDto createDto
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

            var createdArtwork = await _artworkService.CreateOneAsync(userGuid, createDto);
            return Ok(createdArtwork);
        }

        // Get all artworks
        // End-Point: api/v1/artworks
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<List<ArtworkReadDto>>>> GetAll(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var artworkList = await _artworkService.GetAllAsync(paginationOptions);
            var totalCount = await _artworkService.GetArtworkCountAsync();  // Get the total count
            var response = new PaginatedResponse<List<ArtworkReadDto>>
            {
                Items = artworkList,
                TotalCount = totalCount
            };
            return Ok(response);
        }

        // Get by artwork id
        // End-Point: api/v1/artworks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtworkReadDto>> GetById([FromRoute] Guid id)
        {
            var artwork = await _artworkService.GetByIdAsync(id);
            return Ok(artwork);
        }

        // Get by artist Id
        // End-Point: api/v1/artworks/artist/{id}
        [HttpGet("artist/{artistId}")]
        public async Task<ActionResult<List<ArtworkReadDto>>> GetByArtistId(
            [FromRoute] Guid artistId
        )
        {
            var artwork = await _artworkService.GetByArtistIdAsync(artistId);
            return Ok(artwork);
        }

        // Get count of all artworks
        // End-Point: api/v1/artworks/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetArtworkCount()
        {
            var count = await _artworkService.GetArtworkCountAsync();
            return Ok(count);
        }

        // // Get count of artworks by artistId
        // // End-Point: api/v1/artworks/artist/{artistId}/count
        // [HttpGet("artist/{artistId}/count")]
        // public async Task<ActionResult<int>> GetArtworkCountByArtist([FromRoute] Guid artistId)
        // {
        //     var count = await _artworkService.GetArtworkCountByArtistAsync(artistId);
        //     return Ok(count);
        // }

        // Update
        // End-Point: api/v1/artworks/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult> UpdateOne(
            [FromRoute] Guid id,
            [FromBody] ArtworkUpdateDTO updateDTO
        )
        {
            var artwork = await _artworkService.UpdateOneAsync(id, updateDTO);
            return Ok(artwork);
        }

        // Delete
        // End-Point: api/v1/artworks/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult> DeleteOne([FromRoute] Guid id)
        {
            await _artworkService.DeleteOneAsync(id);
            return NoContent();
        }
    }

    public class PaginatedResponse<T>
    {
        public T Items { get; set; }
        public int TotalCount { get; set; }
    }
}
