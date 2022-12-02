
using FantasyFootballMAUI.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Extensions.DependencyInjection;

namespace FantasyFootballMAUI;

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
                fonts.AddFont("Sitka.ttc", "Sitka");
            });


        //#if DEBUG
        //        builder.Logging.AddDebug();
        //#endif


        var app = builder.Build();
        return builder.Build();
    }
}
