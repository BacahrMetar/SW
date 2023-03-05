using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class UserDTO : DTO
    {
        public const string UsersEmailColumnName = "Email";
        public const string UsersPasswordColumnName = "Password";

        private string _email;
        private string _password;

        public string Email { get => _email; }
        public string Password { get => _password; set { _password = value; _controller.Update(_email,UsersPasswordColumnName, value); } }

        public UserDTO(string email, string password) : base(new UsersDalController())
        {
            _email = email;
            _password = password;
        }
    }
}
