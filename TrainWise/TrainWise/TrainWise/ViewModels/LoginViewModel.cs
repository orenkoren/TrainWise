using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TrainWise.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public LoginViewModel()
        {
            Title = "TrainWise";
            Username = "User Name";
            Password = "Password";
        }

    }
}
