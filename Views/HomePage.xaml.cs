
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

		try
		{
			await AddPlayer(dto);
		}
		catch (Exception ex)
		{
			return;
		}
		
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
			await DisplayAlert("Position limit reached", $"{result}", "Ok");
			throw new Exception("exception");
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
			await DisplayAlert("Not successful", $"{result}", "Ok");
			throw new Exception("Team already contains more players than specified limit. Please adjust limits and try again.");
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


		a.Placeholder = CurrentLeagueRules[0].MaxTeamsStr;
		b.Placeholder = CurrentLeagueRules[0].QbCountStr;
		c.Placeholder = CurrentLeagueRules[0].RbCountStr;
		d.Placeholder = CurrentLeagueRules[0].WrCountStr;
		ee.Placeholder = CurrentLeagueRules[0].TeCountStr;
		f.Placeholder = CurrentLeagueRules[0].defensecountStr;
		g.Placeholder = CurrentLeagueRules[0].kcountStr;
		h.Placeholder = CurrentLeagueRules[0].PassingTDPointsStr;
		i.Placeholder = CurrentLeagueRules[0].PPCStr;
		jj.Placeholder = CurrentLeagueRules[0].PPIStr;
		k.Placeholder = CurrentLeagueRules[0].PPTwentyFiveYdsPassStr;
		l.Placeholder = CurrentLeagueRules[0].FortyYardPassBonusStr;
		m.Placeholder = CurrentLeagueRules[0].SixtyYardPassBonusStr;
		n.Placeholder = CurrentLeagueRules[0].ThreeHundredYardPassBonusStr;
		o.Placeholder = CurrentLeagueRules[0].FiveHundredYardPassBonusStr;
		p.Placeholder = CurrentLeagueRules[0].RushingTDPointsStr;
		q.Placeholder = CurrentLeagueRules[0].ReceivingTDPointsStr;
		r.Placeholder = CurrentLeagueRules[0].PPTenRushStr;
		s.Placeholder = CurrentLeagueRules[0].FortyYardRushReceivingBonusStr;
		t.Placeholder = CurrentLeagueRules[0].SixtyYardRushReceivingBonusStr;
		u.Placeholder = CurrentLeagueRules[0].OneHundredYardRushReceivingBonusStr;
		v.Placeholder = CurrentLeagueRules[0].TwoHundredYardRushReceivingBonusStr;
		w.Placeholder = CurrentLeagueRules[0].PPRStr;
		x.Placeholder = CurrentLeagueRules[0].TwoPointConversionStr;
		y.Placeholder = CurrentLeagueRules[0].InterceptionOffenseStr;
		z.Placeholder = CurrentLeagueRules[0].FumbleOffenseStr;
		aa.Placeholder = CurrentLeagueRules[0].SafetyOffenseStr;
		bb.Placeholder = CurrentLeagueRules[0].SackDefenseStr;
		cc.Placeholder = CurrentLeagueRules[0].TackleDefenseStr;
		dd.Placeholder = CurrentLeagueRules[0].FgPuntBlockStr;
		eee.Placeholder = CurrentLeagueRules[0].InterceptionDefenseStr;
		ff.Placeholder = CurrentLeagueRules[0].FumbleDefenseStr;
		gg.Placeholder = CurrentLeagueRules[0].SafetyDefenseStr;
		hh.Placeholder = CurrentLeagueRules[0].IntTdStr;
		ii.Placeholder = CurrentLeagueRules[0].FumbleTdStr;
		jjj.Placeholder = CurrentLeagueRules[0].ReturnTdStr;
		kk.Placeholder = CurrentLeagueRules[0].FgTenToTwentyStr;
		ll.Placeholder = CurrentLeagueRules[0].FgMissedTenStr;
		mm.Placeholder = CurrentLeagueRules[0].FgTwentyToThirtyStr;
		nn.Placeholder = CurrentLeagueRules[0].FgMissedTwentyStr;
		oo.Placeholder = CurrentLeagueRules[0].FgThirtyToFortyStr;
		pp.Placeholder = CurrentLeagueRules[0].FgMissedThirtyStr;
		qq.Placeholder = CurrentLeagueRules[0].FgFortyToFiftyStr;
		rr.Placeholder = CurrentLeagueRules[0].FgMissedfortyStr;
		ss.Placeholder = CurrentLeagueRules[0].FgFiftyToSixtyStr;
		tt.Placeholder = CurrentLeagueRules[0].FgMissedFiftyStr;
		uu.Placeholder = CurrentLeagueRules[0].FgSixtyPlusStr;
		vv.Placeholder = CurrentLeagueRules[0].FgMissedSixtyStr;
		ww.Placeholder = CurrentLeagueRules[0].XpMadeStr;
		xx.Placeholder = CurrentLeagueRules[0].XpMissedStr;
		zz.Placeholder = CurrentLeagueRules[0].MaxPlayersStr;


	}

	private async void OnEnterBtnClicked(object sender, EventArgs e)
	{
		try
		{

			LeagueRules currentLeagueRules = CurrentLeagueRules[0];

			if (a.Text != null && int.Parse(a.Text) > 16) {
				await DisplayAlert("Error", "League cannot contain more than 16 teams. Please lower limit", "Ok");
				return;
			}
			else if (b.Text != null && int.Parse(b.Text) > 4)
			{
				await DisplayAlert("Error", "League cannot contain more than 4 QBs. Please lower limit", "Ok");
				return;
			}
			else if (c.Text != null && int.Parse(c.Text) > 5)
			{
				await DisplayAlert("Error", "League cannot contain more than 5 RBs. Please lower limit", "Ok");
				return;
			}
			else if (d.Text != null && int.Parse(d.Text) > 6)
			{
				await DisplayAlert("Error", "League cannot contain more than 6 WRs. Please lower limit", "Ok");
				return;
			}
			else if (ee.Text != null && int.Parse(ee.Text) > 4)
			{
				await DisplayAlert("Error", "League cannot contain more than 4 TEs. Please lower limit", "Ok");
				return;
			}
			else if (f.Text != null && int.Parse(f.Text) > 3)
			{
				await DisplayAlert("Error", "League cannot contain more than 3 Ds. Please lower limit", "Ok");
				return;
			}
			else if (g.Text != null && int.Parse(g.Text) > 3)
			{
				await DisplayAlert("Error", "League cannot contain more than 3 Ks. Please lower limit", "Ok");
				return;
			}

			int ab;
			int zzb;
			int bbb;
			int cb;
			int db;
			int eeb;
			int fb;
			int gb;
			int hb;
			double ib;
			double jjb;
			int kb;
			int lb;
			int mb;
			int nb;
			int ob;
			int pb;
			int qb;
			int rb;
			int sb;
			int tb;
			int ub;
			int vb;
			double wb;
			int xb;
			int yb;
			int zb;
			int aab;
			int bbbb;
			double ccb;
			int ddb;
			int eeeb;
			int ffb;
			int ggb;
			int hhb;
			int iib;
			int jjjb;
			int kkb;
			int llb;
			int mmb;
			int nnb;
			int oob;
			int ppb;
			int qqb;
			int rrb;
			int ssb;
			int ttb;
			int uub;
			int vvb;
			int wwb;
			int xxb;


			if (a.Text != null)
			{
				ab = int.Parse(a.Text);
			}
			else
			{
				ab = currentLeagueRules.MaxTeams;
			}
			if (zz.Text != null)
			{
				zzb = int.Parse(zz.Text);
			}
			else
			{
				zzb = currentLeagueRules.MaxPlayers;
			}
			if (b.Text != null)
			{
				bbb = int.Parse(b.Text);
			}
			else
			{
				bbb = currentLeagueRules.QbCount;
			}
			if (c.Text != null)
			{
				cb = int.Parse(c.Text);
			}
			else
			{
				cb = currentLeagueRules.RbCount;
			}
			if (d.Text != null)
			{
				db = int.Parse(d.Text);
			}
			else
			{
				db = currentLeagueRules.WrCount;
			}
			if (ee.Text != null)
			{
				eeb = int.Parse(ee.Text);
			}
			else
			{
				eeb = currentLeagueRules.TeCount;
			}
			if (f.Text != null)
			{
				fb = int.Parse(f.Text);
			}
			else
			{
				fb = currentLeagueRules.defensecount;
			}
			if (g.Text != null)
			{
				gb = int.Parse(g.Text);
			}
			else
			{
				gb = currentLeagueRules.kcount;
			}
			if (h.Text != null)
			{
				hb = int.Parse(h.Text);
			}
			else
			{
				hb = currentLeagueRules.PassingTDPoints;
			}
			if (i.Text != null)
			{
				ib = Convert.ToDouble(i.Text);
			}
			else
			{
				ib = currentLeagueRules.PPC;
			}
			if (jj.Text != null)
			{
				jjb = Convert.ToDouble(jj.Text);
			}
			else
			{
				jjb = currentLeagueRules.PPI;
			}
			if (k.Text != null)
			{
				kb = int.Parse(k.Text);
			}
			else
			{
				kb = currentLeagueRules.PPTwentyFiveYdsPass;
			}
			if (l.Text != null)
			{
				lb = int.Parse(l.Text);
			}
			else
			{
				lb = currentLeagueRules.FortyYardPassBonus;
			}
			if (m.Text != null)
			{
				mb = int.Parse(m.Text);
			}
			else
			{
				mb = currentLeagueRules.SixtyYardPassBonus;
			}
			if (n.Text != null)
			{
				nb = int.Parse(n.Text);
			}
			else
			{
				nb = currentLeagueRules.ThreeHundredYardPassBonus;
			}
			if (o.Text != null)
			{
				ob = int.Parse(o.Text);
			}
			else
			{
				ob = currentLeagueRules.FiveHundredYardPassBonus;
			}
			if (p.Text != null)
			{
				pb = int.Parse(p.Text);
			}
			else
			{
				pb = currentLeagueRules.RushingTDPoints;
			}
			if (q.Text != null)
			{
				qb = int.Parse(q.Text);
			}
			else
			{
				qb = currentLeagueRules.ReceivingTDPoints;
			}
			if (r.Text != null)
			{
				rb = int.Parse(r.Text);
			}
			else
			{
				rb = currentLeagueRules.PPTenRush;
			}
			if (s.Text != null)
			{
				sb = int.Parse(s.Text);
			}
			else
			{
				sb = currentLeagueRules.FortyYardRushReceivingBonus;
			}
			if (t.Text != null)
			{
				tb = int.Parse(t.Text);
			}
			else
			{
				tb = currentLeagueRules.SixtyYardRushReceivingBonus;
			}
			if (u.Text != null)
			{
				ub = int.Parse(u.Text);
			}
			else
			{
				ub = currentLeagueRules.OneHundredYardRushReceivingBonus;
			}
			if (v.Text != null)
			{
				vb = int.Parse(v.Text);
			}
			else
			{
				vb = currentLeagueRules.TwoHundredYardRushReceivingBonus;
			}
			if (w.Text != null)
			{
				wb = Convert.ToDouble(w.Text);
			}
			else
			{
				wb = currentLeagueRules.PPR;
			}
			if (x.Text != null)
			{
				xb = int.Parse(x.Text);
			}
			else
			{
				xb = currentLeagueRules.TwoPointConversion;
			}
			if (y.Text != null)
			{
				yb = int.Parse(y.Text);
			}
			else
			{
				yb = currentLeagueRules.InterceptionOffense;
			}
			if (z.Text != null)
			{
				zb = int.Parse(z.Text);
			}
			else
			{
				zb = currentLeagueRules.FumbleOffense;
			}
			if (aa.Text != null)
			{
				aab = int.Parse(aa.Text);
			}
			else
			{
				aab = currentLeagueRules.SafetyOffense;
			}
			if (bb.Text != null)
			{
				bbbb = int.Parse(bb.Text);
			}
			else
			{
				bbbb = currentLeagueRules.SackDefense;
			}
			if (cc.Text != null)
			{
				ccb = Convert.ToDouble(cc.Text);
			}
			else
			{
				ccb = currentLeagueRules.TackleDefense;
			}
			if (dd.Text != null)
			{
				ddb = int.Parse(ee.Text);
			}
			else
			{
				ddb = currentLeagueRules.FgPuntBlock;
			}
			if (eee.Text != null)
			{
				eeeb = int.Parse(eee.Text);
			}
			else
			{
				eeeb = currentLeagueRules.InterceptionDefense;
			}
			if (ff.Text != null)
			{
				ffb = int.Parse(ff.Text);
			}
			else
			{
				ffb = currentLeagueRules.FumbleDefense;
			}
			if (gg.Text != null)
			{
				ggb = int.Parse(gg.Text);
			}
			else
			{
				ggb = currentLeagueRules.SafetyDefense;
			}
			if (hh.Text != null)
			{
				hhb = int.Parse(hh.Text);
			}
			else
			{
				hhb = currentLeagueRules.IntTd;
			}
			if (ii.Text != null)
			{
				iib = int.Parse(ii.Text);
			}
			else
			{
				iib = currentLeagueRules.FumbleTd;
			}
			if (jjj.Text != null)
			{
				jjjb = int.Parse(jj.Text);
			}
			else
			{
				jjjb = currentLeagueRules.ReturnTd;
			}
			if (kk.Text != null)
			{
				kkb = int.Parse(kk.Text);
			}
			else
			{
				kkb = currentLeagueRules.FgTenToTwenty;
			}
			if (ll.Text != null)
			{
				llb = int.Parse(ll.Text);
			}
			else
			{
				llb = currentLeagueRules.FgMissedTen;
			}
			if (mm.Text != null)
			{
				mmb = int.Parse(mm.Text);
			}
			else
			{
				mmb = currentLeagueRules.FgTwentyToThirty;
			}
			if (nn.Text != null)
			{
				nnb = int.Parse(nn.Text);
			}
			else
			{
				nnb = currentLeagueRules.FgMissedTwenty;
			}
			if (oo.Text != null)
			{
				oob = int.Parse(oo.Text);
			}
			else
			{
				oob = currentLeagueRules.FgThirtyToForty;
			}
			if (pp.Text != null)
			{
				ppb = int.Parse(pp.Text);
			}
			else
			{
				ppb = currentLeagueRules.FgMissedThirty;
			}
			if (qq.Text != null)
			{
				qqb = int.Parse(qq.Text);
			}
			else
			{
				qqb = currentLeagueRules.FgFortyToFifty;
			}
			if (rr.Text != null)
			{
				rrb = int.Parse(rr.Text);
			}
			else
			{
				rrb = currentLeagueRules.FgMissedforty;
			}
			if (ss.Text != null)
			{
				ssb = int.Parse(ss.Text);
			}
			else
			{
				ssb = currentLeagueRules.FgFiftyToSixty;
			}
			if (tt.Text != null)
			{
				ttb = int.Parse(tt.Text);
			}
			else
			{
				ttb = currentLeagueRules.FgMissedFifty;
			}
			if (uu.Text != null)
			{
				uub = int.Parse(uu.Text);
			}
			else
			{
				uub = currentLeagueRules.FgSixtyPlus;
			}
			if (vv.Text != null)
			{
				vvb = int.Parse(vv.Text);
			}
			else
			{
				vvb = currentLeagueRules.FgMissedSixty;
			}
			if (ww.Text != null)
			{
				wwb = int.Parse(ww.Text);
			}
			else
			{
				wwb = currentLeagueRules.XpMade;
			}
			if (xx.Text != null)
			{
				xxb = int.Parse(xx.Text);
			}
			else
			{
				xxb = currentLeagueRules.XpMissed;
			}

			LeagueRules newLeagueRules = new LeagueRules(CurrentLeagueRules[0].LeagueId, ab, zzb, bbb, cb, db, eeb, fb, gb, hb, ib, jjb, kb, lb, mb, nb, ob, pb, qb, rb, sb, tb, ub, vb, wb, xb, yb, zb, aab, bbbb, ccb, ddb, eeeb, ffb, ggb, hhb, iib, jjjb, kkb, llb, mmb, nnb, oob, ppb, qqb, rrb, ssb, ttb, uub, vvb, wwb, xxb);
			try
			{
				await PostRules(newLeagueRules);
			}
			catch (Exception ex)
			{
				return;
			}

			a.Text = null;
			b.Text = null;
			c.Text = null;
			d.Text = null;
			ee.Text = null;
			f.Text = null;
			g.Text = null;
			h.Text = null;
			i.Text = null;
			jj.Text = null;
			k.Text = null;
			l.Text = null;
			m.Text = null;
			n.Text = null;
			o.Text = null;
			p.Text = null;
			q.Text = null;
			r.Text = null;
			s.Text = null;
			t.Text = null;
			u.Text = null;
			v.Text = null;
			w.Text = null;
			x.Text = null;
			y.Text = null;
			z.Text = null;
			aa.Text = null;
			bb.Text = null;
			cc.Text = null;
			dd.Text = null;
			eee.Text = null;
			ff.Text = null;
			gg.Text = null;
			hh.Text = null;
			ii.Text = null;
			jjj.Text = null;
			kk.Text = null;
			ll.Text = null;
			mm.Text = null;
			nn.Text = null;
			oo.Text = null;
			pp.Text = null;
			qq.Text = null;
			rr.Text = null;
			ss.Text = null;
			tt.Text = null;
			uu.Text = null;
			vv.Text = null;
			ww.Text = null;
			xx.Text = null;
			zz.Text = null;


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
