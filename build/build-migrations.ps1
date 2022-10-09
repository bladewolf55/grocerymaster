# Generic build script for .NET Migrations

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
    [Parameter(Mandatory = $true)][String]$ProjectDirectory,
    [Parameter(Mandatory = $true)][String]$StartupProjectDirectory,
    [Parameter(Mandatory = $true)][String]$DbContextName,
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
$Title = "BUILD MIGRATIONS"
$WorkingDirectory = Get-Location
$ProjectDirectory = Join-PathClean $WorkingDirectory $ProjectDirectory
$ProjectName = Split-Path $ProjectDirectory -Leaf
$StartupProjectName = Split-Path $StartupProjectDirectory -Leaf
$StartupProjectDirectory = Join-PathClean $WorkingDirectory $StartupProjectDirectory
if (-Not([System.IO.Path]::IsPathRooted($PackageDirectory))) {
    $PackageDirectory = $PackageDirectory = Join-PathClean $WorkingDirectory $PackageDirectory
}
$PackageDirectory
$BundleName = 'efbundle.exe'
$BundleDirectory = Join-PathClean $PackageDirectory "Migrations" $ProjectName
$BundleFilePath = Join-PathClean $BundleDirectory $BundleName
$PackageDirectory = Join-PathClean $PackageDirectory $ProjectName

Write-H1 $Title
Write-H2 "SETTINGS"
Write-Message "CI                       $CI"
Write-Message "Clean                    $Clean"
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

# ===== MAIN =====
try {
    # remove bundle and package
    if (Run { Test-Path $BundleDirectory } ) {
        Run { Remove-Item $BundleDirectory -Recurse }
    }
    if (Run { Test-Path $PackageDirectory } ) {
        Run { Remove-Item $PackageDirectory -Recurse }
    }

    if ($Clean) {
        Write-H2 "CLEAN"
        GitClean 
    }

    Write-H2 "BUNDLE"
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