# Generic build script for .NET apps

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
    [Parameter(Mandatory = $true)][String]$ProjectDirectory,
    [String]$TestProjectDirectory,
    [switch]$CI, 
    [switch]$Clean,
    [switch]$NoTest
)

Write-Host "# BUILD .NET" -ForegroundColor Cyan

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
if ($TestProjectDirectory){
    $TestProjectDirectory = Join-Path $WorkingDirectory $TestProjectDirectory
}
$TestProjectName = Split-Path $TestProjectDirectory -Leaf
$TestResultsDirectory = Join-Path $WorkingDirectory "package" "testresults"
$CoverageResultsDirectory = Join-Path $WorkingDirectory "package" "coverageresults"

Write-Message "CI                       $CI"
Write-Message "Clean                    $Clean"
Write-Message "NoTest                   $NoTest"
Write-Message "ProjectName              $ProjectName"
Write-Message "WorkingDirectory         $WorkingDirectory"
Write-Message "ProjectDirectory         $ProjectDirectory"
Write-Message "PackageDirectory         $PackageDirectory"
Write-Message "TestProjectName          $TestProjectName"
Write-Message "TestProjectDirectory     $TestProjectDirectory"
Write-Message "TestResultsDirectory     $TestResultsDirectory"
Write-Message "CoverageResultsDirectory $CoverageResultsDirectory"

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

    if (-Not($NoTest) -and $TestProjectDirectory -ne '') {
        Write-Message "## TEST"
        $LogFileName = "$TestProjectName" + ".trx"
        Run { dotnet test $TestProjectDirectory --results-directory $TestResultsDirectory --logger "trx;LogFileName=$LogFileName" }
    }

    Write-Message "# PUBLISH"
    Run { dotnet publish --no-restore -c Release --output $PackageDirectory}
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}