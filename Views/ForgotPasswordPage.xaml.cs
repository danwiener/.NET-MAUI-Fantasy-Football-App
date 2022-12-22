
using FantasyFootballMAUI.Models;
using System.Text;

namespace FantasyFootballMAUI;
public partial class ForgotPasswordPage : ContentPage
{
	bool success;
	public ForgotPasswordPage()
	{
		InitializeComponent();
	}

	protected async override void OnAppearing()
	{
		Task scaleTitle = Task.Factory.StartNew(async () => { await TitleLabel.ScaleTo(2, 1000); });
	} // End method

	private async void OnGoBackButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
	}

	private async void OnSendForgotEmailBtn(object sender, EventArgs e)
	{
		try
		{
			string email = EmailEntry.Text;
			ForgotPassword forgotPassword = new ForgotPassword(email);
			await SendForgotPasswordEmailAsync(forgotPassword);

			if (success)
			{
				await Shell.Current.GoToAsync($"{nameof(ResetPage)}");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", "Connection issues, please try again", "Ok");
		}

	} // End method

	public async Task SendForgotPasswordEmailAsync(ForgotPassword dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");
		var url = "https://buttonhookfantasyfootballapi.azurewebsites.net/api/forgot"; // access the forgot endpoint to generate a reset token and send password reset email
		using var client = new HttpClient();

		var response = await client.PostAsync(url, data);

		if (!response.IsSuccessStatusCode)
		{
			await DisplayAlert("Error", "Email did not send, please try again", "Ok");
			success = false;
		}
		else
		{
			await DisplayAlert("Success", "Reset password email sent", "Ok");
			success = true;
		}
	} // End method
}