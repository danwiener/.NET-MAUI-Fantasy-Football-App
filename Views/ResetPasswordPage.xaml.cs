using FantasyFootballMAUI.Models;
using System.Text;

namespace FantasyFootballMAUI;


public partial class ResetPasswordPage : ContentPage
{
	bool success;
	public ResetPasswordPage()
	{
		InitializeComponent();
	}

	protected async override void OnAppearing()
	{
		Task scaleTitle = Task.Factory.StartNew(async () => { await TitleLabel.ScaleTo(2, 1000); });
	} // End method

	private async void OnResetPasswordBtnClicked(object sender, EventArgs e)
	{
		string token = ResetCodeEntry.Text.ToString();
		string password = PasswordEntry.Text.ToString();
		string password_confirm = PasswordConfirmEntry.Text.ToString();
		ResetPassword rp = new ResetPassword(token, password, password_confirm);

		ResetPasswordAsync(rp);

	} // End method

	// Reset user's password
	public async Task ResetPasswordAsync(ResetPassword dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var url = "http://localhost:8000/api/reset"; // access the reset endpoint to check whether passwords match, search db for token equal to token pasted into app, and then find user associated with email attached to token to change that user's password
		using var client = new HttpClient();

		var response = await client.PostAsync(url, data);

		var result = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
		{
			await DisplayAlert("Error", $"{result.ToString()}", "Ok");
			success = false;
		}
		else
		{
			await DisplayAlert("Success", "Your password has been reset, please sign in", "Ok");
			await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
		}
	}

	private void OnShowPasswordBtnClicked(object sender, EventArgs e)
	{
		if (PasswordEntry.IsPassword)
		{
			PasswordEntry.IsPassword = false;
			PasswordConfirmEntry.IsPassword = false;
		}
		else
		{
			PasswordEntry.IsPassword = true;
			PasswordConfirmEntry.IsPassword = true;
		}
	}

	private async void OnGoBackButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync($"{nameof(ForgotPasswordPage)}");
	}
}