using IntroSE.Kanban.Frontend.Model;
using System;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class MainViewModel : NotifiableObject
    {
        private BackendController Controller;

        public MainViewModel()
        {
            this.Controller = new BackendController();
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                this._username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// Login a user
        /// </summary>
        /// <returns></returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <returns></returns>
        public UserModel Register()
        {
            Message = "";
            try
            {
                return Controller.Register(Username, Password);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        
    }
}
