using EventosUy.Domain.Common;

namespace EventosUy.Domain.ValueObjects
{
    public record Password
    {
        public string Hash { get; }

        private Password(string hash) {
            Hash = hash;
        }

        public static Result<Password> Create(string raw) 
        {
            if (string.IsNullOrWhiteSpace(raw)) { return Result<Password>.Failure("Password cannot be empty."); }
            List<string> errors = [];
            if (raw.Length < 12) { errors.Add("Password must have at least 12 characters."); }
            if (!raw.Any(c => char.IsLower(c)) || !raw.Any(c => char.IsUpper(c))) { errors.Add("Password must contain lower and upper cases."); }
            if (!raw.Any(c => char.IsDigit(c))) { errors.Add("Password must contain digits."); }
            if (raw.Any(char.IsWhiteSpace)) { errors.Add("Password cannot contain whitespace."); }

            if (errors.Count != 0) { return Result<Password>.Failure(errors); }

            string hash = PasswordHasher.Hash(raw);
            return Result<Password>.Success(new Password(hash));
        }

        public bool Verify(string raw) { return PasswordHasher.Verify(raw, Hash); }
    }
}
