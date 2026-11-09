namespace Infrastructure.FileStorage;

/// <summary>
/// Resolves file names that come from a caller into a path that is guaranteed to stay inside the
/// configured storage folder.
/// </summary>
/// <remarks>
/// Uploads generate their own <c>{guid}.{extension}</c> names, but the download endpoints accept a
/// client supplied name, so a value such as <c>../../appsettings.json</c> or an absolute path must
/// never reach the file system. Every name is therefore treated as a single file name and the
/// canonical result has to be a child of the storage folder.
/// </remarks>
public static class StoragePathGuard
{
    /// <summary>
    /// A stored name is a file name, never a path: it has no directory separator, no drive, and no
    /// <c>.</c> or <c>..</c> segment.
    /// </summary>
    public static bool IsPlainFileName(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }

        // Rejects "sub/file.txt", "../../appsettings.json", "C:\Windows\win.ini" and "/etc/passwd",
        // because only a name without any directory part survives this comparison.
        if (!string.Equals(fileName, Path.GetFileName(fileName), StringComparison.Ordinal))
        {
            return false;
        }

        // Linux does not treat the backslash as a separator, so reject both separators explicitly.
        // That keeps the check identical on Windows and Unix and stops a Windows style traversal
        // from becoming a literal file name on a Unix host.
        if (fileName.Contains('/') || fileName.Contains('\\'))
        {
            return false;
        }

        if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            return false;
        }

        return fileName is not "." and not "..";
    }

    /// <summary>
    /// Combines <paramref name="fileName"/> with <paramref name="folderPath"/> and confirms that the
    /// canonical path is a direct child of that folder.
    /// </summary>
    public static bool TryResolveInsideFolder(string folderPath, string? fileName, out string resolvedPath)
    {
        resolvedPath = string.Empty;

        if (!IsPlainFileName(fileName))
        {
            return false;
        }

        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var rootPath = Path.GetFullPath(folderPath);
        var rootWithSeparator = rootPath.EndsWith(Path.DirectorySeparatorChar)
            ? rootPath
            : rootPath + Path.DirectorySeparatorChar;

        var candidate = Path.GetFullPath(Path.Combine(rootPath, fileName!));

        if (!candidate.StartsWith(rootWithSeparator, comparison))
        {
            return false;
        }

        resolvedPath = candidate;
        return true;
    }
}
