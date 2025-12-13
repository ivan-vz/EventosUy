using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class JobTitleService : IJobTitleService
    {
        private readonly IJobTitleRepo _repo;
        private readonly IInstitutionService _institutionService;

        public JobTitleService(IJobTitleRepo jobTitleRepo, IInstitutionService institutionService) 
        {
            _repo = jobTitleRepo;
            _institutionService = institutionService;
        } 

        public async Task<Result<Guid>> CreateAsync(string name, string descripcion, Guid institutionId)
        {
            List<string> errors = [];
            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { errors.AddRange(institutionResult.Errors); }

            Result<JobTitle> jobTitleResult = JobTitle.Create(name, descripcion, institutionId);
            if (!jobTitleResult.IsSuccess) { errors.AddRange(jobTitleResult.Errors); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            JobTitle jobTitleInstance = jobTitleResult.Value!;
            await _repo.AddAsync(jobTitleInstance);

            return Result<Guid>.Success(jobTitleInstance.Id);
        }

        public async Task<Result<List<JobTitleCard>>> GetAllByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<JobTitleCard>>.Failure("Institution can not be empty."); }
            List<JobTitle> jobTitles = await _repo.GetAllByInstitutionAsync(institutionId);
            List<JobTitleCard> cards = jobTitles.Select(job => job.GetCard()).ToList();
            
            return Result<List<JobTitleCard>>.Success(cards);
        }

        public async Task<Result<JobTitle>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<JobTitle>.Failure("Institution can not be empty."); }
            JobTitle? jobTitleInstance = await _repo.GetByIdAsync(id);
            if (jobTitleInstance is null) { return Result<JobTitle>.Failure("Job Title not Found."); }

            return Result<JobTitle>.Success(jobTitleInstance);
        }

        public async Task<Result<DTJobTitle>> GetDTAsync(Guid id)
        {
            Result<JobTitle> jobTitleResult = await GetByIdAsync(id);
            if (!jobTitleResult.IsSuccess) { return Result<DTJobTitle>.Failure(jobTitleResult.Errors); }

            JobTitle jobTitleInstance = jobTitleResult.Value!;
            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(jobTitleInstance.Institution);
            if (!institutionResult.IsSuccess) { return Result<DTJobTitle>.Failure(institutionResult.Errors); }

            return Result<DTJobTitle>.Success(jobTitleInstance.GetDT(institutionResult.Value!));
        }
    }
}
