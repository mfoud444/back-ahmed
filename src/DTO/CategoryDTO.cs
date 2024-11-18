using System.ComponentModel.DataAnnotations;

namespace Backend_Teamwork.src.DTO
{
    public class CategoryDTO
    {
        public class CategoryCreateDto
        {
            [
                Required(ErrorMessage = "Name shouldn't be null"),
                MinLength(2, ErrorMessage = "Name should be at at least 2 characters"),
                MaxLength(10, ErrorMessage = "Name shouldn't be more than 10 characters")
            ]
            public string Name { get; set; }
        }

        public class CategoryUpdateDto
        {
            [
                Required(ErrorMessage = "Name shouldn't be null"),
                MinLength(2, ErrorMessage = "Name should be at at least 2 characters"),
                MaxLength(10, ErrorMessage = "Name shouldn't be more than 10 characters")
            ]
            public string Name { get; set; }
        }

        public class CategoryReadDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
