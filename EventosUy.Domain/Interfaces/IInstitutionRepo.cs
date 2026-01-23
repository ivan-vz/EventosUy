using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IInstitutionRepo
    {
        public Task<Institution?> GetByIdAsync(Guid id);
        public Task<List<Institution>> GetAllAsync();
        public Task<bool> ExistsByNicknameAsync(string nickname);
        public Task<bool> ExistsByEmailAsync(string email);
        public Task<bool> ExistsByAcronymAsync(string acronym);
        public Task<bool> ExistsByUrlAsync(string url);
        public Task<bool> ExistsByAddressAsync(string country, string city, string street, string number, int floor);
        public Task AddAsync(Institution institution);
        public Task<bool> RemoveAsync(Guid id);
    }
}
