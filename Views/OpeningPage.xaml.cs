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

		Task rotateLabel21 = Task.Factory.StartNew(async () => { await SignInLabel.RotateYTo(15, 2000); });
		Task rotateLabel22 = Task.Factory.StartNew(async () => { await SignUpLabel.RotateYTo(-15, 2000); });
		Task rotateLeftGrid2 = Task.Factory.StartNew(async () => { await LeftGrid.RotateYTo(15, 2000); });
		Task rotateLeftButton = Task.Factory.StartNew(async () => { await LeftButtonGrid.RotateYTo(15, 2000); });
		Task rotateRightGrid2 = Task.Factory.StartNew(async () => { await RightGrid.RotateYTo(-15, 2000); });
		await Task.Delay(1500);
		Task rotateLabel1 = Task.Factory.StartNew(async () => { await SignInLabel.RotateYTo(35, 600); });
		Task rotateLabel2 = Task.Factory.StartNew(async () => { await SignUpLabel.RotateYTo(-35, 600); });
		Task rotateLeftGrid = Task.Factory.StartNew(async () => { await LeftGrid.RotateYTo(35, 600); });
		Task rotateRightGrid = Task.Factory.StartNew(async () => { await RightGrid.RotateYTo(-35, 600); });

		await Task.Delay(1000);
		Task rotateFootball = Task.Factory.StartNew(async () => { await LoginGrid.RotateXTo(80, 1900); });

		Task rotateLeftButton2 = Task.Factory.StartNew(async () => { await LeftButtonGrid.RotateYTo(35, 600); });

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