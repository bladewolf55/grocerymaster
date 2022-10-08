# Generic build script for .NET Migrations

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

Write-Host "# BUILD MIGRATIONS" -ForegroundColor Cyan

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
$BundleName = 'efbundle.exe'
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
    # remove package
    if (Run { Test-Path $PackageDirectory } ) {
        Run { Remove-Item $PackageDirectory -Recurse }
    }

    if ($Clean) {
        Write-Message "## CLEAN"
        GitClean 
    }

    Write-Message "# BUNDLE"
    if (-Not(Test-Path $BundleDirectory)) {
        Run { New-Item $BundleDirectory -Type Directory }
    }
    # Reason for --configuration Bundle    
    # https://github.com/dotnet/efcore/issues/25555
    # In short, workaround to locked file error
    Run { dotnet ef migrations bundle --project "$ProjectDirectory" --startup-project "$StartupProjectDirectory" --context $DbContextName --configuration Bundle --force --output "$BundleFilePath"}
    # Get appsettings,including Development for local deployment. Development has the connection string.
    Run {Copy-Item -Path $StartupProjectDirectory/appsettings.json -Destination $BundleDirectory }
    Run {Copy-Item -Path $StartupProjectDirectory/appsettings.Development.json -Destination $BundleDirectory }
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}