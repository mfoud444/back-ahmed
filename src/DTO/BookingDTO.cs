using System.ComponentModel.DataAnnotations;
using Backend_Teamwork.src.Entities;

namespace Backend_Teamwork.src.DTO
{
    public class BookingDTO
    {
        public class BookingCreateDto
        {
            [Required(ErrorMessage = "Workshop Id shouldn't be null"),]
            public Guid WorkshopId { get; set; }
            public DateTime? CreatedAt { get; set; } = DateTime.Now;
        }

        public class BookingReadDto
        {
            public Guid Id { get; set; }
            public Status Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public Workshop Workshop { get; set; }
            public User User { get; set; }
        }
    }
}
