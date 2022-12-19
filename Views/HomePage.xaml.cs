
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
	ObservableCollection<Team> teamsBelongedTo;
	ObservableCollection<Team> teamsInLeague;
	ObservableCollection<Team> currentlySelectedTeam;
	ObservableCollection<Team> currentlySelectedTeamInLeague;
	ObservableCollection<League> globalLeagues;
	ObservableCollection<User> creators;
	ObservableCollection<User> teamcreators;
	ObservableCollection<User> globalleaguecreators;
	ObservableCollection<League> currentlySelected;
	ObservableCollection<Player> currentlySelectedPlayer;
	ObservableCollection<LeagueRules> leagueRules;
	ObservableCollection<Player> freeAgents;
	ObservableCollection<Player> teamPlayers;
	ObservableCollection<Player> playersOnCurrentTeam;
	private bool globalHasBeenSelected = false;
	private bool leaguesBelongedToSelected = false;
	private bool globalLeaguesSelected = false;
	private bool currentLeagueHasBeenSelected = false;

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

	public ObservableCollection<Player> FreeAgents
	{
		get => freeAgents;
		set
		{
			freeAgents = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Player> TeamPlayers
	{
		get => teamPlayers;
		set
		{
			teamPlayers = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Player> PlayersOnCurrentTeam
	{
		get => playersOnCurrentTeam;
		set
		{
			playersOnCurrentTeam = value;
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

	public ObservableCollection<Player> CurrentlySelectedPlayer
	{
		get => currentlySelectedPlayer;
		set
		{
			currentlySelectedPlayer = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Team> CurrentlySelectedTeam
	{
		get => currentlySelectedTeam;
		set
		{
			currentlySelectedTeam = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Team> CurrentlySelectedTeamInLeague
	{
		get => currentlySelectedTeamInLeague;
		set
		{
			currentlySelectedTeamInLeague = value;
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

	public ObservableCollection<User> TeamCreators
	{
		get => teamcreators;
		set
		{
			teamcreators = value;
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

	public ObservableCollection<Team> TeamsBelongedTo
	{
		get => teamsBelongedTo;
		set
		{
			teamsBelongedTo = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Team> TeamsInLeague
	{
		get => teamsInLeague;
		set
		{
			teamsInLeague = value;
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
		TeamsBelongedTo = new ObservableCollection<Team>();
		TeamCreators = new ObservableCollection<User>();
		TeamsInLeague = new ObservableCollection<Team>();
		FreeAgents = new ObservableCollection<Player>();
		TeamPlayers = new ObservableCollection<Player>();
		PlayersOnCurrentTeam= new ObservableCollection<Player>();

		LeaguesBelongedToCollectionView.ItemsSource = BelongedTo;
		TeamsBelongedToCollectionView.ItemsSource = TeamsBelongedTo;

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

		// Get leagues
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

				var Url10 = "http://localhost:8000/api/numberteams";
				client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{leagueId}"); // get current number of teams in league
				var response10 = await client.GetAsync(Url10);
				var result10 = await response10.Content.ReadAsStringAsync();

				client.DefaultRequestHeaders.Remove("LeagueIdHeader");
				league.CurrentTeams = int.Parse(JObject.Parse(result10)["numberteams"].ToString());
				league.CurrentTeamsStr = $"Current # Teams ({league.CurrentTeams})";
				if (!BelongedTo.Contains(league))
				{
					BelongedTo.Add(league);
				}
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
				league.CreatorUsernameStr = $"Creator Username ({league.CreatorUsername})";
				league.CreatorNameStr = $"Creator Name ({league.CreatorName})";
			}
		}

		// Get teams
		var Url5 = "http://localhost:8000/api/getteamsbelongedto";
		client.DefaultRequestHeaders.Clear();
		client.DefaultRequestHeaders.Add("TeamsBelongedToHeader", $"{userId}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
		var response5 = await client.GetAsync(Url5);
		var result5 = await response5.Content.ReadAsStringAsync();

		int[]? teamids = (int[])JObject.Parse(result5)["teamsbelongedto"].ToObject<int[]>();

		var Url6 = "http://localhost:8000/api/getteams";

		for (int i = 0; i < teamids.Count(); i++)
		{
			client.DefaultRequestHeaders.Add("TeamIdHeader", $"{teamids[i]}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
			var response6 = await client.GetAsync(Url6);
			var result6 = await response6.Content.ReadAsStringAsync();

			client.DefaultRequestHeaders.Remove("TeamIdHeader");
			int teamid = int.Parse(JObject.Parse(result6)["TeamId"].ToString());
			string teamname = JObject.Parse(result6)["teamname"].ToString();
			string createdondate = JObject.Parse(result6)["createdondate"].ToString();
			int creatorId = int.Parse(JObject.Parse(result6)["creatorid"].ToString());
			int leagueId = int.Parse(JObject.Parse(result6)["leagueid"].ToString());
			Team team = new Team(teamid, teamname, createdondate, creatorId, leagueId, creatorId == GetUserId.UserId);


			var Url7 = "http://localhost:8000/api/getuser"; // retrieve every user which created every league

			client.DefaultRequestHeaders.Add("UsernameEmail", $"Bearer {creatorId}");
			var response7 = await client.GetAsync(Url7);
			var result7 = await response7.Content.ReadAsStringAsync();

			client.DefaultRequestHeaders.Remove("UsernameEmail");

			int userID = int.Parse(JObject.Parse(result7)["UserId"].ToString());
			string username = JObject.Parse(result7)["user_name"].ToString();
			string name = JObject.Parse(result7)["name"].ToString();
			string email = JObject.Parse(result7)["email"].ToString();
			User user = new User(userID, username, name, email);
			TeamCreators.Add(user);


			team.CreatorName = user.Name; // Merge name and username from user into league for collection view binding purposes
			team.CreatorUsername = user.Username;

			var Url8 = "http://localhost:8000/api/getleagues"; // retrieve league info associated with team
			client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{team.League}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
			var response8 = await client.GetAsync(Url8);
			var result8 = await response8.Content.ReadAsStringAsync();

			client.DefaultRequestHeaders.Remove("LeagueIdHeader");

			string leaguename = JObject.Parse(result8)["leaguename"].ToString();

			team.LeagueName = leaguename;
			team.LeagueNameStr = $"League Name ({team.LeagueName})";

			team.TeamIdStr = $"Team ID ({team.TeamId})";
			team.TeamNameStr = $"Team Name ({team.TeamName})";
			team.LeagueNameStr = $"League Name ({team.LeagueName})";
			team.CreatedOnDateStr = $"Created On ({team.CreatedOnDate.ToString()})";
			team.CreatorUsernameStr = $"Creator Username ({team.CreatorUsername})";
			team.CreatorNameStr = $"Creator Name ({team.CreatorName})";

			TeamsBelongedTo.Add(team);
			TeamsBelongedToCollectionView.ItemsSource = TeamsBelongedTo;
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

	private void FreeAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ObservableCollection<Player> currentlySelectedPlayer = new ObservableCollection<Player>();
		currentlySelectedPlayer.Add(e.CurrentSelection.FirstOrDefault() as Player);

		CurrentlySelectedPlayer = currentlySelectedPlayer;

		
	}

	private void CurrentPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ObservableCollection<Player> currentlySelectedPlayer = new ObservableCollection<Player>();
		currentlySelectedPlayer.Add(e.CurrentSelection.FirstOrDefault() as Player);
		CurrentlySelectedPlayer = currentlySelectedPlayer;


		
	} // End method

	private void TeamsBelongedToCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ObservableCollection<Team> currentlySelectedTeam = new ObservableCollection<Team>();
		currentlySelectedTeam.Add(e.CurrentSelection.FirstOrDefault() as Team);
		CurrentlySelectedTeam = currentlySelectedTeam;

		CurrentTeamCollectionView.ItemsSource = CurrentlySelectedTeam;
	}

	private void TeamsInLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ObservableCollection<Team> currentlySelectedTeamInLeague = new ObservableCollection<Team>();
		currentlySelectedTeamInLeague.Add(e.CurrentSelection.FirstOrDefault() as Team);
		CurrentlySelectedTeamInLeague = currentlySelectedTeamInLeague;

		CurrentTeamCollectionView.ItemsSource = CurrentlySelectedTeamInLeague;
	}

		private void GlobalLeaguesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ObservableCollection<League> currentlySelected = new ObservableCollection<League>();
		currentlySelected.Add(e.CurrentSelection.FirstOrDefault() as League);
		CurrentlySelected = currentlySelected;
		CurrentLeagueCollectionView.ItemsSource = CurrentlySelected;

	} // End method


	public async Task GetPlayersInLeague(int leagueid)
	{
		var Url = "http://localhost:8000/api/getplayersinleague";
		using var client = new HttpClient();

		client.DefaultRequestHeaders.Add("PlayersInLeagueHeader", $"{leagueid}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
		var response = await client.GetAsync(Url);
		var result = await response.Content.ReadAsStringAsync();

		int[] playerids = (int[])JObject.Parse(result)["playeridinleague"].ToObject<int[]>();

		var Url2 = "http://localhost:8000/api/getplayers";

		foreach (int id in playerids)
		{
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Add("PlayerIdHeader", $"{id}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to

			var response2 = await client.GetAsync(Url2);
			var result2 = await response2.Content.ReadAsStringAsync();

			int playerid = int.Parse(JObject.Parse(result2)["PlayerId"].ToString());
			int teamid = int.Parse(JObject.Parse(result2)["teamid"].ToString());
			string position = JObject.Parse(result2)["position"].ToString();
			string playername = JObject.Parse(result2)["playername"].ToString();
			string team = JObject.Parse(result2)["team"].ToString();


			Player player = new Player(playerid, teamid, position, playername, team, teamid == 0);
			if (player.FreeAgent)
			{
				FreeAgents.Add(player);
			}
			else
			{
				TeamPlayers.Add(player);
			}
		}
	}
	public async Task GetTeamPlayers(int teamid)
	{
		var Url = "http://localhost:8000/api/getteamplayers";
		using var client = new HttpClient();

		client.DefaultRequestHeaders.Add("TeamPlayersHeader", $"{teamid}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
		var response = await client.GetAsync(Url);
		var result = await response.Content.ReadAsStringAsync();

		int[] playerids = (int[])JObject.Parse(result)["playeridinleague"].ToObject<int[]>();

		var Url2 = "http://localhost:8000/api/getplayers";

		foreach (int id in playerids)
		{
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Add("PlayerIdHeader", $"{id}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to

			var response2 = await client.GetAsync(Url2);
			var result2 = await response2.Content.ReadAsStringAsync();

			int playerid = int.Parse(JObject.Parse(result2)["PlayerId"].ToString());
			string position = JObject.Parse(result2)["position"].ToString();
			string playername = JObject.Parse(result2)["playername"].ToString();
			string team = JObject.Parse(result2)["team"].ToString();

			Player player = new Player(playerid, teamid, position, playername, team, teamid == 0);
			PlayersOnCurrentTeam.Add(player);

		}

	}

	// Get all teams associated with a league
	public async Task GetTeamsInLeague(int leagueid)
	{
		var Url = "http://localhost:8000/api/getteamsinleague";
		using var client = new HttpClient();

		client.DefaultRequestHeaders.Add("TeamsInLeagueHeader", $"{leagueid}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
		var response = await client.GetAsync(Url);
		var result = await response.Content.ReadAsStringAsync();

		int[]? teamids = (int[])JObject.Parse(result)["teamsbelongedto"].ToObject<int[]>();

		var Url2 = "http://localhost:8000/api/getteams";

		foreach (int id in teamids)
		{
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Add("TeamIdHeader", $"{id}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to

			var response2 = await client.GetAsync(Url2);
			var result2 = await response2.Content.ReadAsStringAsync();

			int teamid = int.Parse(JObject.Parse(result2)["TeamId"].ToString());
			string teamname = JObject.Parse(result2)["teamname"].ToString();
			string createdondate = JObject.Parse(result2)["createdondate"].ToString();
			int creatorId = int.Parse(JObject.Parse(result2)["creatorid"].ToString());
			int leagueId = int.Parse(JObject.Parse(result2)["leagueid"].ToString());
			Team team = new Team(teamid, teamname, createdondate, creatorId, leagueId, creatorId == GetUserId.UserId);


			var Url7 = "http://localhost:8000/api/getuser"; // retrieve every user which created every league

			client.DefaultRequestHeaders.Add("UsernameEmail", $"Bearer {creatorId}");
			var response7 = await client.GetAsync(Url7);
			var result7 = await response7.Content.ReadAsStringAsync();

			client.DefaultRequestHeaders.Remove("UsernameEmail");

			int userID = int.Parse(JObject.Parse(result7)["UserId"].ToString());
			string username = JObject.Parse(result7)["user_name"].ToString();
			string name = JObject.Parse(result7)["name"].ToString();
			string email = JObject.Parse(result7)["email"].ToString();
			User user = new User(userID, username, name, email);
			TeamCreators.Add(user);


			team.CreatorName = user.Name; // Merge name and username from user into league for collection view binding purposes
			team.CreatorUsername = user.Username;

			var Url8 = "http://localhost:8000/api/getleagues"; // retrieve league info associated with team
			client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{team.League}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
			var response8 = await client.GetAsync(Url8);
			var result8 = await response8.Content.ReadAsStringAsync();

			client.DefaultRequestHeaders.Remove("LeagueIdHeader");

			string leaguename = JObject.Parse(result8)["leaguename"].ToString();

			team.LeagueName = leaguename;
			team.LeagueNameStr = $"League Name ({team.LeagueName})";

			team.TeamIdStr = $"Team ID ({team.TeamId})";
			team.TeamNameStr = $"Team Name ({team.TeamName})";
			team.LeagueNameStr = $"League Name ({team.LeagueName})";
			team.CreatedOnDateStr = $"Created On ({team.CreatedOnDate.ToString()})";
			team.CreatorUsernameStr = $"Creator Username ({team.CreatorUsername})";
			team.CreatorNameStr = $"Creator Name ({team.CreatorName})";

			TeamsInLeague.Add(team);
		}
	}

	private async void OnViewFreeAgentsClicked(object sender, EventArgs e)
	{
		GoBackFABtn.IsVisible = true;
		ViewFreeAgentsBtn.IsVisible = false;

		FreeAgentsCollectionView.ItemsSource = FreeAgents;

		FreeAgentsCollectionViewGrid.IsVisible= true;
		FreeAgentsCollectionView.IsEnabled = true;

		CurrentPlayersCollectionViewGrid.IsVisible = false;
		CurrentPlayersCollectionView.IsEnabled = false;

		GoBackTeamBtn.IsVisible = false;
		DeleteTeamBtn.IsVisible = false;

		DropBtn.IsVisible = false;
		AddBtn.IsVisible = true;

		TitleLabel1.Text = "FREE AGENTS";
	}

	private async void OnGoBackFAClicked(object sender, EventArgs e)
	{
		GoBackFABtn.IsVisible = false;
		ViewFreeAgentsBtn.IsVisible = true;

		AddBtn.IsVisible = false;
		DropBtn.IsVisible = true;

		GoBackTeamBtn.IsVisible = true;
		DeleteTeamBtn.IsVisible = true;

		if (CurrentlySelectedPlayer is not null)
		{
			CurrentlySelectedPlayer.Clear();
		}
		FreeAgentsCollectionView.SelectedItem = null;
		CurrentPlayersCollectionView.SelectedItem = null;

		FreeAgentsCollectionViewGrid.IsVisible = false;
		FreeAgentsCollectionView.IsEnabled = false;

		CurrentPlayersCollectionViewGrid.IsVisible = true;
		CurrentPlayersCollectionView.IsEnabled = true;

		Team team = CurrentTeamCollectionView.SelectedItem as Team;

		TitleLabel1.Text = $"TEAM \"{team.TeamName.ToUpper()}\" ROSTER";

		await GetNumberTeams();
	}

	private async void OnDropPlayerClicked(object sender, EventArgs e)
	{
		if (CurrentPlayersCollectionView.SelectedItem == null)
		{
			await DisplayAlert("No player selected", "Select a player to drop now", "Ok");
			return;
		}


		Player? player = CurrentPlayersCollectionView.SelectedItem as Player;

		DropPlayerDTO dto = new DropPlayerDTO(player.PlayerId, player.PlayerName);

		await DropPlayer(dto);

		player.TeamId = 0;
		PlayersOnCurrentTeam.Remove(player);
		TeamPlayers.Remove(player);
		FreeAgents.Add(player);

		CurrentPlayersCollectionView.ItemsSource = PlayersOnCurrentTeam;

	}


	public async Task GetNumberTeams()
	{
		var Url = "http://localhost:8000/api/numberteams";
		using var client = new HttpClient();

		foreach (League league in GlobalLeagues)
		{
			client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{league.LeagueId}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to

			var response = await client.GetAsync(Url);
			var result = await response.Content.ReadAsStringAsync();
			client.DefaultRequestHeaders.Remove("LeagueIdHeader"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
			int? numberteams = int.Parse(JObject.Parse(result)["numberteams"].ToString());
			if (numberteams != null)
			{
				league.CurrentTeams = (int)numberteams;
				league.CurrentTeamsStr = $"Current # Teams ({league.CurrentTeams})";
			}
		}
		foreach (League league in BelongedTo)
		{
			client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{league.LeagueId}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to

			var response = await client.GetAsync(Url);
			var result = await response.Content.ReadAsStringAsync();
			client.DefaultRequestHeaders.Remove("LeagueIdHeader"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
			int? numberteams = int.Parse(JObject.Parse(result)["numberteams"].ToString());
			if (numberteams != null)
			{
				league.CurrentTeams = (int)numberteams;
				league.CurrentTeamsStr = $"Current # Teams ({league.CurrentTeams})";
			}
		}
		foreach (League league in CurrentlySelected)
		{
			client.DefaultRequestHeaders.Add("LeagueIdHeader", $"{league.LeagueId}"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to

			var response = await client.GetAsync(Url);
			var result = await response.Content.ReadAsStringAsync();
			client.DefaultRequestHeaders.Remove("LeagueIdHeader"); // add user id to LeaguesBelongedTo header to receive back leagues user belongs to
			int? numberteams = int.Parse(JObject.Parse(result)["numberteams"].ToString());
			if (numberteams != null)
			{
				league.CurrentTeams = (int)numberteams;
				league.CurrentTeamsStr = $"Current # Teams ({league.CurrentTeams})";
			}
		}

	}
	public async Task DropPlayer(DropPlayerDTO dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var url = "http://localhost:8000/api/dropplayer"; // access the register endpoint to register new user
		using var client = new HttpClient();

		var response = await client.PostAsync(url, data);

		var result = await response.Content.ReadAsStringAsync();

		Team? team = CurrentTeamCollectionView.SelectedItem as Team;
		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", $"{dto.PlayerName} has been dropped from \"{team.TeamName}\"", "Ok");
		}
		else
		{
			await DisplayAlert("Not successful", "Please try again", "Ok");
		}
	}

		private async void OnAddPlayerClicked(object sender, EventArgs e)
	{
		if (FreeAgentsCollectionView.SelectedItem == null)
		{
			await DisplayAlert("No free agent selected", "Select a player to add now", "Ok");
			return;
		}
		Team? team = CurrentTeamCollectionView.SelectedItem as Team;
		Player? player = FreeAgentsCollectionView.SelectedItem as Player;

		AddPlayerDTO dto = new AddPlayerDTO(player.PlayerId, player.PlayerName, team.TeamName, team.TeamId);

		await AddPlayer(dto);
		FreeAgents.Where(p => p.PlayerId == dto.PlayerId).FirstOrDefault().TeamId = dto.TeamId;
		TeamPlayers.Add(FreeAgents.Where(p => p.PlayerId == dto.PlayerId).FirstOrDefault());
		FreeAgents.Remove(FreeAgents.Where(p => p.PlayerId == dto.PlayerId).FirstOrDefault());

		if (PlayersOnCurrentTeam != null)
		{
			PlayersOnCurrentTeam.Clear();
		}
		foreach (Player playerp in TeamPlayers.Where(p => p.TeamId == dto.TeamId)) 
		{
			PlayersOnCurrentTeam.Add(playerp);
		}

		CurrentPlayersCollectionView.ItemsSource = PlayersOnCurrentTeam;

		FreeAgentsCollectionViewGrid.IsVisible = false;
		FreeAgentsCollectionView.IsEnabled = false;

		CurrentPlayersCollectionViewGrid.IsVisible = true;
		CurrentPlayersCollectionView.IsEnabled = true;

		AddBtn.IsVisible = false;
		DropBtn.IsVisible = true;

		GoBackFABtn.IsVisible = false;
		ViewFreeAgentsBtn.IsVisible = true;
		GoBackTeamBtn.IsVisible = true;
		DeleteTeamBtn.IsVisible = true;
		TitleLabel1.Text = $"TEAM \"{team.TeamName.ToUpper()}\" ROSTER";
	}

	public async Task AddPlayer(AddPlayerDTO dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var url = "http://localhost:8000/api/addplayer"; // access the register endpoint to register new user
		using var client = new HttpClient();

		var response = await client.PostAsync(url, data);

		var result = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", $"{dto.PlayerName} has been added to \"{dto.TeamName}\"", "Ok");
		}
		else
		{
			await DisplayAlert("Not successful", "Please try again", "Ok");
		}

	} // End method


	private async void OnViewTeamBtnClicked(object sender, EventArgs e)
	{
		if (LeaguesBelongedToGrid.IsVisible)
		{
			if (TeamsBelongedToCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No team selected", "Please select a team to view", "Ok");
				return;
			}
			Team? getPlayers = TeamsBelongedToCollectionView.SelectedItem as Team;
			await GetPlayersInLeague(getPlayers.League);

			leaguesBelongedToSelected = true;

			CurrentTeamCollectionViewGrid.IsVisible = true;
			CurrentTeamCollectionView.IsEnabled = true;
			CurrentTeamCollectionView.SelectedItem = CurrentlySelectedTeam.FirstOrDefault();
			Team? team = CurrentTeamCollectionView.SelectedItem as Team;

			await GetTeamPlayers(team.TeamId);
			TitleLabel1.Text = $"TEAM \"{team.TeamName.ToUpper()}\" ROSTER";

			TeamsBelongedToGrid.IsVisible = false;
			TeamsBelongedToCollectionView.IsEnabled = false;

			LeaguesBelongedToGrid.IsVisible = false;
			LeaguesBelongedToCollectionView.IsEnabled = false;

			CurrentPlayersCollectionView.ItemsSource = PlayersOnCurrentTeam;
			CurrentPlayersCollectionViewGrid.IsVisible = true;
			CurrentPlayersCollectionView.IsEnabled = true;

			ViewLeagueBtn.IsVisible = false;
			DeleteBtn.IsVisible = false;
			OrLabel.IsVisible = false;
			JoinCreateBtn.IsVisible = false;
			ViewFreeAgentsBtn.IsVisible = true;
			DropBtn.IsVisible = true;
		}

		if (GlobalLeaguesGrid.IsVisible)
		{
			if (TeamsBelongedToCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No team selected", "Please select a team to view", "Ok");
				return;
			}

			Team? getPlayers = TeamsBelongedToCollectionView.SelectedItem as Team;
			await GetPlayersInLeague(getPlayers.League);



			globalLeaguesSelected = true;
			CurrentTeamCollectionViewGrid.IsVisible = true;
			CurrentTeamCollectionView.IsEnabled = true;
			CurrentTeamCollectionView.SelectedItem = CurrentlySelectedTeam.FirstOrDefault();
			Team? team = CurrentTeamCollectionView.SelectedItem as Team;
			await GetTeamPlayers(team.TeamId);
			GlobalLeaguesGrid.IsVisible = false;
			GlobalLeaguesCollectionView.IsEnabled = false;

			TeamsBelongedToGrid.IsVisible = false;
			TeamsBelongedToCollectionView.IsEnabled = false;

			TitleLabel1.Text = $"TEAM \"{team.TeamName.ToUpper()}\" ROSTER";

			TeamsInLeagueGrid.IsVisible = false;
			TeamsInLeagueCollectionView.IsEnabled = false;

			CurrentPlayersCollectionView.ItemsSource = PlayersOnCurrentTeam;
			CurrentPlayersCollectionViewGrid.IsVisible = true;
			CurrentPlayersCollectionView.IsEnabled = true;

			ViewLeagueBtn.IsVisible = false;
			DeleteBtn.IsVisible = false;
			CreateLeagueBtn.IsVisible = false;
			GoBackBtn2.IsVisible = false;
			ViewFreeAgentsBtn.IsVisible = true;
			DropBtn.IsVisible = true;

		}



		if (CurrentLeagueCollectionViewGrid.IsVisible)
		{
			if (TeamsInLeagueCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No team selected", "Please select a team to view", "Ok");
				return;
			}

			Team? getPlayers = TeamsInLeagueCollectionView.SelectedItem as Team;
			await GetPlayersInLeague(getPlayers.League);

			currentLeagueHasBeenSelected = true;
			CurrentTeamCollectionViewGrid.IsVisible = true;
			CurrentTeamCollectionView.IsEnabled = true;

			CurrentTeamCollectionView.SelectedItem = TeamsInLeagueCollectionView.SelectedItem;
			Team? team = CurrentTeamCollectionView.SelectedItem as Team;
			await GetTeamPlayers(team.TeamId);

			TitleLabel1.Text = $"TEAM \"{team.TeamName.ToUpper()}\" ROSTER";

			TeamsInLeagueGrid.IsVisible = false;
			TeamsInLeagueCollectionView.IsEnabled = false;

			CurrentLeagueCollectionViewGrid.IsVisible = false;
			CurrentLeagueCollectionView.IsEnabled = false;


			CurrentPlayersCollectionView.ItemsSource = PlayersOnCurrentTeam;
			CurrentPlayersCollectionViewGrid.IsVisible = true;
			CurrentPlayersCollectionView.IsEnabled = true;

			GoBackBtn.IsVisible = false;
			DeleteBtn.IsVisible = false;
			JoinLeagueBtn.IsVisible = false;
			RulesBtn.IsVisible = false;
			ViewFreeAgentsBtn.IsVisible = true;
			DropBtn.IsVisible = true;
		}


		TitleLabel2.Text = $"TEAM INFO";
		ViewTeamBtn.IsVisible = false;
		GoBackTeamBtn.IsVisible = true;
	}

	private async void OnViewLeagueClicked(object sender, EventArgs e)
	{
		if (LeaguesBelongedToGrid.IsVisible)
		{
			if (LeaguesBelongedToCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No league selected", "Please select a league to view", "Ok");
				return;
			}
			if (CurrentTeamCollectionViewGrid.IsVisible)
			{
				CurrentTeamCollectionViewGrid.IsVisible = false;
				CurrentTeamCollectionView.IsEnabled = false;
				GoBackTeamBtn.IsVisible = false;
				ViewTeamBtn.IsVisible = true;
			}
			CurrentLeagueCollectionViewGrid.IsVisible = true;
			CurrentLeagueCollectionView.IsEnabled = true;
			CurrentLeagueCollectionView.SelectedItem = CurrentlySelected.FirstOrDefault();


			League league = CurrentLeagueCollectionView.SelectedItem as League;
			await GetTeamsInLeague(league.LeagueId);
			TeamsBelongedToGrid.IsVisible = false;
			TeamsBelongedToCollectionView.IsEnabled = false;

			TeamsInLeagueCollectionView.ItemsSource = TeamsInLeague;
			TeamsInLeagueGrid.IsVisible = true;
			TeamsInLeagueCollectionView.IsEnabled = true;

			TitleLabel2.Text = "LEAGUE TEAMS";
			TitleLabel1.Text = "LEAGUE INFO";

			LeaguesBelongedToGrid.IsVisible = false;
			LeaguesBelongedToCollectionView.IsEnabled = false;
			globalHasBeenSelected = false;

			OrLabel.IsVisible= false;
			JoinCreateBtn.IsVisible= false;

			await GetNumberTeams();
		}
		else if (GlobalLeaguesGrid.IsVisible)
		{
			if (GlobalLeaguesCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No league selected", "Please select a league to view", "Ok");
				return;
			}
			if (CurrentTeamCollectionViewGrid.IsVisible)
			{
				CurrentTeamCollectionViewGrid.IsVisible = false;
				CurrentTeamCollectionView.IsEnabled = false;
				GoBackTeamBtn.IsVisible = false;
				ViewTeamBtn.IsVisible = true;
			}
			CurrentLeagueCollectionViewGrid.IsVisible = true;
			CurrentLeagueCollectionView.IsEnabled = true;
			CurrentLeagueCollectionView.SelectedItem = CurrentlySelected.FirstOrDefault();

			League league = CurrentLeagueCollectionView.SelectedItem as League;
			await GetTeamsInLeague(league.LeagueId);
			TeamsBelongedToGrid.IsVisible = false;
			TeamsBelongedToCollectionView.IsEnabled = false;

			TeamsInLeagueCollectionView.ItemsSource = TeamsInLeague;
			TeamsInLeagueGrid.IsVisible = true;
			TeamsInLeagueCollectionView.IsEnabled = true;

			TitleLabel2.Text = "LEAGUE TEAMS";
			TitleLabel1.Text = "LEAGUE INFO";
			GlobalLeaguesGrid.IsVisible = false;
			GlobalLeaguesCollectionView.IsEnabled = false;
			GoBackBtn2.IsVisible = false;
			JoinLeagueBtn.IsVisible = false;
			CreateLeagueBtn.IsVisible = false;
			globalHasBeenSelected = true;


			await GetNumberTeams();
		}

		JoinLeagueBtn.IsVisible = true;
		RulesBtn.IsVisible = true;
		ViewLeagueBtn.IsVisible= false;
		GoBackBtn.IsVisible = true;


	}


	private async void OnDeleteTeamClicked(object sender, EventArgs e)
	{
		DeleteTeamDTO dto;
		if (TeamsInLeagueGrid.IsVisible)
		{
			Team team = TeamsInLeagueCollectionView.SelectedItem as Team;
			if (!team.CreatedByCurrentUser && team is not null)
			{
				await DisplayAlert("Not team owner", "You may only delete teams that you created", "Ok");
				return;
			}
			dto = new DeleteTeamDTO(team.TeamId, team.TeamName);
			await DeleteTeam(dto);

			FreeAgents.Clear();
			TeamPlayers.Clear();
			await GetPlayersInLeague(team.League);
			foreach (Team item in TeamsInLeague)
			{
				if (item.TeamId == dto.teamid)
				{
					TeamsInLeague.Remove(item);
					break;
				}
			}
			foreach (Team item in TeamsBelongedTo)
			{
				if (item.TeamId == dto.teamid)
				{
					TeamsBelongedTo.Remove(item);
					break;
				}
			}
		}
		else if (TeamsBelongedToGrid.IsVisible)
		{
			if (TeamsBelongedToCollectionView.SelectedItem == null)
			{
				await DisplayAlert("No team selected", "Please select a team to delete", "Ok");
				return;
			}
			if (!CurrentlySelectedTeam[0].CreatedByCurrentUser)
			{
				await DisplayAlert("Not team owner", "You may only delete teams that you created", "Ok");
				return;
			}
			else
			{
				Team team = TeamsBelongedToCollectionView.SelectedItem as Team;
				dto = new DeleteTeamDTO(team.TeamId, team.TeamName);
				await DeleteTeam(dto);

				FreeAgents.Clear();
				TeamPlayers.Clear();
				await GetPlayersInLeague(team.League);
				foreach (Team item in TeamsInLeague)
				{
					if (item.TeamId == dto.teamid)
					{
						TeamsInLeague.Remove(item);
						break;
					}
				}
				foreach (Team item in TeamsBelongedTo)
				{
					if (item.TeamId == dto.teamid)
					{
						TeamsBelongedTo.Remove(item);
						break;
					}
				}
			}
		}
		else if (CurrentTeamCollectionViewGrid.IsVisible)
		{
			Team team = CurrentTeamCollectionView.SelectedItem as Team;
			if (!team.CreatedByCurrentUser && team is not null)
			{
				await DisplayAlert("Not team owner", "You may only delete teams that you created", "Ok");
				return;
			}
			dto = new DeleteTeamDTO(team.TeamId, team.TeamName);
			await DeleteTeam(dto);

			FreeAgents.Clear();
			TeamPlayers.Clear();
			PlayersOnCurrentTeam.Clear();
			await GetPlayersInLeague(team.League);
			foreach (Team item in TeamsInLeague)
			{
				if (item.TeamId == dto.teamid)
				{
					TeamsInLeague.Remove(item);
					break;
				}
			}
			foreach (Team item in TeamsBelongedTo)
			{
				if (item.TeamId == dto.teamid)
				{
					TeamsBelongedTo.Remove(item);
					break;
				}
			}

			CurrentTeamCollectionViewGrid.IsVisible = false;
			CurrentTeamCollectionView.IsEnabled = false;

			CurrentPlayersCollectionViewGrid.IsVisible = false;
			CurrentPlayersCollectionView.IsEnabled = false;

			if (globalHasBeenSelected)
			{
				GlobalLeaguesGrid.IsVisible = true;
				GlobalLeaguesCollectionView.IsEnabled = true;

				TeamsBelongedToGrid.IsVisible = true;
				TeamsBelongedToCollectionView.IsEnabled = true;

				TitleLabel2.Text = "LEAGUE TEAMS";
				TitleLabel1.Text = "GLOBAL LEAGUES";
				globalHasBeenSelected = false;
				ViewTeamBtn.IsVisible = true;
				DeleteTeamBtn.IsVisible = true;
				GoBackTeamBtn.IsVisible = false;

				ViewLeagueBtn.IsVisible = true;
				DeleteBtn.IsVisible = true;
				CreateLeagueBtn.IsVisible = true;
				GoBackBtn2.IsVisible = true;

				ViewFreeAgentsBtn.IsVisible = false;
				DropBtn.IsVisible = false;
			}

			if (leaguesBelongedToSelected)
			{
				LeaguesBelongedToGrid.IsVisible = true;
				LeaguesBelongedToCollectionView.IsEnabled = true;

				TeamsBelongedToGrid.IsVisible = true;
				TeamsBelongedToCollectionView.IsEnabled = true;

				TitleLabel2.Text = "LEAGUE TEAMS";
				TitleLabel1.Text = "LEAGUES BELONGED TO";
				leaguesBelongedToSelected = false;
				ViewTeamBtn.IsVisible = true;
				DeleteTeamBtn.IsVisible = true;
				GoBackTeamBtn.IsVisible = false;

				ViewLeagueBtn.IsVisible = true;
				DeleteBtn.IsVisible = true;
				OrLabel.IsVisible = true;
				JoinCreateBtn.IsVisible = true;

				ViewFreeAgentsBtn.IsVisible = false;
				DropBtn.IsVisible = false;
			}
		}
		if (CurrentlySelectedTeam is not null)
		{
			CurrentlySelectedTeam.Clear();
		}
		if (CurrentlySelectedTeamInLeague is not null)
		{
			CurrentlySelectedTeamInLeague.Clear();
		}
		CurrentTeamCollectionView.SelectedItem = null;
		TeamsInLeagueCollectionView.SelectedItem = null;
		TeamsBelongedToCollectionView.SelectedItem = null;
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
		for (int i = 0; i < TeamsBelongedTo.Count; i++)
		{
			if (TeamsBelongedTo[i].League == dto.leagueid)
			{
				TeamsBelongedTo.Remove(TeamsBelongedTo[i]);
			}
		}
		for (int i = 0; i < TeamsInLeague.Count; i++)
		{
			if (TeamsInLeague[i].League == dto.leagueid)
			{
				TeamsInLeague.Remove(TeamsInLeague[i]);
			}
		}
		for (int i = 0; i < CurrentLeagueRules.Count; i++)
		{
			if (CurrentLeagueRules[i].LeagueId == dto.leagueid)
			{
				CurrentLeagueRules.Remove(CurrentLeagueRules[i]);
			}
		}


	} // End method

	public async Task DeleteTeam(DeleteTeamDTO dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var url = "http://localhost:8000/api/deleteteam"; // access the register endpoint to register new user
		using var client = new HttpClient();

		var response = await client.PostAsync(url, data);

		var result = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", $"{dto.teamname} has been deleted", "Ok");
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
				league.CreatorUsernameStr = $"Creator Username ({league.CreatorUsername})";
				league.CreatorNameStr = $"Creator Name ({league.CreatorName})";

				GlobalLeagues.Add(league);
			}
		}
	}

	private void OnJoinLeagueClicked(object sender, EventArgs e)
	{
		CreateTeamGridOuter.IsVisible = true;
		CreateTeamGrid.IsVisible = true;

		if (TeamsInLeagueGrid.IsVisible)
		{
			TeamsInLeagueGrid.IsVisible = false;
			TeamsInLeagueCollectionView.IsEnabled = false;
		}
		if (CurrentTeamCollectionViewGrid.IsVisible)
		{
			CurrentTeamCollectionViewGrid.IsVisible = false;
			CurrentTeamCollectionView.IsEnabled = false;
		}
		TitleLabel2.Text = "CREATE TEAM";

		GoBackBtn.IsVisible = false;
		DeleteBtn.IsVisible = false;
		JoinLeagueBtn.IsVisible = false;
		RulesBtn.IsVisible = false;

		ViewTeamBtn.IsVisible = false;
		GoBackTeamBtn.IsVisible = true;
		DeleteTeamBtn.IsVisible = false;
		EnterTeamBtn.IsVisible = true;
	}

	private async void OnGoBackTeamClicked(object sender, EventArgs e)
	{
		if (CurrentPlayersCollectionViewGrid.IsVisible)
		{
			CurrentPlayersCollectionViewGrid.IsVisible = false;
			CurrentPlayersCollectionView.IsEnabled = false;


		}
		if (CreateTeamGridOuter.IsVisible)
		{
			CreateTeamGridOuter.IsVisible = false;
			CreateTeamGrid.IsVisible = false;

			TeamNameEntry.Text = null;

			GoBackTeamBtn.IsVisible = false;
			ViewTeamBtn.IsVisible = true;
			EnterTeamBtn.IsVisible = false;
			DeleteTeamBtn.IsVisible = true;

			TeamsInLeagueGrid.IsVisible = true;
			TeamsInLeagueCollectionView.IsEnabled = true;
			TitleLabel2.Text = "LEAGUE TEAMS";

			GoBackBtn.IsVisible = true;
			DeleteBtn.IsVisible = true;
			JoinLeagueBtn.IsVisible = true;
			RulesBtn.IsVisible = true;

			return;
		}
		if (CurrentTeamCollectionViewGrid.IsVisible)
		{
			if (leaguesBelongedToSelected)
			{

				LeaguesBelongedToGrid.IsVisible = true;
				LeaguesBelongedToCollectionView.IsEnabled = true;

				PlayersOnCurrentTeam.Clear();
				CurrentTeamCollectionView.SelectedItem = null;

				CurrentTeamCollectionViewGrid.IsVisible = false;
				CurrentTeamCollectionView.IsEnabled = false;

				CurrentPlayersCollectionViewGrid.IsVisible = false;
				CurrentPlayersCollectionView.IsEnabled = false;
				CurrentPlayersCollectionView.SelectedItem = null;


				ViewLeagueBtn.IsVisible = true;
				DeleteBtn.IsVisible = true;
				OrLabel.IsVisible = true;
				JoinCreateBtn.IsVisible = true;
				ViewFreeAgentsBtn.IsVisible = false;
				DropBtn.IsVisible = false;
				leaguesBelongedToSelected = false;

				TitleLabel1.Text = "LEAGUES BELONGED TO";

			}
			if (globalHasBeenSelected)
			{

				CurrentLeagueCollectionViewGrid.IsVisible = true;
				CurrentLeagueCollectionView.IsEnabled = true;

				PlayersOnCurrentTeam.Clear();
				CurrentTeamCollectionView.SelectedItem = null;

				CurrentTeamCollectionViewGrid.IsVisible = false;
				CurrentTeamCollectionView.IsEnabled = false;

				CurrentPlayersCollectionViewGrid.IsVisible = false;
				CurrentPlayersCollectionView.IsEnabled = false;
				CurrentPlayersCollectionView.SelectedItem = null;


				GoBackBtn.IsVisible = true;
				DeleteBtn.IsVisible = true;
				JoinLeagueBtn.IsVisible = true;
				RulesBtn.IsVisible = true;
				ViewTeamBtn.IsVisible = true;
				
				ViewFreeAgentsBtn.IsVisible = false;
				DropBtn.IsVisible = false;
				globalLeaguesSelected = false;

				TitleLabel1.Text = "GLOBAL LEAGUES";

			}
			if (currentLeagueHasBeenSelected)
			{
				CurrentLeagueCollectionViewGrid.IsVisible = true;
				CurrentLeagueCollectionView.IsEnabled = true;


				PlayersOnCurrentTeam.Clear();
				CurrentTeamCollectionView.SelectedItem = null;

				CurrentTeamCollectionViewGrid.IsVisible = false;
				CurrentTeamCollectionView.IsEnabled = false;

				CurrentPlayersCollectionViewGrid.IsVisible = false;
				CurrentPlayersCollectionView.IsEnabled = false;
				CurrentPlayersCollectionView.SelectedItem = null;

				GoBackBtn.IsVisible = true;
				DeleteBtn.IsVisible = true;
				JoinLeagueBtn.IsVisible = true;
				RulesBtn.IsVisible = true;
				ViewFreeAgentsBtn.IsVisible = false;
				DropBtn.IsVisible = false;
				
				TitleLabel1.Text = "LEAGUE INFO";
			}
			if (PlayersOnCurrentTeam != null)
			{
				PlayersOnCurrentTeam.Clear();
			}
			if (CurrentlySelectedPlayer != null)
			{
				CurrentlySelectedPlayer.Clear();
			}
		}
		if (CurrentLeagueCollectionViewGrid.IsVisible)
		{
			TeamsInLeagueGrid.IsVisible = true;
			TeamsInLeagueCollectionView.IsEnabled = true;
			TitleLabel2.Text = "LEAGUE TEAMS";

		}
		else
		{
			CurrentTeamCollectionViewGrid.IsVisible = false;
			CurrentTeamCollectionView.IsEnabled = false;

			TeamsBelongedToGrid.IsVisible = true;
			TeamsBelongedToCollectionView.IsEnabled = true;
			TitleLabel2.Text = "MY TEAMS";
		}

		if (CurrentlySelectedTeam is not null)
		{
			CurrentlySelectedTeam.Clear();
		}
		if (CurrentlySelectedTeamInLeague is not null)
		{
			CurrentlySelectedTeamInLeague.Clear();
		}
		TeamsInLeagueCollectionView.SelectedItem = null;
		TeamsBelongedToCollectionView.SelectedItem = null;

		GoBackTeamBtn.IsVisible = false;
		ViewTeamBtn.IsVisible = true;
	}


	private void OnCreateLeagueClicked(object sender, EventArgs e)
	{
		CreateLeagueGridOuter.IsVisible = true;
		CreateLeagueGrid.IsVisible = true;

		GlobalLeaguesGrid.IsVisible = false;
		GlobalLeaguesCollectionView.IsEnabled = false;

		DeleteBtn.IsVisible = false;
		CreateLeagueBtn.IsVisible = false;
		GoBackBtn.IsVisible = false;
		EnterLeagueBtn.IsVisible = true;
		ViewLeagueBtn.IsVisible = false;
		GoBackBtn.IsVisible = true;
		GoBackBtn2.IsVisible = false;
		TitleLabel1.Text = "CREATE LEAGUE";
	}

	private async void OnEnterTeamClicked(object sender, EventArgs e)
	{
		if (TeamNameEntry.Text is null)
		{
			await DisplayAlert("Must enter a valid team name", "Please enter a team name now", "Ok");
			return;
		}

		League league = CurrentLeagueCollectionView.SelectedItem as League;

		CreateTeamDTO dto = new CreateTeamDTO(TeamNameEntry.Text, GetUserId.UserId, league.LeagueId);

		await CreateTeam(dto);

		ViewTeamBtn.IsVisible = true;
		DeleteTeamBtn.IsVisible = true;

		GoBackTeamBtn.IsVisible = false;
		EnterTeamBtn.IsVisible = false;

		CreateTeamGrid.IsVisible = false;
		CreateTeamGridOuter.IsVisible = false;

		TeamsInLeagueGrid.IsVisible = true;
		TeamsInLeagueCollectionView.IsEnabled = true;

		TitleLabel2.Text = "LEAGUE TEAMS";


		TeamsInLeague.Clear();
	    await getUserEmailAndUserName(GetUserId.UserId);
		await GetTeamsInLeague(league.LeagueId);
		TeamsInLeagueCollectionView.ItemsSource = TeamsInLeague;

		GoBackBtn.IsVisible = true;
		DeleteBtn.IsVisible = true;
		JoinLeagueBtn.IsVisible = true;
		RulesBtn.IsVisible = true;
	}

	private async Task CreateTeam(CreateTeamDTO dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var Url = "http://localhost:8000/api/createteam";
		using var client = new HttpClient();

		var response = await client.PostAsync(Url, data);
		var result = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", $"Created team {dto.team_name}", "Ok");
		}
		else
		{
			await DisplayAlert("Not successful", "Please try again", "Ok");
		}
	}

	private async void OnEnterLeagueBtnClicked(object sender, EventArgs e)
	{
		string maxteams = MaxTeamsCreateLeagueEntry.Text;
		foreach (char c in maxteams)
		{
			if (Char.IsLetter(c))
			{
				await DisplayAlert("Invalid max team entry", "Please enter a valid number of max teams now", "Ok");
				return;
			}
		}
		if (int.Parse(MaxTeamsCreateLeagueEntry.Text) > 16)
		{
			await DisplayAlert("League cannot contain more than 16 teams", "Please enter a number less than or equal to 16", "Ok");
			return;
		}
		CreateLeagueDTO league = new CreateLeagueDTO(LeagueNameEntry.Text, int.Parse(MaxTeamsCreateLeagueEntry.Text), GetUserId.UserId);

		await PostLeague(league);

		CreateLeagueGridOuter.IsVisible = false;
		CreateLeagueGrid.IsVisible = false;

		GlobalLeaguesGrid.IsVisible = true;
		GlobalLeaguesCollectionView.IsEnabled = true;

		TitleLabel1.Text = "GLOBAL LEAGUES";

		GlobalLeagues.Clear();
		BelongedTo.Clear();

		int.TryParse(GetUserId.UserId.ToString(), out int UserId);
		await getUserEmailAndUserName(UserId);
		await GetGlobalLeagues();


		LeaguesBelongedToCollectionView.ItemsSource = BelongedTo;
		GlobalLeaguesCollectionView.ItemsSource = GlobalLeagues;

		GlobalLeaguesCollectionView.SelectedItem = GlobalLeagues.Where(l => l.LeagueName == league.LeagueName).FirstOrDefault();

		GoBackBtn.IsVisible = false;
		EnterLeagueBtn.IsVisible = false;

		ViewLeagueBtn.IsVisible = true;
		DeleteBtn.IsVisible = true;
		CreateLeagueBtn.IsVisible = true;
		GoBackBtn2.IsVisible = true;
	}

	public async Task PostLeague(CreateLeagueDTO dto)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var Url = "http://localhost:8000/api/createleague";
		using var client = new HttpClient();

		var response = await client.PostAsync(Url, data);
		var result = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", $"Created league {dto.LeagueName}", "Ok");
		}
		else
		{
			await DisplayAlert("Not successful", "Please try again", "Ok");
		}
	}

	private async void OnGoBackClicked(object sender, EventArgs e)
	{
		if (LeagueRulesGrid.IsVisible)
		{
			LeagueRulesGrid.IsVisible = false;
			LeagueRulesCollectionView.IsEnabled = false;

			CurrentLeagueCollectionViewGrid.IsVisible = true;
			CurrentLeagueCollectionView.IsEnabled = true;

			if (CurrentLeagueRules is not null)
			{
				CurrentLeagueRules.Clear();
			}
			if (CurrentTeamCollectionViewGrid.IsVisible)
			{
				CurrentTeamCollectionViewGrid.IsVisible = false;
				CurrentTeamCollectionView.IsEnabled = false;
				GoBackTeamBtn.IsVisible = false;
				ViewTeamBtn.IsVisible = true;

				TeamsInLeagueGrid.IsVisible = true;
				TeamsInLeagueCollectionView.IsEnabled = true;

				TitleLabel2.Text = "LEAGUE TEAMS";

			}

			ViewTeamBtn.IsVisible = true;
			DeleteTeamBtn.IsVisible = true;
			DeleteBtn.IsVisible = true;
			JoinLeagueBtn.IsVisible = true;
			RulesBtn.IsVisible = true;
			EditBtn.IsVisible = false;
			TitleLabel1.Text = "LEAGUE INFO";

			return;

		}
		if (EditLeagueRulesGrid.IsVisible)
		{
			EditLeagueRulesGrid.IsVisible = false;
			EditLeagueRulesGridOuter.IsVisible = false;

			LeagueRulesGrid.IsVisible = true;
			LeagueRulesCollectionView.IsEnabled = true;

			EditBtn.IsVisible = true;
			EnterRulesBtn.IsVisible = false;
			TitleLabel1.Text = "LEAGUE RULES";

			return;

		}
		if (CreateLeagueGrid.IsVisible)
		{
			CreateLeagueGridOuter.IsVisible = false;
			CreateLeagueGrid.IsVisible = false;
			GlobalLeaguesCollectionView.IsEnabled = true;
			GlobalLeaguesGrid.IsVisible = true;

			EnterLeagueBtn.IsVisible = false;
			DeleteBtn.IsVisible = true;
			TitleLabel1.Text = "GLOBAL LEAGUES";
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

			TeamsBelongedToCollectionView.ItemsSource = TeamsBelongedTo;
			TeamsBelongedToGrid.IsVisible = true;
			TeamsBelongedToCollectionView.IsEnabled = true;

			TeamsInLeagueCollectionView.ItemsSource = TeamsInLeague;
			TeamsInLeagueCollectionView.SelectedItem = null;
			TeamsInLeagueGrid.IsVisible = false;
			TeamsInLeagueCollectionView.IsEnabled = false;

			TeamsInLeague.Clear();

			TitleLabel2.Text = "MY TEAMS";



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

			TeamsBelongedToCollectionView.ItemsSource = TeamsBelongedTo;
			TeamsBelongedToGrid.IsVisible = true;
			TeamsBelongedToCollectionView.IsEnabled = true;

			TeamsInLeagueCollectionView.ItemsSource = TeamsInLeague;
			TeamsInLeagueCollectionView.SelectedItem = null;
			TeamsInLeagueGrid.IsVisible = false;
			TeamsInLeagueCollectionView.IsEnabled = false;

			TeamsInLeague.Clear();

			TitleLabel2.Text = "MY TEAMS";
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


		LeagueRulesGrid.IsVisible = true;
		LeagueRulesCollectionView.IsEnabled = true;

		CurrentLeagueCollectionViewGrid.IsVisible = false;
		CurrentLeagueCollectionView.IsEnabled = false;

		ViewTeamBtn.IsVisible = false;
		DeleteTeamBtn.IsVisible = false;

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
		int maxplayers = int.Parse(JObject.Parse(result)["maxplayers"].ToString()); // extract league rules from http response
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
		double tackledefense = Convert.ToDouble(JObject.Parse(result)["tackledefense"].ToString()); // extract league rules from http response
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

		LeagueRules leaguerules = new LeagueRules(leagueid, maxteams, maxplayers, qbcount, rbcount, wrcount, tecount, defensecount, kcount, passingtdpoints, ppc, ppi, PPTwentyFiveYdsPass, fortyyardpassbonus, sixtyyardpassbonus, threehundredyardpassbonus, fivehundredyardpassbonus, rushingtdpoints, receivingtdpoints, pptenrush, fortyyardrushreceivingbonus, sixtyyardrushreceivingbonus, onehundredyardrushreceivingbonus, twohundredyardrushreceivingbonus, ppr, twopointconversion, interceptionoffense, fumbleoffense, safetyoffense, sackdefense, tackledefense, fgpuntblock, interceptiondefense, fumbledefense, safetydefense, inttd, fumbletd, returntd, fgtentotwenty, fgmissedten, fgtwentytothirty, fgmissedtwenty, fgthirtytoforty, fgmissedthirty, fgfortytofifty, fgmissedforty, fgfiftytosixty, fgmissedfifty, fgsixtyplus, fgmissedsixty, xpmade, xpmissed);
		CurrentLeagueRules.Add(leaguerules);
	}

	public async Task PostRules(LeagueRules lr)
	{
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(lr);
		var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

		var Url = "http://localhost:8000/api/postleaguerules";
		using var client = new HttpClient();

		var response = await client.PostAsync(Url, data);
		var result = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", $"Updated league rules", "Ok");
		}
		else
		{
			await DisplayAlert("Not successful", "Please try again", "Ok");
		}
	}

	private async void OnEditBtnClicked(object sender, EventArgs e)
	{
		bool createdByCurrent = BelongedTo.Where(l => l.LeagueId == CurrentLeagueRules[0].LeagueId).Select(l => l.CreatedByCurrentUser).FirstOrDefault();
		if (!createdByCurrent)
		{
			await DisplayAlert("Error", "You must be league commissioner to edit a league's rules", "Ok");
			return;
		}

		EditLeagueRulesGridOuter.IsVisible = true;
		EditLeagueRulesGrid.IsVisible = true;
		LeagueRulesCollectionView.IsEnabled = false;
		LeagueRulesGrid.IsVisible = false;

		EditBtn.IsVisible = false;
		EnterRulesBtn.IsVisible = true;

		TitleLabel1.Text = "EDITING LEAGUE RULES";


		a.Text = CurrentLeagueRules[0].MaxTeams.ToString();
		b.Text = CurrentLeagueRules[0].QbCount.ToString();
		c.Text = CurrentLeagueRules[0].RbCount.ToString();
		d.Text = CurrentLeagueRules[0].WrCount.ToString();
		ee.Text = CurrentLeagueRules[0].TeCount.ToString();
		f.Text = CurrentLeagueRules[0].DCount.ToString();
		g.Text = CurrentLeagueRules[0].KCount.ToString();
		h.Text = CurrentLeagueRules[0].PassingTDPoints.ToString();
		i.Text = CurrentLeagueRules[0].PPC.ToString();
		jj.Text = CurrentLeagueRules[0].PPI.ToString();
		k.Text = CurrentLeagueRules[0].PPTwentyFiveYdsPass.ToString();
		l.Text = CurrentLeagueRules[0].FortyYardPassBonus.ToString();
		m.Text = CurrentLeagueRules[0].SixtyYardPassBonus.ToString();
		n.Text = CurrentLeagueRules[0].ThreeHundredYardPassBonus.ToString();
		o.Text = CurrentLeagueRules[0].FiveHundredYardPassBonus.ToString();
		p.Text = CurrentLeagueRules[0].RushingTDPoints.ToString();
		q.Text = CurrentLeagueRules[0].ReceivingTDPoints.ToString();
		r.Text = CurrentLeagueRules[0].PPTenRush.ToString();
		s.Text = CurrentLeagueRules[0].FortyYardRushReceivingBonus.ToString();
		t.Text = CurrentLeagueRules[0].SixtyYardRushReceivingBonus.ToString();
		u.Text = CurrentLeagueRules[0].OneHundredYardRushReceivingBonus.ToString();
		v.Text = CurrentLeagueRules[0].TwoHundredYardRushReceivingBonus.ToString();
		w.Text = CurrentLeagueRules[0].PPR.ToString();
		x.Text = CurrentLeagueRules[0].TwoPointConversion.ToString();
		y.Text = CurrentLeagueRules[0].InterceptionOffense.ToString();
		z.Text = CurrentLeagueRules[0].FumbleOffense.ToString();
		aa.Text = CurrentLeagueRules[0].SafetyOffense.ToString();
		bb.Text = CurrentLeagueRules[0].SackDefense.ToString();
		cc.Text = CurrentLeagueRules[0].TackleDefense.ToString();
		dd.Text = CurrentLeagueRules[0].FgPuntBlock.ToString();
		eee.Text = CurrentLeagueRules[0].InterceptionDefense.ToString();
		ff.Text = CurrentLeagueRules[0].FumbleDefense.ToString();
		gg.Text = CurrentLeagueRules[0].SafetyDefense.ToString();
		hh.Text = CurrentLeagueRules[0].IntTd.ToString();
		ii.Text = CurrentLeagueRules[0].FumbleTd.ToString();
		jjj.Text = CurrentLeagueRules[0].ReturnTd.ToString();
		kk.Text = CurrentLeagueRules[0].FgTenToTwenty.ToString();
		ll.Text = CurrentLeagueRules[0].FgMissedTen.ToString();
		mm.Text = CurrentLeagueRules[0].FgTwentyToThirty.ToString();
		nn.Text = CurrentLeagueRules[0].FgMissedTwenty.ToString();
		oo.Text = CurrentLeagueRules[0].FgThirtyToForty.ToString();
		pp.Text = CurrentLeagueRules[0].FgMissedThirty.ToString();
		qq.Text = CurrentLeagueRules[0].FgFortyToFifty.ToString();
		rr.Text = CurrentLeagueRules[0].FgMissedforty.ToString();
		ss.Text = CurrentLeagueRules[0].FgFiftyToSixty.ToString();
		tt.Text = CurrentLeagueRules[0].FgMissedFifty.ToString();
		uu.Text = CurrentLeagueRules[0].FgSixtyPlus.ToString();
		vv.Text = CurrentLeagueRules[0].FgMissedSixty.ToString();
		ww.Text = CurrentLeagueRules[0].XpMade.ToString();
		xx.Text = CurrentLeagueRules[0].XpMissed.ToString();
		zz.Text = CurrentLeagueRules[0].MaxPlayers.ToString();


	}

	private async void OnEnterBtnClicked(object sender, EventArgs e)
	{
		try
		{
			if (int.Parse(a.Text) > 16) {
				await DisplayAlert("Error", "League cannot contain more than 16 teams. Please lower limit", "Ok");
				return;
			}
			else if (int.Parse(b.Text) > 4)
			{
				await DisplayAlert("Error", "League cannot contain more than 4 QBs. Please lower limit", "Ok");
				return;
			}
			else if (int.Parse(c.Text) > 5)
			{
				await DisplayAlert("Error", "League cannot contain more than 5 RBs. Please lower limit", "Ok");
				return;
			}
			else if (int.Parse(d.Text) > 6)
			{
				await DisplayAlert("Error", "League cannot contain more than 6 WRs. Please lower limit", "Ok");
				return;
			}
			else if (int.Parse(ee.Text) > 4)
			{
				await DisplayAlert("Error", "League cannot contain more than 4 TEs. Please lower limit", "Ok");
				return;
			}
			else if (int.Parse(f.Text) > 3)
			{
				await DisplayAlert("Error", "League cannot contain more than 3 Ds. Please lower limit", "Ok");
				return;
			}
			else if (int.Parse(g.Text) > 3)
			{
				await DisplayAlert("Error", "League cannot contain more than 3 Ks. Please lower limit", "Ok");
				return;
			}
			LeagueRules newLeagueRules = new LeagueRules(CurrentLeagueRules[0].LeagueId, int.Parse(a.Text), int.Parse(zz.Text), int.Parse(b.Text), int.Parse(c.Text), int.Parse(d.Text), int.Parse(ee.Text), int.Parse(f.Text), int.Parse(g.Text), int.Parse(h.Text), Convert.ToDouble(i.Text), Convert.ToDouble(jj.Text), int.Parse(k.Text), int.Parse(l.Text), int.Parse(m.Text), int.Parse(n.Text), int.Parse(o.Text), int.Parse(p.Text), int.Parse(q.Text), int.Parse(r.Text), int.Parse(s.Text), int.Parse(t.Text), int.Parse(u.Text), int.Parse(v.Text), Convert.ToDouble(w.Text), int.Parse(x.Text), int.Parse(y.Text), int.Parse(z.Text), int.Parse(aa.Text), int.Parse(bb.Text), Convert.ToDouble(cc.Text), int.Parse(dd.Text), int.Parse(eee.Text), int.Parse(ff.Text), int.Parse(gg.Text), int.Parse(hh.Text), int.Parse(ii.Text), int.Parse(jjj.Text), int.Parse(kk.Text), int.Parse(ll.Text), int.Parse(mm.Text), int.Parse(nn.Text), int.Parse(oo.Text), int.Parse(pp.Text), int.Parse(qq.Text), int.Parse(rr.Text), int.Parse(ss.Text), int.Parse(tt.Text), int.Parse(uu.Text), int.Parse(vv.Text), int.Parse(ww.Text), int.Parse(xx.Text));
			await PostRules(newLeagueRules);

			CurrentLeagueRules.Clear();
			await GetRules(newLeagueRules.LeagueId);
			LeagueRulesCollectionView.ItemsSource = CurrentLeagueRules;
			BelongedTo.Where(l => l.LeagueId == newLeagueRules.LeagueId).FirstOrDefault().MaxTeams = newLeagueRules.MaxTeams;
			GlobalLeagues.Where(l => l.LeagueId == newLeagueRules.LeagueId).FirstOrDefault().MaxTeams = newLeagueRules.MaxTeams;

			EditLeagueRulesGridOuter.IsVisible = false;
			EditLeagueRulesGrid.IsVisible = false;
			LeagueRulesCollectionView.IsEnabled = true;
			LeagueRulesGrid.IsVisible = true;

			EditBtn.IsVisible = true;
			EnterRulesBtn.IsVisible = false;

			TitleLabel1.Text = "LEAGUE RULES";
		}
		catch (FormatException fex)
		{
			await DisplayAlert("Invalid input", "One or more fields have invalid input. Please make sure you enter a valid number in every field", "Ok");
		}
	}



} // End class
