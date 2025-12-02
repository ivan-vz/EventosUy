using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;
using System.Net;

namespace EventosUy.Infrastructure.Repositorios
{
    internal class InstitutionRepo : IInstitutionRepo
    {
        private readonly HashSet<Institution> _institutions;

        public InstitutionRepo() { _institutions = []; }

        public Task AddAsync(Institution institution)
        {
            _institutions.Add(institution);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByAddressAsync(Address address) { return Task.FromResult(_institutions.Any(institution => institution.Address.Equals(address))); }

        public Task<bool> ExistsByEmailAsync(Email email) { return Task.FromResult(_institutions.Any(institution => institution.Email.Equals(email))); }

        public Task<bool> ExistsByNicknameAsync(string nickname) { return Task.FromResult(_institutions.Any(institution => institution.Nickname.Equals(nickname))); }

        public Task<bool> ExistsByUrlAsync(Url url) { return Task.FromResult(_institutions.Any(institution => institution.Url.Equals(url))); }

        public Task<List<Guid>> GetAllAsync() { return Task.FromResult(_institutions.Select(institution => institution.Id).ToList()); }

        public Task<Institution?> GetByIdAsync(Guid id) { return Task.FromResult(_institutions.SingleOrDefault(institution => institution.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _institutions.RemoveWhere(institution => institution.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
