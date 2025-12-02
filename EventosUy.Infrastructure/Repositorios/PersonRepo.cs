using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Infrastructure.Repositorios
{
    internal class PersonRepo : IPersonRepo
    {
        private readonly HashSet<Person> _persons;

        public PersonRepo() { _persons = []; }

        public Task AddAsync(Person person)
        {
            _persons.Add(person);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByEmailAsync(Email email) { return Task.FromResult(_persons.Any(person => person.Email.Equals(email))); }

        public Task<bool> ExistsByNicknameAsync(string nickname) { return Task.FromResult(_persons.Any(person => person.Nickname.Equals(nickname))); }

        public Task<List<Guid>> GetAllAsync() { return Task.FromResult(_persons.Select(person => person.Id).ToList()); }

        public Task<Person?> GetByIdAsync(Guid id) { return Task.FromResult(_persons.SingleOrDefault(person => person.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _persons.RemoveWhere(person => person.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
