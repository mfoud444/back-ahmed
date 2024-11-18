using System.ComponentModel.DataAnnotations;
using Backend_Teamwork.src.Entities;

namespace Backend_Teamwork.src.DTO
{
    public class WorkshopDTO
    {
        public class WorkshopCreateDTO
        {
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
        }

        public class WorkshopReadDTO
        {
            public Guid Id { get; set; }
            public string? Name { get; set; }
            public string? Location { get; set; }
            public string? Description { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public decimal Price { get; set; }
            public int Capacity { get; set; }
            public bool Availability { get; set; }
            public DateTime CreatedAt { get; set; }
            public User User { get; set; }
        }

        public class WorkshopUpdateDTO
        {
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

            [Required(ErrorMessage = "Availability shouldn't be null")]
            public bool Availability { get; set; }

            [Range(1.0, double.MaxValue, ErrorMessage = "Price should be greater than zero.")]
            public decimal Price { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Capacity should be greater than zero.")]
            public int Capacity { get; set; }
        }
    }
}
