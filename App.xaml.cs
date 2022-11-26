namespace FantasyFootballMAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        var navPage = new NavigationPage(new OpeningPage());
        navPage.BarBackgroundColor = Color.FromArgb("eeeeee");
        navPage.BarTextColor = Color.FromArgb("233b66");
        navPage.WidthRequest = 600;

        MainPage = navPage;
	}
}
