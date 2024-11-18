using System.ComponentModel.DataAnnotations;
using static Backend_Teamwork.src.DTO.ArtworkDTO;

namespace Backend_Teamwork.src.DTO
{
    public class OrderDetailDTO
    {
        public class OrderDetailCreateDto
        {
            [Required(ErrorMessage = "Artwork Id shouldn't be null")]
            public Guid ArtworkId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Quantity should be greater than zero.")]
            public int Quantity { get; set; }
        }

        public class OrderDetailReadDto
        {
            public Guid Id { get; set; }
            public int Quantity { get; set; }
            public ArtworkReadDto? Artwork { get; set; }
        }

        public class OrderDetailUpdateDto
        {
            [Range(1, int.MaxValue, ErrorMessage = "Quantity should be greater than zero.")]
            public int Quantity { get; set; }
        }
    }
}
