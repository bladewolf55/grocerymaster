using GroceryMaster.Data;
using GroceryMaster.UI.Pages;
using GroceryMaster.UI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CommunityToolkit.Maui;


namespace GroceryMaster.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var dbPath = Path.Join( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"grocerymaster.db");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<GroceryMasterDbContext>(options =>
            {
                //options.UseSqlite($"Data Source={dbPath}", sqliteOptions => 
                //    sqliteOptions.MigrationsAssembly("GroceryMaster.Migrations"));
                options.UseSqlite($"Data Source={dbPath}");
                options.LogTo(Console.WriteLine);
            }
            );
            builder.Services.AddSingleton<IGroceryDataService, GroceryDataService>();
            builder.Services.AddSingleton<StoresEdit>();
            builder.Services.AddSingleton<MainPage>();

            var app = builder.Build();

            // Ensure database created and update to date
            var db = app.Services.GetService<GroceryMasterDbContext>();
            //db.Database.Migrate();
            db.Database.EnsureCreated();


            return app;
        }
    }
}