using System.ComponentModel.DataAnnotations;

namespace Backend_Teamwork.src.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }

        [
            Required(ErrorMessage = "Payment method shouldn't be null"),
            MinLength(10, ErrorMessage = "Payment method should be at at least 10 characters"),
            MaxLength(30, ErrorMessage = "Payment method shouldn't be more than 30 characters")
        ]
        public string PaymentMethod { get; set; }

        [Range(1.0, double.MaxValue, ErrorMessage = "Amount should be greater than zero.")]
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
        public Guid? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }
}
