using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Backend_Teamwork.src.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [
            Required(ErrorMessage = "Name shouldn't be null"),
            MinLength(2, ErrorMessage = "Name should be at at least 2 characters"),
            MaxLength(10, ErrorMessage = "Name shouldn't be more than 10 characters")
        ]
        public string Name { get; set; }

        [
            Required(ErrorMessage = "Phone number shouldn't be null"),
            RegularExpression(
                @"^\+966[5][0-9]{8}$",
                ErrorMessage = "Phone number should be a valid Saudi phone number"
            )
        ]
        public string PhoneNumber { get; set; }

        [
            Required(ErrorMessage = "Email shouldn't be null"),
            EmailAddress(ErrorMessage = "Email should be with right format: @gmail.com")
        ]
        public string Email { get; set; }

        [
            Required(ErrorMessage = "Password shouldn't be null."),
            MinLength(8, ErrorMessage = "Password should be at at least 8 characters")
        ]
        public string Password { get; set; }
        public string? Description { set; get; }

        [Required(ErrorMessage = "Salt shouldn't be null")]
        public byte[]? Salt { get; set; }

        [Required(ErrorMessage = "Role shouldn't be null")]
        public UserRole Role { get; set; } = UserRole.Customer;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum UserRole
        {
            Admin,
            Customer,
            Artist,
        }
    }
}
