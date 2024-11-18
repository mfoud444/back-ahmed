using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class CategoryRepository
    {
        private readonly DbSet<Category> _category;
        private readonly DatabaseContext _databaseContext;

        public CategoryRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _category = databaseContext.Set<Category>();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _category.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _category.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _category.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Category>> GetWithPaginationAsync(PaginationOptions paginationOptions)
        {
            return await _category
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<Category>> SortByNameAsync()
        {
            return await _category.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _category.AddAsync(category);
            await _databaseContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _category.Update(category);
            await _databaseContext.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Category category)
        {
            _category.Remove(category);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
