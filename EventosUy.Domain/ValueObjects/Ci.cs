using EventosUy.Domain.Common;

namespace EventosUy.Domain.ValueObjects
{
    public record Ci
    {
        public string Value { get; init; }
        public string Verifier { get; init; }
        private Ci(string identity, string verifier) 
        { 
            Value = identity;
            Verifier = verifier;
        }

        private static readonly IEnumerable<int> Sequence = [2, 9, 8, 7, 6, 3, 4];

        private static readonly int Module = 10;

        public static Result<Ci> Create(string id) 
        {
            if (string.IsNullOrWhiteSpace(id)) { return Result<Ci>.Failure("Ci cannot be empty."); }
            List<string> errors = [];
            if (id.Length < 7) { errors.Add("Ci must have 8 digits."); }
            if (id.Length > 7) { errors.Add("Don't enter the verifier digit."); }
            if (id.Any(c => !char.IsDigit(c))) { errors.Add("Ci must contain only digits."); }

            if (errors.Count != 0) { return Result<Ci>.Failure(errors); }

            var sum = id.Select((c, idx) => (c - '0') * Sequence.ElementAt(idx)).Sum();

            var mod = sum % Module;
            var verifier = mod == 0 ? 0 : Module - mod;

            Ci ci = new(id, verifier.ToString());

            return Result<Ci>.Success(ci);
        }

        public string GetClean() { return Value; }
        public string GetFormatted() { return $"{Value[0]}.{Value.Substring(1, 3)}.{Value.Substring(4,3)}-{Verifier}"; }
    }
}
