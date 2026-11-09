using Microsoft.Extensions.Configuration;

namespace ASPNET.BackEnd.Common.Configuration;

/// <summary>
/// Adds the values of a <c>.env</c> file to the application configuration.
/// </summary>
/// <remarks>
/// <para>
/// Environment variables are invisible by nature, so local development gets a readable file
/// instead: copy <c>.env.example</c> to <c>.env</c> and keep the file out of version control.
/// </para>
/// <para>
/// A <c>.env</c> file is only a convenience for development and for Docker Compose. Variables that
/// the host already defined always win, so containers, CI pipelines and production hosts can keep
/// using their own protected configuration source without the file being able to override it.
/// </para>
/// <para>
/// Keys use the same format as environment variables: <c>Jwt__Key</c> is bound to <c>Jwt:Key</c>.
/// A double quote, a single quote, blank lines, the <c>export</c> prefix and <c>#</c> comments
/// are supported.
/// </para>
/// </remarks>
public static class DotEnvConfigurationExtensions
{
    public const string FileName = ".env";

    private const int MaxSearchDepth = 5;

    /// <summary>
    /// Loads the first <c>.env</c> file found while walking up from <paramref name="contentRootPath"/>.
    /// </summary>
    /// <returns>The full path of the loaded file, or <c>null</c> when no file was found.</returns>
    public static string? AddDotEnvFile(this IConfigurationBuilder builder, string contentRootPath)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var filePath = ResolveFilePath(contentRootPath);
        if (filePath is null)
        {
            return null;
        }

        var values = new List<KeyValuePair<string, string?>>();

        foreach (var pair in ParseFile(filePath))
        {
            // A host environment variable always wins, the file only fills the gaps.
            if (Environment.GetEnvironmentVariable(ToEnvironmentVariableName(pair.Key)) is not null)
            {
                continue;
            }

            // The in memory provider does not translate the "__" separator of the environment
            // variables, so "Jwt__Key" has to become "Jwt:Key" explicitly.
            values.Add(new KeyValuePair<string, string?>(ToConfigurationKey(pair.Key), pair.Value));
        }

        if (values.Count == 0)
        {
            return filePath;
        }

        builder.AddInMemoryCollection(values);

        return filePath;
    }

    /// <summary>
    /// "Jwt__Key" (environment variable format) becomes "Jwt:Key" (configuration format).
    /// </summary>
    private static string ToConfigurationKey(string key)
    {
        return key.Replace("__", ":", StringComparison.Ordinal);
    }

    /// <summary>
    /// "Jwt:Key" (configuration format) becomes "Jwt__Key" (environment variable format).
    /// </summary>
    private static string ToEnvironmentVariableName(string key)
    {
        return key.Replace(":", "__", StringComparison.Ordinal);
    }

    /// <summary>
    /// Resolves the <c>.env</c> file by walking up from the content root, so both
    /// <c>Presentation/ASPNET/.env</c> (dotnet run, Visual Studio) and the repository root
    /// <c>/.env</c> (Docker Compose) are picked up.
    /// </summary>
    public static string? ResolveFilePath(string contentRootPath)
    {
        var directory = new DirectoryInfo(contentRootPath);

        for (var depth = 0; directory is not null && depth <= MaxSearchDepth; depth++)
        {
            var candidate = Path.Combine(directory.FullName, FileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            // The repository root is the last directory that is inspected.
            if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
            {
                break;
            }

            directory = directory.Parent;
        }

        return null;
    }

    private static Dictionary<string, string> ParseFile(string filePath)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in File.ReadAllLines(filePath))
        {
            var line = rawLine.Trim();

            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            if (line.StartsWith("export ", StringComparison.OrdinalIgnoreCase))
            {
                line = line["export ".Length..].TrimStart();
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = line[..separatorIndex].Trim();
            if (key.Length == 0)
            {
                continue;
            }

            values[key] = Unquote(line[(separatorIndex + 1)..].Trim());
        }

        return values;
    }

    private static string Unquote(string value)
    {
        if (value.Length == 0)
        {
            return value;
        }

        var quote = value[0];
        if ((quote == '"' || quote == '\'') && value.Length >= 2 && value[^1] == quote)
        {
            return value[1..^1];
        }

        // Drop a trailing comment of an unquoted value, for example "Jwt__Key=abc # local only".
        var commentIndex = value.IndexOf(" #", StringComparison.Ordinal);

        return commentIndex > 0 ? value[..commentIndex].TrimEnd() : value;
    }
}
