using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class EmploymentService : IEmploymentService
    {
        private readonly IEmploymentRepo _repo;
        private readonly IPersonService _personService;
        private readonly IInstitutionService _institutionService;
        private readonly IJobTitleService _jobTitleService;
        private readonly IProfessionalProfileService _professionalProfileService;

        public EmploymentService(IEmploymentRepo employmentRepo, IPersonService personService, IInstitutionService institutionService, IJobTitleService jobTitleService, IProfessionalProfileService professionalProfileService)
        {
            _repo = employmentRepo;
            _personService = personService;
            _institutionService = institutionService;
            _jobTitleService = jobTitleService;
            _professionalProfileService = professionalProfileService;
        }

        public async Task<Result<Guid>> CreateAsync(DateOnly from, DateOnly to, Guid institutionId, Guid professionalId, Guid jobTitleId)
        {
            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { return Result<Guid>.Failure(institutionResult.Error!); }

            Result<ProfessionalProfile> professionalResult = await _professionalProfileService.GetByIdAsync(professionalId);
            if (!professionalResult.IsSuccess) { return Result<Guid>.Failure(professionalResult.Error!); }
            
            Result<JobTitle> jobTitleResult = await _jobTitleService.GetByIdAsync(jobTitleId);
            if (!jobTitleResult.IsSuccess) { return Result<Guid>.Failure(jobTitleResult.Error!); }

            if (await _repo.ExistsAsync(institutionId, professionalId, jobTitleId)) { return Result<Guid>.Failure("Employment already exist."); }

            Result<Employment> employmentResult = Employment.Create(from, to, jobTitleId, professionalId, institutionId);
            if (!employmentResult.IsSuccess) { return Result<Guid>.Failure(employmentResult.Error!); }

            Employment employmentInstance = employmentResult.Value!;
            await _repo.AddAsync(employmentInstance);

            return Result<Guid>.Success(employmentInstance.Id);
        }

        public async Task<Result<List<EmploymentCardByInstitution>>> GetAllByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<EmploymentCardByInstitution>>.Failure("Institution can not be empty."); }
            List<Employment> employees = await _repo.GetAllByInstitutionAsync(institutionId);

            List<EmploymentCardByInstitution> cards = [];
            foreach (Employment employee in employees)
            {
                Result<Person> personResult = await _personService.GetByIdAsync(employee.ProfessionalProfile); //Profesional profile tiene el mismo id que la persona a la que representa
                if (!personResult.IsSuccess) { return Result<List<EmploymentCardByInstitution>>.Failure(personResult.Error!); }

                Result<JobTitle> jobTitleResult = await _jobTitleService.GetByIdAsync(employee.JobTitle);
                if (!jobTitleResult.IsSuccess) { return Result<List<EmploymentCardByInstitution>>.Failure(jobTitleResult.Error!); }

                cards.Add(employee.GetEmploymentCardByInstitution(personResult.Value!, jobTitleResult.Value!));
            }

            return Result<List<EmploymentCardByInstitution>>.Success(cards);
        }

        public async Task<Result<List<EmploymentCardByPerson>>> GetAllByPersonAsync(Guid personId)
        {
            if (personId == Guid.Empty) { return Result<List<EmploymentCardByPerson>>.Failure("Person can not be empty."); }
            List<Employment> employees = await _repo.GetAllByProfessionalAsync(personId);

            List<EmploymentCardByPerson> cards = [];
            foreach (Employment employee in employees) 
            {
                Result<Institution> institutionResult = await _institutionService.GetByIdAsync(employee.Institution);
                if (!institutionResult.IsSuccess) { return Result<List<EmploymentCardByPerson>>.Failure(institutionResult.Error!); }

                Result<JobTitle> jobTitleResult = await _jobTitleService.GetByIdAsync(employee.JobTitle);
                if (!jobTitleResult.IsSuccess) { return Result<List<EmploymentCardByPerson>>.Failure(jobTitleResult.Error!); }

                cards.Add(employee.GetEmploymentCardByPerson(institutionResult.Value!, jobTitleResult.Value!));
            }

            return Result<List<EmploymentCardByPerson>>.Success(cards);
        }

        public async Task<Result<DTEmployment>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTEmployment>.Failure("Employment can not be empty."); }
            Employment? employmentInstance = await _repo.GetByIdAsync(id);
            if (employmentInstance is null) { return Result<DTEmployment>.Failure("Employment not Found."); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(employmentInstance.Institution);
            if (!institutionResult.IsSuccess) { return Result<DTEmployment>.Failure(institutionResult.Error!); }

            Result<JobTitle> jobTitleResult = await _jobTitleService.GetByIdAsync(employmentInstance.JobTitle);
            if (!jobTitleResult.IsSuccess) { return Result<DTEmployment>.Failure(jobTitleResult.Error!); }

            return Result<DTEmployment>.Success(employmentInstance.GetDT(institutionResult.Value!, jobTitleResult.Value!));
        }
    }
}
