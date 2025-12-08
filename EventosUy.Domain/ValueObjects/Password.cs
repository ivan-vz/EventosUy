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
            if (raw.Length < 12) { return Result<Password>.Failure("Password must have at least 12 characters."); }
            if (!raw.Any(c => char.IsLower(c)) || !raw.Any(c => char.IsUpper(c))) { return Result<Password>.Failure("Password must contain lower and upper cases."); }
            if (!raw.Any(c => char.IsDigit(c))) { return Result<Password>.Failure("Password must contain letters."); }
            if (raw.Any(char.IsWhiteSpace)) { return Result<Password>.Failure("Password cannot contain whitespace."); }

            string hash = PasswordHasher.Hash(raw);
            return Result<Password>.Success(new Password(hash));
        }

        public bool Verify(string raw) { return PasswordHasher.Verify(raw, Hash); }
    }
}
