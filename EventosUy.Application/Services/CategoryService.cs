using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<Result<Guid>> CreateAsync(string name, string description)
        {
            if (await _categoryRepo.ExistsAsync(name)) { return Result<Guid>.Failure("Category already exist.");  }

            Result<Category> categoryResult = Category.Create(name, description);

            if (!categoryResult.IsSuccess) { return Result<Guid>.Failure(categoryResult.Error!); }

            Category categoryInstance = categoryResult.Value!;
            return Result<Guid>.Success(categoryInstance.Id);
        }

        public async Task<Result<List<CategoryCard>>> GetAllAsync()
        {
            List<Category> categories = await _categoryRepo.GetAllAsync();
            List<CategoryCard> cards = categories.Select(category => category.GetCard()).ToList();

            return Result<List<CategoryCard>>.Success(cards);
        }

        public async Task<Result<Category>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Category>.Failure("Category can not be empty."); }
            Category? categoryInstance = await _categoryRepo.GetByIdAsync(id);

            if (categoryInstance is null) { return Result<Category>.Failure("Category not Found."); }

            return Result<Category>.Success(categoryInstance);
        }
    }
}
