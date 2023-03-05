using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BackendController
    {
        private Service gs;
        public BackendController(Service gradingService)
        {
            this.gs = gradingService;
        }
        public BackendController()
        {
            this.gs = new Service();
            gs.LoadData();
        }

        /// <summary>
        /// Function to log in the user. Calls backend function.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public UserModel Login(string email, string password)
        {

            String login = gs.Login(email, password);


            Response<string> emailRes = JsonSerializer.Deserialize<Response<string>>(login);

            if (!string.IsNullOrEmpty(emailRes.ErrorMessage))

            {
                throw new Exception(emailRes.ErrorMessage);
            }

            return new UserModel(this, emailRes.ReturnValue);
        }

        /// <summary>
        /// Function to register a new user. Calls backend function.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public UserModel Register(string email, string password)

        {
            String register = gs.Register(email, password);

            Response<string> registerRes = JsonSerializer.Deserialize<Response<string>>(register);

            if (!string.IsNullOrEmpty(registerRes.ErrorMessage))

            {
                throw new Exception(registerRes.ErrorMessage);
            }

            return new UserModel(this, email);

        }

        /// <summary>
        ///  Function to log out the user. Calls backend function.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <exception cref="Exception"></exception>
        public void Logout(string email)
        {
            String logout = gs.Logout(email);

            Response<string> logoutRes = JsonSerializer.Deserialize<Response<string>>(logout);

            if (!string.IsNullOrEmpty(logoutRes.ErrorMessage))

            {
                throw new Exception(logoutRes.ErrorMessage);
            }

        }
        /// <summary>
        /// Function to get all user's boards
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns> Collection of all user boards </returns>
        /// <exception cref="Exception"></exception>
        public ObservableCollection<BoardModel> GetAllUserBoards(string email)
        {
            string Boards = gs.GetAllUserBoards(email);
            Response<List<Board>> ResBoards = JsonSerializer.Deserialize<Response<List<Board>>>(Boards);
            if (!string.IsNullOrEmpty(ResBoards.ErrorMessage))
            {
                throw new Exception(ResBoards.ErrorMessage);
            }
            List<Board> boards = ResBoards.ReturnValue;
            ObservableCollection<BoardModel> boardModels =new ObservableCollection<BoardModel>();
            foreach (Board board in boards)
            {
                BoardModel b = new BoardModel(this, board);
                boardModels.Add(b);
            }
            return boardModels;

        }







    }
}
