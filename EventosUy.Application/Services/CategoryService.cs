using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _repo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async Task<Result<Guid>> CreateAsync(string name, string description)
        {
            if (await _repo.ExistsAsync(name)) { return Result<Guid>.Failure("Category already exist.");  }

            Result<Category> categoryResult = Category.Create(name, description);

            if (!categoryResult.IsSuccess) { return Result<Guid>.Failure(categoryResult.Errors); }

            Category categoryInstance = categoryResult.Value!;
            await _repo.AddAsync(categoryInstance);

            return Result<Guid>.Success(categoryInstance.Id);
        }

        public async Task<Result<List<CategoryCard>>> GetAllAsync()
        {
            List<Category> categories = await _repo.GetAllAsync();
            List<CategoryCard> cards = categories.Select(category => category.GetCard()).ToList();

            return Result<List<CategoryCard>>.Success(cards);
        }

        public async Task<Result<Category>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Category>.Failure("Category can not be empty."); }
            Category? categoryInstance = await _repo.GetByIdAsync(id);

            if (categoryInstance is null) { return Result<Category>.Failure("Category not Found."); }

            return Result<Category>.Success(categoryInstance);
        }
    }
}
