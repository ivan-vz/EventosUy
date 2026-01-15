using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Services
{

    internal class ProfessionalProfileService : IProfessionalProfileService
    {
        private readonly IProfessionalProfileRepo _repo;
        private readonly IPersonService _personService;

        public ProfessionalProfileService(IProfessionalProfileRepo professionalProfileRepo, IPersonService personService) 
        {
            _repo = professionalProfileRepo;
            _personService = personService;
        }

        public async Task<Result<Guid>> RequestVerificationAsync(string linkTree, List<string> specialities, Guid personId)
        {
            List<string> errors = [];
            Result<Url> urlResult = Url.Create(linkTree);
            if (urlResult.IsFailure) { errors.AddRange(urlResult.Errors); }

            Result<Client> personResult = await _personService.GetByIdAsync(personId);
            if (!personResult.IsSuccess) { errors.AddRange(personResult.Errors); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            Result<ProfessionalProfile> professionalResult = ProfessionalProfile.Create(personId, urlResult.Value!, specialities);
            if (!professionalResult.IsSuccess) { return Result<Guid>.Failure(professionalResult.Errors); }

            await _repo.AddAsync(professionalResult.Value!);

            return Result<Guid>.Success(personId);
        }

        public async Task<Result> ApproveAsync(Guid personId)
        {
            Result<ProfessionalProfile> professionalResult = await GetByIdAsync(personId);
            if (!professionalResult.IsSuccess) { return Result.Failure(professionalResult.Errors); }

            professionalResult.Value!.Approve();

            return Result.Success();
        }

        public async Task<Result<List<UserCard>>> GetAllAsync()
        {
            List<ProfessionalProfile> professionals = await _repo.GetAllVerifiedAsync();

            List<UserCard> cards = [];
            List<string> errors = [];
            foreach (ProfessionalProfile professional in professionals)
            {
                Result<Client> personResult = await _personService.GetByIdAsync(professional.Id);
                if (!personResult.IsSuccess) { errors.AddRange(personResult.Errors); }

                cards.Add(professional.GetCard(personResult.Value!));
            }

            if (errors.Any()) { return Result<List<UserCard>>.Failure(errors); }
            
            return Result<List<UserCard>>.Success(cards);
        }

        public async Task<Result<List<UserCard>>> GetAllPendingAsync()
        {
            List<ProfessionalProfile> professionals = await _repo.GetAllPendingAsync();
            
            List<UserCard> cards = [];
            List<string> errors = [];
            foreach (ProfessionalProfile professional in professionals) 
            {
                Result<Client> personResult = await _personService.GetByIdAsync(professional.Id);
                if (!personResult.IsSuccess) { errors.AddRange(personResult.Errors); }

                cards.Add(professional.GetCard(personResult.Value!));
            }

            if (errors.Any()) { return Result<List<UserCard>>.Failure(errors); }

            return Result<List<UserCard>>.Success(cards);
        }

        public async Task<Result<List<UserCard>>> GetAllUnverifiedAsync()
        {
            List<ProfessionalProfile> professionals = await _repo.GetAllVerifiedAsync();
            List<Guid> verifiedIds = professionals.Select(p => p.Id).ToList();

            Result<List<UserCard>> personsResult = await _personService.GetAllExceptAsync(verifiedIds);
            if (personsResult.IsFailure) { return Result<List<UserCard>>.Failure(personsResult.Errors); }

            return Result<List<UserCard>>.Success(personsResult.Value!);
        }

        public async Task<Result<ProfessionalProfile>> GetByIdAsync(Guid professionalId)
        {
            if (professionalId == Guid.Empty) { return Result<ProfessionalProfile>.Failure("Professional can not be empty"); }
            ProfessionalProfile? professionalInstance = await _repo.GetByPersonAsync(professionalId);
            if (professionalInstance is null) { return Result<ProfessionalProfile>.Failure("Profile not Found."); }

            return Result<ProfessionalProfile>.Success(professionalInstance);
        }

        public async Task<Result<DTProfessionalProfile>> GetDTAsync(Guid personId)
        {
            if (personId == Guid.Empty) { return Result<DTProfessionalProfile>.Failure("Professional can not be empty"); }
            ProfessionalProfile? professionalInstance = await _repo.GetByPersonAsync(personId);
            if (professionalInstance is null) { return Result<DTProfessionalProfile>.Failure("Profile not Found."); }

            return Result<DTProfessionalProfile>.Success(professionalInstance.GetDT());
        }

        public async Task<Result> RejectAsync(Guid personId)
        {
            Result<ProfessionalProfile> professionalResult = await GetByIdAsync(personId);
            if (!professionalResult.IsSuccess) { return Result.Failure(professionalResult.Errors); }

            professionalResult.Value!.Reject();

            return Result.Success();
        }
    }
}
