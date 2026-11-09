using Microsoft.Extensions.Configuration;
using System.Text;

namespace Infrastructure.SecurityManager.Tokens;

/// <summary>
/// Reads and validates the <c>Jwt</c> configuration section.
/// </summary>
/// <remarks>
/// The signing key is a deployment secret, therefore it is not stored in <c>appsettings.json</c>.
/// The validation fails fast during start up and explains every supported configuration source,
/// so the application never falls back to a default key that is public knowledge.
/// </remarks>
public static class TokenSettingsValidation
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// HMAC-SHA256 requires a key of at least 256 bits, so a shorter secret must be rejected.
    /// </summary>
    public const int MinimumKeyLengthInBytes = 32;

    /// <summary>
    /// Default keys that were published with earlier releases. Rejecting them keeps an existing
    /// deployment from continuing to sign tokens with a value that everybody can read on GitHub.
    /// </summary>
    private static readonly string[] PublishedKeys =
    {
        "YourVeryStrongAndSecureSecretKeyWhichIs32Chars!",
    };

    public static TokenSettings GetValidated(IConfiguration configuration)
    {
        return Validate(configuration.GetSection(SectionName).Get<TokenSettings>());
    }

    public static TokenSettings Validate(TokenSettings? settings)
    {
        if (settings is null)
        {
            throw new InvalidOperationException(BuildMessage("The Jwt configuration section is missing."));
        }

        if (string.IsNullOrWhiteSpace(settings.Key))
        {
            throw new InvalidOperationException(BuildMessage("Jwt:Key is empty."));
        }

        if (PublishedKeys.Any(publishedKey => string.Equals(publishedKey, settings.Key, StringComparison.Ordinal)))
        {
            throw new InvalidOperationException(
                BuildMessage("Jwt:Key still contains the default key that was published with earlier releases."));
        }

        if (Encoding.UTF8.GetByteCount(settings.Key) < MinimumKeyLengthInBytes)
        {
            throw new InvalidOperationException(
                BuildMessage($"Jwt:Key must be at least {MinimumKeyLengthInBytes} bytes long."));
        }

        if (string.IsNullOrWhiteSpace(settings.Issuer))
        {
            throw new InvalidOperationException(BuildMessage("Jwt:Issuer is empty."));
        }

        if (string.IsNullOrWhiteSpace(settings.Audience))
        {
            throw new InvalidOperationException(BuildMessage("Jwt:Audience is empty."));
        }

        return settings;
    }

    private static string BuildMessage(string problem)
    {
        return $"""
            {problem} The application cannot sign or validate access tokens without a deployment specific secret.

            Configure the key through the first source that is available (highest precedence first):
              1. Environment variable      Jwt__Key
              2. .env file                 Jwt__Key=<secret>
                                           searched in the repository root and in the content root,
                                           loaded only when the file exists, never committed (see .env.example)
              3. dotnet user-secrets       dotnet user-secrets set "Jwt:Key" "<secret>" --project Presentation/ASPNET

            Generate a random key with one of these commands:
              powershell -NoProfile -ExecutionPolicy Bypass -File scripts/new-jwt-key.ps1
              openssl rand -base64 48

            The key must be at least {MinimumKeyLengthInBytes} bytes long, unique per deployment and it must never
            be committed. Changing the key invalidates every access token that was issued before the change.
            """;
    }
}
