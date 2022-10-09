# Grocery Master

## Introduction
Shared grocery lists that work

## Environment
> Changing the environment? Change the CI/CD pipeline!

Install                          | Version    
---------------------------------|------------
.NET                             | 7
PowerShell                       | 7.x
Visual Studio                    | VS 2022 with .NET MAUI workload
EF Core Tools                    | latest
Android SDKs                     | 31, 32, 33

Sources:
*   [.NET](https://dotnet.microsoft.com/download/dotnet)
*   [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-windows)
*   [Visual Studio](https://visualstudio.microsoft.com/downloads/)

## Getting Started
> Be sure to use the correct versions from above.

**Environment**  
1.  Install PowerShell 7.x.
1.  Install .NET 7.
1.  Install Android SDKs from Visual Studio > Tools > Android > Android SDK Manager (see Microsoft's .NET MAUI installation instructions for more info)

```powershell
$ErrorActionPreference = 'Stop'
# ef core tools are installed as part of the build script
```

**Application** 
```powershell
# Clone source
$ErrorActionPreference = 'Stop'
$userRoot = $env:userprofile
cd "$userRoot/source/repos"
git clone https://github.com/bladewolf55/grocerymaster.git
cd GroceryMaster
# Build app
./build/build.ps1
```

## Daily Development
Things needed to smoothly develop, such as how to run service dependencies.

## Migrations
Example:

`dotnet ef migrations add InitialCreate --project GroceryMaster.Services --startup-project GroceryMaster.Migrations --context GroceryMasterDbContext`

## When to run the build script
```powershell
./build/build.ps1
```
The build script simulates what the CI server will do, and is intended to catch build errors before they get to the server. The script should be run before the "final" PR commit. That is:

1.  Before pushing the final feature branch, AND/OR
1.  Before pushing to the mainline branch

## Troubleshooting
