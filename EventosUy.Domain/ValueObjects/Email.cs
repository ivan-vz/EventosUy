using EventosUy.Domain.Common;
using System.Text.RegularExpressions;

namespace EventosUy.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; init; }

        private Email(string value) { Value = value; }

        private static readonly Regex EmailRegex = new Regex(
            @"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
            );

        public static Result<Email> Create(string value) 
        {
            if (string.IsNullOrWhiteSpace(value)) { return Result<Email>.Failure("Email cannot be empty."); }

            if (!EmailRegex.IsMatch(value)) { return Result<Email>.Failure("Email is not formatted correctly."); }
          
            Email email = new Email(value);

            return Result<Email>.Success(email);
        }
    }
}
