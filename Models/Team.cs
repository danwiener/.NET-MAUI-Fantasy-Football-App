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

		[JsonPropertyName("teamname")]
		public string TeamName { get; set; } = default!;

		[JsonPropertyName("createdondate")]
		public string CreatedOnDate { get; set; } = default!;


		[JsonPropertyName("creator")]
		public int Creator { get; set; } = default!; // user id goes here

		[JsonPropertyName("leagueid")]
		public int League { get; set; }

		public string LeagueName { get; set; }

		public string CreatorUsername { get; set; } = default!;

		public string CreatorName { get; set; } = default!;

		public bool CreatedByCurrentUser { get; set; } = default!;

		public Team(int id, string name, string createdondate, int creator, int leagueid, bool createdbycurrentuser)
		{
			TeamId = id;
			TeamName = name;
			CreatedOnDate = createdondate;
			Creator = creator;
			League = leagueid;
			CreatedByCurrentUser = createdbycurrentuser;
		}
	}
}
