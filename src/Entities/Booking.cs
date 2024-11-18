using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend_Teamwork.src.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Status shouldn't be null")]
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; } 

        [Required(ErrorMessage = "Workshop Id shouldn't be null")]
        public Guid WorkshopId { get; set; }
        public Workshop Workshop { get; set; } = null!;

        [Required(ErrorMessage = "User Id shouldn't be null")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Payment? Payment { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Pending,
        Confirmed,
        Canceled,
        Rejected,
    }
}
