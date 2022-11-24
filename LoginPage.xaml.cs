﻿using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FantasyFootballMAUI;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    } // End constructor
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
        Navigation.PushAsync(new RegisterPage());
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
}