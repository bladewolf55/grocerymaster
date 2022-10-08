# Generic test script for .NET apps

# If you need customizations, copy/paste this script and be fully customized.
# Only add to this script if the change is truly generic.

[CmdletBinding()]
Param (
    [Parameter(Mandatory = $true)][String]$ProjectDirectory,
    [switch]$CI, 
    [switch]$Clean
)

Write-Host "# TEST .NET" -ForegroundColor Cyan

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
$TestResultsDirectory = Join-Path $WorkingDirectory "package" "testresults"
$CoverageResultsDirectory = Join-Path $WorkingDirectory "package" "coverageresults"

Write-Message "CI                       $CI"
Write-Message "Clean                    $Clean"
Write-Message "NoTest                   $NoTest"
Write-Message "ProjectName              $ProjectName"
Write-Message "WorkingDirectory         $WorkingDirectory"
Write-Message "ProjectDirectory         $ProjectDirectory"
Write-Message "TestResultsDirectory     $TestResultsDirectory"
Write-Message "CoverageResultsDirectory $CoverageResultsDirectory"

try {
    if ($Clean) {
        Write-Message "## CLEAN"
        GitClean 
    }

    # Should run dotnet within project directory. Required if global.json is used.
    Run {Set-Location $ProjectDirectory}

	Write-Message "## TEST"
	$LogFileName = "$TestProjectName" + ".trx"
	Run { dotnet test --results-directory $TestResultsDirectory --logger "trx;LogFileName=$LogFileName" }
}
catch {}
finally {
    Write-Host $Error -ForegroundColor Red
    Set-Location $WorkingDirectory
}