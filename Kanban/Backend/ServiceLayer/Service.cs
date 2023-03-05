using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Service
    {
        private readonly UserService _userService;
        private readonly BoardService _boardService;
        private readonly TaskService _taskService;
        public Service()
        {
            _userService = new UserService();
            _boardService = _userService.GetBoardService();
            _taskService = _boardService.GetTaskService();
            var options = new JsonSerializerOptions { WriteIndented = true };
        }

        public UserService getUs()
        {
            return _userService;
        }
        public BoardService getBs()
        {
            return _boardService;
        }
        public TaskService getTs()
        {
            return _taskService;
        }


        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            string response = JsonSerializer.Serialize(_userService.Register(email, password), new JsonSerializerOptions { WriteIndented = true });
            return response;
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            string reponse = JsonSerializer.Serialize(_userService.Login(email, password), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email)
        {
            string response = JsonSerializer.Serialize(_userService.Logout(email), new JsonSerializerOptions { WriteIndented = true });
            return response;
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            string reponse = JsonSerializer.Serialize(_boardService.LimitColumn(email, boardName, columnOrdinal, limit), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            string reponse = JsonSerializer.Serialize(_boardService.GetColumnLimit(email, boardName, columnOrdinal), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            string reponse = JsonSerializer.Serialize(_boardService.GetColumnName(email, boardName, columnOrdinal), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            string reponse = JsonSerializer.Serialize(_taskService.AddTask(email, boardName, title, description, dueDate), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            string response = JsonSerializer.Serialize(_taskService.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate), new JsonSerializerOptions { WriteIndented = true });
            return response;
        }


        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            string reponse = JsonSerializer.Serialize(_taskService.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            string reponse = JsonSerializer.Serialize(_taskService.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            string response = JsonSerializer.Serialize(_taskService.AdvanceTask(email, boardName, columnOrdinal, taskId), new JsonSerializerOptions { WriteIndented = true });
            return response;
        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            string reponse = JsonSerializer.Serialize(_boardService.GetColumn(email, boardName, columnOrdinal), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddBoard(string email, string name)
        {
            string response = JsonSerializer.Serialize(_boardService.AddBoard(email, name), new JsonSerializerOptions { WriteIndented = true });
            return response;
        }


        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string RemoveBoard(string email, string name)
        {
            string response = JsonSerializer.Serialize(_boardService.RemoveBoard(email, name), new JsonSerializerOptions { WriteIndented = true });
            return response;
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            string reponse = JsonSerializer.Serialize(_taskService.InProgressTasks(email), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetUserBoards(string email)
        {
            string reponse = JsonSerializer.Serialize(_boardService.GetUserBoards(email), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            string reponse = JsonSerializer.Serialize(_boardService.GetBoardName(boardId), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string JoinBoard(string email, int boardID)
        {
            string reponse = JsonSerializer.Serialize(_boardService.JoinBoard(email, boardID), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            string reponse = JsonSerializer.Serialize(_boardService.LeaveBoard(email, boardID), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            string reponse = JsonSerializer.Serialize(_taskService.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadData()
        {
            string reponse = JsonSerializer.Serialize(_userService.LoadData(), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteData()
        {
            string reponse = JsonSerializer.Serialize(_userService.DeleteData(), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }


        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            string reponse = JsonSerializer.Serialize(_boardService.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }

        /// <summary>
        /// This method gets all of user boards
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetAllUserBoards(string email)
        {
            string reponse = JsonSerializer.Serialize(_boardService.GetAllUserBoards(email), new JsonSerializerOptions { WriteIndented = true });
            return reponse;
        }
    }
}
