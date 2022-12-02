using FantasyFootballMAUI;
using FantasyFootballMAUI.Helper;
using FantasyFootballMAUI.Models;
using FantasyFootballMAUI.ViewModels;
using Newtonsoft.Json.Linq;
using System.Text;


namespace FantasyFootballMAUI;

[QueryProperty(nameof(GetUserId), "userid")]
public partial class HomePage : ContentPage
{
	private string _userId;
	private string username;
	private string name;
	private string email;

	private string GetUserId
	{
		get => _userId;
		set { _userId = value.ToString(); }
	}
	public string Username { get => username; set => username = value; }
	public string Name { get => name; set => name = value; }
	public string Email { get => email; set => email = value; }

	public HomePage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		page.Title = $"{GetUserId}";
		//int.TryParse(userId, out int UserId);
		//UserIdId id = new UserIdId(UserId);

		//await getUserEmailAndUserName(id);

		//page.Title = $"Signed in as: {Username} ({Email})"; 
	} // End method


	public async Task getUserEmailAndUserName(int userId)
	{
		var Url = "http://localhost:8000/api/getuser";
		using var client = new HttpClient();

		client.DefaultRequestHeaders.Add("UsernameEmail", $"Bearer {userId}"); // add user id to UsernameEmail header to receive back user and user's information

		var response = await client.GetAsync(Url);
		var result = await response.Content.ReadAsStringAsync();

		Username = JObject.Parse(result)["user_name"].ToString(); // receive username of logged in user
		Name = JObject.Parse(result)["name"].ToString(); // receive name of logged in user
		Email = JObject.Parse(result)["email"].ToString(); // receive email of logged in user
	} // End method

}