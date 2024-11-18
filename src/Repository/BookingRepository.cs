using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class BookingRepository
    {
        private readonly DbSet<Booking> _booking;
        private readonly DatabaseContext _databaseContext;

        public BookingRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _booking = databaseContext.Set<Booking>();
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Booking>> GetByUserIdAsync(Guid userId)
        {
            return await _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByStatusAsync(string status)
        {
            return await _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .Where(b => b.Status.ToString().ToLower() == status.ToLower())
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByUserIdAndStatusAsync(Guid userId, string status)
        {
            return await _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .Where(b => b.Status.ToString() == status.ToString() && b.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByWorkshopIdAndStatusAsync(
            Guid workshopId,
            Status status
        )
        {
            return await _booking
                .Where(b => b.WorkshopId == workshopId && b.Status == status)
                .ToListAsync();
        }

        public async Task<bool> GetByUserIdAndWorkshopIdAsync(Guid userId, Guid workshopId)
        {
            return await _booking.AnyAsync(b => b.UserId == userId && b.WorkshopId == workshopId);
        }

        public async Task<List<Booking>> GetWithPaginationAsync(PaginationOptions paginationOptions)
        {
            return await _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByUserIdWithPaginationAsync(
            PaginationOptions paginationOptions
        )
        {
            var bookings = _booking.Where(b => b.UserId.ToString() == paginationOptions.Search);
            return await bookings
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .ToListAsync();
        }

        public async Task<Booking?> CreateAsync(Booking booking)
        {
            await _booking.AddAsync(booking);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(booking.Id);
        }

        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            _booking.Update(booking);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(booking.Id);
        }
    }
}
