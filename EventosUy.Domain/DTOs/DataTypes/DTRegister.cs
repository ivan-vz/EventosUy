using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTRegister
    {
        public string RegisterType { get; init; }
        public string Edition { get; init; }
        public DateTimeOffset Created { get; init; }
        public decimal Total { get; init; }
        public string? Sponsor_code { get; init; }
        public Participation Participation { get; init; }
        public RegisterState State { get; init; }

        public DTRegister(string registerType, string edition, DateTimeOffset created, decimal total, string? sponsor_code, Participation participation, RegisterState state) 
        {
            RegisterType = registerType;
            Edition = edition;
            Created = created;
            Total = total;
            Sponsor_code = sponsor_code;
            Participation = participation;
            State = state;
        }
    }
}
