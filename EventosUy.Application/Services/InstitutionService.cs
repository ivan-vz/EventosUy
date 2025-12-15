using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Services
{
    internal class InstitutionService : IInstitutionService
    {
        private readonly IInstitutionRepo _repo;

        public InstitutionService(IInstitutionRepo institutionRepo) { _repo = institutionRepo; }

        public async Task<Result<Guid>> CreateAsync(string nickname, string password, string email, string name, string acronym, string description, string url, string country, string city, string street, string number)
        {
            List<string> errors = [];
            Result<Password> passwordResult = Password.Create(password);
            if (passwordResult.IsFailure) { errors.AddRange(passwordResult.Errors); }

            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure) { errors.AddRange(emailResult.Errors); }

            Result<Url> urlResult = Url.Create(url);
            if (urlResult.IsFailure) { errors.AddRange(urlResult.Errors); }

            Result<Address> addressResult = Address.Create(country, city, street, number);
            if (addressResult.IsFailure) { errors.AddRange(addressResult.Errors); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            if (await _repo.ExistsByNicknameAsync(nickname)) { errors.Add("Nickname already in use."); }
            if (await _repo.ExistsByEmailAsync(emailResult.Value!)) { errors.Add("Email already in use."); }
            if (await _repo.ExistsByAcronymAsync(acronym)) { errors.Add("Acronym already in use."); }
            if (await _repo.ExistsByUrlAsync(urlResult.Value!)) { errors.Add("Url already in use."); }
            if (await _repo.ExistsByAddressAsync(addressResult.Value!)) { errors.Add("Address already in use."); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            Result<Institution> institutionResult = Institution.Create(nickname, acronym, passwordResult.Value!, emailResult.Value!, name, urlResult.Value!, addressResult.Value!, description);
            if (institutionResult.IsFailure) { return Result<Guid>.Failure(institutionResult.Errors); }

            Institution institutionInstance = institutionResult.Value!;
            return Result<Guid>.Success(institutionInstance.Id);
        }

        public async Task<Result<List<ProfileCard>>> GetAllAsync()
        {
            List<Institution> institutions = await _repo.GetAllAsync();
            List<ProfileCard> cards = institutions.Select(institution => institution.GetCard()).ToList();

            return Result<List<ProfileCard>>.Success(cards);
        }

        public async Task<Result<Institution>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Institution>.Failure("Institution can not be empty."); }
            Institution? institutionInstance = await _repo.GetByIdAsync(id);

            if (institutionInstance is null) { return Result<Institution>.Failure("Institution not Found."); }

            return Result<Institution>.Success(institutionInstance);
        }

        public async Task<Result<DTInsitution>> GetDTAsync(Guid id)
        {
            Result<Institution> institutionResult = await GetByIdAsync(id);
            if (institutionResult.IsFailure) { return Result<DTInsitution>.Failure(institutionResult.Errors); }

            Institution institutionInstance = institutionResult.Value!;
            return Result<DTInsitution>.Success(institutionInstance.GetDT());
        }
    }
}
