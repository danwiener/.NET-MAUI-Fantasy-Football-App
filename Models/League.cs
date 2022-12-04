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
		public Image Image { get; set; }
		[JsonPropertyName("leagueid")]
		public int LeagueId { get; set; }

		[JsonPropertyName("leaguename")]
		public string LeagueName { get; set; } = default!;

		[JsonPropertyName("maxteams")]
		public int MaxTeams { get; set; } = default!;

		[JsonPropertyName("creator")]
		public int Creator { get; set; } = default!; // user id goes here

		public string CreatorUsername { get; set; } = default!;

		public string CreatorName { get; set; } = default!;

		public bool CreatedByCurrentUser { get; set; } = default!;

		public League(int id, string name, int maxteams, int creator)
		{
			LeagueId = id;
			LeagueName = name;
			MaxTeams = maxteams;
			Creator = creator;
		}
	}
}
