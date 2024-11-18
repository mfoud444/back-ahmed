using System.ComponentModel.DataAnnotations;
using static Backend_Teamwork.src.DTO.OrderDetailDTO;
using static Backend_Teamwork.src.DTO.UserDTO;

namespace Backend_Teamwork.src.DTO
{
    public class OrderDTO
    {
        // DTO for creating a new order
        public class OrderCreateDto
        {
            [
                Required(ErrorMessage = "Address shouldn't be null"),
                MinLength(10, ErrorMessage = "Address should be at at least 10 characters"),
                MaxLength(30, ErrorMessage = "Address shouldn't be more than 30 characters")
            ]
            public string ShippingAddress { get; set; }
            public DateTime? CreatedAt { get; set; } = DateTime.Now;

            [Required(ErrorMessage = "Order details shouldn't be null")]
            public List<OrderDetailCreateDto> OrderDetails { get; set; }
        }

        // DTO for reading order data
        public class OrderReadDto
        {
            public Guid Id { get; set; }
            public decimal TotalAmount { get; set; }
            public string? ShippingAddress { get; set; }
            public DateTime? CreatedAt { get; set; }
            public UserReadDto User { get; set; }
            public List<OrderDetailReadDto> OrderDetails { get; set; }
        }

        // DTO for updating an existing order
        public class OrderUpdateDto
        {
            [Range(
                1.0,
                double.MaxValue,
                ErrorMessage = "Total amount should be greater than zero."
            )]
            public decimal TotalAmount { get; set; }

            [
                Required(ErrorMessage = "Address shouldn't be null"),
                MinLength(10, ErrorMessage = "Address should be at at least 10 characters"),
                MaxLength(30, ErrorMessage = "Address shouldn't be more than 30 characters")
            ]
            public string? ShippingAddress { get; set; }
        }
    }
}
