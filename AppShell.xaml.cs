using Microsoft.Maui.ApplicationModel.Communication;
using System.Xml.Linq;

namespace FantasyFootballMAUI;

public partial class AppShell : Shell
{
    string email;
	public AppShell(string email)
	{
        this.email= email;
		InitializeComponent();
	} // End constructor

    protected async override void OnAppearing()
    {
        base.OnAppearing();


        await DisplayAlert("Congratulations", $"{email} logged in successfully", "Ok");
    } // End method
}
