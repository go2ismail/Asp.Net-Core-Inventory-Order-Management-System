using Microsoft.Extensions.Configuration;
using System.Text;

namespace Infrastructure.SecurityManager.Tokens;

/// <summary>
/// Reads and validates the <c>Jwt</c> configuration section.
/// </summary>
/// <remarks>
/// <para>
/// The application has two explicit modes. Demo mode (<c>IsDemoVersion=true</c>, the default of
/// <c>appsettings.json</c>) keeps the documented demo key so the public live demo and a fresh clone
/// start without any setup. Production mode (<c>IsDemoVersion=false</c>) rejects the demo key, an
/// empty key, and a key shorter than 256 bits, and the validation fails fast during start up with a
/// message that names every supported configuration source.
/// </para>
/// <para>
/// The demo key is public knowledge, so production must never run with the demo mode enabled.
/// </para>
/// </remarks>
public static class TokenSettingsValidation
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// HMAC-SHA256 requires a key of at least 256 bits, so a shorter secret must be rejected in both modes.
    /// </summary>
    public const int MinimumKeyLengthInBytes = 32;

    /// <summary>
    /// Default keys that were published with earlier releases. Demo mode accepts them, production rejects
    /// them so an existing deployment stops signing tokens with a value that everybody can read on GitHub.
    /// </summary>
    private static readonly string[] PublishedKeys =
    {
        "YourVeryStrongAndSecureSecretKeyWhichIs32Chars!",
    };

    public static TokenSettings GetValidated(IConfiguration configuration, bool isDemoVersion)
    {
        return Validate(configuration.GetSection(SectionName).Get<TokenSettings>(), isDemoVersion);
    }

    public static TokenSettings Validate(TokenSettings? settings, bool isDemoVersion)
    {
        if (settings is null)
        {
            throw new InvalidOperationException(BuildMessage("The Jwt configuration section is missing.", isDemoVersion));
        }

        if (string.IsNullOrWhiteSpace(settings.Key))
        {
            throw new InvalidOperationException(BuildMessage("Jwt:Key is empty.", isDemoVersion));
        }

        var isPublishedKey = PublishedKeys.Any(
            publishedKey => string.Equals(publishedKey, settings.Key, StringComparison.Ordinal));

        if (isPublishedKey && !isDemoVersion)
        {
            throw new InvalidOperationException(
                BuildMessage(
                    "Jwt:Key still contains the demo key that is published with the repository. It is only accepted while IsDemoVersion is true.",
                    isDemoVersion));
        }

        if (Encoding.UTF8.GetByteCount(settings.Key) < MinimumKeyLengthInBytes)
        {
            throw new InvalidOperationException(
                BuildMessage($"Jwt:Key must be at least {MinimumKeyLengthInBytes} bytes long.", isDemoVersion));
        }

        if (string.IsNullOrWhiteSpace(settings.Issuer))
        {
            throw new InvalidOperationException(BuildMessage("Jwt:Issuer is empty.", isDemoVersion));
        }

        if (string.IsNullOrWhiteSpace(settings.Audience))
        {
            throw new InvalidOperationException(BuildMessage("Jwt:Audience is empty.", isDemoVersion));
        }

        return settings;
    }

    private static string BuildMessage(string problem, bool isDemoVersion)
    {
        var mode = isDemoVersion
            ? "Demo mode (IsDemoVersion=true) accepts the documented demo key that appsettings.json contains."
            : "Production mode (IsDemoVersion=false) requires a deployment specific key.";

        return $"""
            {problem} The application cannot sign or validate access tokens without a signing key.

            {mode}

            Configure the key through the first source that is available (highest precedence first):
              1. Environment variable      Jwt__Key
              2. .env file                 Jwt__Key=<secret>
                                           searched in the repository root and in the content root,
                                           loaded only when the file exists, never committed (see .env.example)
              3. dotnet user-secrets       dotnet user-secrets set "Jwt:Key" "<secret>" --project Presentation/ASPNET

            Generate a random key with one of these commands:
              powershell -NoProfile -ExecutionPolicy Bypass -File scripts/new-secrets.ps1
              openssl rand -base64 48

            The key must be at least {MinimumKeyLengthInBytes} bytes long, unique per deployment and it must never
            be committed. Changing the key invalidates every access token that was issued before the change.
            Production deployments must also set IsDemoVersion=false.
            """;
    }
}
