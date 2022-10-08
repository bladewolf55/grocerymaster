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
    -ProjectDirectory src/GroceryMaster.UnitTests `
    -TestProjectDirectory src/GroceryMaster.UnitTests `
    -CI:$CI `
    -Clean:$Clean `
    -NoTest:$NoTest

# Package
./build-dotnet `
    -ProjectDirectory src/GroceryMaster.Services `
    -CI:$CI `
    -Clean:$Clean `
    -NoTest:$True

./build-maui `
    -ProjectDirectory src/GroceryMaster.UI `
    -CI:$CI `
    -Clean:$Clean `
    -NoTest:$True

# Data
./build-migrations `
    -ProjectDirectory src/GroceryMaster.Migrations `
    -StartupProjectDirectory src/GroceryMaster.Migrations `
    -DbContextName GroceryMasterDbContext `
    -CI:$CI `
    -Clean:$Clean `

$sw.Stop()
Write-Host "=========================="
Write-Host Total elapsed time: $sw.Elapsed.TotalMinutes.ToString("f") minutes -ForegroundColor Green
Write-Host "=========================="
