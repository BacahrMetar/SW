using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private readonly UserController uc;
        private readonly BoardService bs;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserService()
        {
            uc = new UserController();
            bs = new BoardService(new BoardController(uc));
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting UserService log!");
        }

        public BoardService GetBoardService()
        {
            return bs;
        }

        /// <summary>
        /// This function tests Requirement 7
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> Register(string email, string password)
        {
            try
            {
                uc.Register(email, password);
                log.Debug(email+" was registered!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);           
                return new Response<string>(e); 
            }
        }
        /// <summary>
        /// This function tests Requirement 8
        /// This method login a user to the system 
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>Response with user email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> Login(string email, string password)
        {
            try
            {
                uc.Login(email, password);
                log.Debug(email+" was logged in!");
                return new Response<string>(email);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }
        /// <summary>
        /// This function tests Requirement 8
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> Logout(string email)
        {
            try
            {
                uc.Logout(email);
                log.Debug(email+" was Logged out!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }
        /// <summary>
        /// this function does not test a specific requirement
        /// This method returns a specific user. 
        /// </summary>
        /// <param name="email">The email of the user we want to return</param>
        /// <returns>User object with suitable email that was given, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public User GetUser(string email)
        {
            log.Debug("Attepting to get user "+email);
            if (email == null)
                throw new Exception("Null email!");
            User res = uc.getUser(email);
            if (res==null)
                throw new Exception("User not existing!");
            log.Debug("User was returned succesfuly!");
            return res;
        }

        /// <summary>
        /// this function does not test a specific requirement
        /// This method cleans all users in project and all boards a specific user.
        /// Used for tests.
        /// </summary>
        /// <returns>Response with user email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void cleanUsers()
        {
            uc.cleanUsers();
            bs.cleanBoards();
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> LoadData()
        {
            try
            {
                uc.LoadData();
                bs.LoadData();
                return new Response<string>();
            }
            catch(Exception e)
            {
                return new Response<string>(e);
            }
            

        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        ///
        public Response<string> DeleteData()
        {
            try
            {
                DeleteDataDB();
                uc.DeleteData();
                bs.DeleteData();
                
                return new Response<string>();
            }
            catch(Exception e)
            {
                return new Response<string>(e);
            }
            
        }

        /// <summary>
        /// delete all data from the DataBase
        /// </summary>
        /// <returns></returns>
        public Response<string> DeleteDataDB()
        {
            uc.DeleteDataDB();
            return new Response<string>();
        }



    }
}
