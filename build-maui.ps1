# Generic build script for .NET apps

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
    [Parameter(Mandatory = $true)][String]$SourceDirectory,
    [Parameter(Mandatory = $true)][String]$ProjectName,
    [String]$TestProjectName,
    [switch]$CI, 
    [switch]$NoClean,
    [switch]$NoTest
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
$PackageDirectory = Join-Path $WorkingDirectory "package" $ProjectName
$PackageDirectoryWindows = Join-Path $PackageDirectory "windows"
$PackageDirectoryAndroid = Join-Path $PackageDirectory "android"
$PackageDirectoryIos = Join-Path $PackageDirectory "ios"
$PackageDirectoryMac = Join-Path $PackageDirectory "mac"
$TestProjectDirectory = Join-Path $WorkingDirectory $SourceDirectory $TestProjectName
$TestResultsDirectory = Join-Path $WorkingDirectory "package" "testresults"
$CoverageResultsDirectory = Join-Path $WorkingDirectory "package" "coverageresults"

Write-Message "CI                       $CI"
Write-Message "NoClean                  $NoClean"
Write-Message "NoTest                   $NoTest"
Write-Message "SourceDirectory          $SourceDirectory"
Write-Message "ProjectName              $ProjectName"
Write-Message "WorkingDirectory         $WorkingDirectory"
Write-Message "ProjectDirectory         $ProjectDirectory"
Write-Message "PackageDirectory         $PackageDirectory"
Write-Message "PackageDirectoryWindows  $PackageDirectoryWindows"
Write-Message "PackageDirectoryAndroid  $PackageDirectoryAndroid"
Write-Message "PackageDirectoryIos      $PackageDirectoryIos"
Write-Message "PackageDirectoryMac      $PackageDirectoryMac"
Write-Message "TestProjectName          $TestProjectName"
Write-Message "TestProjectDirectory     $TestProjectDirectory"
Write-Message "TestResultsDirectory     $TestResultsDirectory"
Write-Message "CoverageResultsDirectory $CoverageResultsDirectory"

try {
    if (-Not($CI) -And -Not($NoClean)) {
        Write-Message "## CLEAN"
        GitClean 
        # remove other untracked folders that aren't fully cleaned
        if (Run { Test-Path $PackageDirectory } ) {
            Run { Remove-Item $PackageDirectory -Recurse }
        }
    }

    Write-Message "## BUILD"
    Run { dotnet build $ProjectDirectory -c Release -t:Rebuild -p:WarningLevel=1 -warnAsMessage:"CS1591" }

    if (-Not($NoTest) -and $TestProjectName -ne '') {
        Write-Message "## TEST"
        $LogFileName = "$TestProjectName" + ".trx"
        Run { dotnet test $TestProjectDirectory --results-directory $TestResultsDirectory --logger "trx;LogFileName=$LogFileName" }
    }

    Write-Message "# PUBLISH"
    Run { dotnet publish $ProjectDirectory -c Release --output $PackageDirectoryWindows --framework net6.0-windows10.0.19041.0}
    Run { dotnet publish $ProjectDirectory -c Release --output $PackageDirectoryAndroid --framework net6.0-android}
    # Run { dotnet publish $ProjectDirectory -c Release --output $PackageDirectoryIos     --framework net6.0-ios}
    # Run { dotnet publish $ProjectDirectory -c Release --output $PackageDirectoryMac     --framework net6.0-maccatalyst}
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}