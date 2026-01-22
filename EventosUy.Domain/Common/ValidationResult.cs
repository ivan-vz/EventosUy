using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Common
{
    public class ValidationResult
    {
        public DuplicateField DuplicateFields { get; private set; }
        public bool IsSuccess => DuplicateFields == DuplicateField.NONE;
        public bool IsFailure => !IsSuccess;

        public ValidationResult() { DuplicateFields = DuplicateField.NONE; }

        public void AddDuplicate(DuplicateField field) { DuplicateFields |= field;  } //[Flags] maneja binarios

        public bool HasDuplicate(DuplicateField field) { return DuplicateFields.HasFlag(field); }
    }
}
