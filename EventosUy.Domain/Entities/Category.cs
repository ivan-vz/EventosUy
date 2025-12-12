using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using System.Runtime.InteropServices;

namespace EventosUy.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; private set; }
        public DateTimeOffset Created { get; init; }
        public bool Active { get; private set; }

        private Category(string name, string description) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Created = DateTimeOffset.UtcNow;
            Active = true;
        }

        public static Result<Category> Create(string name, string description) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name can not be empty."); }
            if (string.IsNullOrWhiteSpace(description)) { errors.Add("Description can not be empty."); }

            if (errors.Any()) { return Result<Category>.Failure(errors); }

            Category category = new Category(name, description);

            return Result<Category>.Success(category);
        }

        public CategoryCard GetCard() { return new CategoryCard(Id, Name, Description, Created); }
    }
}
