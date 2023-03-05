using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private readonly BoardController bc;
        private readonly TaskService ts;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardService(BoardController bc)
        {
            this.bc = bc;
            this.ts = new TaskService(bc);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting BoardService log!");
        }

        /// <summary>
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> AddBoard(string email, string name)
        {
            try
            {
                bc.AddBoard(email, name);
                log.Debug("Adding Board was executed!");
                return new Response<String>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<String>(e);
            }
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> RemoveBoard(string email, string name)
        {
            try
            {
                bc.RemoveBoard(email, name);
                log.Debug("Removing Board was executed!");
                return new Response<String>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<String>(e);
            }
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<List<Task>> GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                Column c =bc.GetColumn(email, boardName, columnOrdinal);
                log.Debug("returned tasks in "+bc.GetColumnName(email, boardName, columnOrdinal)+" column!");
                return new Response<List<Task>>(c.GetTasks());
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<List<Task>>(e);
            }
        }

        /// <summary>
        /// this method doesnt test a specific Requirement 
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column name value, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string columnName =bc.GetColumnName(email, boardName, columnOrdinal);
                log.Debug("Column name was returned!");
                return new Response<String>(columnName);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<String>(e);
            }
        }

        /// <summary>
        /// this method doesnt tests a specific Requirement 
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column's ordinal. The first column is identified by 0,the second by 1 and the third by 2</param>
        /// <returns>Response with column limit value, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<int> GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int columnLimit =bc.GetColumnLimit(email, boardName, columnOrdinal);
                log.Debug("Column limit was returned!");
                return new Response<int>(columnLimit);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<int>(e);
            }
        }

      

        /// <summary>
        /// this method tests Requirement 10
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                bc.LimitColumn(email, boardName, columnOrdinal, limit);
                log.Debug("Limit Column was executed!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }


        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<List<int>> GetUserBoards(string email){
            try
            {
                List<int> boardIDs=bc.GetUserBoards(email);
                log.Debug("Get User Boards was executed!");
                return new Response<List<int>>(boardIDs);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<List<int>>(e);
            }
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> GetBoardName(int boardId)
        {
            try
            {
                string boardName=bc.GetBoardName(boardId);
                log.Debug("Get Board Name board was executed!");
                return new Response<string>(boardName);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> JoinBoard(string email, int boardID)
        {
            try
            {
                bc.JoinBoard(email, boardID);
                log.Debug("Join board was executed!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> LeaveBoard(string email, int boardID)
        {
            try
            {
                bc.LeaveBoard(email, boardID);
                log.Debug("Leave board was executed!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                bc.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                log.Debug("Transfer board ownershipwas executed!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }
        
        public void cleanBoards()
        {
            bc.cleanBoards();
        }

        public TaskService GetTaskService()
        {
            return this.ts;
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
                bc.LoadData();
                log.Debug("Load data executed!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
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
                bc.DeleteData();
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }

        /// <summary>
        /// This method gets all of user boards
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>Response with a list of user boards</returns>
        public Response<List<Board>> GetAllUserBoards(string email)
        {
            try
            {
                List<Board> allUserBoards=bc.GetAllUserBoards(email);
                return new Response<List<Board>>(allUserBoards);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<List<Board>>(e);
            }
        }

    }


}
