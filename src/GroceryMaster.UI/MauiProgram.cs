using GroceryMaster.Data;
using GroceryMaster.Maui.Maui.Pages;
using GroceryMaster.Maui.Maui.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GroceryMaster.Maui.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddDbContext<GroceryMasterDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("GroceryMasterDb"))
            );
            builder.Services.AddSingleton<IGroceryDataService, GroceryDataService>();
            builder.Services.AddSingleton<StoreEdit>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}