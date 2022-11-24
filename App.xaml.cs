namespace FantasyFootballMAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        var navPage = new NavigationPage(new LoginPage());
        navPage.BarBackgroundColor = Color.FromArgb("9ac6c5");
        navPage.BarTextColor = Color.FromArgb("360568");

        MainPage = navPage;
	}
}
