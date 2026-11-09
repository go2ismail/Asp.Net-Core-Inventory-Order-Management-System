<#
.SYNOPSIS
    Generates every production secret and writes them to the .env file in one command.

.DESCRIPTION
    The application starts in demo mode by default (IsDemoVersion=true), so a fresh clone needs no
    setup. A production deployment must switch the mode off and provide deployment specific secrets:

      - Jwt__Key                             at least 32 bytes
      - AspNetIdentity__DefaultAdmin__Password  at least 12 characters
      - IsDemoVersion=false

    This script generates the two random values, updates the .env file in place (every other line is
    kept), and sets IsDemoVersion=false. `.env` is ignored by git and by Docker.

    Run it once before the first production start up. Running it again rotates both secrets: every
    user has to sign in again and the administrator password changes.

.PARAMETER Path
    Target file. Defaults to the .env file in the repository root.

.PARAMETER KeyByteLength
    Number of random bytes for the JWT key. The default of 48 bytes produces a 64 character key.

.PARAMETER PasswordLength
    Length of the generated administrator password. The default of 24 characters is longer than the
    minimum of 12 characters that the application requires.

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
    [string] $AdminEmail = "admin@root.com",
    [switch] $PassThru
)

$ErrorActionPreference = "Stop"

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

# appsettings.json defaults to demo mode, so a production file has to turn it off explicitly.
$lines = New-Object System.Collections.Generic.List[string]
$demoModeWritten = $false

foreach ($line in @(Get-Content -LiteralPath $Path)) {
    if ($line -match '^\s*(export\s+)?IsDemoVersion\s*=') {
        $lines.Add("IsDemoVersion=false")
        $demoModeWritten = $true
    }
    else {
        $lines.Add($line)
    }
}

if (-not $demoModeWritten) {
    $lines.Add("IsDemoVersion=false")
}

[System.IO.File]::WriteAllText($Path, (($lines -join "`n") + "`n"), (New-Object System.Text.UTF8Encoding($false)))

Write-Host ""
Write-Host "Production secrets written to: $Path"
Write-Host "IsDemoVersion=false"
Write-Host "Jwt__Key=$key"
Write-Host "AspNetIdentity__DefaultAdmin__Email=$AdminEmail"
Write-Host "AspNetIdentity__DefaultAdmin__Password=$password"
Write-Host "Treat these values as secrets. Running this script again rotates both of them, and"
Write-Host "rotating the JWT key signs out every user."

if ($PassThru) {
    [pscustomobject]@{
        Path = $Path
        IsDemoVersion = $false
        JwtKey = $key
        AdminEmail = $AdminEmail
        AdminPassword = $password
    }
}
