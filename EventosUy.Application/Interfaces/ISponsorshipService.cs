
namespace EventosUy.Application.Interfaces
{
    internal interface ISponsorshipService
    {
        Task<bool> ValidateCode(Guid registerTypeId, string sponsorCode);
    }
}
