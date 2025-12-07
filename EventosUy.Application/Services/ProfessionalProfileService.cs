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

        public async Task<Result<Guid>> RequestVerificationAsync(Url linkTree, List<string> specialities, Guid personId)
        {
            Result<Person> personResult = await _personService.GetByIdAsync(personId);
            if (!personResult.IsSuccess) { return Result<Guid>.Failure(personResult.Error!); }

            Result<ProfessionalProfile> professionalResult = ProfessionalProfile.Create(personId, linkTree, specialities);
            if (!professionalResult.IsSuccess) { return Result<Guid>.Failure(professionalResult.Error!); }

            await _repo.AddAsync(professionalResult.Value!);

            return Result<Guid>.Success(personId);
        }

        public async Task<Result> ApproveAsync(Guid personId)
        {
            Result<ProfessionalProfile> professionalResult = await GetByIdAsync(personId);
            if (!professionalResult.IsSuccess) { return Result.Failure(professionalResult.Error!); }

            professionalResult.Value!.Approve();

            return Result.Success();
        }

        public async Task<Result<List<ProfileCard>>> GetAllAsync()
        {
            List<ProfessionalProfile> professionals = await _repo.GetAllAsync();

            List<ProfileCard> cards = [];
            foreach (ProfessionalProfile professional in professionals)
            {
                Result<Person> personResult = await _personService.GetByIdAsync(professional.Id);
                if (!personResult.IsSuccess) { return Result<List<ProfileCard>>.Failure(personResult.Error!); }

                cards.Add(professional.GetCard(personResult.Value!));
            }

            return Result<List<ProfileCard>>.Success(cards);
        }

        public async Task<Result<List<ProfileCard>>> GetAllPendingAsync()
        {
            List<ProfessionalProfile> professionals = await _repo.GetAllPendingAsync();
            
            List<ProfileCard> cards = [];
            foreach (ProfessionalProfile professional in professionals) 
            {
                Result<Person> personResult = await _personService.GetByIdAsync(professional.Id);
                if (!personResult.IsSuccess) { return Result<List<ProfileCard>>.Failure(personResult.Error!); }

                cards.Add(professional.GetCard(personResult.Value!));
            }

            return Result<List<ProfileCard>>.Success(cards);
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
            if (!professionalResult.IsSuccess) { return Result.Failure(professionalResult.Error!); }

            professionalResult.Value!.Reject();

            return Result.Success();
        }
    }
}
