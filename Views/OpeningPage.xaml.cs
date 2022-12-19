namespace FantasyFootballMAUI;

public partial class OpeningPage : ContentPage
{
	public OpeningPage()
	{
		InitializeComponent();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		Task scaleTitle = Task.Factory.StartNew(async () => { await TitleLabel.ScaleTo(1, 1000); });
	} // End method

	private void OnLoginBtnClicked(object sender, EventArgs e)
	{
		SemanticScreenReader.Announce(LoginBtn.Text);
		Navigation.PushAsync(new LoginPage());
	}

	private void OnRegisterBtnClicked(object sender, EventArgs e)
	{
		SemanticScreenReader.Announce(RegisterBtn.Text);
		Navigation.PushAsync(new RegisterPage());
	}
}