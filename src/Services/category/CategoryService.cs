using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.CategoryDTO;

namespace Backend_Teamwork.src.Services.category
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ArtworkRepository _artworkRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            CategoryRepository categoryRepository,
            IMapper mapper,
            ArtworkRepository artworkRepository
        )
        {
            _categoryRepository = categoryRepository;
            _artworkRepository = artworkRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryReadDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories.Count == 0)
            {
                throw CustomException.NotFound($"Categories not found");
            }
            return _mapper.Map<List<Category>, List<CategoryReadDto>>(categories);
        }

        public async Task<CategoryReadDto> GetByIdAsync(Guid id)
        {
            var foundCategory = await _categoryRepository.GetByIdAsync(id);
            if (foundCategory == null)
            {
                throw CustomException.NotFound($"Category with id: {id} not found");
            }
            return _mapper.Map<Category, CategoryReadDto>(foundCategory);
        }

        public async Task<CategoryReadDto> GetByNameAsync(string name)
        {
            var foundCategory = await _categoryRepository.GetByNameAsync(name);
            if (foundCategory == null)
            {
                throw CustomException.NotFound($"Category with name: {name} not found");
            }
            return _mapper.Map<Category, CategoryReadDto>(foundCategory);
        }

        public async Task<List<CategoryReadDto>> GetWithPaginationAsync(
            PaginationOptions paginationOptions
        )
        {
            var foundCategories = await _categoryRepository.GetWithPaginationAsync(
                paginationOptions
            );
            if (foundCategories.Count == 0)
            {
                throw CustomException.NotFound($"Categories not found");
            }
            return _mapper.Map<List<Category>, List<CategoryReadDto>>(foundCategories);
        }

        public async Task<List<CategoryReadDto>> SortByNameAsync()
        {
            var categories = await _categoryRepository.SortByNameAsync();
            if (categories.Count == 0)
            {
                throw CustomException.NotFound($"Categories not found");
            }
            return _mapper.Map<List<Category>, List<CategoryReadDto>>(categories);
        }

        public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto category)
        {
            var foundName = await _categoryRepository.GetByNameAsync(category.Name);
            if (foundName != null)
            {
                throw CustomException.BadRequest($"Invalid category name");
            }
            var mappedCategory = _mapper.Map<CategoryCreateDto, Category>(category);
            var createdCategory = await _categoryRepository.CreateAsync(mappedCategory);
            return _mapper.Map<Category, CategoryReadDto>(createdCategory);
        }

        public async Task<CategoryReadDto> UpdateAsync(Guid id, CategoryUpdateDto category)
        {
            var foundCategory = await _categoryRepository.GetByIdAsync(id);
            var foundName = await _categoryRepository.GetByNameAsync(category.Name);
            if (foundCategory == null)
            {
                throw CustomException.NotFound($"Category with id: {id} not found");
            }
            if (foundName != null)
            {
                throw CustomException.BadRequest($"Invalid category name");
            }
            _mapper.Map(category, foundCategory);
            var updatedCategory = await _categoryRepository.UpdateAsync(foundCategory);
            return _mapper.Map<Category, CategoryReadDto>(updatedCategory);
        }

        public async Task DeleteAsync(Guid id)
        {
            var foundCategory = await _categoryRepository.GetByIdAsync(id);
            //var foundArtwork = await _artworkRepository.GetByCategoryIdAsync(foundCategory.Id);
            if (foundCategory == null)
            {
                throw CustomException.NotFound($"Category with id: {id} not found");
            }
            /*if (foundArtwork != null)
            {
                throw CustomException.NotFound($"Invalid deleting");
            }*/
            await _categoryRepository.DeleteAsync(foundCategory);
        }
    }
}
