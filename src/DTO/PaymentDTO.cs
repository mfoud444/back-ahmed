using System.ComponentModel.DataAnnotations;
using Backend_Teamwork.src.Entities;

namespace Backend_Teamwork.src.DTO
{
    public class PaymentDTO
    {
        public class PaymentCreateDTO
        {
            [
                Required(ErrorMessage = "Payment method shouldn't be null"),
                MinLength(10, ErrorMessage = "Payment method should be at at least 10 characters"),
                MaxLength(30, ErrorMessage = "Payment method shouldn't be more than 30 characters")
            ]
            public string PaymentMethod { get; set; }

            [Range(1.0, double.MaxValue, ErrorMessage = "Price should be greater than zero.")]
            public decimal Amount { get; set; }
            public DateTime? CreatedAt { get; set; } = DateTime.Now;
            public Guid? OrderId { get; set; } = Guid.Empty;
            public Guid? BookingId { get; set; } = Guid.Empty;
        }

        public class PaymentReadDTO
        {
            public Guid Id { get; set; }
            public string PaymentMethod { get; set; }
            public decimal Amount { get; set; }
            public DateTime CreatedAt { get; set; }
            public Order? Order { get; set; }
            public Booking? Booking { get; set; }
        }

        public class PaymentUpdateDTO
        {
            [
                Required(ErrorMessage = "Payment method shouldn't be null"),
                MinLength(10, ErrorMessage = "Payment method should be at at least 10 characters"),
                MaxLength(30, ErrorMessage = "Payment method shouldn't be more than 30 characters")
            ]
            public string PaymentMethod { get; set; }

            [Range(1.0, double.MaxValue, ErrorMessage = "Amount should be greater than zero.")]
            public decimal Amount { get; set; }
        }
    }
}
