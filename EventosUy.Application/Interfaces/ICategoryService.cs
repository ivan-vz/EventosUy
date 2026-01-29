using EventosUy.Application.DTOs.DataTypes.Detail;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<(DTCategory? dt, ValidationResult validation)> CreateAsync(string name);
        public Task<DTCategory?> GetByIdAsync(Guid id);
        public Task<IEnumerable<string>> GetAllAsync();
        public Task<bool> ExistsAsync(string name);
        public Task<bool> ExistsAsync(IEnumerable<string> names);
        public Task<DTCategory?> DeleteAsync(Guid id);
    }
}
