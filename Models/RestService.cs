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

}
