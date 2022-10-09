# Generic build script for .NET apps

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
    # Either full path or relative to $PSScriptRoot
    [Parameter(Mandatory = $true)][String]$ProjectDirectory,
    [Parameter(Mandatory = $true)][String]$PackageDirectory,
    [switch]$CI, 
    [switch]$Clean
)

# ===== INIT =====
if ($PSVersionTable.PSEdition -ne "Core") {
    throw "ERROR: You need to be running in PowerShell Core."
}
# Ensure running from where script is located
Set-Location $PSScriptRoot

# ===== FUNCTIONS  =====
. ./shared-functions.ps1

# ===== SETTINGS =====
$Title = "BUILD .NET"
$WorkingDirectory = Get-Location
$ProjectDirectory = Join-PathClean $WorkingDirectory $ProjectDirectory
$ProjectName = Split-Path $ProjectDirectory -Leaf
if (-Not([System.IO.Path]::IsPathRooted($PackageDirectory))) {
    $PackageDirectory = $PackageDirectory = Join-PathClean $WorkingDirectory $PackageDirectory
}
$PackageDirectory = Join-PathClean $PackageDirectory $ProjectName

Write-H1 $Title
Write-H2 "SETTINGS"
Write-Message "CI                       $CI"
Write-Message "Clean                    $Clean"
Write-Message "ProjectName              $ProjectName"
Write-Message "WorkingDirectory         $WorkingDirectory"
Write-Message "ProjectDirectory         $ProjectDirectory"
Write-Message "PackageDirectory         $PackageDirectory"

# ===== MAIN =====
try {
    # remove package
    if (Run { Test-Path $PackageDirectory } ) {
        Run { Remove-Item $PackageDirectory -Recurse }
    }

    if ($Clean) {
        Write-Heading "CLEAN" 2
        GitClean 
    }

    # Should run dotnet within project directory. Required if global.json is used.
    Run {Set-Location $ProjectDirectory}

    Write-H2 "BUILD"
    Run { dotnet build -c Release -p:WarningLevel=1 -warnAsMessage:"CS1591" }

    Write-H2 "PUBLISH"
    Run { dotnet publish --no-restore -c Release --output $PackageDirectory}
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}