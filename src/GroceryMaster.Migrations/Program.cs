using GroceryMaster.Data;
using Microsoft.EntityFrameworkCore.Design;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, builder) =>
    {
        // CreateDefaultBuilder automatically reads configs
        //IHostEnvironment env = context.HostingEnvironment;
        //builder.SetBasePath(AppContext.BaseDirectory)
        //.AddJsonFile("appsettings.json")
        //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //.AddEnvironmentVariables();
    })
    .ConfigureServices((builder, services) =>
    {
        IConfiguration configuration = builder.Configuration;
        services.AddSqlite<GroceryMasterDbContext>(
            configuration.GetConnectionString("GroceryMasterDb"), 
            options => options.MigrationsAssembly("GroceryMaster.Migrations"));
    })
    
    .Build();

await host.RunAsync();