using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyFootballMAUI.Models
{
    public class User
    {
		[JsonPropertyName("UserId")]
		public int UserId { get; set; }

		[JsonPropertyName("user_name")]
		public string Username { get; set; } = default!;

		[JsonPropertyName("name")]
		public string Name { get; set; } = default!;

		[JsonPropertyName("email")]
		public string Email { get; set; } = default!;


		public User(int id, string username, string name, string email)
		{
			this.UserId= id;
			this.Username= username;
			this.Name= name;
			this.Email= email;
		}
	}
}
