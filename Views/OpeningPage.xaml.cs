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
		Task scaleTitle = Task.Factory.StartNew(async () => { await TitleLabel.ScaleTo(1, 2000); });

		await Task.Delay(1000);
		Task rotateLabel1 = Task.Factory.StartNew(async () => { await SignInLabel.RotateYTo(35, 1200); });
		Task rotateLabel2 = Task.Factory.StartNew(async () => { await SignUpLabel.RotateYTo(-35, 1200); });
		Task rotateLeftGrid = Task.Factory.StartNew(async () => { await LeftGrid.RotateYTo(35, 1200); });
		Task rotateRightGrid = Task.Factory.StartNew(async () => { await RightGrid.RotateYTo(-35, 1200); });
		Task rotateFootball = Task.Factory.StartNew(async () => { await LoginGrid.RotateXTo(85, 300); });
		//await Task.Delay(700);
		//Task rotateFootball2 = Task.Factory.StartNew(async () => { await LoginGrid.RotateXTo(85, 800); });
	} // End method

	//private void OnLoginBtnClicked(object sender, EventArgs e)
	//{
	//	SemanticScreenReader.Announce(LoginBtn.Text);
	//	Navigation.PushAsync(new LoginPage());
	//}

	//private void OnRegisterBtnClicked(object sender, EventArgs e)
	//{
	//	SemanticScreenReader.Announce(RegisterBtn.Text);
	//	Navigation.PushAsync(new RegisterPage());
	//}
}