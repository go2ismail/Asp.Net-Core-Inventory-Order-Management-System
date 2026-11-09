using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using NUnit.Framework;

namespace E2E;

[SetUpFixture]
public class E2EServerFixture
{
    private const string DefaultBaseUrl = "http://127.0.0.1:5187";
    private const string DefaultDatabaseName = "WHMS-LTE-FS-E2E";

    private static readonly ConcurrentQueue<string> LogTail = new();
    private static readonly object LogLock = new();
    private static Process? _server;
    private static string? _repoRoot;

    public static string BaseUrl { get; private set; } = DefaultBaseUrl;

    public static string RepoRoot => _repoRoot ??= FindRepoRoot();

    [OneTimeSetUp]
    public async Task StartServerAsync()
    {
        var externalBaseUrl = Environment.GetEnvironmentVariable("E2E_BASE_URL");
        if (!string.IsNullOrWhiteSpace(externalBaseUrl))
        {
            BaseUrl = externalBaseUrl.TrimEnd('/');
            await WaitUntilRespondingAsync(TimeSpan.FromSeconds(60), throwOnExit: false);
            return;
        }

        if (await IsRespondingAsync())
        {
            throw new InvalidOperationException(
                $"{BaseUrl} is already serving a response. Stop the process that owns the port, " +
                "or set E2E_BASE_URL to reuse an application that is already running.");
        }

        var logDirectory = Path.Combine(RepoRoot, "TestResults");
        Directory.CreateDirectory(logDirectory);
        var logFile = Path.Combine(logDirectory, "server.log");
        File.WriteAllText(logFile, string.Empty);

        var databaseName = Environment.GetEnvironmentVariable("E2E_DB_NAME") ?? DefaultDatabaseName;
        var projectDirectory = Path.Combine(RepoRoot, "Presentation", "ASPNET");

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run --project ASPNET.csproj --configuration Debug --no-launch-profile",
            WorkingDirectory = projectDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        startInfo.Environment["Kestrel__Endpoints__Http__Url"] = BaseUrl;
        startInfo.Environment["ConnectionStrings__DefaultConnection"] =
            $"Server=localhost\\SQLEXPRESS;Database={databaseName};User=dev;Password=dev;TrustServerCertificate=True;";
        startInfo.Environment["IsDemoVersion"] = "true";
        startInfo.Environment["AspNetIdentity__Lockout__MaxFailedAccessAttempts"] = "1000";
        // A random key per run: the tests never depend on a committed or local secret.
        startInfo.Environment["Jwt__Key"] = CreateRandomJwtKey();

        _server = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        _server.OutputDataReceived += (_, args) => AppendLog(logFile, args.Data);
        _server.ErrorDataReceived += (_, args) => AppendLog(logFile, args.Data);

        if (!_server.Start())
        {
            throw new InvalidOperationException("Failed to start the ASP.NET application for E2E tests.");
        }

        _server.BeginOutputReadLine();
        _server.BeginErrorReadLine();

        await WaitUntilRespondingAsync(TimeSpan.FromMinutes(5), throwOnExit: true);
    }

    [OneTimeTearDown]
    public void StopServer()
    {
        if (_server is null)
        {
            return;
        }

        try
        {
            if (!_server.HasExited)
            {
                _server.Kill(entireProcessTree: true);
                _server.WaitForExit(10000);
            }
        }
        catch
        {
            // Best-effort cleanup of the test server.
        }
        finally
        {
            _server.Dispose();
            _server = null;
        }
    }

    private static string CreateRandomJwtKey()
    {
        var keyBytes = RandomNumberGenerator.GetBytes(48);

        return Convert.ToBase64String(keyBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    private static async Task WaitUntilRespondingAsync(TimeSpan timeout, bool throwOnExit)
    {
        var deadline = DateTime.UtcNow + timeout;

        while (DateTime.UtcNow < deadline)
        {
            if (throwOnExit && _server is not null && _server.HasExited)
            {
                throw new InvalidOperationException(
                    $"The application exited with code {_server.ExitCode} before becoming ready.{Environment.NewLine}{GetLogTail()}");
            }

            if (await IsRespondingAsync())
            {
                return;
            }

            await Task.Delay(1000);
        }

        throw new TimeoutException(
            $"The application did not respond at {BaseUrl} within {timeout.TotalSeconds:0} seconds.{Environment.NewLine}{GetLogTail()}");
    }

    private static async Task<bool> IsRespondingAsync()
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            using var response = await client.GetAsync($"{BaseUrl}/");
            return response.StatusCode == HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }

    private static void AppendLog(string logFile, string? line)
    {
        if (line is null)
        {
            return;
        }

        LogTail.Enqueue(line);
        while (LogTail.Count > 200 && LogTail.TryDequeue(out _))
        {
        }

        lock (LogLock)
        {
            try
            {
                File.AppendAllText(logFile, line + Environment.NewLine);
            }
            catch
            {
                // Logging must never break the test run.
            }
        }
    }

    private static string GetLogTail()
    {
        var lines = LogTail.ToArray();
        return lines.Length == 0
            ? "No server output was captured."
            : $"Last server output:{Environment.NewLine}{string.Join(Environment.NewLine, lines)}";
    }

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Indotalent.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException(
            $"Could not locate Indotalent.sln by walking up from {AppContext.BaseDirectory}.");
    }
}
