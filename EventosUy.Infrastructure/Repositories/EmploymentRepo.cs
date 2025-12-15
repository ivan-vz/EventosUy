using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class EmploymentRepo : IEmploymentRepo
    {
        private readonly HashSet<Employment> _employments;

        public EmploymentRepo() { _employments = []; }

        public Task AddAsync(Employment employment)
        {
            _employments.Add(employment);
            return Task.CompletedTask;
        }

        public Task<List<Employment>> GetAllByInstitutionAsync(Guid institutionId) { return Task.FromResult(_employments.Where(employment => employment.Institution == institutionId).ToList()); }

        public Task<List<Employment>> GetAllByProfessionalAsync(Guid professionalId) 
        {
            return Task.FromResult(_employments.Where(employment => employment.ProfessionalProfile == professionalId).ToList());
        }

        public Task<Employment?> GetByIdAsync(Guid id) { return Task.FromResult(_employments.SingleOrDefault(employment => employment.Id == id)); }

        public Task<bool> ExistsAsync(Guid institutionId, Guid professionalId, Guid jobTitleId) 
        { 
            return Task.FromResult(_employments.Any(employment => employment.Institution == institutionId && employment.ProfessionalProfile == professionalId && employment.JobTitle == jobTitleId));
        }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _employments.RemoveWhere(employment => employment.Id == id);
            return Task.FromResult(result > 0);
        }

        public Task<bool> ValidateContractAsync(Guid person, Guid institution)
        {
            return Task.FromResult(_employments.Any(employment => employment.State == EmploymentState.ACTIVE && employment.Institution == institution && employment.ProfessionalProfile == person));
        }
    }
}
