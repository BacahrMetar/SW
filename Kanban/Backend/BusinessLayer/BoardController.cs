using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

[assembly: InternalsVisibleTo("NUnitTestProject1")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardController
    {
        private Dictionary<string, Dictionary<int, Board>> Boards = new Dictionary<string, Dictionary<int, Board>>();// email,<boardname , Board>
        private int _taskId; //counter of task id, must be next id for a new task
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly UserController uc;
        private int _boardId; //counter of board id, must be next id for new board
        private const int ILLEGAL_ID= -1;
        private const string BACKLOG_COLUMN_NAME = "backlog";
        private const string INPROGRESS_COLUMN_NAME = "in progress";
        private const string DONE_COLUMN_NAME = "done";

        private BoardDalController _boardsDalController;
        private UserBoardMapper _userBoardMapper;
        private TaskDalController _taskDalController;
        private ColumnDalController _columnDalController;
        

        /// <summary>
        /// Constructor to the BoardController
        /// </summary>
        /// <param name="uc"></param>
        public BoardController(UserController uc)
        {
            Boards = new Dictionary<string, Dictionary<int, Board>>();
            _taskId = 0; //should get it from data
            _boardId = 0; //should get it from data
            this.uc = uc;
            this._boardsDalController = new BoardDalController();
            this._userBoardMapper = new UserBoardMapper();
            this._taskDalController = new TaskDalController();
            this._columnDalController = new ColumnDalController();
        }
        /// <summary>
        /// Method to check if user exist
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>boolean - representing if a user exist</returns>
        public bool checkExist(string email)
        {
            if (email != null)
            {
                email = email.ToLower();
            }
            return uc.isExist(email);
        }
        /// <summary>
        /// Method to check if a user is logged in
        /// </summary>
        /// <param name="email"></param>
        /// <returns>boolean - representing if the user exist</returns>
        public bool checkIsLoggedIn(string email)
        {
            if (email != null)
            {
                email = email.ToLower();
            }
            return uc.isLogged(email);
        }
        /// <summary>
        /// Method for adding board to user (The user will own the board).
        /// </summary>
        /// <param name="email">The email of the user we add the board to</param>
        /// <param name="boardName">The board's name to add</param>
        /// <exception cref="Exception"></exception>
        public void AddBoard(string email, string boardName)
        {
            log.Debug("User "+email+" attempts to add board named "+boardName);
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to add board");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Non existing user tried to remove board");
                throw new Exception("User does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Non logged in user tried to add board");
                throw new Exception("User isnt logged in");
            }

            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User attempted to add board with null board name");
                throw new Exception("Board name is null");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Debug("User doesnt have any boards so we create empty board set");
                Boards[email] = new Dictionary<int, Board>();
            }

            int boardID = GetBoardIDbyName(email, boardName);

            if (boardID!=ILLEGAL_ID)
            {
                log.Error("User trying to add an already exist board name");
                throw new Exception("User already has this Board name");
            }

            try
            {
                BoardDTO boardDTO = new BoardDTO(_boardId, boardName, email);
                Board b = new Board(boardDTO);
                _boardsDalController.Insert(b.DTO); //insert to boards table
               // UserBoardDTO userBoard = new UserBoardDTO(b.ID, email);
               // _userBoardMapper.Insert(userBoard); //insert to user - boards mapper table
                Boards[email].Add(b.ID, b); //added to ram
                b.Owner = email; //added to ram
                log.Debug("User added board with id "+ _boardId + " successfuly. Now the user is the owner of the board.");
            }
            catch(Exception e)
            {
                log.Error("User attempted to add board but falid!");
                throw new Exception(e.Message);
            }
            _boardId++;
            log.Debug("Id counter was increased to "+ _boardId);

        }


        /// <summary>
        /// Method for reciving board id by board name.
        /// </summary>
        /// <param name="email">The name we want to check if already exist</param>
        /// <param name="boardName">The name we need to get the id</param>
        /// <returns>Id of the board if user has it, else -1</returns>
        /// <exception cref="Exception"></exception>
        private int GetBoardIDbyName(string email, string boardName)
        {
            if (email != null)
            {
                email = email.ToLower();
            }
            Dictionary<int, Board> userBoards = GetBoards(email);
            foreach (KeyValuePair<int, Board> entry in userBoards)
            {
                Board b = entry.Value;
                if (b.Name.Equals(boardName))
                    return b.ID;
            }
            return ILLEGAL_ID;
        }


        /// <summary>
        /// Method for removing a board from a user, all tasks and colums will be deleted to.
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="boardName">The boards name we want to remove</param>
        /// <exception cref="Exception"></exception>
        public void RemoveBoard(string email, string boardName)
        {
            
            log.Debug("User " + email + " attempts to remove board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to remove board");
                throw new Exception("Email is null / empty / white spaces only");
            }

            email = email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Non existing user tried to remove board");
                throw new Exception("user does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Non logged in user tried to remove board");
                throw new Exception("User isnt logged in");
            }

            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User attempted to remove board with null board name");
                throw new Exception("BoardName is null");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards tried to remove board");
                throw new Exception("User does not have boards to remove!");
            }

            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Error("User attempted to remove non existing board");
                throw new Exception("Board with this name "+boardName+" doesnt exist for the user "+email+"!");
            }

            Board b = Boards[email][boardid];

            if (!b.IsOwner(email))
            {
                log.Error("User that isnt owner of board tried to remove it");
                throw new Exception("User is not owner of board. Must be owner to remove!");
            }
            try
            {
                DeleteBoardFromData(b.ID);
                List<string> boardMembers=b.Members;
                foreach (string user in boardMembers)
                {
                    Boards[user].Remove(boardid);
                }
                log.Debug("Deleted the board from all members successfuly!");
                
                Boards[email].Remove(boardid);
                log.Debug("Deleted the board from owner successfuly!");
            }
            catch(Exception e)
            {
                log.Error("User attempted to remove board but falid!");
                throw new Exception(e.Message);
            }
            

        }

        /// <summary>
        /// Deletes board from all relevent tables
        /// </summary>
        /// <param name="boardId">The board id</param>
        public void DeleteBoardFromData(int boardId)
        {
            _boardsDalController.DeleteBoard(boardId); //delete board from Boards table
            _userBoardMapper.DeleteBoard(boardId); //delete board from UserBoards table
            _columnDalController.DeleteBoard(boardId); //delete board from Columns table
            _taskDalController.DeleteBoard(boardId); //delete board from Tasks table
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// It also removes the board from the user.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <exception cref="Exception"></exception>
        public void LeaveBoard(string email, int boardID)
        {
            
            log.Debug("User " + email + " attempts to leave board of id " + boardID);

           

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to leave board");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Non existing user try to leave board");
                throw new Exception("user does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Non logged in user try to leave board");
                throw new Exception("User isnt logged in");
            }

            if (boardID < 0)
            {
                log.Error("User attempted to leave board with illegal id");
                throw new Exception("The board id isnt legal");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards tried to leave board");
                throw new Exception("User does not have boards!");
            }

            if (!Boards[email].ContainsKey(boardID))
            {
                log.Error("User attempted to leave a board he doesnt have");
                throw new Exception("User does not have the board!");
            }

            Board b = Boards[email][boardID];

            if (b.IsOwner(email))
            {
                log.Error("The owner of the board attempted to leave it");
                throw new Exception("Owner of the board cant leave it");
            }
            try
            {
                //Boards[email].Remove(boardID);
                //log.Debug("Deleted the board from the user successfuly!");  
                b.UnAssignAllTasks(email);
                log.Debug("All users assigned tasks in the board were un assigned.");
                b.RemoveMember(email);
                log.Debug("The user was removed from the board member list successfuly!");
                Boards[email].Remove(boardID);
                log.Debug("Deleted the board from the user successfuly!");
            }
            catch(Exception e)
            {
                log.Error("User attempted to leave board but falid!");
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// Method to receive all user's in progress Tasks (only which he is assigned to)
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <returns>List - of tasks representing all InProgress tasks of a user</returns>
        /// <exception cref="Exception"></exception>
        public List<Task> InProgressTasks(string email)
        {
         
            log.Debug("User " + email + " attempts to get all his in-progress tasks");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to get in progress tasks of a board");
                throw new Exception("Email is null / empty / white spaces only");
            }
                email = email.ToLower();
           
            if (!checkExist(email))
            {
                log.Error("Non existing user tried to get In Progress Tasks");
                throw new Exception("user does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Non logged in user tried to get In Progress Tasks");
                throw new Exception("User isnt logged in");
            }
            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards tried to get In Progress Tasks");
                throw new Exception("User does not have boards!");
            }
            try
            {
                List<Task> res = new List<Task>();
                Dictionary<int, Board> userBoards = GetBoards(email);
                if (userBoards == null)
                    return res; //returns empty task list
                foreach (KeyValuePair<int, Board> entry in userBoards)
                {
                    Board b = entry.Value;
                    List<Task> inProgressTasks = b.GetInProgress();
                    res = res.Concat(inProgressTasks).ToList();
                    //check which tasks the user assigned to
                    foreach(Task t in res)
                    {
                        if (!t.Assignee.Equals(email))
                        {
                            res.Remove(t);
                        }
                    }
                }
                log.Debug("The user got all in progress tasks successfuly!");
                return res;
            }
            catch (Exception e)
            {
                log.Error("User attempted to get his in progress tasks but falid!");
                throw new Exception(e.Message);
            }
        }

    

        /// <summary>
        /// Method for getting a specific column in a users board
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="boardName">The board's name</param>
        /// <param name="columnOrdinal">The column ordinal</param>
        /// <returns> Column- representing the desired column</returns>
        /// <exception cref="Exception"></exception>
        public Column GetColumn(string email, string boardName, int columnOrdinal)
        {
           
            log.Debug("User " + email + " attempts to get column "+columnOrdinal+" from board named "+boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to get column of board");
                throw new Exception("Email is null / empty / white spaces only");
            }
            
            email = email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Not existing User attempted get a column");
                throw new Exception("User does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Not Logged in user attempted get a column");
                throw new Exception("User isnt logged in");
            }

            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to get column of board");
                throw new Exception("Board name is null / empty / white spaces only");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards attempted get a column");
                throw new Exception("User does not have boards!");
            }

            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Error("User attempted to get a column from board he doesnt have");
                throw new Exception("Board with this name doesnt exist for the user!");
            }
            try
            {
                Board board = Boards[email][boardid];
                Column col = board.GetColumn(columnOrdinal);
                log.Debug("The user got the column successfuly!");
                return col;
            }
            catch (Exception e)
            {
                log.Error("User attempted to get his in progress tasks but falid!");
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Method to get a column name by its ordinal and board name
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="ordinal">The ordinal of the wanted column</param>
        /// <returns>string- Representing the column name</returns>
        /// <exception cref="Exception"></exception>
        public string GetColumnName(string email, string boardName, int ordinal)
        {
           
            log.Debug("User " + email + " attempts to get " + ordinal + " column name from board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to get column name");
                throw new Exception("Email is null / empty / white spaces only");
            }
            
            email = email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Non existing user try to get a column name");
                throw new Exception("User does not exist");
            }
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board attempted to get column name");
                throw new Exception("Board name is null / empty / white spaces only");
            }

            if (!Boards.ContainsKey(email))
            {
                throw new Exception("User does not have boards!");
            }

            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Error("User tried to get a column name from board he doesnt have  ");
                throw new Exception("board does not exist");
            }
            if (ordinal < 0 || ordinal > 2)
            {
                log.Error("User tried to get a column name with illegal column ordinal ");
                throw new Exception("Ordinal value cant be " + ordinal);
            }

            try
            {
                Board board = Boards[email][boardid];
                string columnName = board.GetColumnName(ordinal);
                log.Debug("The user got column name successfuly!");
                return columnName;
            }
            catch (Exception e)
            {
                log.Error("User attempted to get a column name but falid!");
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Method for getting a column's limit
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="boardName">The board name</param>
        /// <param name="columnOrdinal">The ordinal of the wanted column</param>
        /// <returns>int - Representing the column limit</returns>
        /// <exception cref="Exception"></exception>
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
         
            log.Debug("User " + email + " attempts to get " + columnOrdinal + " column limit from board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to get column limit");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("Not existing user attempted get a column limit");
                throw new Exception("User does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user attempted get a column limit");
                throw new Exception("User isnt logged in");
            }
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to get column limit");
                throw new Exception("Board name is null / empty / white spaces only");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards attempted get a column limit");
                throw new Exception("User does not have boards!");
            }

            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID) 
            { 
                log.Debug("User attempted get a column limit from board he doesnt have");
                throw new Exception("Board with this name doesnt exist!");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                log.Error("User tried to get a column limit with illegal column ordinal ");
                throw new Exception("Ordinal value cant be " + columnOrdinal);
            }
            try
            {
                Board board = Boards[email][boardid];
                int limit = board.GetColumnLimit(columnOrdinal);
                log.Debug("The user got column limit successfuly!");
                return limit;
            }
            catch (Exception e)
            {
                log.Error("User attempted to get a column limit but falid!");
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Method for limiting the column size
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="boardName">The board name</param>
        /// <param name="columnOrdinal">The column ordinal</param>
        /// <param name="limit">The new limit we would like to set</param>
        /// <exception cref="Exception"></exception>
        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            
            log.Debug("User " + email + " attempts to limit " + columnOrdinal + " column from board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to limit column");
                throw new Exception("Email is null / empty / white spaces only");
            }
            
            email = email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Not existing user attempted Limit a column ");
                throw new Exception("User does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user attempted Limit a column ");
                throw new Exception("User isnt logged in");
            }
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to limit column");
                throw new Exception("Board name is null / empty / white spaces only");
            }
            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards attempted Limit ac olumn ");
                throw new Exception("User does not have boards!");
            }
            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID) 
            { 
                log.Error("User attempted to Limit a column from board he doesnt have");
                throw new Exception("Board with this name doesnt exist!");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                log.Error("User attempted to limit non existing column");
                throw new Exception("Column ordinal not legal");
            }
            
            try
            {
                Boards[email][boardid].LimitColumn(columnOrdinal, limit);
                log.Debug("Column limit updated successfuly to "+limit);
            }
            catch (Exception e)
            {
                log.Debug("User attepmted to limit a column but failed");
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Method for adding a new task to a board
        /// </summary>
        /// <param name="email">The user's email (must be owner\member of the board)</param>
        /// <param name="boardName">The board's name</param>
        /// <param name="title">The task's title, must be for most length of 50</param>
        /// <param name="description">The task's description, must be for most length of 300, can be null (if description doesnt wanted)</param>
        /// <param name="dueDate">The task's due date, must be in future</param>
        /// <exception cref="Exception"></exception>
        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
          
            log.Debug("User " + email + " attempts to add new task to board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to add task");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("Not existing user attempted to add task");
                throw new Exception("User does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user attempted to add task");
                throw new Exception("User isnt logged in");
            }

            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to add task");
                throw new Exception("Email is null / empty / white spaces only");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no board attempted to add task");
                throw new Exception("User does not have boards!");
            }

            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Debug("User attempted to add task to board he doesnt have");
                throw new Exception("Board with this name doesnt exist!");
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrEmpty(title))
            {
                log.Error("User with null / empty / white spaces only, title attempted to add task");
                throw new Exception("Title is null / empty / white spaces only");
            }

            if (description==null || (description.Length!=0 && string.IsNullOrWhiteSpace(description)))
            {
                log.Error("User with null / empty / white spaces only, description attempted to add task");
                throw new Exception("Description is null / empty / white spaces only");
            }

            try
            {
                Task t = new Task(title, description, dueDate, _taskId,boardid);
                _taskDalController.Insert(t.DTO); //adding the task to data
                Boards[email][boardid].AddTask(t); //add task to RAM
                log.Debug("New task added successfuly to board");
                _taskId++;
            }
            catch (Exception e)
            {
                log.Debug("User tried to add new task but faild");
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// Method for updating a task's title
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The wanted task id</param>
        /// <param name="title">The new title to update</param>
        /// <exception cref="Exception"></exception>
        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
          
            log.Debug("User " + email + " attempts to update task with id "+taskId+" title in " + columnOrdinal + " column from board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to update task title");
                throw new Exception("Email is null / empty / white spaces only"); 
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("Not exisitng user attempted to update tasks title");
                throw new Exception("User does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user attempted to update tasks title");
                throw new Exception("User isnt logged in");
            }
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to update task title");
                throw new Exception("Board name is null / empty / white spaces only");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards attempted to updates tasks title");
                throw new Exception("User does not have boards!");
            }

            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Error("User attempted to updates tasks title from board he doesnt have");
                throw new Exception("Board with this name doesnt exist!");
            }
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrEmpty(title))
            {
                log.Error("User with null / empty / white spaces only title attempted to update task title");
                throw new Exception("Title is null / empty / white spaces only");
            }

            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                log.Error("User attempted to update task title in non existing column");
                throw new Exception("Column ordinal not legal");
            }

            if (taskId < 0)
            {
                log.Error("User attempted to update task title with illegal task id");
                throw new Exception("Illegal task id");
            }

            Board board = Boards[email][boardid];

            try
            {
                Task t = board.GetTask(taskId);
                t.UpdateTitle(title,email); //if user is not assignee he wouldnt be able to update
                log.Debug("User successfuly updated task title");

            }
            catch (Exception e)
            {
                log.Error("User attempted to update task title but failed");
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// Method for updating task's description
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The wanted task id</param>
        /// <param name="description">The new description, can be null</param>
        /// <exception cref="Exception"></exception>
        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
           
            log.Debug("User " + email + " attempts to update task with id " + taskId + " description in " + columnOrdinal + " column from board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to update task description");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("Not exsiting user attempted to update tasks description");
                throw new Exception("user does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user  attempted to update tasks description");
                throw new Exception("User isnt logged in");
            }
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to update task description");
                throw new Exception("Board name is null / empty / white spaces only");
            }
            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards attempted to updates tasks description ");
                throw new Exception("User does not have boards!");
            }
            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Error("User attempted to updates tasks description for board he doesnt have");
                throw new Exception("Board with this name doesnt exist!");
            }

            if (description == null || (description.Length != 0 && string.IsNullOrWhiteSpace(description)))
            {
                log.Error("User with null / empty / white spaces only, description attempted to update description");
                throw new Exception("Description is null / empty / white spaces only");
            }

            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                log.Error("User attempted to update task description from non existing column");
                throw new Exception("Column ordinal not legal");
            }

            if (taskId < 0)
            {
                log.Error("User attempted to update task description with illegal task id");
                throw new Exception("Illegal task id");
            }
            
            Board board = Boards[email][boardid];
            try
            {
                Task t = board.GetTask(taskId);
                t.UpdateDescription(description,email);//if user is not assignee he wouldnt be able to update
                log.Debug("User successfuly updated task description");
            }
            catch (Exception e)
            {
                log.Error("User attempted to update task description but failed");
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// Method for updating task's due date
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The wanted task id</param>
        /// <param name="dueDate">The new due date</param>
        /// <exception cref="Exception"></exception>
        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
          
            log.Debug("User " + email + " attempts to update task with id " + taskId + " due date in " + columnOrdinal + " column from board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to update task due date");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("Not exsiting user attempted to update tasks due date");
                throw new Exception("User does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user attempted to update tasks due date");
                throw new Exception("User isnt logged in");
            }
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to update task due date");
                throw new Exception("Board name is null / empty / white spaces only");
            }
            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards attempted to update tasks due date");
                throw new Exception("User does not have boards!");
            }

            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Error("User attempted to update tasks due date from not exisitng board name");
                throw new Exception("Board with this name doesnt exist!");
            }

            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                log.Error("User attempted to update task due date from non existing column ");
                throw new Exception("Column ordinal not legal");
            }
            if (taskId < 0)
            {
                log.Error("User attempted to update task due date with illegal task id");
                throw new Exception("Illegal task id");
            }
            
            Board board = Boards[email][boardid];

            try
            {
                Task t = board.GetTask(taskId);
                t.UpdateTaskDueDate(dueDate,email);//if user is not assignee he wouldnt be able to update
                log.Debug("User successfuly updated task due date");
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Method to advance a task
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The wanted task id</param>
        /// <exception cref="Exception"></exception>
        public void AdvanceTask(string email, string boardName, int columnOrdinal , int taskId)
        {
          
            log.Debug("User " + email + " attempts to advance task with id " + taskId + " in " + columnOrdinal + " column from board named " + boardName);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to advance task");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("non existing User attempted to advance task ");
                throw new Exception("user does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in User attempted to advance task ");
                throw new Exception("User isnt logged in");
            }
            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to advance task");
                throw new Exception("Board name is null / empty / white spaces only");
            }
            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards attempted to advance task");
                throw new Exception("User does not have boards!");
            }
            int boardid = GetBoardIDbyName(email, boardName);

            if (boardid == ILLEGAL_ID)
            {
                log.Error("User attempted to advance task fron not exsiitng board name");
                throw new Exception("Board with this name doesnt exist!");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                log.Error("User attempted to advance task from non existing column");
                throw new Exception("Column ordinal not legal");
            }
            if (taskId < 0)
            {
                log.Error("User attempted to advance task with Illegal task id");
                throw new Exception("Illegal task id");
            }
            
            Board board = Boards[email][boardid];

            try
            {
                Task t = board.GetTask(taskId);
                if (t.Assignee==null || !t.Assignee.Equals(email))
                {
                    log.Error("User that is not assignee attempted to advance task");
                    throw new Exception("User that is not assignee cant advance tasks ");
                }
                string colName = t.GetColumName();

                if (colName.Equals(DONE_COLUMN_NAME))
                {
                    log.Error("User attempted to advance task from Done column");
                    throw new Exception("Cannot advance from Done column ");
                }
                else if (colName.Equals(BACKLOG_COLUMN_NAME))
                {
                    Column col = board.GetColumn(INPROGRESS_COLUMN_NAME);
                    if (col.isFull())
                    {
                        log.Error("User attempted to advance task to a full column");
                        throw new Exception("Column " + col.GetColumnName() + " is already full! limit= " + col.GetTasksLimit());
                    }
                    else
                    {
                        t.DTO.ColumnOrdinal = 1; //update task's column ordinal in data base
                        board.MoveTask(t, INPROGRESS_COLUMN_NAME);
                        t.SetColumnName(INPROGRESS_COLUMN_NAME);
                        board.RemoveTask(t, BACKLOG_COLUMN_NAME);
                        log.Debug("User successfuly advanced task from "+ BACKLOG_COLUMN_NAME + " column to "+INPROGRESS_COLUMN_NAME+" column.");
                    }
                }
                else
                {
                    Column col = board.GetColumn(DONE_COLUMN_NAME);
                    if (col.isFull())
                    {
                        log.Error("User attempted to advance task to a full column"); ;
                        throw new Exception("Column " + col.GetColumnName() + " is already full! limit= " + col.GetTasksLimit());
                    }
                    else
                    {
                        t.DTO.ColumnOrdinal = 2; //update task's column ordinal in data base
                        board.MoveTask(t, DONE_COLUMN_NAME);
                        t.SetColumnName(DONE_COLUMN_NAME);
                        board.RemoveTask(t, INPROGRESS_COLUMN_NAME);
                        log.Debug("User successfuly advanced task from " + INPROGRESS_COLUMN_NAME + " column to " + DONE_COLUMN_NAME + " column.");
                    }
                }

            }
            catch (Exception e)
            {
                log.Error("User attempted to advance task but faild");
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Method for getting dictionary of boards of a specific user
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Dictionary - representing the user's dictionary of boards</returns>
        /// <exception cref="Exception"></exception>
        public Dictionary<int, Board> GetBoards(string email)
        {
           
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to get his boards");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("Not existing user attempted to get boards");
                throw new Exception("User does not exist");
            }
            
            if (!Boards.ContainsKey(email))
                return new Dictionary<int, Board>(); //doesnt have any boards

            return Boards[email];
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>List of users boards</returns>
        public List<int> GetUserBoards(string email)
        {
           
            log.Debug("User " + email + " attempts to get all his boards ids");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to get his boards id list");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email = email.ToLower();
            
            if (!checkExist(email))
            {
                log.Error("Not existing user attempted to get boards id list");
                throw new Exception("user does not exist");
            }
            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user attempted to get boards id list ");
                throw new Exception("User isnt logged in");
            }
            if (!Boards.ContainsKey(email))
                return new List<int>(); //user doesnt have boards
            else
            {
                Dictionary<int, Board> boards = GetBoards(email);
                List<int> boardsIDs = new List<int>();
                foreach (KeyValuePair<int, Board> entry in boards)
                {
                    Board b = entry.Value;
                    boardsIDs.Add(b.ID);
                }
                log.Debug("User " + email + " successuly got all his boards ids");
                return boardsIDs;
            }
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>string representing the board's name (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            log.Debug("User attempts to get board with id "+boardId+" name");

            if (boardId < 0)
            {
                log.Error("Tried to get board with illegal id");
                throw new Exception("Cant have board with id " + boardId);
            }
            if (boardId > _boardId)
            {
                log.Error("Tried to get board id greater than highest id");
                throw new Exception("board with id " + boardId + " doesnt exist");
            }
            try
            {
                Board b = FindBoardByID(boardId);
                if(b == null)
                {
                    log.Error("Tried to get board with id, but it doesnt exist");
                    throw new Exception("board with id " + boardId + " doesnt exist");
                }
                string boardName = b.Name;
                log.Debug("User recived board name successfully");
                return boardName;
            }
            catch(Exception e)
            {
                log.Error("User attempted to recive board name but failed");
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// Returns the board with the selected id
        /// </summary>
        /// <param name="boardId">The wanted board id</param>
        /// <returns>Returns the board with the selected id</returns>
        public Board FindBoardByID(int boardId)
        {
            List<string> emails = uc.getAllUsersEmails();
            foreach (string email in emails)
            {
                if (Boards.ContainsKey(email))
                {
                    if (Boards[email].ContainsKey(boardId))
                    {
                        return Boards[email][boardId];
                    }
                }
            }
            return null;
        }
                

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void JoinBoard(string email, int boardID)
        {
            
            log.Debug("User " + email + " attempts to join board "+boardID);
            

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null / empty / white spaces only, email attempted to get join board");
                throw new Exception("Email is null / empty / white spaces only");
            }
            email=email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Not existing user try to join board");
                throw new Exception("user does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Not logged in user try to join board");
                throw new Exception("User isnt logged in");
            }

            if (boardID < 0)
            {
                log.Error("User attempted join board with illegal id");
                throw new Exception("Illegal id");
            }
            Board board = FindBoardByID(boardID);
            if (board == null)
            {
                log.Error("User attempted join board that not exist");
                throw new Exception("Board not exist.");
            }
            try
            {
                if (!Boards.ContainsKey(email)) //if user doesnt have any boards
                {
                    Boards[email] = new Dictionary<int, Board>();
                    Boards[email][boardID] = board;
                    log.Debug("Board was added to user");
                    board.AddMember(email);
                    log.Debug("User was added to boards member list");
                    log.Debug("User joined board successfuly");
                }
                else
                {
                    log.Debug("Check if user already has a board with same name");
                    string boardName = GetBoardName(boardID);
                    foreach (KeyValuePair<int, Board> entry in Boards[email])
                    {
                        Board b = entry.Value;
                        if (b.Name.Equals(boardName))
                        {
                            log.Error("User attempted join board with name he already has");
                            throw new Exception("User already has a board with name " + boardName);
                        }
                    }
                    Boards[email][boardID] = board;
                    log.Debug("Board was added to user");
                    board.AddMember(email);
                    log.Debug("User was added to boards member list");
                    log.Debug("User joined board successfuly");
                }
            }

            catch (Exception e)
            {
                log.Error("User attempted to join board but failed");
                throw new Exception(e.Message);
            }

        }
               
           

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {

           
            log.Debug("User " + currentOwnerEmail + " attempts to transer ownership of board named, " + boardName + " to " + newOwnerEmail);

            if (string.IsNullOrWhiteSpace(currentOwnerEmail) || string.IsNullOrEmpty(currentOwnerEmail))
            {
                log.Error("Current owner with null email attempted transfer ownership");
                throw new Exception("Current owner email is null");
            }

            currentOwnerEmail=currentOwnerEmail.ToLower();

            if (string.IsNullOrWhiteSpace(newOwnerEmail) || string.IsNullOrEmpty(newOwnerEmail))
            {
                log.Error("New owner with null email attempted transfer ownership");
                throw new Exception("New owner email is null");
            }

            newOwnerEmail = newOwnerEmail.ToLower();


            if (!checkExist(currentOwnerEmail))
            {
                log.Error("Not existing user try to transfer ownership");
                throw new Exception("Old owner does not exist");
            }

            if (!checkExist(newOwnerEmail))
            {
                log.Error("Not existing user try to be the owner of new board");
                throw new Exception("New owner does not exist");
            }

            if (!checkIsLoggedIn(currentOwnerEmail))
            {
                log.Error("Not logged in user try to transfer ownership of board");
                throw new Exception("Old owner isnt logged in");
            }

            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to transfer ownership of the board");
                throw new Exception("Board name is null / empty / white spaces only");
            }

            if (!Boards.ContainsKey(currentOwnerEmail))  
            {
                log.Error("User with no boards try to transfer ownership of board");
                throw new Exception("User doesnt have any boards to transfer");
            }

            int boardID = GetBoardIDbyName(currentOwnerEmail, boardName);

            if (boardID == ILLEGAL_ID)
            {
                log.Error("User attempted to transer ownership of a board he doesnt have");
                throw new Exception("User doesnt have board with name "+boardName);
            }

            Board b = Boards[currentOwnerEmail][boardID];

            if (!b.Owner.Equals(currentOwnerEmail))
            {
                log.Error("User attempted to transer ownership of a board he doesnt own");
                throw new Exception("User doesnt own board with name " + boardName);
            }

            if (!b.IsMember(newOwnerEmail))
            {
                log.Error("User attempted to transer ownership of a board to another user that is not joined to the board");
                throw new Exception("Cannot transfer ownership of a board to a user that is not member of the board");
            }

            b.Owner = newOwnerEmail;
            int boardid = b.ID;
            BoardDTO boardDTO = new BoardDTO(boardid, boardName, newOwnerEmail);
            boardDTO.Owner = newOwnerEmail;
            b.RemoveMember(newOwnerEmail);
            b.AddMember(currentOwnerEmail);

            log.Debug("Successfuly transfered ownership!");
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
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
           
            log.Debug("User " + email + " attempts to assign task "+taskID+" in column "+columnOrdinal+" in board " + boardName + " to " + emailAssignee);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null email attempted assign task to another user");
                throw new Exception("User email is null");
            }
            email = email.ToLower();
            

            if (string.IsNullOrWhiteSpace(emailAssignee) || string.IsNullOrEmpty(emailAssignee))
            {
                log.Error("User with null email attempted to be assigned to task");
                throw new Exception("New assignee email is null");
            }
            emailAssignee = emailAssignee.ToLower();

            if (!checkExist(email))
            {
                log.Error("Not existing user try to assign task to another user");
                throw new Exception("User does not exist");
            }

            if (!checkExist(emailAssignee))
            {
                log.Error("Not existing user try to be assigned to a task");
                throw new Exception("New assignee does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Non logged in user try to assign a task to other user");
                throw new Exception("User isnt logged in");
            }

            if (string.IsNullOrWhiteSpace(boardName) || string.IsNullOrEmpty(boardName))
            {
                log.Error("User with null / empty / white spaces only, board name attempted to assign task");
                throw new Exception("Board name is null / empty / white spaces only");
            }

            if (!Boards.ContainsKey(email))
            {
                log.Error("User with no boards try to assign a task to other user");
                throw new Exception("User doesnt have any boards so he cant assign tasks");
            }

            if (!Boards.ContainsKey(emailAssignee))
            {
                log.Error("User with no boards try to be assigned to a task");
                throw new Exception("User doesnt have any boards so he cant be assigned to tasks(Must be member of board)");
            }

            int boardID = GetBoardIDbyName(email, boardName);

            if (boardID == ILLEGAL_ID)
            {
                log.Error("User attempted to assgin task of a board he doesnt have");
                throw new Exception("User doesnt have board with name " + boardName);
            }

            Board b = Boards[email][boardID];

            if (!b.IsMember(emailAssignee) && !b.Owner.Equals(emailAssignee))
            {
                log.Error("User attempted to be assgined to a task of a board he doesnt have");
                throw new Exception("New assignee is not member of the board " + boardName);
            }

            Task t = b.GetTask(taskID);

            if (t.GetColumName().Equals(DONE_COLUMN_NAME))
            {
                log.Error("User attempted to be assigned to a done task.");
                throw new Exception("A done task can't be assigned");
            }

            if (t.Assignee == null)
            {
                t.DTO.Assignee = emailAssignee; //update in data
                t.Assignee = emailAssignee;
                log.Debug("Task was successfuly assigned!");
            }
            else
            {
                if (!t.Assignee.Equals(email))
                {
                    log.Error("User that is not assignee of a task attempted to assign it to another user.");
                    throw new Exception("User that is not assigne of the task, cant assign it");
                }
                t.DTO.Assignee = emailAssignee; //update in data
                t.Assignee = emailAssignee;
                log.Debug("Task was successfuly assigned!");
            }
        }

        /// <summary>
        /// method for earsing all board on the BoardController
        /// </summary>
        public void cleanBoards()
        {
            Boards.Clear();
            _taskId = 0;
            _boardId = 0;
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
            List<BoardDTO> boards = _boardsDalController.SelectAllBoards();
            foreach (BoardDTO board in boards)
            {
                Board b = new Board(board);
                string owner = b.Owner;
                if (!Boards.ContainsKey(owner)) //adds the board to owner in dictionary
                {
                    Boards.Add(owner, new Dictionary<int, Board>()); //if owner didnt have boards
                    Boards[owner][b.ID] = b;
                }
                else
                {
                    Boards[owner][b.ID] = b;
                }

                List<string> members = b.Members;
                foreach (string member in members) //adds the board to members in dictionary
                {
                    if (!Boards.ContainsKey(member)) //if member didnt have boards
                    {
                        Boards.Add(member, new Dictionary<int, Board>());
                        Boards[member][b.ID] = b;
                    }
                    else
                    {
                        Boards[member][b.ID] = b;
                    }

                }
            }
            int maxBoardInd=_boardsDalController.GetMaxIndex();
            if (maxBoardInd < 0)
            {
                _boardId = 0; //if no boards in data
            }
            else
            {
                _boardId = maxBoardInd + 1; //to assamble the next possible index for a board
            }
            int maxTaskInd = _taskDalController.GetMaxIndex();
            if (maxTaskInd < 0)
            {
                _taskId = 0; //if no boards in data
            }
            else
            {
                _taskId = maxTaskInd + 1; //to assamble the next possible index for a board
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
        public void DeleteData()
        {
            Boards.Clear();
            _taskId = 0;
            _boardId = 0;
        }

        /// <summary>
        /// This method gets all of user boards
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A list of user boards</returns>
        public List<Board> GetAllUserBoards(string email)
        {
            log.Debug("User " + email + " attempts to get all his boards");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                log.Error("User with null email attempted get all his boards");
                throw new Exception("User email is null");
            }
            email = email.ToLower();

            if (!checkExist(email))
            {
                log.Error("Not existing user try to get all his boards");
                throw new Exception("User does not exist");
            }

            if (!checkIsLoggedIn(email))
            {
                log.Error("Non logged in user try get all his boards");
                throw new Exception("User isnt logged in");
            }

            List<int> BoardsIds=GetUserBoards(email);
            List<Board> result=new List<Board>();
            foreach (int id in BoardsIds)
            {
                Board b = Boards[email][id];
                result.Add(b);
            }
            return result;
        }
    }
}


