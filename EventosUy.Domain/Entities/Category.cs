using EventosUy.Domain.Common;

namespace EventosUy.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTimeOffset Created { get; init; }
        public bool Active { get; private set; }

        private Category(string name) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Created = DateTimeOffset.UtcNow;
            Active = true;
        }

        public static Result<Category> Create(string name, string description) 
        {
            if (string.IsNullOrWhiteSpace(name)) { return Result<Category>.Failure("Name can not be empty."); }

            Category categoryInstance = new(name);

            return Result<Category>.Success(categoryInstance);
        }
    }
}
