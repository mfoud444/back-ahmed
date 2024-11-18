using System.ComponentModel.DataAnnotations;

namespace Backend_Teamwork.src.Entities
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public Artwork Artwork { get; set; } = null!;

        [Required(ErrorMessage = "Artwork Id shouldn't be null")]
        public Guid ArtworkId { get; set; }
        public Order Order { get; set; } = null!;

        [Required(ErrorMessage = "Order Id shouldn't be null")]
        public Guid OrderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity should be greater than zero.")]
        public int Quantity { get; set; }
    }
}
