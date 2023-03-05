using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

[assembly: InternalsVisibleTo("NUnitTestProject1")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        private string _email;
        private string _password;
        private bool _isLogged;
        private UserDTO dto;

        /// <summary>
        /// Constructor to the user' initialiez the isLogged field as false
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public User(string email, string password)
        {
            email.ToLower();
            Password = password;
            Email = email;
            IsLogged = true;
             
          dto = new UserDTO(email, password);
        }
        /// <summary>
        /// constructor from dto object
        /// </summary>
        /// <param name="userdto"></param>
        public User(UserDTO userdto)
        {
            this.Email = userdto.Email;
            this.Password = userdto.Password;
           
        }
        /// <summary>
        /// email,password and is logged getters and setters
        /// </summary>
        public string Email { get { return _email; } set { _email = value; } }
        [JsonIgnore]
        public string Password { get { return _password; } set { _password = value; } }
        [JsonIgnore]
        public bool IsLogged { get { return _isLogged; } set { _isLogged = value; } }
        public UserDTO DTO { get { return dto; } }

        public bool getIsLogged()
        {
            return _isLogged;
        }
        /// <summary>
        /// Login from the kanban data base
        /// </summary>
        /// </summary>
        /// <param name="pass"></param>  
        public bool Login(string pass)
        {
            if (_isLogged)
            {
                throw new Exception ("User already logged in");
            }
            if (pass.Equals(_password))
            {
                _isLogged = true;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Logout from the kanban data base
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Logout()
        {
            if (!_isLogged)
            {
                throw new Exception("User already logged out");
            }
            IsLogged = false;
            return true;
        }
   
    }
}
