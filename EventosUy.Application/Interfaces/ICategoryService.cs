using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    internal interface ICategoryService
    {
        public Task<Result<Guid>> CreateAsync(string name, string description);
        public Task<Result<Category>> GetByIdAsync(Guid id);
        public Task<Result<List<CategoryCard>>> GetAllAsync();
    }
}
