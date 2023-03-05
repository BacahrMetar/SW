using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController
    {
        private readonly Dictionary<string, User> _users = new Dictionary<string, User>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private UsersDalController _usersDalController;
        private const int PASSWORD_MAX_LENGHT = 20;
        private const int PASSWORD_MIN_LENGHT = 6;

        /// <summary>
        /// Constructor for UserController 
        /// </summary>
        public UserController()
        {
            this._users = new Dictionary<string, User>();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting UserService log!");
            _usersDalController= new UsersDalController();
        }

        public Dictionary<string, User> getUsers() { return _users; }

        /// <summary>
        /// method for Register a user 
        /// </summary>
        /// <param name="email">The email of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <exception cref="Exception"></exception>
        public void Register(string email, string password)
        {
            
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email)) {
                log.Debug("User with null / empty / white spaces only, email attempted register");
                throw new Exception("Email is null / empty / white spaces only");
            }

            email = email.ToLower(); //to legalize email, as done in real life

            if (!isGoodEmail(email))
            {
                log.Debug("a user attempted to register with ilegal email");
                throw new Exception("Email is not legal.");
            }
            
            if (!isGoodPass(password))
            {
                log.Debug("user attempted to register with invalid password");
                throw new Exception("Password doesnt meet Requirements. Length should be 6-20 characters," + "\n" +
                    "at least one uppercase,one lowercase and one number");
            }
            if (_users.ContainsKey(email))
            {
                log.Debug("existing user try to register");
                throw new Exception("Email " + email + " already exists");
            }
            
            User u = new(email, password);
            UserDTO dto = u.DTO;
            _usersDalController.Insert(dto);
            _users[email] = u; //adding to dictionary of existing users
        }


        /// <summary>
        /// Method for checking if password is valid
        /// </summary>
        /// <param name="pass">The password to check</param>
        /// <returns>boolean-representing if paswword is valid or not</returns>
        private bool isGoodPass(string pass)
        {
            if (string.IsNullOrWhiteSpace(pass) || string.IsNullOrEmpty(pass))
                throw new Exception("Password is null / empty / white spaces only");
            if (pass.Length < PASSWORD_MIN_LENGHT || pass.Length > PASSWORD_MAX_LENGHT)
                return false;
            bool containsAtLeastOneUppercase = pass.Any(char.IsUpper);
            bool containsAtLeastOneLowercase = pass.Any(char.IsLower);
            bool containsAtLeastOneNumber = pass.Any(char.IsDigit);

            return containsAtLeastOneLowercase & containsAtLeastOneUppercase & containsAtLeastOneNumber;
        }

        /// <summary>
        /// Method for checking if email adress is valid
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>boolean-representing if email address is valid or not</returns>
        private bool isGoodEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)|| string.IsNullOrEmpty(email))
                throw new Exception("Email is null / empty / white spaces only");
            Regex regex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
           
            return regex.IsMatch(email);
        }
        /// <summary>
        /// Method for login an existing user user
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <exception cref="Exception"></exception>
        public void Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Debug("User with null / empty / white spaces only, email attempted to login");
                throw new Exception("Email is null / empty / white spaces only");
            }

            email = email.ToLower(); //to legalize email as it was registered

            if (isExist(email))
            {
                User u = getUser(email);
                if (u.IsLogged == true)
                {
                    log.Debug("Logged in user attepted to login");
                    throw new Exception("User is already logged in!");
                }
                if (password is null)
                {
                    log.Debug("User with null password attempted register");
                    throw new Exception("Password is null");
                }
                bool logged = u.Login(password);
                if (!logged)
                {
                    log.Debug("User with invalid username or password");
                    throw new Exception("Wrong Email or Password!"); //for security we'll say both email and password
                }
            }
            else
            {
                log.Debug("User with invalid username or password");
                throw new Exception("Wrong Email or Password!");
            }
            
        }
       

        /// <summary>
        /// Method for logut a user
        /// </summary>
        /// <param name="email">The user email to logout</param>
        /// <exception cref="Exception"></exception>
        public void Logout(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Debug("User with null / empty / white spaces only, email attempted to logout");
                throw new Exception("Email is null / empty / white spaces only");
            }

            email = email.ToLower();

            if (isExist(email))
            {
                User u = getUser(email);
                if (u.IsLogged == false)
                {
                    log.Debug("Logged out user attepted to logout!");
                    throw new Exception("User is already logged out!");
                }
                u.Logout();
            }
            else
            {
                log.Debug("user is not exist");
                throw new Exception("Not existing user!");
            }
        }
        /// <summary>
        /// Getter for a user by its email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User object - the user with the email</returns>
        public User getUser(string email)
        {
            if(isExist(email))
                return _users[email];
            return null;
        }
        /// <summary>
        /// method for checking if user is logged in
        /// </summary>
        /// <param name="email"></param>
        /// <returns>boolean- representing if user is logged in or not</returns>
        public bool isLogged(string email)
        {
            email.ToLower();
            if (isExist(email))
            {
                User u = getUser(email);
                return u.getIsLogged();
            }
            return false;
        }
        /// <summary>
        /// method for checking if a user exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns>boolean-representing if a user exist or not</returns>
        public bool isExist(string email)
        {
            return _users.ContainsKey(email);
        }
        /// <summary>
        /// method for deleting all user 
        /// </summary>
        public void cleanUsers()
        {
            _users.Clear();
        }

        /// <summary>
        /// Method to recive all users emails
        /// </summary>
        /// <returns>List of all user emails</returns>
        public List<string> getAllUsersEmails()
        {
            List<string> emails = new List<string>();
            foreach (KeyValuePair<string, User> entry in _users)
            {
                emails.Add(entry.Value.Email);
            }
            return emails;
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void LoadData()
        {
            List<UserDTO> userDTOs = _usersDalController.SelectAllUsers();
            foreach(UserDTO user in userDTOs)
            {
                User u = new User(user);
                _users[u.Email] = u;
            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public void DeleteData()
        {
            _users.Clear();
        }


        /// <summary>
        /// delete all data from the DataBase
        /// </summary>
        /// <returns></returns>
        public void DeleteDataDB()
        {
            _usersDalController.DeleteDataDB();
        }
    }

}
