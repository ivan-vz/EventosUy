using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.Interfaces
{
    public interface IInstitutionService
    {
        public Task<Result<Guid>> CreateAsync(string nickname, string password, string email, string name, string acronym, string description, string url, string country, string city, string street, string number);
        public Task<DTInstitution?> GetByIdAsync(Guid id);
        public Task<IEnumerable<UserCard>> GetAllAsync();
        public Task<Result<DTInstitution>> GetDTAsync(Guid id);
    }
}
