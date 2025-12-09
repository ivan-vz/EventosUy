using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Services
{
    internal class PersonService : IPersonService
    {
        private readonly IPersonRepo _repo;

        public PersonService(IPersonRepo personRepo) 
        {
            _repo = personRepo;
        }

        public async Task<Result<Guid>> CreateAsync(string nickname, string password, string email, string firstName, string? lastName, string firstSurname, string lastSurname, DateOnly birthday)
        {
            List<string> errors = [];
            Result<Password> passwordResult = Password.Create(password);
            if (passwordResult.IsFailure) { errors.AddRange(passwordResult.Errors); }
            
            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure) { errors.AddRange(emailResult.Errors); }

            Result<Name> nameResult = Name.Create(firstSurname, lastSurname, firstName, lastName);
            if (nameResult.IsFailure) { errors.AddRange(nameResult.Errors); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            if (await _repo.ExistsByEmailAsync(emailResult.Value!)) { errors.Add("Email already in use.");  }
            if (await _repo.ExistsByNicknameAsync(nickname)) { errors.Add("Nickname already in use."); }
            
            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            Result<Person> personResult = Person.Create(nickname, passwordResult.Value!, emailResult.Value!, nameResult.Value!, birthday);
            if (personResult.IsFailure) { return Result<Guid>.Failure(personResult.Errors); }

            Person personInstance = personResult.Value!;
            await _repo.AddAsync(personInstance);

            return Result<Guid>.Success(personInstance.Id);
        }

        public async Task<Result<List<ProfileCard>>> GetAllAsync()
        {
            List<Person> persons = await _repo.GetAllAsync();
            List<ProfileCard> cards = persons.Select(person => person.GetCard()).ToList();

            return Result<List<ProfileCard>>.Success(cards);
        }

        public async Task<Result<List<ProfileCard>>> GetAllExceptAsync(List<Guid> ids)
        {
            List<Person> persons = await _repo.GetAllExceptAsync(ids);
            List<ProfileCard> cards = persons.Select(person => person.GetCard()).ToList();

            return Result<List<ProfileCard>>.Success(cards);
        }

        public async Task<Result<Person>> GetByIdAsync(Guid personId)
        {
            if (personId == Guid.Empty) { return Result<Person>.Failure("Person can not be empty."); }
            Person? personInstance = await _repo.GetByIdAsync(personId);
            if (personInstance is null) { return Result<Person>.Failure("Person not Found."); }

            return Result<Person>.Success(personInstance);
        }

        public async Task<Result<DTPerson>> GetDT(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTPerson>.Failure("Person can not be empty."); }
            Person? personInstance = await _repo.GetByIdAsync(id);
            if (personInstance is null) { return Result<DTPerson>.Failure("Person not Found."); }

            return Result<DTPerson>.Success(personInstance.GetDT());
        }
    }
}
