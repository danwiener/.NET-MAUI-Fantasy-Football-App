using FantasyFootballMAUI;
using FantasyFootballMAUI.Models;
using Newtonsoft.Json.Linq;
using System.Text;


namespace FantasyFootballMAUI;

[QueryProperty(nameof(GetUserId), "userid")]
public partial class HomePage : ContentPage
{
	private UserDTO _userdto;
	private string username;
	private string name;
	private string email;

	public UserDTO GetUserId
	{
		get => _userdto;
		set { _userdto = value;
			OnPropertyChanged();
		}
	}
	public string Username { get => username; set => username = value; }
	public string Name { get => name; set => name = value; }
	public string Email { get => email; set => email = value; }

	public HomePage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		int.TryParse(GetUserId.UserId.ToString(), out int UserId);

		await getUserEmailAndUserName(UserId);

		LoginLabel.Text = $"Signed in as: {Username} ({Email})";
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