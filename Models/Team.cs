using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyFootballMAUI.Models
{
    public class Team
    {
		//public String ImageSource { get; set; } = default!;

		[JsonPropertyName("teamid")]
		public int TeamId { get; set; }

		public string TeamIdStr { get; set; }

		[JsonPropertyName("teamname")]
		public string TeamName { get; set; } = default!;

		public string TeamNameStr { get; set; }

		[JsonPropertyName("createdondate")]
		public string CreatedOnDate { get; set; } = default!;

		public string CreatedOnDateStr { get; set; }


		[JsonPropertyName("creator")]
		public int Creator { get; set; } = default!; // user id goes here

		[JsonPropertyName("leagueid")]
		public int League { get; set; }

		public string LeagueName { get; set; }

		public string LeagueNameStr { get; set; }

		public string CreatorUsername { get; set; } = default!;

		public string CreatorUsernameStr { get; set; }

		public string CreatorName { get; set; } = default!;

		public string CreatorNameStr { get; set; }

		public bool CreatedByCurrentUser { get; set; } = default!;

		public Team(int id, string name, string createdondate, int creator, int leagueid, bool createdbycurrentuser)
		{
			TeamId = id;
			TeamName = name;
			CreatedOnDate = createdondate;
			Creator = creator;
			League = leagueid;
			CreatedByCurrentUser = createdbycurrentuser;

			TeamIdStr = $"Team ID ({TeamId})";
			TeamNameStr = $"Team Name ({TeamName})";
			LeagueNameStr = $"League Name ({LeagueName})";
			CreatedOnDateStr = $"Created On ({CreatedOnDate.ToString()})";
			CreatorUsernameStr = $"Creator Username ({CreatorUsername})";
			CreatorNameStr = $"Creator Name ({CreatorName})";

		}
	}
}
