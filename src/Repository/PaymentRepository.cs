using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class PaymentRepository
    {
        private readonly DbSet<Payment> _payment;
        private readonly DatabaseContext _databaseContext;

        public PaymentRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _payment = databaseContext.Set<Payment>();
        }

        // create in database
        public async Task<Payment?> CreateOneAsync(Payment newPayment)
        {
            await _payment.AddAsync(newPayment);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(newPayment.Id);
        }

        // get by id
        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _payment
                .Include(p => p.Order)
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // delete
        public async Task<bool> DeleteOneAsync(Payment deletePayment)
        {
            _payment.Remove(deletePayment);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        // update
        public async Task<bool> UpdateOneAsync(Payment updatePayment)
        {
            if (updatePayment == null)
                return false;
            _payment.Update(updatePayment);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        // get all
        public async Task<List<Payment>> GetAllAsync()
        {
            return await _payment.Include(p => p.Order).Include(p => p.Booking).ToListAsync();
        }
    }
}
