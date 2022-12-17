using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyFootballMAUI.Models
{
	public class Player
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		[JsonPropertyName("PlayerId")]
		public int PlayerId { get; set; }

		[JsonPropertyName("teamid")]
		public int? TeamId { get; set;} // Fantasy team player associated with

		[JsonPropertyName("position")]
		public string Position { get; set; } = default!;

		[JsonPropertyName("playername")]
		public string PlayerName { get; set; } = default!;

		[JsonPropertyName("team")]
		public string Team { get; set; } = default!;

		public bool FreeAgent { get => FreeAgent;
			set
			{
				FreeAgent = TeamId == null;
			}
		}

		public Player(int id, string position, string playername, string team, bool freeagent)
		{
			PlayerId = id;
			Position = position;
			PlayerName = playername;
			Team = team;
			FreeAgent = freeagent;
		}
	}
}
