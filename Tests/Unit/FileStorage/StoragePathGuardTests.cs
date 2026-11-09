using Infrastructure.FileStorage;
using NUnit.Framework;

namespace Unit.FileStorage;

[TestFixture]
public class StoragePathGuardTests
{
    [TestCase("document.pdf", true)]
    [TestCase("0f8fad5b-d9cb-469f-a165-70867728950e.png", true)]
    [TestCase("..hidden.txt", true)]
    [TestCase(".hidden", true)]
    [TestCase("name with spaces.txt", true)]
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("   ", false)]
    [TestCase(".", false)]
    [TestCase("..", false)]
    [TestCase("../appsettings.json", false)]
    [TestCase("..\\appsettings.json", false)]
    [TestCase("..\\..\\appsettings.json", false)]
    [TestCase("sub/file.txt", false)]
    [TestCase("sub\\file.txt", false)]
    [TestCase("/etc/passwd", false)]
    [TestCase("C:\\Windows\\win.ini", false)]
    [TestCase("C:/Windows/win.ini", false)]
    [TestCase("file.txt/", false)]
    [TestCase("file.txt\\", false)]
    public void IsPlainFileName_RejectsEveryPathLikeValue(string? fileName, bool expected)
    {
        Assert.That(StoragePathGuard.IsPlainFileName(fileName), Is.EqualTo(expected));
    }

    [Test]
    public void TryResolveInsideFolder_ResolvesAPlainFile()
    {
        var folder = CreateTemporaryFolder();
        try
        {
            var expectedPath = Path.Combine(folder, "document.txt");
            File.WriteAllText(expectedPath, "content");

            var resolved = StoragePathGuard.TryResolveInsideFolder(folder, "document.txt", out var resolvedPath);

            Assert.That(resolved, Is.True);
            Assert.That(resolvedPath, Is.EqualTo(Path.GetFullPath(expectedPath)));
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }

    [Test]
    public void TryResolveInsideFolder_ResolvesAMissingPlainFile()
    {
        var folder = CreateTemporaryFolder();
        try
        {
            var resolved = StoragePathGuard.TryResolveInsideFolder(folder, "missing.txt", out var resolvedPath);

            Assert.That(resolved, Is.True);
            Assert.That(resolvedPath, Is.EqualTo(Path.GetFullPath(Path.Combine(folder, "missing.txt"))));
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }

    [TestCase("../outside.txt")]
    [TestCase("..\\outside.txt")]
    [TestCase("..\\..\\appsettings.json")]
    [TestCase("sub/file.txt")]
    [TestCase("sub\\file.txt")]
    [TestCase("/etc/passwd")]
    [TestCase("C:\\Windows\\win.ini")]
    public void TryResolveInsideFolder_RejectsNamesOutsideTheFolder(string fileName)
    {
        var folder = CreateTemporaryFolder();
        try
        {
            var resolved = StoragePathGuard.TryResolveInsideFolder(folder, fileName, out var resolvedPath);

            Assert.That(resolved, Is.False);
            Assert.That(resolvedPath, Is.Empty);
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }

    private static string CreateTemporaryFolder()
    {
        var folder = Path.Combine(Path.GetTempPath(), $"indotalent-guard-{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        return folder;
    }
}
