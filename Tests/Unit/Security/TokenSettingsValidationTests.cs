using Infrastructure.SecurityManager.Tokens;
using NUnit.Framework;

namespace Unit.Security;

[TestFixture]
public class TokenSettingsValidationTests
{
    private const string PublishedDemoKey = "YourVeryStrongAndSecureSecretKeyWhichIs32Chars!";
    private const string ValidKey = "0123456789abcdef0123456789abcdef";

    [Test]
    public void DemoMode_AcceptsThePublishedDemoKey()
    {
        var settings = CreateSettings(PublishedDemoKey);

        var result = TokenSettingsValidation.Validate(settings, isDemoVersion: true);

        Assert.That(result.Key, Is.EqualTo(PublishedDemoKey));
    }

    [Test]
    public void ProductionMode_RejectsThePublishedDemoKey()
    {
        var settings = CreateSettings(PublishedDemoKey);

        var exception = Assert.Throws<InvalidOperationException>(
            () => TokenSettingsValidation.Validate(settings, isDemoVersion: false));

        Assert.That(exception!.Message, Does.Contain("demo key"));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void EmptyKey_IsRejectedInBothModes(bool isDemoVersion)
    {
        var settings = CreateSettings("");

        Assert.Throws<InvalidOperationException>(
            () => TokenSettingsValidation.Validate(settings, isDemoVersion));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ShortKey_IsRejectedInBothModes(bool isDemoVersion)
    {
        var settings = CreateSettings("too-short");

        var exception = Assert.Throws<InvalidOperationException>(
            () => TokenSettingsValidation.Validate(settings, isDemoVersion));

        Assert.That(exception!.Message, Does.Contain(TokenSettingsValidation.MinimumKeyLengthInBytes.ToString()));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ValidKey_IsAcceptedInBothModes(bool isDemoVersion)
    {
        var settings = CreateSettings(ValidKey);

        var result = TokenSettingsValidation.Validate(settings, isDemoVersion);

        Assert.That(result.Key, Is.EqualTo(ValidKey));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void EmptyIssuerOrAudience_IsRejected(bool isDemoVersion)
    {
        Assert.Throws<InvalidOperationException>(
            () => TokenSettingsValidation.Validate(CreateSettings(ValidKey, issuer: ""), isDemoVersion));
        Assert.Throws<InvalidOperationException>(
            () => TokenSettingsValidation.Validate(CreateSettings(ValidKey, audience: " "), isDemoVersion));
    }

    [Test]
    public void MissingSettings_IsRejected()
    {
        Assert.Throws<InvalidOperationException>(
            () => TokenSettingsValidation.Validate(null, isDemoVersion: true));
    }

    private static TokenSettings CreateSettings(
        string key,
        string issuer = "YourIssuer",
        string audience = "YourAudience")
    {
        return new TokenSettings
        {
            Key = key,
            Issuer = issuer,
            Audience = audience,
            ExpireInMinute = 30,
            ClockSkewInMinute = 0,
        };
    }
}
