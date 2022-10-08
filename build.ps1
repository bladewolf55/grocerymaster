[CmdletBinding()]
Param (
    [switch]$CI, 
    [switch]$Clean,
    [switch]$NoTest
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
    -Clean:$Clean `
    -NoTest:$NoTest

# Package
./build-dotnet `
    -SourceDirectory src `
    -ProjectName GroceryMaster.Services `
    -CI:$CI `
    -Clean:$Clean `
    -NoTest:$True

./build-maui `
    -SourceDirectory src `
    -ProjectName GroceryMaster.UI `
    -CI:$CI `
    -Clean:$Clean `
    -NoTest:$True

# Data
./build-migration `
    -SourceDirectory src `
    -ProjectName GroceryMaster.Migrations `
    -StartupProjectName GroceryMaster.Migrations `
    -DbContextName GroceryMasterDbContext `
    -CI:$CI `
    -Clean:$Clean `

$sw.Stop()
Write-Host "=========================="
Write-Host Total elapsed time: $sw.Elapsed.TotalMinutes.ToString("f") minutes -ForegroundColor Green
Write-Host "=========================="
