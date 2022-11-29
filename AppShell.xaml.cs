﻿using Microsoft.Maui.ApplicationModel.Communication;
using System.Xml.Linq;

namespace FantasyFootballMAUI;

public partial class AppShell : Shell
{
    //string email;
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(OpeningPage), typeof(OpeningPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

	} // End constructor

    protected async override void OnAppearing()
    {
        base.OnAppearing();
    } // End method
}
