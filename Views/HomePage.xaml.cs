
using FantasyFootballMAUI;
using FantasyFootballMAUI.Models;
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
	ObservableCollection<League> globalLeagues;
	ObservableCollection<User> creators;
	ObservableCollection<User> globalleaguecreators;
	ObservableCollection<League> currentlySelected;
	ObservableCollection<LeagueRules> leagueRules;
	private bool globalHasBeenSelected = false;

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

	public ObservableCollection<League> GlobalLeagues
	{
		get => globalLeagues;
		set
		{
			globalLeagues = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<League> CurrentlySelected
	{
		get => currentlySelected;
		set
		{
			currentlySelected = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<LeagueRules> CurrentLeagueRules
	{
		get => leagueRules;
		set
		{
			leagueRules = value;
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

	public ObservableCollection<User> GlobalLeagueCreators
	{
		get => globalleaguecreators;
		set
		{
			globalleaguecreators = value;
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
		GlobalLeagues = new ObservableCollection<League>();
		GlobalLeagueCreators= new ObservableCollection<User>();
		CurrentLeagueRules = new ObservableCollection<LeagueRules>();

		LeaguesBelongedToCollectionView.ItemsSource = BelongedTo;

		int.TryParse(GetUserId.UserId.ToString(), out int UserId);
		await getUserEmailAndUserName(UserId);
		await GetGlobalLeagues();

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
				League league = new League(leagueId, leaguename, maxteams, creatorId, creatorId == GetUserId.UserId);

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
	}

	//Event handlers
	private void LeaguesBelongedToCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ObservableCollection<League> currentlySelected = new ObservableCollection<League>();
		currentlySelected.Add(e.CurrentSelection.FirstOrDefault() as League);

		CurrentlySelected = currentlySelected;

		CurrentLeagueCollectionView.ItemsSource = CurrentlySelected;

	} // End method

	private void GlobalLeaguesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ObservableCollection<League> currentlySelected = new ObservableCollection<League>();
		currentlySelected.Add(e.CurrentSelection.FirstOrDefault() as League);
		CurrentlySelected = currentlySelected;
		CurrentLeagueCollectionView.ItemsSource = CurrentlySelected;

	} // End method

	private async void OnViewLeagueClicked(object sender, EventArgs e)
	{
		if (LeaguesBelongedToGrid.IsVisible)
		{
			if (LeaguesBelongedToCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No league selected", "Please select a league to view", "Ok");
				return;
			}
			CurrentLeagueCollectionViewGrid.IsVisible = true;
			CurrentLeagueCollectionView.IsEnabled = true;
			CurrentLeagueCollectionView.SelectedItem = CurrentlySelected.FirstOrDefault();



			LeaguesBelongedToGrid.IsVisible = false;
			LeaguesBelongedToCollectionView.IsEnabled = false;
			globalHasBeenSelected = false;

			OrLabel.IsVisible= false;
			JoinCreateBtn.IsVisible= false;
		}
		else if (GlobalLeaguesGrid.IsVisible)
		{
			if (GlobalLeaguesCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No league selected", "Please select a league to view", "Ok");
				return;
			}
			CurrentLeagueCollectionViewGrid.IsVisible = true;
			CurrentLeagueCollectionView.IsEnabled = true;
			CurrentLeagueCollectionView.SelectedItem = CurrentlySelected.FirstOrDefault();
			GlobalLeaguesGrid.IsVisible = false;
			GlobalLeaguesCollectionView.IsEnabled = false;
			GoBackBtn2.IsVisible = false;
			JoinLeagueBtn.IsVisible = false;
			CreateLeagueBtn.IsVisible = false;
			globalHasBeenSelected = true;
		}

		JoinLeagueBtn.IsVisible = true;
		RulesBtn.IsVisible = true;
		ViewLeagueBtn.IsVisible= false;
		GoBackBtn.IsVisible = true;
		TitleLabel1.Text = "LEAGUE INFO";

	}

	private async void OnDeleteLeagueClicked(object sender, EventArgs e)
	{
		if (!CurrentlySelected[0].CreatedByCurrentUser)
		{
			await DisplayAlert("Not league owner", "You may only delete leagues that you created", "Ok");
			return;
		}

		DeleteLeagueDTO dto;
		if (LeaguesBelongedToGrid.IsVisible)
		{
			if (LeaguesBelongedToCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No league selected", "Please select a league to delete", "Ok");
				return;
			}
			if (LeaguesBelongedToCollectionView.SelectedItem != null)
			{
				League league = LeaguesBelongedToCollectionView.SelectedItem as League;
				dto = new DeleteLeagueDTO(league.LeagueId, league.LeagueName);
				await DeleteLeague(dto);
				foreach (League item in BelongedTo)
				{
					if (item.LeagueId == dto.leagueid)
					{
						BelongedTo.Remove(item);
						break;
					}
				}
				foreach (League item in GlobalLeagues)
				{
					if (item.LeagueId == dto.leagueid)
					{
						GlobalLeagues.Remove(item);
						break;
					}
				}
			}
		}
		else if (GlobalLeaguesGrid.IsVisible)
		{
			if (GlobalLeaguesCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No league selected", "Please select a league to delete", "Ok");
				return;
			}
			if (GlobalLeaguesCollectionView.SelectedItem != null)
			{
				League league = GlobalLeaguesCollectionView.SelectedItem as League;
				dto = new DeleteLeagueDTO(league.LeagueId, league.LeagueName);
				await DeleteLeague(dto);
				foreach (League item in BelongedTo)
				{
					if (item.LeagueId == dto.leagueid)
					{
						BelongedTo.Remove(item);
						break;
					}
				}
				foreach (League item in GlobalLeagues)
				{
					if (item.LeagueId == dto.leagueid)
					{
						GlobalLeagues.Remove(item);
						break;
					}
				}
			}
		}
		else if (CurrentLeagueCollectionViewGrid.IsVisible)
		{
			if (CurrentLeagueCollectionView.SelectedItem != null)
			{
				League league = CurrentLeagueCollectionView.SelectedItem as League;
				dto = new DeleteLeagueDTO(league.LeagueId, league.LeagueName);
				await DeleteLeague(dto);
				foreach (League item in BelongedTo)
				{
					if (item.LeagueId == dto.leagueid)
					{
						BelongedTo.Remove(item);
						break;
					}
				}
				foreach (League item in GlobalLeagues)
				{
					if (item.LeagueId == dto.leagueid)
					{
						GlobalLeagues.Remove(item);
						break;
					}
				}
				if (globalHasBeenSelected)
				{
					GlobalLeaguesGrid.IsVisible = true;
					GlobalLeaguesCollectionView.IsEnabled = true;
					CreateLeagueBtn.IsVisible = true;
					GoBackBtn2.IsVisible = true;
					JoinLeagueBtn.IsVisible = false;
					TitleLabel1.Text = "GLOBAL LEAGUES";
				}
				else
				{
					LeaguesBelongedToGrid.IsVisible = true;
					LeaguesBelongedToCollectionView.IsEnabled = true;
					JoinLeagueBtn.IsVisible = false;
					OrLabel.IsVisible = true;
					JoinCreateBtn.IsVisible = true;
					TitleLabel1.Text = "LEAGUES BELONGED TO";
				}
			}
		}


		CurrentLeagueCollectionViewGrid.IsVisible = false;
		CurrentLeagueCollectionView.IsEnabled = false;

		if (CurrentlySelected != null)
		{
			CurrentlySelected.RemoveAt(0);
		}
		else
		{
			await DisplayAlert("No league selected", "Please select a league to delete", "Ok");
			return;
		}
		LeaguesBelongedToCollectionView.SelectedItem = null;
		CurrentLeagueCollectionView.SelectedItem= null;
		GlobalLeaguesCollectionView.SelectedItem= null;
			

		GoBackBtn.IsVisible = false;
		RulesBtn.IsVisible = false;
		ViewLeagueBtn.IsVisible = true;


	}

	public async Task DeleteLeague(DeleteLeagueDTO dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var url = "http://localhost:8000/api/deleteleague"; // access the register endpoint to register new user
		using var client = new HttpClient();

		var response = await client.PostAsync(url, data);

		var result = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", $"{dto.leaguename} has been deleted", "Ok");
		}
		else
		{
			await DisplayAlert("Not successful", "Please try again", "Ok");
		}
	} // End method

	private async void OnJoinOtherClicked(object sender, EventArgs e)
	{

		if (CurrentlySelected is not null)
		{
			CurrentlySelected.Clear();
		}
		LeaguesBelongedToCollectionView.SelectedItem = null; // Clear selected items which were causing bug with delete functionality
		CurrentLeagueCollectionView.SelectedItem = null;
		GlobalLeaguesCollectionView.SelectedItem = null;

		LeaguesBelongedToGrid.IsVisible = false;
		LeaguesBelongedToCollectionView.IsEnabled = false;

		GlobalLeaguesCollectionView.ItemsSource = GlobalLeagues;
		GlobalLeaguesGrid.IsVisible = true;
		GlobalLeaguesCollectionView.IsEnabled = true;

		OrLabel.IsVisible= false;
		JoinCreateBtn.IsVisible = false;

		CreateLeagueBtn.IsVisible = true;
		GoBackBtn2.IsVisible = true;

		globalHasBeenSelected = true;


		TitleLabel1.Text = "GLOBAL LEAGUES";
	}

	public async Task GetGlobalLeagues()
	{
		var Url = "http://localhost:8000/api/getgloballeagueids";
		using var client = new HttpClient();

		var response = await client.GetAsync(Url);
		var result = await response.Content.ReadAsStringAsync();

		int[]? leagueids = (int[])JObject.Parse(result)["leaguesbelongedto"].ToObject<int[]>();
		if (leagueids is null)
		{
			await DisplayAlert("No leagues created", "Be the first, create one now!", "Ok");
		}
		else
		{
			var Url2 = "http://localhost:8000/api/getleagues";
			for (int i = 0; i < leagueids.Count(); i++)
			{
				client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{leagueids[i]}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
				var response2 = await client.GetAsync(Url2);
				var result2 = await response2.Content.ReadAsStringAsync();

				client.DefaultRequestHeaders.Remove("LeagueIdHeader");
				int leagueId = int.Parse(JObject.Parse(result2)["LeagueId"].ToString());
				string leaguename = JObject.Parse(result2)["leaguename"].ToString();
				int maxteams = int.Parse(JObject.Parse(result2)["maxteams"].ToString());
				int creatorId = int.Parse(JObject.Parse(result2)["creator"].ToString());
				League league = new League(leagueId, leaguename, maxteams, creatorId, creatorId == GetUserId.UserId);

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

				GlobalLeagues.Add(league);
				var Url3 = "http://localhost:8000/api/getuser"; // retrieve every user which created every league

				client.DefaultRequestHeaders.Add("UsernameEmail", $"Bearer {creatorId}");
				var response3 = await client.GetAsync(Url3);
				var result3 = await response3.Content.ReadAsStringAsync();

				client.DefaultRequestHeaders.Remove("UsernameEmail");

				int userID = int.Parse(JObject.Parse(result3)["UserId"].ToString());
				string username = JObject.Parse(result3)["user_name"].ToString();
				string name = JObject.Parse(result3)["name"].ToString();
				string email = JObject.Parse(result3)["email"].ToString();
				User user = new User(userID, username, name, email);
				GlobalLeagueCreators.Add(user);


				league.CreatorName = user.Name; // Merge name and username from user into league for collection view binding purposes
				league.CreatorUsername = user.Username;
			}
		}
	}

	private void OnJoinLeagueClicked(object sender, EventArgs e)
	{

	}

	private void OnCreateLeagueClicked(object sender, EventArgs e)
	{

	}

	private void OnGoBackClicked(object sender, EventArgs e)
	{
		if (LeagueRulesGrid.IsVisible)
		{
			LeagueRulesGrid.IsVisible = false;
			LeagueRulesCollectionView.IsEnabled = false;

			CurrentLeagueCollectionViewGrid.IsVisible = true;
			CurrentLeagueCollectionView.IsEnabled = true;

			DeleteBtn.IsVisible = true;
			JoinLeagueBtn.IsVisible = true;
			RulesBtn.IsVisible = true;
			EditBtn.IsVisible = false;
			TitleLabel1.Text = "LEAGUE INFO";
			return;

		}
		if (!globalHasBeenSelected)
		{

			CurrentLeagueCollectionViewGrid.IsVisible = false;
			CurrentLeagueCollectionView.IsEnabled = false;

			LeaguesBelongedToGrid.IsVisible = true;
			LeaguesBelongedToCollectionView.IsEnabled = true;
			TitleLabel1.Text = "LEAGUES BELONGED TO";

			LeaguesBelongedToCollectionView.SelectedItem = null;

			OrLabel.IsVisible = true;
			JoinCreateBtn.IsVisible = true;

		}
		else if (globalHasBeenSelected)
		{
			CurrentLeagueCollectionViewGrid.IsVisible = false;
			CurrentLeagueCollectionView.IsEnabled = false;

			GlobalLeaguesGrid.IsVisible = true;
			GlobalLeaguesCollectionView.IsEnabled = true;
			GoBackBtn2.IsVisible = true;
			TitleLabel1.Text = "GLOBAL LEAGUES";
			CreateLeagueBtn.IsVisible = true;

			GlobalLeaguesCollectionView.SelectedItem = null;
		}


		if (CurrentlySelected is not null)
		{
			CurrentlySelected.Clear();
		}
		JoinLeagueBtn.IsVisible = false;
		RulesBtn.IsVisible = false;
		GoBackBtn.IsVisible= false;
		ViewLeagueBtn.IsVisible = true;

	} // End method

	private void OnGoBack2Clicked(object sender, EventArgs e)
	{
		LeaguesBelongedToCollectionView.SelectedItem = null; // Clear selected items which were causing bug with delete functionality
		CurrentLeagueCollectionView.SelectedItem = null;
		GlobalLeaguesCollectionView.SelectedItem = null;

		LeaguesBelongedToGrid.IsVisible = true;
		LeaguesBelongedToCollectionView.IsEnabled = true;

		GlobalLeaguesGrid.IsVisible = false;
		GlobalLeaguesCollectionView.IsEnabled = false;

		GoBackBtn2.IsVisible= false;

		JoinLeagueBtn.IsVisible= false;
		CreateLeagueBtn.IsVisible= false;

		globalHasBeenSelected = false;
		OrLabel.IsVisible = true;
		JoinCreateBtn.IsVisible = true;

		TitleLabel1.Text = "LEAGUES BELONGED TO";

	} // End method

	// Retrieve league rules for selected league
	private async void OnViewEditRulesClicked(object sender, EventArgs e)
	{
		TitleLabel1.Text = "LEAGUE RULES";
		League league = CurrentLeagueCollectionView.SelectedItem as League;
		int id = league.LeagueId;

		await GetRules(id);
		LeagueRulesCollectionView.ItemsSource = CurrentLeagueRules;
		ObservableCollection<LeagueRules> currentLeagueRules = new ObservableCollection<LeagueRules>();
		CurrentLeagueRules = currentLeagueRules;

		LeagueRulesGrid.IsVisible = true;
		LeagueRulesCollectionView.IsEnabled = true;

		CurrentLeagueCollectionViewGrid.IsVisible = false;
		CurrentLeagueCollectionView.IsEnabled = false;


		LeagueRulesGrid.IsVisible= true;
		LeagueRulesCollectionView.IsEnabled = true;

		DeleteBtn.IsVisible= false;
		JoinLeagueBtn.IsVisible= false;
		RulesBtn.IsVisible = false;
		EditBtn.IsVisible = true;

	} // End method

	public async Task GetRules(int leagueid)
	{
		var Url = "http://localhost:8000/api/getleaguerules";
		using var client = new HttpClient();
		client.DefaultRequestHeaders.Add("LeagueIdForRulesHeader", $"{leagueid}");

		var response = await client.GetAsync(Url);
		var result = await response.Content.ReadAsStringAsync();

		int maxteams = int.Parse(JObject.Parse(result)["maxteams"].ToString()); // extract league rules from http response
		int qbcount = int.Parse(JObject.Parse(result)["qbcount"].ToString()); // extract league rules from http response
		int rbcount = int.Parse(JObject.Parse(result)["rbcount"].ToString()); // extract league rules from http response
		int wrcount = int.Parse(JObject.Parse(result)["wrcount"].ToString()); // extract league rules from http response
		int tecount = int.Parse(JObject.Parse(result)["tecount"].ToString()); // extract league rules from http response
		int defensecount = int.Parse(JObject.Parse(result)["defensecount"].ToString()); // extract league rules from http response
		int kcount = int.Parse(JObject.Parse(result)["kcount"].ToString()); // extract league rules from http response
		int passingtdpoints = int.Parse(JObject.Parse(result)["passingtdpoints"].ToString()); // extract league rules from http response
		double ppc = Convert.ToDouble(JObject.Parse(result)["ppc"].ToString()); // extract league rules from http response
		double ppi = Convert.ToDouble(JObject.Parse(result)["ppi"].ToString()); // extract league rules from http response
		int PPTwentyFiveYdsPass = int.Parse(JObject.Parse(result)["pptwentyfivepass"].ToString()); // extract league rules from http response
		int fortyyardpassbonus = int.Parse(JObject.Parse(result)["fortyyardpassbonus"].ToString()); // extract league rules from http response
		int sixtyyardpassbonus = int.Parse(JObject.Parse(result)["sixtyyardpassbonus"].ToString()); // extract league rules from http response
		int threehundredyardpassbonus = int.Parse(JObject.Parse(result)["threehundredyardpassbonus"].ToString()); // extract league rules from http response
		int fivehundredyardpassbonus = int.Parse(JObject.Parse(result)["fivehundredyardpassbonus"].ToString()); // extract league rules from http response
		int rushingtdpoints = int.Parse(JObject.Parse(result)["rushingtdpoints"].ToString()); // extract league rules from http response
		int receivingtdpoints = int.Parse(JObject.Parse(result)["receivingtdpoints"].ToString()); // extract league rules from http response
		int pptenrush = int.Parse(JObject.Parse(result)["pptenrush"].ToString()); // extract league rules from http response
		int fortyyardrushreceivingbonus = int.Parse(JObject.Parse(result)["fortyyardrushreceivingbonus"].ToString()); // extract league rules from http response
		int sixtyyardrushreceivingbonus = int.Parse(JObject.Parse(result)["sixtyyardrushreceivingbonus"].ToString()); // extract league rules from http response
		int onehundredyardrushreceivingbonus = int.Parse(JObject.Parse(result)["onehundredyardrushreceivingbonus"].ToString()); // extract league rules from http response
		int twohundredyardrushreceivingbonus = int.Parse(JObject.Parse(result)["twohundredyardrushreceivingbonus"].ToString()); // extract league rules from http response
		double ppr = Convert.ToDouble(JObject.Parse(result)["ppr"].ToString()); // extract league rules from http response
		int twopointconversion = int.Parse(JObject.Parse(result)["twopointconversion"].ToString()); // extract league rules from http response
		int interceptionoffense = int.Parse(JObject.Parse(result)["interceptionoffense"].ToString()); // extract league rules from http response
		int fumbleoffense = int.Parse(JObject.Parse(result)["fumbleoffense"].ToString()); // extract league rules from http response
		int safetyoffense = int.Parse(JObject.Parse(result)["safetyoffense"].ToString()); // extract league rules from http response
		int sackdefense = int.Parse(JObject.Parse(result)["sackdefense"].ToString()); // extract league rules from http response
		int tackledefense = int.Parse(JObject.Parse(result)["tackledefense"].ToString()); // extract league rules from http response
		int fgpuntblock = int.Parse(JObject.Parse(result)["fgpuntblock"].ToString()); // extract league rules from http response
		int interceptiondefense = int.Parse(JObject.Parse(result)["interceptiondefense"].ToString()); // extract league rules from http response
		int fumbledefense = int.Parse(JObject.Parse(result)["fumbledefense"].ToString()); // extract league rules from http response
		int safetydefense = int.Parse(JObject.Parse(result)["safetydefense"].ToString()); // extract league rules from http response
		int inttd = int.Parse(JObject.Parse(result)["inttd"].ToString()); // extract league rules from http response
		int fumbletd = int.Parse(JObject.Parse(result)["fumbletd"].ToString()); // extract league rules from http response
		int returntd = int.Parse(JObject.Parse(result)["returntd"].ToString()); // extract league rules from http response
		int fgtentotwenty = int.Parse(JObject.Parse(result)["fgtentotwenty"].ToString()); // extract league rules from http response
		int fgmissedten = int.Parse(JObject.Parse(result)["fgmissedten"].ToString()); // extract league rules from http response
		int fgtwentytothirty = int.Parse(JObject.Parse(result)["fgtwentytothirty"].ToString()); // extract league rules from http response
		int fgmissedtwenty = int.Parse(JObject.Parse(result)["fgmissedtwenty"].ToString()); // extract league rules from http response
		int fgthirtytoforty = int.Parse(JObject.Parse(result)["fgthirtytoforty"].ToString()); // extract league rules from http response
		int fgmissedthirty = int.Parse(JObject.Parse(result)["fgmissedthirty"].ToString()); // extract league rules from http response
		int fgfortytofifty = int.Parse(JObject.Parse(result)["fgfortytofifty"].ToString()); // extract league rules from http response
		int fgmissedforty = int.Parse(JObject.Parse(result)["fgmissedforty"].ToString()); // extract league rules from http response
		int fgfiftytosixty = int.Parse(JObject.Parse(result)["fgfiftytosixty"].ToString()); // extract league rules from http response
		int fgmissedfifty = int.Parse(JObject.Parse(result)["fgmissedfifty"].ToString()); // extract league rules from http response
		int fgsixtyplus = int.Parse(JObject.Parse(result)["fgsixtyplus"].ToString()); // extract league rules from http response
		int fgmissedsixty = int.Parse(JObject.Parse(result)["fgmissedsixty"].ToString()); // extract league rules from http response
		int xpmade = int.Parse(JObject.Parse(result)["xpmade"].ToString()); // extract league rules from http response
		int xpmissed = int.Parse(JObject.Parse(result)["xpmissed"].ToString()); // extract league rules from http response
		LeagueRules leaguerules = new LeagueRules(leagueid);
		CurrentLeagueRules.Add(leaguerules);
	}

	private void OnEditBtnClicked(object sender, EventArgs e)
	{

	}



} // End class
