# Generic build script for .NET apps

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
    [Parameter(Mandatory = $true)][String]$ProjectDirectory,
    [switch]$CI, 
    [switch]$Clean
)

Write-Host "# BUILD .NET MAUI" -ForegroundColor Cyan

# ===== FUNCTIONS  =====
. ./build-functions.ps1

# ===== INIT =====
if ($PSVersionTable.PSEdition -ne "Core") {
    throw "ERROR: You need to be running in PowerShell Core."
}

# ===== MAIN =====
Write-Message "## SETTINGS"
$WorkingDirectory = Get-Location
$ProjectDirectory = Join-Path $WorkingDirectory $ProjectDirectory
$ProjectName = Split-Path $ProjectDirectory -Leaf
$PackageDirectory = Join-Path $WorkingDirectory "package" $ProjectName
$PackageDirectoryWindows = Join-Path $PackageDirectory "windows"
$PackageDirectoryAndroid = Join-Path $PackageDirectory "android"
$PackageDirectoryIos = Join-Path $PackageDirectory "ios"
$PackageDirectoryMac = Join-Path $PackageDirectory "mac"

Write-Message "CI                       $CI"
Write-Message "Clean                    $Clean"
Write-Message "ProjectName              $ProjectName"
Write-Message "WorkingDirectory         $WorkingDirectory"
Write-Message "ProjectDirectory         $ProjectDirectory"
Write-Message "PackageDirectory         $PackageDirectory"
Write-Message "PackageDirectoryWindows  $PackageDirectoryWindows"
Write-Message "PackageDirectoryAndroid  $PackageDirectoryAndroid"
Write-Message "PackageDirectoryIos      $PackageDirectoryIos"
Write-Message "PackageDirectoryMac      $PackageDirectoryMac"

try {
    # remove package
    if (Run { Test-Path $PackageDirectory } ) {
        Run { Remove-Item $PackageDirectory -Recurse }
    }

    if ($Clean) {
        Write-Message "## CLEAN"
        GitClean 
    }

    # Should run dotnet within project directory. Required if global.json is used.
    Run {Set-Location $ProjectDirectory}

    Write-Message "## BUILD"
    Run { dotnet build -c Release -p:WarningLevel=1 -warnAsMessage:"CS1591" }

    Write-Message "# PUBLISH"
    Run { dotnet publish --no-restore -c Release --output $PackageDirectoryWindows --framework net6.0-windows10.0.19041.0}
    Run { dotnet publish --no-restore -c Release --output $PackageDirectoryAndroid --framework net6.0-android32.0}
    # Run { dotnet publish -c Release --output $PackageDirectoryIos     --framework net6.0-ios}
    # Run { dotnet publish -c Release --output $PackageDirectoryMac     --framework net6.0-maccatalyst}
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}