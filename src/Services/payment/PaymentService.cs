using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.PaymentDTO;

namespace Backend_Teamwork.src.Services.payment
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly OrderRepository _orderRepository;
        private readonly BookingRepository _bookingRepository;

        private readonly IMapper _mapper;

        public PaymentService(
            PaymentRepository paymentRepository,
            IMapper mapper,
            OrderRepository orderRepository,
            BookingRepository bookingRepository
        )
        {
            _paymentRepository = paymentRepository;
            _orderRepository= orderRepository;
            _bookingRepository= bookingRepository;
            _mapper = mapper;
        }

        public async Task<PaymentReadDTO> CreateOneAsync(PaymentCreateDTO createpaymentDto)
        {
            var payment = _mapper.Map<PaymentCreateDTO, Payment>(createpaymentDto);
            var paymentCreated = await _paymentRepository.CreateOneAsync(payment);
            return _mapper.Map<Payment, PaymentReadDTO>(paymentCreated);
        }

        public async Task<List<PaymentReadDTO>> GetAllAsync()
        {
            var paymentList = await _paymentRepository.GetAllAsync();
            if (paymentList == null || !paymentList.Any())
            {
                throw CustomException.NotFound($"Payments not found");
            }
            return _mapper.Map<List<Payment>, List<PaymentReadDTO>>(paymentList);
        }

        public async Task<PaymentReadDTO> GetByIdAsync(Guid id)
        {
            var foundpayment = await _paymentRepository.GetByIdAsync(id);
            if (foundpayment == null)
            {
                throw CustomException.NotFound($"Payment with id: {id} not found");
            }
            return _mapper.Map<Payment, PaymentReadDTO>(foundpayment);
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var foundpayment = await _paymentRepository.GetByIdAsync(id);
            if (foundpayment == null)
            {
                throw CustomException.NotFound($"Payment with id: {id} not found");
            }
            return await _paymentRepository.DeleteOneAsync(foundpayment);
            ;
        }

        public async Task<bool> UpdateOneAsync(Guid id, PaymentUpdateDTO paymentupdateDto)
        {
            var foundpayment = await _paymentRepository.GetByIdAsync(id);
            if (foundpayment == null)
            {
                throw CustomException.NotFound($"Payment with id: {id} not found");
            }
            _mapper.Map(paymentupdateDto, foundpayment);
            return await _paymentRepository.UpdateOneAsync(foundpayment);
        }
    }
}
