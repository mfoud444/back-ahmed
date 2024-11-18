using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class UserRepository
    {
        private readonly DbSet<User> _user;
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _user = databaseContext.Set<User>();
        }

        public async Task<List<User>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Combined search logic with OR for name, email, or phone number
            var userQuery = _user.Where(a =>
                a.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                || a.Email.ToLower().Contains(paginationOptions.Search.ToLower())
                || a.PhoneNumber.Contains(paginationOptions.Search) // if u try this the dont put the number with +, it does not work
            // I think I need to add the country code in the front-end part for the phone number search.
            );

            // Apply pagination
            userQuery = userQuery
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            // Sorting logic
            userQuery = paginationOptions.SortOrder switch
            {
                "name_desc" => userQuery.OrderByDescending(a => a.Name),
                "email_desc" => userQuery.OrderByDescending(a => a.Email),
                "email_asc" => userQuery.OrderBy(a => a.Email),
                _ => userQuery.OrderBy(a => a.Name), // Default to ascending by name
            };

            return await userQuery.ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _user.CountAsync();
        }

        public async Task<User> CreateOneAsync(User newUser)
        {
            await _user.AddAsync(newUser);
            await _databaseContext.SaveChangesAsync();
            return newUser;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _user.FindAsync(id);
        }

        public async Task<bool> DeleteOneAsync(User User)
        {
            if (User == null)
                return false;
            _user.Remove(User);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOneAsync(User updateUser)
        {
            if (updateUser == null)
                return false;
            _user.Update(updateUser);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _user.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _user.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _user.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
