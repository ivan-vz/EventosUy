using Microsoft.AspNetCore.Identity;

namespace EventosUy.Domain.Common
{
    internal static class PasswordHasher
    {
        private static readonly PasswordHasher<object> _hasher = new();

        public static string Hash(string raw) { return _hasher.HashPassword(null!, raw); }

        public static bool Verify(string raw, string hash) 
        {
            var result = _hasher.VerifyHashedPassword(null!, hash, raw);

            return result != PasswordVerificationResult.Failed;
        }
    }
}
