[CmdletBinding()]
Param (
    [switch]$CI, 
    [switch]$NoClean
    # ,    [switch]$NoTest
    )

# Immediately exit on any error
$ErrorActionPreference = 'Stop'

$sw = [System.Diagnostics.Stopwatch]::StartNew()

# Environment
./build-environment

# Test
./build-dotnet `
    -SourceDirectory src `
    -ProjectName GroceryMaster.UnitTests `
    -TestProjectName GroceryMaster.UnitTests `
    -CI:$CI `
    -NoClean:$NoClean `
    -NoTest:$False

# Package
./build-dotnet `
    -SourceDirectory src `
    -ProjectName GroceryMaster.Services `
    -CI:$CI `
    -NoClean:$NoClean `
    -NoTest:$True

./build-maui `
    -SourceDirectory src `
    -ProjectName GroceryMaster.UI `
    -CI:$CI `
    -NoClean:$NoClean `
    -NoTest:$True

# Data
./build-migration `
    -SourceDirectory src `
    -ProjectName GroceryMaster.Migrations `
    -StartupProjectName GroceryMaster.Migrations `
    -DbContextName GroceryMasterDbContext `
    -CI:$CI `
    -NoClean:$NoClean `

$sw.Stop()
Write-Host "=========================="
Write-Host Total elapsed time: $sw.Elapsed.TotalMinutes.ToString("f") minutes -ForegroundColor Green
Write-Host "=========================="