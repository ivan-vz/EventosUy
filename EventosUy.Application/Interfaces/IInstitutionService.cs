using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IInstitutionService
    {
        public Task<Result<Guid>> CreateAsync(string nickname, string password, string email, string name, string description, string url, string country, string city, string street, string number);
        public Task<Result<Institution>> GetByIdAsync(Guid id);
        public Task<Result<List<ProfileCard>>> GetAllAsync();
        public Task<Result<DTInsitution>> GetDTAsync(Guid id);
    }
}
