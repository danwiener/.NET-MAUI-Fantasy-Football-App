namespace FantasyFootballMAUI;

public partial class OpeningPage : ContentPage
{
	public OpeningPage()
	{
		InitializeComponent();
	}

	private void OnLoginBtnClicked(object sender, EventArgs e)
	{
        SemanticScreenReader.Announce(LoginBtn.Text);
        Navigation.PushAsync(new LoginPage());
    }

	private void OnRegisterBtnClicked(object sender, EventArgs e)
	{
        SemanticScreenReader.Announce(RegisterBtn.Text);
        Navigation.PushAsync(new RegisterPage2());
    }
}