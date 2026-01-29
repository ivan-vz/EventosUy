using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using FluentValidation.Results;

namespace EventosUy.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _repo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async Task<(DTCategory? dt, ValidationResult validation)> CreateAsync(string name)
        {
            var validationResult = new ValidationResult();

            if (await _repo.ExistsAsync(name))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    validationResult.Errors.Add
                        (
                            new ValidationFailure("Name", "Name is already in use.")
                        );
                }

                if (!validationResult.IsValid) { return (null, validationResult); }
            }

            var category = new Category(name: name);

            await _repo.AddAsync(category);

            var dt = new DTCategory(id: category.Id, name: category.Name, created: category.Created);

            return (dt, validationResult);
        }

        public async Task<bool> ExistsAsync(string name) { return await _repo.ExistsAsync(name); }

        public async Task<bool> ExistsAsync(IEnumerable<string> names)
        {
            foreach (string name in names) 
            {
                if (!await _repo.ExistsAsync(name)) 
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            List<Category> categories = await _repo.GetAllAsync();
            List<string> names = [.. categories.Select(category => category.Name) ];

            return names;
        }

        public async Task<DTCategory?> GetByIdAsync(Guid id)
        {
            Category? category = await _repo.GetByIdAsync(id);

            if (category is null) { return null; }

            var dt = new DTCategory(id: category.Id, name: category.Name, created: category.Created);

            return dt;
        }

        public async Task<DTCategory?> DeleteAsync(Guid id)
        {
            Category? category = await _repo.GetByIdAsync(id);

            if (category is null) { return null; }

            category.Active = false;

            var dt = new DTCategory(id: category.Id, name: category.Name, created: category.Created);

            return dt;
        }
    }
}
