namespace Infrastructure.SeedManager.Systems;

/// <summary>
/// Validates the credentials of the administrator account that is created on the first start up.
/// </summary>
/// <remarks>
/// <para>
/// The application has two explicit modes. Demo mode (<c>IsDemoVersion=true</c>, the default of
/// <c>appsettings.json</c>) keeps the documented <c>123456</c> credential so the live demo and a
/// fresh clone can sign in immediately. Production mode (<c>IsDemoVersion=false</c>) requires a
/// deployment specific password of at least 12 characters and rejects the password that earlier
/// releases published, so an existing installation is forced to rotate it.
/// </para>
/// <para>
/// The checks fail fast with an actionable message instead of silently creating an account with a
/// value that anybody can read in the public repository.
/// </para>
/// </remarks>
public static class DefaultAdminCredentialPolicy
{
    public const string SectionName = "AspNetIdentity:DefaultAdmin";

    /// <summary>
    /// The password that earlier releases shipped in <c>appsettings.json</c>. Demo mode accepts it;
    /// production rejects it both as a configured value and as the stored password of an existing
    /// administrator account.
    /// </summary>
    public const string PublishedDefaultPassword = "123456";

    /// <summary>
    /// The administrator account exists in every deployment, so production gets a stronger minimum
    /// length than the <c>AspNetIdentity:Password:RequiredLength</c> of the identity options.
    /// </summary>
    public const int MinimumPasswordLength = 12;

    public static string EnsureEmail(string? email, bool isDemoVersion = false)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException(
                BuildMessage("AspNetIdentity:DefaultAdmin:Email is empty.", isDemoVersion));
        }

        return email;
    }

    public static string Validate(string? email, string? password, bool isDemoVersion)
    {
        EnsureEmail(email, isDemoVersion);

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                BuildMessage("AspNetIdentity:DefaultAdmin:Password is empty.", isDemoVersion));
        }

        if (isDemoVersion)
        {
            // The demo mode intentionally keeps the published credential. The identity options still
            // apply, so the value must satisfy AspNetIdentity:Password:RequiredLength as well.
            return password;
        }

        if (string.Equals(password, PublishedDefaultPassword, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                BuildMessage(
                    "AspNetIdentity:DefaultAdmin:Password still contains the demo password that is published with the repository. It is only accepted while IsDemoVersion is true.",
                    isDemoVersion));
        }

        if (password.Length < MinimumPasswordLength)
        {
            throw new InvalidOperationException(
                BuildMessage(
                    $"AspNetIdentity:DefaultAdmin:Password must be at least {MinimumPasswordLength} characters long.",
                    isDemoVersion));
        }

        return password;
    }

    /// <summary>
    /// Message for an existing installation whose administrator still uses the published password.
    /// </summary>
    public static string BuildRotationRequiredMessage(string email)
    {
        return $"""
            The administrator account {email} still uses the password that earlier releases published, so
            the application refuses to start until a replacement is configured.

            {Sources()}

            The password must be at least {MinimumPasswordLength} characters long and must differ from
            the value that earlier releases published. It is rotated automatically at the next start up,
            so the published password stops working without a manual database update. Production
            deployments must also set IsDemoVersion=false.
            """;
    }

    private static string BuildMessage(string problem, bool isDemoVersion)
    {
        var mode = isDemoVersion
            ? "Demo mode (IsDemoVersion=true) accepts the documented demo password that appsettings.json contains."
            : "Production mode (IsDemoVersion=false) requires a deployment specific password.";

        return $"""
            {problem} The application cannot create the administrator account without a password.

            {mode}

            {Sources()}

            The account is only created when the database has no roles yet. Set the password once and
            change it afterwards from the profile page of the application.
            """;
    }

    private static string Sources()
    {
        return """
            Configure the value through the first source that is available (highest precedence first):
              1. Environment variable      AspNetIdentity__DefaultAdmin__Password
              2. .env file                 AspNetIdentity__DefaultAdmin__Password=<password>
                                           searched in the repository root and in the content root,
                                           loaded only when the file exists, never committed (see .env.example)
              3. dotnet user-secrets       dotnet user-secrets set "AspNetIdentity:DefaultAdmin:Password" "<password>" --project Presentation/ASPNET
            """;
    }
}
