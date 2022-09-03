# Generic build script for .NET apps

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
    [Parameter(Mandatory = $true)][String]$SourceDirectory,
    [Parameter(Mandatory = $true)][String]$ProjectName,
    [Parameter(Mandatory = $true)][String]$StartupProjectName,
    [Parameter(Mandatory = $true)][String]$DbContextName,    
    [switch]$CI,
    [switch]$NoClean
)

# ===== FUNCTIONS  =====
. ./build-functions.ps1

# ===== INIT =====
if ($PSVersionTable.PSEdition -ne "Core") {
    throw "ERROR: You need to be running in PowerShell Core."
}

# ===== MAIN =====
Write-Message "## SETTINGS"
$WorkingDirectory = Get-Location
$ProjectDirectory = Join-Path $WorkingDirectory $SourceDirectory $ProjectName
$StartupProjectDirectory = Join-Path $WorkingDirectory $SourceDirectory $StartupProjectName
$PackageDirectory = Join-Path $WorkingDirectory "package" $ProjectName
$BundleName = $DbContextName + "Migration.exe"
$BundleDirectory = Join-Path $WorkingDirectory "package" "Migrations" $ProjectName
$BundleFilePath = Join-Path $BundleDirectory $BundleName

Write-Message "CI                       $CI"
Write-Message "NoClean                  $NoClean"
Write-Message "SourceDirectory          $SourceDirectory"
Write-Message "ProjectName              $ProjectName"
Write-Message "StartupProjectName       $StartupProjectName"
Write-Message "DbContextName            $DbContextName"
Write-Message "WorkingDirectory         $WorkingDirectory"
Write-Message "ProjectDirectory         $ProjectDirectory"
Write-Message "StartupProjectDirectory  $StartupProjectDirectory"
Write-Message "PackageDirectory         $PackageDirectory"
Write-Message "BundleName               $BundleName"
Write-Message "BundleDirectory          $BundleDirectory"
Write-Message "BundleFilePath           $BundleFilePath"

try {
    if (-Not($CI) -And -Not($NoClean)) {
        Write-Message "## CLEAN"
        GitClean 
        # remove other untracked folders that aren't fully cleaned
        if (Run { Test-Path $PackageDirectory } ) {
            Run { Remove-Item $PackageDirectory -Recurse }
        }
    }

    Write-Message "# BUNDLE"
    if (-Not(Test-Path $BundleDirectory)) {
        Run { New-Item $BundleDirectory -Type Directory }
    }
    # Reason for --configuration Bundle
    # https://github.com/dotnet/efcore/issues/25555
    Run { dotnet ef migrations bundle --project "$ProjectDirectory" --startup-project "$StartupProjectDirectory" --context $DbContextName --configuration Bundle --force --output "$BundleFilePath"}
    # Get appsettings
    Run {Copy-Item -Path $StartupProjectDirectory/* -Destination $BundleDirectory -Include 'appsettings.*' }
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}