using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyFootballMAUI.Models
{

	public class League
	{
		[JsonPropertyName("leagueid")]
		public int LeagueId { get; set; }

		public string LeagueIdStr { get; set; }

		[JsonPropertyName("leaguename")]
		public string LeagueName { get; set; } = default!;

		public string LeagueNameStr { get; set; }

		[JsonPropertyName("maxteams")]
		public int MaxTeams { get; set; } = default!;

		public string MaxTeamsStr { get; set; }

		[JsonPropertyName("currentteams")]
		public int CurrentTeams { get; set; }

		public string CurrentTeamsStr { get; set; }

		[JsonPropertyName("creator")]
		public int Creator { get; set; } = default!; // user id goes here

		public string CreatorUsername { get; set; } = default!;

		public string CreatorUsernameStr { get; set; }

		public string CreatorName { get; set; } = default!;

		public string CreatorNameStr { get; set; } 

		public bool CreatedByCurrentUser { get; set; } = default!;


		public League(int id, string name, int maxteams, int creator, bool createdbycurrentuser)
		{
			LeagueId = id;
			LeagueName = name;
			MaxTeams = maxteams;
			Creator = creator;
			CreatedByCurrentUser= createdbycurrentuser;

			LeagueIdStr = $"League ID ({LeagueId})";
			LeagueNameStr = $"League Name ({LeagueName})";
			MaxTeamsStr = $"Max # Teams ({MaxTeams})";
			CurrentTeamsStr = $"Current # Teams ({CurrentTeams})";
			CreatorUsernameStr = $"Creator Username ({CreatorUsername})";
			CreatorNameStr = $"Creator Name ({CreatorName})";
		}
	}
}
