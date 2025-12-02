using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Interfaces
{
    public interface IInstitutionRepo
    {
        public Task<Institution?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllAsync();
        public Task<bool> ExistsByNicknameAsync(string nickname);
        public Task<bool> ExistsByEmailAsync(Email email);
        public Task<bool> ExistsByUrlAsync(Url url);
        public Task<bool> ExistsByAddressAsync(Address address);
        public Task AddAsync(Institution institution);
        public Task<bool> RemoveAsync(Guid id);
    }
}
