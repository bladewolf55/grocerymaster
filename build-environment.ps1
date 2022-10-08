# The idea is to run this once to verify/update the environmen dependencies. Might be too complicated.

Write-Host "# BUILD ENVIRONMENT" -ForegroundColor Cyan

# ===== FUNCTIONS  =====
. ./build-functions.ps1

# ===== INIT =====
if ($PSVersionTable.PSEdition -ne "Core") {
    throw "ERROR: You need to be running in PowerShell Core."
}

# ===== VERIFY =====
Write-Message "Update ef tools"
Run {dotnet tool update --global dotnet-ef}