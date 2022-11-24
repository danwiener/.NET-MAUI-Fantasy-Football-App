using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FantasyFootballMAUI;

public partial class LoginPage2 : ContentPage
{
    // Data fields for user API constructor
    private string user_name;
    private string name;
    private string email;
    private string password;
    private string password_confirm;
    public LoginPage2(string user_name, string name, string email, string password, string password_confirm)
    {
        InitializeComponent();
        this.user_name = user_name;
        this.name = name;
        this.email = email;
        this.password = password;
        this.password_confirm = password_confirm;

    } // End constructor

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var existingPages = Navigation.NavigationStack.ToList();
        Navigation.RemovePage(existingPages[0]);

        NewUser nu = new NewUser(user_name, name, email, password, password_confirm);
        RegisterNewUserAsync(nu);
    } // End method

    public async Task RegisterNewUserAsync(NewUser nu)
    {
        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(nu);
        var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var url = "http://localhost:8000/api/register"; // access the register endpoint to register new user
        using var client = new HttpClient();

        var response = await client.PostAsync(url, data);

        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Congratulations", $"{nu.email} registered successfully", "Ok");
        }
        else
        {
            await DisplayAlert("Not successful", "Please try again", "Ok");
        }
    } // End method

    private async void OnLoginBtnClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(LoginBtn.Text);
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        NewLogin newlogin = new NewLogin(email, password);

        LoginUserAsync(newlogin);
    } // End method
    private void OnRegisterBtnClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(RegisterBtn.Text);
        Navigation.PopAsync();
    } // End method
    //    Microsoft.Maui.Controls.Application.Current?.CloseWindow(Microsoft.Maui.Controls.Application.Current.MainPage.Window);

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

        if (response2.IsSuccessStatusCode)
        {
            Microsoft.Maui.Controls.Application.Current.MainPage = new AppShell(nl.email);
        }
        else
        {
            await DisplayAlert("Error", $"Please try again", "Ok");
        }
    } // End method
} // End class
