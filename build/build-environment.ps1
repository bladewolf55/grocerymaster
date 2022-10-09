# The idea is to run this once to verify/update the environment dependencies. Might be too complicated.

# ===== INIT =====
if ($PSVersionTable.PSEdition -ne "Core") {
    throw "ERROR: You need to be running in PowerShell Core."
}
# Ensure running from where script is located
Set-Location $PSScriptRoot

# ===== FUNCTIONS =====
. ./shared-functions.ps1

# ===== SETTINGS =====
$Title = 'BUILD ENVIRONMENT'
Write-H1 $Title

# ===== MAIN =====
Write-Message "Update ef tools"
Run {dotnet tool update --global dotnet-ef}


# Ensure a newline
Write-Host ""