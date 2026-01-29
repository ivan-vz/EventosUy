using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using FluentValidation.Results;

namespace EventosUy.Application.Services
{
    public class ClientService(IClientRepo clientRepo) : IClientService
    {
        private readonly IClientRepo _repo = clientRepo;

        public async Task<(DTClient? Client, ValidationResult validation)> CreateAsync(DTInsertClient dtInsert)
        {
            var validationResult = new ValidationResult();

            if (await _repo.ExistsByEmailAsync(dtInsert.Email)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Email", "Emails already in use.")
                    );
            }

            if (await _repo.ExistsByNicknameAsync(dtInsert.Nickname))
            {
                validationResult.Errors.Add(
                    new ValidationFailure("Nickname", "Nickname already in use.")
                );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            var hash = PasswordHasher.Hash(dtInsert.Password);

            var client = new Client(
                nickname: dtInsert.Nickname,
                password: hash,
                email: dtInsert.Email,
                firstName: dtInsert.FirstName,
                lastName: dtInsert.LastName,
                firstSurname: dtInsert.FirstSurname,
                lastSurname: dtInsert.LastSurname,
                birthday: dtInsert.Birthday,
                ci: dtInsert.Ci
            );
            
            await _repo.AddAsync(client);

            var dtClient = new DTClient(
                id: client.Id,
                nickname: client.Nickname,
                email: client.Email,
                firstName: client.FirstName,
                lastName: client.LastName,
                firstSurname: client.FirstSurname,
                lastSurname: client.LastSurname,
                birthday: client.Birthday,
                created: client.Created,
                ci: client.Ci
            );

            return (dtClient, validationResult);
        }

        public async Task<IEnumerable<UserCard>> GetAllAsync()
        {
            List<Client> clients = await _repo.GetAllAsync();
            List<UserCard> cards = [.. clients.Select(client => new UserCard(client.Id, client.Nickname, client.Email) )];

            return cards;
        }

        public async Task<(DTClient? dt, UserCard? card)> GetByIdAsync(Guid id)
        {
            Client? client = await _repo.GetByIdAsync(id);

            if (client is null) { return (null, null); }
            
            var dt = new DTClient(
                id: client.Id,
                nickname: client.Nickname,
                email: client.Email,
                firstName: client.FirstName,
                lastName: client.LastName,
                firstSurname: client.FirstSurname,
                lastSurname: client.LastSurname,
                birthday: client.Birthday,
                created: client.Created,
                ci: client.Ci
            );

            var card = new UserCard(client.Id, client.Nickname, client.Email);

            return (dt, card);
        }

        public async Task<(DTClient? Client, ValidationResult validation)> UpdateAsync(DTUpdateClient dtUpdate)
        {
            var client = await _repo.GetByIdAsync(dtUpdate.Id);

            var validationResult = new ValidationResult();
            
            if (client == null) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Id", "Client not found.")
                    );
                
                return (null, validationResult);
            }

            if ( client.Nickname != dtUpdate.Nickname && await _repo.ExistsByNicknameAsync(dtUpdate.Nickname) )
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Nickname", "Nickname is already in use.")
                    );
            }

            if (client.Email != dtUpdate.Email && await _repo.ExistsByEmailAsync(dtUpdate.Email) )
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Email", "Email is already in use.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            client.Nickname = dtUpdate.Nickname;
            client.Email = dtUpdate.Email;

            if (!PasswordHasher.Verify(dtUpdate.Password, client.Password)) { client.Password = PasswordHasher.Hash(dtUpdate.Password); }

            var dt = new DTClient(
                id: client.Id,
                nickname: client.Nickname,
                email: client.Email,
                firstName: client.FirstName,
                lastName: client.LastName,
                firstSurname: client.FirstSurname,
                lastSurname: client.LastSurname,
                birthday: client.Birthday,
                created: client.Created,
                ci: client.Ci
            );

            return (dt, validationResult);
        }

        public async Task<DTClient?> DeleteAsync(Guid id)
        {
            var client = await _repo.GetByIdAsync(id);

            if (client == null) { return null; }

            client.Active = false;

            var dt = new DTClient(
                id: client.Id,
                nickname: client.Nickname,
                email: client.Email,
                firstName: client.FirstName,
                lastName: client.LastName,
                firstSurname: client.FirstSurname,
                lastSurname: client.LastSurname,
                birthday: client.Birthday,
                created: client.Created,
                ci: client.Ci
            );

            return dt;
        }
    }
}
