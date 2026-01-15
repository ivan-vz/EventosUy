using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Infrastructure.Repositories
{
    internal class PersonRepo : IPersonRepo
    {
        private readonly HashSet<Client> _persons;

        public PersonRepo() { _persons = []; }

        public Task AddAsync(Client person)
        {
            _persons.Add(person);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByEmailAsync(Email email) { return Task.FromResult(_persons.Any(person => person.Email.Equals(email))); }

        public Task<bool> ExistsByNicknameAsync(string nickname) { return Task.FromResult(_persons.Any(person => person.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase))); }

        public Task<List<Client>> GetAllAsync() { return Task.FromResult(_persons.ToList()); }

        public Task<List<Client>> GetAllExceptAsync(List<Guid> ids) { return Task.FromResult(_persons.ExceptBy(ids, person => person.Id).ToList()); }

        public Task<Client?> GetByIdAsync(Guid id) { return Task.FromResult(_persons.SingleOrDefault(person => person.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _persons.RemoveWhere(person => person.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
