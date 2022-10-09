[CmdletBinding()]
Param (
    # Convention is running from a root/build folder
    # User can pass in path relative to where calling from,
    # e.g. if in repo root:
    # . build/build.ps1 package
    [String]$PackageDirectory = (Join-Path $PSScriptRoot '../package'),
    [switch]$CI, 
    [switch]$Clean,
    [switch]$NoTest,
    [switch]$TestOnly
    )

# ===== INIT =====
$StartDirectory = Get-Location
# Immediately exit on any error
$ErrorActionPreference = 'Stop'
# Ensure running from where script is located
Set-Location $PSScriptRoot

# ===== FUNCTIONS =====
. ./shared-functions.ps1 

function End() {
    $sw.Stop()
    Write-Host "=========================="
    Write-Host Total elapsed time: $sw.Elapsed.TotalMinutes.ToString("f") minutes -ForegroundColor Green
    Write-Host "=========================="
    Set-Location $StartDirectory
    Exit
}

# ===== SETTINGS =====
$Title = "BUILD ALL"
$PackageDirectory = Convert-PathClean $PackageDirectory
if (-Not([System.IO.Path]::IsPathRooted($PackageDirectory))) {
	$PackageDirectory = Join-PathClean $StartDirectory $PackageDirectory
}

Write-H1 $Title
Write-H2 "SETTINGS"
Write-Message "CI                       $CI"
Write-Message "Clean                    $Clean"
Write-Message "StartDirectory           $StartDirectory"
Write-Message "PackageDirectory         $PackageDirectory"
Write-Message ""

# ===== MAIN =====
$sw = [System.Diagnostics.Stopwatch]::StartNew()

# Environment
./build-environment

# Test
if (-Not($NoTest)) {
    ./test-dotnet `
        -ProjectDirectory ../src/GroceryMaster.UnitTests `
        -PackageDirectory $PackageDirectory `
        -CI:$CI `
        -Clean:$Clean 
}

if ($TestOnly) {
    End
}

# Build and Package
./build-dotnet `
    -ProjectDirectory ../src/GroceryMaster.Services `
    -PackageDirectory $PackageDirectory `
    -CI:$CI `
    -Clean:$Clean

./build-maui `
    -ProjectDirectory ../src/GroceryMaster.UI `
    -PackageDirectory $PackageDirectory `
    -CI:$CI `
    -Clean:$Clean

# Data
./build-migrations `
    -ProjectDirectory ../src/GroceryMaster.Migrations `
    -StartupProjectDirectory ../src/GroceryMaster.Migrations `
    -DbContextName GroceryMasterDbContext `
    -PackageDirectory $PackageDirectory `
    -CI:$CI `
    -Clean:$Clean `

# Exit with stats
End