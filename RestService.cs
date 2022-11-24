﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FantasyFootballMAUI
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
            this.email=email;
            this.password=password;
        }
    }
}