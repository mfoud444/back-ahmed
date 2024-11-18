using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.PaymentDTO;

namespace Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Payment>>> GetPayments()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Payment>> GetPaymentById(Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            return Ok(payment);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Payment>> CreatePayment(PaymentCreateDTO newPayment)
        {
            var payment = await _paymentService.CreateOneAsync(newPayment);
            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePayment(Guid id)
        {
            await _paymentService.DeleteOneAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdatePayment(Guid id, PaymentUpdateDTO updatedPayment)
        {
            var payment = _paymentService.UpdateOneAsync(id,updatedPayment);
            return Ok(payment);
        }
    }
}
