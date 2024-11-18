using System.ComponentModel.DataAnnotations;

namespace Backend_Teamwork.src.Entities
{
    public class Workshop
    {
        public Guid Id { get; set; }

        [
            Required(ErrorMessage = "Name shouldn't be null"),
            MinLength(10, ErrorMessage = "Name should be at at least 10 characters"),
            MaxLength(30, ErrorMessage = "Name shouldn't be more than 30 characters")
        ]
        public string Name { get; set; }

        [
            Required(ErrorMessage = "Location shouldn't be null"),
            MinLength(10, ErrorMessage = "Location should be at at least 10 characters"),
            MaxLength(30, ErrorMessage = "Location shouldn't be more than 30 characters")
        ]
        public string Location { get; set; }

        [
            Required(ErrorMessage = "Description shouldn't be null"),
            MinLength(30, ErrorMessage = "Description should be at at least 30 characters"),
            MaxLength(200, ErrorMessage = "Description shouldn't be more than 200 characters")
        ]
        public string Description { get; set; }

        [Required(ErrorMessage = "StartTime shouldn't be null")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime shouldn't be null")]
        public DateTime EndTime { get; set; }

        [Range(1.0, double.MaxValue, ErrorMessage = "Price should be greater than zero.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity should be greater than zero.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Availability shouldn't be null")]
        public bool Availability { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "User Id shouldn't be null")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
