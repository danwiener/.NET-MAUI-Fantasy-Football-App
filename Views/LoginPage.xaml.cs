using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using FantasyFootballMAUI.Models;
using Microsoft.Maui.ApplicationModel.Communication;

namespace FantasyFootballMAUI;

public partial class LoginPage : ContentPage
{
    bool success;

    private string user_Id;

	public string userId { get => user_Id; set => user_Id = value; }

	public LoginPage()
    {
        InitializeComponent();

    } // End constructor

    protected async override void OnAppearing()
    {
        Task scaleTitle = Task.Factory.StartNew(async() => { await TitleLabel.ScaleTo(3, 1000); });
		//Task scaleLoginBtn = Task.Factory.StartNew(async () => { await LoginBtn.ScaleTo(1, 500); }); // To scale sign in button
    } // End method
		private async void OnLoginBtnClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(LoginBtn.Text);
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        NewLogin newlogin = new NewLogin(email, password);
        await LoginUserAsync(newlogin);

        UserDTO dto = new UserDTO(int.Parse(userId));
        var navParam = new Dictionary<string, object>()
        {
            {"userid", dto }
        };
        await Shell.Current.GoToAsync($"{nameof(HomePage)}", navParam);
	} // End method


    ////    Microsoft.Maui.Controls.Application.Current?.CloseWindow(Microsoft.Maui.Controls.Application.Current.MainPage.Window);

    private async void OnForgotPasswordBtnClicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"{nameof(ForgotPasswordPage)}");
	}


	public async Task LoginUserAsync(NewLogin nl)
    {
        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(nl);
        var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var url = "http://localhost:8000/api/login"; // access the login endpoint to receive access token from authorization server
        using var client = new HttpClient();

        var response = await client.PostAsync(url, data);

        if (!response.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "Invalid credentials - Try again", "Ok");
        }

        var result = await response.Content.ReadAsStringAsync(); // receive the access token
        string accessToken = JObject.Parse(result)["token"].ToString(); // parse access token to string

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}"); // add to Authorization header to send back to API to request access to resource server/protected data

        var newUrl = "http://localhost:8000/api/user"; // get request to user endpoint to receive protected user information and log user in

        var response2 = await client.GetAsync(newUrl);

        var result2 = await response2.Content.ReadAsStringAsync();
        userId = JObject.Parse(result2)["UserId"].ToString(); // receive user id of user logged in

        if (response2.IsSuccessStatusCode)
        {
			success = false;
			await DisplayAlert("Success", $"{nl.email} logged in successfully", "Ok");
        }
        else
        {
			success = false;
			await DisplayAlert("Error", $"Please try again", "Ok");
        }
    } // End method

    // Go to register page
    private async void OnSignUpButtonClicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
	} // End method

	private void OnForgotPasswordButtonClicked(object sender, EventArgs e)
    {

    }

	// Show passsword
	private void OnShowPasswordBtnClicked(object sender, EventArgs e)
    {
        if (PasswordEntry.IsPassword)
        {
			PasswordEntry.IsPassword = false;
		}
        else
        {
            PasswordEntry.IsPassword = true;
        }
    } // End method
}
