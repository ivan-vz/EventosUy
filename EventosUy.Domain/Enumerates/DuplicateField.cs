namespace EventosUy.Domain.Enumerates
{
    [Flags]
    public enum DuplicateField
    {
        NONE = 0,
        NAME = 1,
        NICKNAME = 2,
        INITIALS = 4,
        EDITION = 8,
        INSTITUTION = 16,
        EMAIL = 32,
        ACRONYM = 64,
        URL = 128,
        ADDRESS = 256,
        CI = 512
    }
}
