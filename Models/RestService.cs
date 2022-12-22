using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyFootballMAUI.Models
{
    public class NewUser
    {
        public string user_name;
        public string name;
        public string email;
        public string password;
        public string password_confirm;
        public NewUser(string user_name, string name, string email, string password, string password_confirm)
        {
            this.user_name = user_name;
            this.name = name;
            this.email = email;
            this.password = password;
            this.password_confirm = password_confirm;
        }
    }

    public class NewLogin
    {
        public string email;
        public string password;

        public NewLogin(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }

    public class ForgotPassword
    {
        public string email;

        public ForgotPassword(string email)
        {
            this.email = email;
        }
    } // End class

    public class ResetPassword
    {
        public string token;

        public string password;

        public string password_confirm;


        public ResetPassword(string token, string password, string password_confirm)
        {
            this.token = token;
            this.password = password;
            this.password_confirm = password_confirm;
        }
    } // End class

    public class UserDTO
    {
        public int UserId;

        public UserDTO(int UserId)
        {
            this.UserId = UserId;
        }
    }

    public class DeleteLeagueDTO
    {
        public int leagueid;
        public string leaguename;

		public DeleteLeagueDTO(int leagueid, string leaguename)
        {
            this.leagueid = leagueid;
            this.leaguename = leaguename;
        }
    }

	public class DeleteTeamDTO
	{
		public int teamid;
		public string teamname;
		public DeleteTeamDTO(int teamid, string teamname)
		{
			this.teamid = teamid;
            this.teamname = teamname;
		}
	}

	public class CreateLeagueDTO
    {
		[JsonPropertyName("leaguename")]
		public string LeagueName { get; set; } = default!;

		[JsonPropertyName("maxteams")]
		public int MaxTeams { get; set; } = default!;

		[JsonPropertyName("creator")]
		public int Creator { get; set; } = default!;

        public CreateLeagueDTO(string leagueName, int maxTeams, int creator)
		{
			LeagueName = leagueName;
			MaxTeams = maxTeams;
			Creator = creator;
        }
    }

    public class AddPlayerDTO
    {
        [JsonPropertyName("PlayerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("playername")]
        public string PlayerName { get; set; }

        [JsonPropertyName("teamname")]
        public string TeamName { get; set; }

        [JsonPropertyName("teamid")]
        public int TeamId { get; set; } // Fantasy team player associated with

        public AddPlayerDTO(int playerid, string playername, string teamname, int teamid)
        {
            PlayerId = playerid;
            PlayerName = playername;
            TeamName = teamname;
            TeamId = teamid;
        }
    }

    public class DropPlayerDTO
    {
		[JsonPropertyName("PlayerId")]
		public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public DropPlayerDTO(int playerId, string playername)
		{
			PlayerId = playerId;
            PlayerName = playername;
		}
	}

    public class CreateTeamDTO
    {
		public string team_name { get; set; }

		public int creator_id { get; set; }

		public int league_id { get; set; }

        public CreateTeamDTO(string teamname, int creatorid, int leagueid)
        {
			team_name = teamname;
			creator_id = creatorid;
			league_id = leagueid;
        }
    }

    public class DefenseDTO
    {
		[JsonPropertyName("LeagueId")]
		public int LeagueId { get; set; }

		[JsonPropertyName("defensecount")]
		public int DCount { get; set; }
	}

    public class SignoutDTO
    {
        public int UserId;
    }

}
