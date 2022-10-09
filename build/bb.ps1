Param (
	[String]$PackageDirectory = (Join-Path $PSScriptRoot '../package')
)

$PackageDirectory
$StartDirectory = Get-Location
Set-Location $PSScriptRoot
. ./shared-functions.ps1

$PackageDirectory = Convert-PathClean $PackageDirectory

if (-Not([System.IO.Path]::IsPathRooted($PackageDirectory))) {
	$PackageDirectory = Join-PathClean $StartDirectory $PackageDirectory
}

$PackageDirectory


exit
./build-dotnet ../src/GroceryMaster.Services $PackageDirectory

Set-Location $StartDirectory