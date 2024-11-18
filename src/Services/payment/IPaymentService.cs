using static Backend_Teamwork.src.DTO.PaymentDTO;

namespace Backend_Teamwork.src.Services.payment
{
    public interface IPaymentService
    {

        Task<PaymentReadDTO?> CreateOneAsync(PaymentCreateDTO createpaymentDto);
        Task<List<PaymentReadDTO?>> GetAllAsync();
        Task<PaymentReadDTO?> GetByIdAsync(Guid id);
        Task<bool> DeleteOneAsync(Guid id);
        Task<bool> UpdateOneAsync(Guid id, PaymentUpdateDTO updatepaymentDto);
    }
}