using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using FluentValidation.Results;

namespace EventosUy.Application.Services
{
    public class RegisterTypeService : IRegisterTypeService
    {
        private readonly IRegisterTypeRepo _repo;
        private readonly IEditionService _editionService;

        public RegisterTypeService(IRegisterTypeRepo registerTypeRepo, IEditionService editionService)
        {
            _repo = registerTypeRepo;
            _editionService = editionService;
        }

        public async Task<(DTRegisterType?, ValidationResult)> CreateAsync(DTInsertRegisterType dtInsert)
        {
            var validationResult = new ValidationResult();

            if (await _repo.ExistsAsync(dtInsert.Name)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Name", "Name already in use.")
                    );
            }

            var edition = _editionService.GetByIdAsync(dtInsert.Edition);

            if (edition is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition", "Edition not found.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }
            
            var registerType = new RegisterType(
                name: dtInsert.Name, 
                description: dtInsert.Description, 
                price: dtInsert.Price, 
                quota: dtInsert.Quota, 
                editionId: dtInsert.Edition
                );

            await _repo.AddAsync(registerType);

            var editionCard = await _editionService.GetCardByIdAsync(dtInsert.Edition);

            var dt = new DTRegisterType(
                    id: registerType.Id,
                    name: registerType.Name,
                    description: registerType.Description,
                    price: registerType.Price,
                    quota: registerType.Quota,
                    created: registerType.Created,
                    editionCard: editionCard!
                );

            return (dt, validationResult);
        }

        public async Task<IEnumerable<RegisterTypeCard>> GetAllByEditionAsync(Guid editionId)
        {
            List<RegisterType> registerTypes = await _repo.GetAllByEditionAsync(editionId);
            List<RegisterTypeCard> cards = [.. registerTypes.Select(rt => new RegisterTypeCard(rt.Id, rt.Name) )];

            return cards;
        }

        public async Task<DTRegisterType?> GetByIdAsync(Guid id)
        {
            var registerType = await _repo.GetByIdAsync(id);

            if (registerType is null) { return null; }

            var editionCard = await _editionService.GetCardByIdAsync(registerType.Edition);

            var dt = new DTRegisterType(
                    id: registerType.Id,
                    name: registerType.Name,
                    description: registerType.Description,
                    price: registerType.Price,
                    quota: registerType.Quota,
                    created: registerType.Created,
                    editionCard: editionCard!
                );

            return dt;
        }

        public async Task<DTRegisterType?> DeleteAsync(Guid id)
        {
            var registerType = await _repo.GetByIdAsync(id);

            if (registerType is null) { return null; }

            registerType.Active = false;

            var editionCard = await _editionService.GetCardByIdAsync(registerType.Edition);

            var dt = new DTRegisterType(
                    id: registerType.Id,
                    name: registerType.Name,
                    description: registerType.Description,
                    price: registerType.Price,
                    quota: registerType.Quota,
                    created: registerType.Created,
                    editionCard: editionCard!
                );

            return dt;
        }
    }
}
