
using FantasyFootballMAUI;
using FantasyFootballMAUI.Models;
using FantasyFootballMAUI.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FantasyFootballMAUI;

[QueryProperty(nameof(GetUserId), "userid")]
public partial class HomePage : ContentPage
{
	private UserDTO _userdto;
	private string username;
	private string name;
	private string email;
	ObservableCollection<League> belongedTo;
	ObservableCollection<User> creators;

	User user;

	// UserId logged in as
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
	public ObservableCollection<League> BelongedTo { get => belongedTo; 
		set
		{
			belongedTo = value;
			OnPropertyChanged();
		}
	}
	public ObservableCollection<User> Creators { get => creators; 
		set
		{
			creators = value;
			OnPropertyChanged();
		} 
	}

	public HomePage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		BelongedTo = new ObservableCollection<League>();
		Creators = new ObservableCollection<User>();

		LeaguesBelongedToCollectionView.ItemsSource = BelongedTo;

		int.TryParse(GetUserId.UserId.ToString(), out int UserId);
		await getUserEmailAndUserName(UserId);

		Title = "Home";
		UsernameLabel.Text = $"Signed in as: {user.Username} ({user.Email})";

		Task scaleTitle = Task.Factory.StartNew(async () => { await TitleLabel1.ScaleTo(1, 1000); });
		Task scaleTitle2 = Task.Factory.StartNew(async () => { await TitleLabel2.ScaleTo(1, 1000); });


	} // End method

	int j = 1;
	public async Task getUserEmailAndUserName(int userId)
	{
		var Url = "http://localhost:8000/api/getuser";
		using var client = new HttpClient();

		client.DefaultRequestHeaders.Add("UsernameEmail", $"Bearer {userId}"); // add user id to UsernameEmail header to receive back user and user's information

		var response = await client.GetAsync(Url);
		var result = await response.Content.ReadAsStringAsync();

		Username = JObject.Parse(result)["user_name"].ToString();
		Name = JObject.Parse(result)["name"].ToString();
		Email = JObject.Parse(result)["email"].ToString();

		user = new User(userId, Username, Name, Email);

		var Url2 = "http://localhost:8000/api/getleaguesbelongedto";
		client.DefaultRequestHeaders.Remove("UsernameEmail");
		client.DefaultRequestHeaders.Add("LeaguesBelongedToHeader", $"{userId}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
		var response2 = await client.GetAsync(Url2);
		var result2 = await response2.Content.ReadAsStringAsync();

		int[]? leagueids = (int[])JObject.Parse(result2)["leaguesbelongedto"].ToObject<int[]>();
		if (leagueids is null)
		{
			await DisplayAlert("No leagues joined or created", "Please join or create a league now", "Ok");
		}
		else
		{
			var Url3 = "http://localhost:8000/api/getleagues";

			for (int i = 0; i < leagueids.Count(); i++)
			{
				client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{leagueids[i]}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
				var response3 = await client.GetAsync(Url3);
				var result3 = await response3.Content.ReadAsStringAsync();

				client.DefaultRequestHeaders.Remove("LeagueIdHeader");
				int leagueId = int.Parse(JObject.Parse(result3)["LeagueId"].ToString());
				string leaguename = JObject.Parse(result3)["leaguename"].ToString();
				int maxteams = int.Parse(JObject.Parse(result3)["maxteams"].ToString());
				int creatorId = int.Parse(JObject.Parse(result3)["creator"].ToString());
				League league = new League(leagueId, leaguename, maxteams, creatorId);
				league.CreatedByCurrentUser = creatorId == GetUserId.UserId;

				string fmt = "000";
				string withLeadingZeroes = j.ToString(fmt); // pad image path suffix with adjusted leading 0s
				league.ImageSource = $"image_part_{withLeadingZeroes}.jpg";
				if (j < 60)
				{
					j++;
				}
				else
				{
					j = 1;
				}

				BelongedTo.Add(league);

				var Url4 = "http://localhost:8000/api/getuser"; // retrieve every user which created every league

				client.DefaultRequestHeaders.Add("UsernameEmail", $"Bearer {creatorId}");
				var response4 = await client.GetAsync(Url4);
				var result4 = await response4.Content.ReadAsStringAsync();

				client.DefaultRequestHeaders.Remove("UsernameEmail");

				int userID = int.Parse(JObject.Parse(result4)["UserId"].ToString());
				string username = JObject.Parse(result4)["user_name"].ToString();
				string name = JObject.Parse(result4)["name"].ToString();
				string email = JObject.Parse(result4)["email"].ToString();
				User user = new User(userID, username, name, email);
				Creators.Add(user);


				league.CreatorName = user.Name; // Merge name and username from user into league for collection view binding purposes
				league.CreatorUsername = user.Username;
			}
		}
		//int j = 1;
		//string fmt = "000";
		//for (int i = 0; i < belongedTo.Count(); i++)
		//{
		//	int j = 1;
		//	string fmt = "000";
		//	string withLeadingZeroes = j.ToString(fmt); // pad image path suffix with adjusted leading 0s
		//	belongedTo[i].ImageSource = $"image_part_{withLeadingZeroes}.jpg";
		//	if (j < 60)
		//	{
		//		j++;
		//	}
		//	else
		//	{
		//		j = 1;
		//	}
		//}
	}

	private void LeaguesBelongedToCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{

	}

	//private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	//{
	//	var league = e.CurrentSelection.FirstOrDefault() as League;
	//	if (league == null)
	//	{
	//		return;
	//	}

	//	((CollectionView)sender).SelectedItem = null;

	//}
} // End method
