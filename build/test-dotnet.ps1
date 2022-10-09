# Generic test script for .NET apps

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
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
$Title = 'TEST .NET'
$WorkingDirectory = Get-Location
$ProjectDirectory = Join-PathClean $WorkingDirectory $ProjectDirectory 
$ProjectName = Split-Path $ProjectDirectory -Leaf
if (-Not([System.IO.Path]::IsPathRooted($PackageDirectory))) {
    $PackageDirectory = $PackageDirectory = Join-PathClean $WorkingDirectory $PackageDirectory
}
$TestResultsDirectory = Join-PathClean $PackageDirectory "testresults"
$CoverageResultsDirectory = Join-PathClean $PackageDirectory "coverageresults"

Write-H1 $Title
Write-H2 "SETTINGS"
Write-Message "CI                       $CI"
Write-Message "Clean                    $Clean"
Write-Message "NoTest                   $NoTest"
Write-Message "ProjectName              $ProjectName"
Write-Message "WorkingDirectory         $WorkingDirectory"
Write-Message "ProjectDirectory         $ProjectDirectory"
Write-Message "TestResultsDirectory     $TestResultsDirectory"
Write-Message "CoverageResultsDirectory $CoverageResultsDirectory"

# ===== MAIN =====
try {
    if ($Clean) {
        Write-H2 "CLEAN"
        GitClean 
    }

    # Should run dotnet within project directory. Required if global.json is used.
    Run {Set-Location $ProjectDirectory}

	Write-H2 "TEST"
	$LogFileName = "$TestProjectName" + ".trx"
	Run { dotnet test --results-directory $TestResultsDirectory --logger "trx;LogFileName=$LogFileName" }
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}