using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class WorkshopRepository
    {
        private readonly DbSet<Workshop> _workshops;
        private readonly DatabaseContext _databaseContext;

        public WorkshopRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _workshops = databaseContext.Set<Workshop>();
        }

        // create in database
        public async Task<Workshop?> CreateOneAsync(Workshop newWorkshop)
        {
            await _workshops.AddAsync(newWorkshop);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(newWorkshop.Id);
        }

        // get by id
        public async Task<Workshop?> GetByIdAsync(Guid id)
        {
            return await _workshops.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);
        }

        // delete
        public async Task<bool> DeleteOneAsync(Workshop deleteWorkshop)
        {
            _workshops.Remove(deleteWorkshop);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        // update
        public async Task<bool> UpdateOneAsync(Workshop updateWorkshop)
        {
            if (updateWorkshop == null)
                return false;
            _workshops.Update(updateWorkshop);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        // get all
        public async Task<List<Workshop>> GetAllAsync()
        {
            return await _workshops.Include(o => o.User).ToListAsync();
        }

        public async Task<List<Workshop>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Combined search logic with OR for name, email, or phone number
            var userQuery = _workshops.Where(a =>
                a.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                || a.Location.ToLower().Contains(paginationOptions.Search.ToLower())
            );

            // Apply pagination
            userQuery = userQuery
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            // Apply sorting logic
            userQuery = paginationOptions.SortOrder switch
            {
                "name_desc" => userQuery.OrderByDescending(a => a.Name),
                "location_asc" => userQuery.OrderBy(a => a.Location),
                "location_desc" => userQuery.OrderByDescending(a => a.Location),
                "price_desc" => userQuery.OrderByDescending(a => a.Price),
                "price_asc" => userQuery.OrderBy(a => a.Price),
                "date_desc" => userQuery.OrderByDescending(a => a.CreatedAt),
                "date_asc" => userQuery.OrderBy(a => a.CreatedAt),
                "capacity_desc" => userQuery.OrderByDescending(a => a.Capacity),
                _ => userQuery.OrderBy(a => a.Name), // Default to ascending by name
            };

            return await userQuery.ToListAsync();
        }
    }
}
