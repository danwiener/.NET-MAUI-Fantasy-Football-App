﻿
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

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
                fonts.AddFont("Aloevera-OVoWO.ttf", "Aloevera");
                fonts.AddFont("Poppins-Medium.ttf", "Poppins");
                fonts.AddFont("Poppins-Thin.ttf", "PT");
            });


        //#if DEBUG
        //        builder.Logging.AddDebug();
        //#endif


        var app = builder.Build();
        return builder.Build();
    }
}
