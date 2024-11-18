using System.ComponentModel.DataAnnotations;

namespace Backend_Teamwork.src.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        [Range(1.0, double.MaxValue, ErrorMessage = "Total amount should be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [
            Required(ErrorMessage = "Address shouldn't be null"),
            MinLength(10, ErrorMessage = "Address should be at at least 10 characters"),
            MaxLength(30, ErrorMessage = "Address shouldn't be more than 30 characters")
        ]
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "User Id shouldn't be null")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required(ErrorMessage = "Order details shouldn't be null")]
        public List<OrderDetails> OrderDetails { get; set; }
        public Payment? Payment { get; set; }
    }
}
