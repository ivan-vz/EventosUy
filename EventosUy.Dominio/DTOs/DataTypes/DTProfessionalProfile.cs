using EventosUy.Dominio.Enumerados;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTProfessionalProfile
    {
        public string LinkTree { get; init; }
        public List<String> Specialities { get; init; }
        public RequestState State { get; init; }

        public DTProfessionalProfile(string linkTree, List<string> specialities, RequestState state)
        {
            LinkTree = linkTree;
            Specialities = specialities;
            State = state;
        }
    }
}
