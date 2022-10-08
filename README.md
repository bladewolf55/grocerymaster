

## Migrations
dotnet ef migrations add InitialCreate --project GroceryMaster.Services --startup-project GroceryMaster.Migrations --context GroceryMasterDbContext



## Troubleshooting
The .NET MAUI app currently depends on the .NET 6 SDK being installed and used. The `global.json` file enforces this requirement. To verify which SDKs are installed, 

`dotnet --list-sdks`

For reference, here are the steps to creating the global.json and restoring the project's workload. Citation: https://stackoverflow.com/a/73894027/14709316

```powershell
# Use the output from --list-sdks to determine the latest 6. version

# Navigate to project folder, install global.json
dotnet new globaljson --sdk-version [from-above]] --roll-forward latestMinor

# Restore project workloads
dotnet workload restore
```