using EventosUy.Domain.Entities;
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

        public Task<List<Guid>> GetAllByInstitutionn(Guid institutionId) 
        { 
            return Task.FromResult(_employments.Where(employment => employment.Institution == institutionId).Select(employment => employment.Id).ToList()); 
        }

        public Task<List<Guid>> GetAllByProfessional(Guid professionalId)
        {
            return Task.FromResult(_employments.Where(employment => employment.ProfessionalProfile == professionalId).Select(employment => employment.Id).ToList());
        }

        public Task<Employment?> GetByIdAsync(Guid id) { return Task.FromResult(_employments.SingleOrDefault(employment => employment.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _employments.RemoveWhere(employment => employment.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
