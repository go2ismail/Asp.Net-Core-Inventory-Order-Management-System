namespace Infrastructure.SecurityManager.AspNetIdentity;

public class IdentitySettings
{
    public PasswordOptions Password { get; init; } = null!;
    public LockoutOptions Lockout { get; init; } = null!;
    public UserOptions User { get; init; } = null!;
    public SignInOptions SignIn { get; init; } = null!;
    public DefaultAdminOptions DefaultAdmin { get; init; } = null!;

    public class PasswordOptions
    {
        public bool RequireDigit { get; init; }
        public bool RequireLowercase { get; init; }
        public bool RequireUppercase { get; init; }
        public bool RequireNonAlphanumeric { get; init; }
        public int RequiredLength { get; init; }
    }

    public class LockoutOptions
    {
        public int DefaultLockoutTimeSpanInMinutes { get; init; }
        public int MaxFailedAccessAttempts { get; init; }
        public bool AllowedForNewUsers { get; init; }
    }

    public class UserOptions
    {
        public bool RequireUniqueEmail { get; init; }
    }

    public class SignInOptions
    {
        public bool RequireConfirmedEmail { get; init; }
    }

    public class DefaultAdminOptions
    {
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}

