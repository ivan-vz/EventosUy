using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IPersonRepo _repo;

        public ClientService(IPersonRepo personRepo) 
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

            Result<Client> personResult = Client.Create(nickname, passwordResult.Value!, emailResult.Value!, nameResult.Value!, birthday);
            if (personResult.IsFailure) { return Result<Guid>.Failure(personResult.Errors); }

            Client personInstance = personResult.Value!;
            await _repo.AddAsync(personInstance);

            return Result<Guid>.Success(personInstance.Id);
        }

        public async Task<IEnumerable<UserCard>> GetAllAsync()
        {
            List<Client> persons = await _repo.GetAllAsync();
            List<UserCard> cards = persons.Select(person => person.GetCard()).ToList();

            return cards;
        }

        public async Task<DTClient?> GetByIdAsync(Guid personId)
        {
            Client? clientInstance = await _repo.GetByIdAsync(personId);

            if (clientInstance is null) { return null; }
            
            var dt = new DTClient
            (
                clientInstance.Id,
                clientInstance.Nickname,
                clientInstance.Email.Value,
                clientInstance.Name.FullName,
                clientInstance.Birthday,
                clientInstance.Created,
                clientInstance.CI.GetFormatted()
            );

            return dt;
        }

        public async Task<Result<DTClient>> GetDT(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTClient>.Failure("Person can not be empty."); }
            Client? personInstance = await _repo.GetByIdAsync(id);
            if (personInstance is null) { return Result<DTClient>.Failure("Person not Found."); }

            return Result<DTClient>.Success(personInstance.GetDT());
        }
    }
}
