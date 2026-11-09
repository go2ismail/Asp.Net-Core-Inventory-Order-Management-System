<#
.SYNOPSIS
    Generates every production secret and writes them to the .env file in one command.

.DESCRIPTION
    The application starts in demo mode by default (IsDemoVersion=true), so a fresh clone needs no
    setup. A production deployment must switch the mode off and provide deployment specific secrets:

      - Jwt__Key                             at least 32 bytes
      - AspNetIdentity__DefaultAdmin__Password  at least 12 characters
      - MSSQL_SA_PASSWORD                    only needed by docker-compose.yml
      - IsDemoVersion=false

    This script generates the random values, updates the .env file in place (every other line is
    kept), and sets IsDemoVersion=false. `.env` is ignored by git and by Docker.

    Run it once before the first production start up. Running it again rotates the secrets: every
    user has to sign in again, the administrator password changes, and the SQL Server password
    changes (recreate the database container volume or change the password inside SQL Server).

.PARAMETER Path
    Target file. Defaults to the .env file in the repository root.

.PARAMETER KeyByteLength
    Number of random bytes for the JWT key. The default of 48 bytes produces a 64 character key.

.PARAMETER PasswordLength
    Length of the generated administrator password. The default of 24 characters is longer than the
    minimum of 12 characters that the application requires.

.PARAMETER SqlPasswordLength
    Length of the generated SQL Server password. The default of 24 characters is used by
    docker-compose.yml for the sa account.

.PARAMETER AdminEmail
    Email address of the administrator account. Defaults to admin@root.com.

.PARAMETER PassThru
    Returns the generated values instead of writing them to the output stream only.

.EXAMPLE
    powershell -NoProfile -ExecutionPolicy Bypass -File scripts/new-secrets.ps1

.EXAMPLE
    .\scripts\new-secrets.ps1 -Path C:\deploy\wms\.env -PasswordLength 32
#>
[CmdletBinding()]
param(
    [string] $Path,
    [ValidateRange(32, 128)]
    [int] $KeyByteLength = 48,
    [ValidateRange(12, 128)]
    [int] $PasswordLength = 24,
    [ValidateRange(8, 128)]
    [int] $SqlPasswordLength = 24,
    [string] $AdminEmail = "admin@root.com",
    [switch] $PassThru
)

$ErrorActionPreference = "Stop"

function New-RandomSecret {
    param(
        [Parameter(Mandatory = $true)]
        [int] $Length
    )

    # Unambiguous alphanumeric alphabet: safe inside .env files, shells, YAML, JSON and connection strings.
    $alphabet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789"
    $chars = [char[]]::new($Length)
    $random = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    $buffer = [byte[]]::new(1)
    $limit = 256 - (256 % $alphabet.Length)
    try {
        for ($index = 0; $index -lt $Length; $index++) {
            do {
                $random.GetBytes($buffer)
            } while ($buffer[0] -ge $limit)

            $chars[$index] = $alphabet[$buffer[0] % $alphabet.Length]
        }
    }
    finally {
        $random.Dispose()
    }

    return -join $chars
}

if ([string]::IsNullOrWhiteSpace($Path)) {
    $repoRoot = Split-Path -Parent $PSScriptRoot
    $Path = Join-Path $repoRoot ".env"
}

$Path = [System.IO.Path]::GetFullPath($Path)
$directory = Split-Path -Parent $Path
if (-not (Test-Path -LiteralPath $directory)) {
    throw "The directory '$directory' does not exist."
}

$jwtKeyScript = Join-Path $PSScriptRoot "new-jwt-key.ps1"
$adminPasswordScript = Join-Path $PSScriptRoot "new-admin-password.ps1"

foreach ($script in @($jwtKeyScript, $adminPasswordScript)) {
    if (-not (Test-Path -LiteralPath $script)) {
        throw "Required script '$script' was not found."
    }
}

$key = @(& $jwtKeyScript -Path $Path -ByteLength $KeyByteLength -PassThru)[-1]
$password = @(& $adminPasswordScript -Path $Path -Length $PasswordLength -Email $AdminEmail -PassThru)[-1]
$sqlPassword = New-RandomSecret -Length $SqlPasswordLength

# appsettings.json defaults to demo mode, so a production file has to turn it off explicitly.
$lines = New-Object System.Collections.Generic.List[string]
$demoModeWritten = $false
$sqlPasswordWritten = $false

foreach ($line in @(Get-Content -LiteralPath $Path)) {
    if ($line -match '^\s*(export\s+)?IsDemoVersion\s*=') {
        $lines.Add("IsDemoVersion=false")
        $demoModeWritten = $true
    }
    elseif ($line -match '^\s*(export\s+)?MSSQL_SA_PASSWORD\s*=') {
        $lines.Add("MSSQL_SA_PASSWORD=$sqlPassword")
        $sqlPasswordWritten = $true
    }
    else {
        $lines.Add($line)
    }
}

if (-not $demoModeWritten) {
    $lines.Add("IsDemoVersion=false")
}

if (-not $sqlPasswordWritten) {
    $lines.Add("MSSQL_SA_PASSWORD=$sqlPassword")
}

[System.IO.File]::WriteAllText($Path, (($lines -join "`n") + "`n"), (New-Object System.Text.UTF8Encoding($false)))

Write-Host ""
Write-Host "Production secrets written to: $Path"
Write-Host "IsDemoVersion=false"
Write-Host "Jwt__Key=$key"
Write-Host "AspNetIdentity__DefaultAdmin__Email=$AdminEmail"
Write-Host "AspNetIdentity__DefaultAdmin__Password=$password"
Write-Host "MSSQL_SA_PASSWORD=$sqlPassword"
Write-Host "Treat these values as secrets. Running this script again rotates them, and"
Write-Host "rotating the JWT key signs out every user."

if ($PassThru) {
    [pscustomobject]@{
        Path = $Path
        IsDemoVersion = $false
        JwtKey = $key
        AdminEmail = $AdminEmail
        AdminPassword = $password
        SqlPassword = $sqlPassword
    }
}
