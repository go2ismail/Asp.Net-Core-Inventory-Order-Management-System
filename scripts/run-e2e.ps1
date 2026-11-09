$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
Push-Location $repoRoot
try {
    $service = Get-Service -Name 'MSSQL$SQLEXPRESS' -ErrorAction SilentlyContinue
    if (-not $service -or $service.Status -ne "Running") {
        throw "SQL Server Express service 'MSSQL`$SQLEXPRESS' is not running. Start it before running E2E tests."
    }

    $sqlcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\180\Tools\Binn\SQLCMD.EXE"
    if (-not (Test-Path -LiteralPath $sqlcmd)) {
        $found = Get-ChildItem "C:\Program Files\Microsoft SQL Server" -Recurse -Filter "SQLCMD.EXE" -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($found) {
            $sqlcmd = $found.FullName
        }
    }
    if (-not (Test-Path -LiteralPath $sqlcmd)) {
        throw "sqlcmd.exe was not found. Install the SQL Server command line tools or create the E2E database manually."
    }

    $dbCreator = (& $sqlcmd -S "localhost\SQLEXPRESS" -U dev -P dev -C -h -1 -W -Q "SET NOCOUNT ON; SELECT IS_SRVROLEMEMBER('dbcreator');" 2>&1 | Out-String).Trim()
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to connect to 'localhost\SQLEXPRESS' as dev/dev. Create the login or update Tests/E2E/Infrastructure/E2EServerFixture.cs. Output: $dbCreator"
    }

    if ($dbCreator -eq "1") {
        & $sqlcmd -S "localhost\SQLEXPRESS" -U dev -P dev -C -Q "IF DB_ID('WHMS-LTE-FS-E2E') IS NULL CREATE DATABASE [WHMS-LTE-FS-E2E];" | Out-Null
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to create the WHMS-LTE-FS-E2E database."
        }
    } else {
        Write-Warning "Login 'dev' has no dbcreator permission. Falling back to the existing WHMS-LTE-FS database."
        $env:E2E_DB_NAME = "WHMS-LTE-FS"
    }

    dotnet build Indotalent.sln -c Debug
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed."
    }

    $playwright = "Tests/E2E/bin/Debug/net10.0/playwright.ps1"
    if (-not (Test-Path -LiteralPath $playwright)) {
        throw "playwright.ps1 was not found. Build the E2E project first."
    }
    & $playwright install chromium
    if ($LASTEXITCODE -ne 0) {
        throw "Playwright browser installation failed."
    }

    dotnet test Tests/E2E/E2E.csproj -c Debug --no-build --logger "console;verbosity=normal"
    if ($LASTEXITCODE -ne 0) {
        throw "E2E tests failed."
    }
}
finally {
    Pop-Location
}
