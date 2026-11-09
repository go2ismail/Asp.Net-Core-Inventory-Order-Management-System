using Infrastructure.SeedManager.Systems;
using NUnit.Framework;

namespace Unit.Security;

[TestFixture]
public class DefaultAdminCredentialPolicyTests
{
    private const string AdminEmail = "admin@root.com";
    private const string StrongPassword = "A-strong-production-password-1";

    [Test]
    public void DemoMode_AcceptsThePublishedDemoPassword()
    {
        var result = DefaultAdminCredentialPolicy.Validate(
            AdminEmail,
            DefaultAdminCredentialPolicy.PublishedDefaultPassword,
            isDemoVersion: true);

        Assert.That(result, Is.EqualTo(DefaultAdminCredentialPolicy.PublishedDefaultPassword));
    }

    [Test]
    public void DemoMode_AcceptsAConfiguredPassword()
    {
        var result = DefaultAdminCredentialPolicy.Validate(AdminEmail, "demo-pass", isDemoVersion: true);

        Assert.That(result, Is.EqualTo("demo-pass"));
    }

    [Test]
    public void ProductionMode_RejectsThePublishedDemoPassword()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => DefaultAdminCredentialPolicy.Validate(
                AdminEmail,
                DefaultAdminCredentialPolicy.PublishedDefaultPassword,
                isDemoVersion: false));

        Assert.That(exception!.Message, Does.Contain("demo password"));
    }

    [Test]
    public void ProductionMode_RejectsAPasswordShorterThanTheMinimum()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => DefaultAdminCredentialPolicy.Validate(
                AdminEmail,
                new string('a', DefaultAdminCredentialPolicy.MinimumPasswordLength - 1),
                isDemoVersion: false));

        Assert.That(exception!.Message, Does.Contain(DefaultAdminCredentialPolicy.MinimumPasswordLength.ToString()));
    }

    [Test]
    public void ProductionMode_AcceptsAStrongPassword()
    {
        var result = DefaultAdminCredentialPolicy.Validate(AdminEmail, StrongPassword, isDemoVersion: false);

        Assert.That(result, Is.EqualTo(StrongPassword));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void EmptyPassword_IsRejectedInBothModes(string? password)
    {
        Assert.Throws<InvalidOperationException>(
            () => DefaultAdminCredentialPolicy.Validate(AdminEmail, password, isDemoVersion: true));
        Assert.Throws<InvalidOperationException>(
            () => DefaultAdminCredentialPolicy.Validate(AdminEmail, password, isDemoVersion: false));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void EmptyEmail_IsRejectedInBothModes(string? email)
    {
        Assert.Throws<InvalidOperationException>(
            () => DefaultAdminCredentialPolicy.Validate(email, StrongPassword, isDemoVersion: true));
        Assert.Throws<InvalidOperationException>(
            () => DefaultAdminCredentialPolicy.Validate(email, StrongPassword, isDemoVersion: false));
    }
}
