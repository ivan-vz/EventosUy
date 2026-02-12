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
    public class InstitutionService(IInstitutionRepo institutionRepo) : IInstitutionService
    {
        private readonly IInstitutionRepo _repo = institutionRepo;

        public async Task<(DTInstitution? dt, ValidationResult validation)> CreateAsync(DTInsertInstitution dtInsert)
        {
            var validationResult = new ValidationResult();

            if (await _repo.ExistsByNicknameAsync(dtInsert.Nickname)) 
            { 
                 validationResult.Errors.Add(
                    new ValidationFailure("Nickname", "Nickname already in use.")
                 ); 
            }
            if (await _repo.ExistsByEmailAsync(dtInsert.Email)) 
            {
                 validationResult.Errors.Add(
                    new ValidationFailure("Email", "Email already in use.")
                 );
            }

            if (await _repo.ExistsByAcronymAsync(dtInsert.Acronym)) 
            {
                 validationResult.Errors.Add(
                    new ValidationFailure("Acronym", "Acronym already in use.")
                 );
            }

            if (await _repo.ExistsByUrlAsync(dtInsert.Url)) 
            { 
                 validationResult.Errors.Add(
                    new ValidationFailure("Url", "Url already in use.")
                 ); 
            }

            if (await _repo.ExistsByAddressAsync(dtInsert.Country, dtInsert.City, dtInsert.Street, dtInsert.Number, dtInsert.Floor))
            {
                validationResult.Errors.Add(
                    new ValidationFailure("Address", "Address already in use.")
                 );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            var hash = PasswordHasher.Hash(dtInsert.Password);

            var institution = new Institution
                (
                    nickname: dtInsert.Nickname,
                    password: hash,
                    acronym: dtInsert.Acronym,
                    email: dtInsert.Email,
                    name: dtInsert.Name,
                    description: dtInsert.Description,
                    url: dtInsert.Url,
                    country: dtInsert.Country,
                    city: dtInsert.City,
                    street: dtInsert.Street,
                    number: dtInsert.Number,
                    floor: dtInsert.Floor
                );

            await _repo.AddAsync(institution);

            var dtInstitution = new DTInstitution
                (
                    id: institution.Id,
                    nickname: institution.Nickname,
                    email: institution.Email,
                    name: institution.Name,
                    acronym: institution.Acronym,
                    description: institution.Description,
                    url: institution.Url,
                    country: institution.Country,
                    city: institution.City,
                    street: institution.Street,
                    number: institution.Number,
                    floor: institution.Floor,
                    created: institution.Created
                );

            return (dtInstitution, validationResult);
        }

        public async Task<IEnumerable<UserCard>> GetAllAsync()
        {
            List<Institution> institutions = await _repo.GetAllAsync();
            List<UserCard> cards = [.. institutions.Select(institution => new UserCard(institution.Id, institution.Nickname, institution.Email))];

            return cards;
        }

        public async Task<(DTInstitution? dt, UserCard? card)> GetByIdAsync(Guid id)
        {
            Institution? institution = await _repo.GetByIdAsync(id);

            if (institution is null) { return (null, null); }
            
            var dt = new DTInstitution
                (
                    id: institution.Id,
                    nickname: institution.Nickname,
                    email: institution.Email,
                    name: institution.Name,
                    acronym: institution.Acronym,
                    description: institution.Description,
                    url: institution.Url,
                    country: institution.Country,
                    city: institution.City,
                    street: institution.Street,
                    number: institution.Number,
                    floor: institution.Floor,
                    created: institution.Created
                );

            var card = new UserCard(institution.Id, institution.Nickname, institution.Email);

            return (dt, card);
        }

        public async Task<(DTInstitution? dt, ValidationResult validation)> UpdateAsync(DTUpdateInstitution dtUpdate)
        {
            var institution = await _repo.GetByIdAsync(dtUpdate.Id);

            var validationResult = new ValidationResult();

            if (institution == null) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Id", "Institution not found.")
                    );

                return (null, validationResult);
            }

            if (institution.Nickname != dtUpdate.Nickname && await _repo.ExistsByNicknameAsync(dtUpdate.Nickname))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Nickname", "Nickname is already in use.")
                    );
            }

            if (institution.Email != dtUpdate.Email && await _repo.ExistsByEmailAsync(dtUpdate.Email))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Email", "Email is already in use.")
                    );
            }

            if (institution.Url != dtUpdate.Url && await _repo.ExistsByUrlAsync(dtUpdate.Url))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Url", "Url is already in use.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            institution.Nickname = dtUpdate.Nickname;
            institution.Email = dtUpdate.Email;
            institution.Description = dtUpdate.Description;
            institution.Url = dtUpdate.Url;
            if (!PasswordHasher.Verify(dtUpdate.Password, institution.Password)) { institution.Password = PasswordHasher.Hash(dtUpdate.Password); }

            var dt = new DTInstitution
                (
                    id: institution.Id,
                    nickname: institution.Nickname,
                    email: institution.Email,
                    name: institution.Name,
                    acronym: institution.Acronym,
                    description: institution.Description,
                    url: institution.Url,
                    country: institution.Country,
                    city: institution.City,
                    street: institution.Street,
                    number: institution.Number,
                    floor: institution.Floor,
                    created: institution.Created
                );

            return (dt, validationResult);
        }

        public async Task<DTInstitution?> DeleteAsync(Guid id)
        {
            var institution = await _repo.GetByIdAsync(id);

            if (institution == null) { return null; }

            institution.Active = false;
            
            var dt = new DTInstitution
                (
                    id: institution.Id,
                    nickname: institution.Nickname,
                    email: institution.Email,
                    name: institution.Name,
                    acronym: institution.Acronym,
                    description: institution.Description,
                    url: institution.Url,
                    country: institution.Country,
                    city: institution.City,
                    street: institution.Street,
                    number: institution.Number,
                    floor: institution.Floor,
                    created: institution.Created
                );

            return dt;
        }
    }
}
