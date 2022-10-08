[CmdletBinding()]
Param (
    [switch]$CI, 
    [switch]$Clean,
    [switch]$NoTest,
    [switch]$TestOnly
    )

# Immediately exit on any error
$ErrorActionPreference = 'Stop'

$sw = [System.Diagnostics.Stopwatch]::StartNew()

# Functions
function End() {
    $sw.Stop()
    Write-Host "=========================="
    Write-Host Total elapsed time: $sw.Elapsed.TotalMinutes.ToString("f") minutes -ForegroundColor Green
    Write-Host "=========================="
    Exit
}

# Environment
./build-environment


# Test
if (-Not($NoTest)) {
    ./test-dotnet `
        -ProjectDirectory src/GroceryMaster.UnitTests `
        -CI:$CI `
        -Clean:$Clean 
}

if ($TestOnly) {
    End
}

# Build and Package
./build-dotnet `
    -ProjectDirectory src/GroceryMaster.Services `
    -CI:$CI `
    -Clean:$Clean

./build-maui `
    -ProjectDirectory src/GroceryMaster.UI `
    -CI:$CI `
    -Clean:$Clean

# Data
./build-migrations `
    -ProjectDirectory src/GroceryMaster.Migrations `
    -StartupProjectDirectory src/GroceryMaster.Migrations `
    -DbContextName GroceryMasterDbContext `
    -CI:$CI `
    -Clean:$Clean `

End