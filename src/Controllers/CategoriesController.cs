using System.Text.Json;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.category;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.CategoryDTO;

namespace Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryReadDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDto>> GetCategoryById([FromRoute] Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<CategoryReadDto>> GetCategoryByName([FromRoute] string name)
        {
            var category = await _categoryService.GetByNameAsync(name);
            return Ok(category);
        }

        [HttpGet("page")]
        public async Task<ActionResult<List<CategoryReadDto>>> GetCategoriesWithPaginationAsync(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var categories = await _categoryService.GetWithPaginationAsync(paginationOptions);
            return Ok(categories);
        }

        [HttpGet("sort")]
        public async Task<ActionResult<List<CategoryReadDto>>> SortCategoriesByName()
        {
            var categories = await _categoryService.SortByNameAsync();
            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryReadDto>> CreateCategory(
            [FromBody] CategoryCreateDto categoryDTO
        )
        {
            var category = await _categoryService.CreateAsync(categoryDTO);
            return CreatedAtAction(nameof(CreateCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryReadDto>> UpdateCategory(
            [FromRoute] Guid id,
            [FromBody] CategoryUpdateDto categoryDTO
        )
        {
            var category = await _categoryService.UpdateAsync(id, categoryDTO);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }


    }
}
