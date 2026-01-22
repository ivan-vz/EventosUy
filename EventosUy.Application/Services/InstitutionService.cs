using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Services
{
    public class InstitutionService : IInstitutionService
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

        public async Task<IEnumerable<UserCard>> GetAllAsync()
        {
            List<Institution> institutions = await _repo.GetAllAsync();
            List<UserCard> cards = institutions.Select(institution => institution.GetCard()).ToList();

            return cards;
        }

        public async Task<DTInstitution?> GetByIdAsync(Guid id)
        {
            Institution? institutionInstance = await _repo.GetByIdAsync(id);

            if (institutionInstance is null) { return null; }
            
            var dt = new DTInstitution
                (
                    institutionInstance.Nickname,
                    institutionInstance.Email.Value,
                    institutionInstance.Name,
                    institutionInstance.Acronym,
                    institutionInstance.Url.Value,
                    institutionInstance.Description,
                    institutionInstance.Address.FullAddress,
                    institutionInstance.Created
                );

            return dt;
        }

        public async Task<Result<DTInstitution>> GetDTAsync(Guid id)
        {
            Result<Institution> institutionResult = await GetByIdAsync(id);
            if (institutionResult.IsFailure) { return Result<DTInstitution>.Failure(institutionResult.Errors); }

            Institution institutionInstance = institutionResult.Value!;
            return Result<DTInstitution>.Success(institutionInstance.GetDT());
        }
    }
}
