using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private readonly BoardController bc;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal TaskService(BoardController bc)
        {
            this.bc = bc;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting TaskService log!");
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
        public Response<string> AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                bc.AddTask(email, boardName, title, description, dueDate);
                log.Debug("Adding Task was executed!");
                return new Response<String>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }

        ///// <summary>
        ///// this method tests Requirement 12
        ///// This method adds a new task without a description needed.
        ///// </summary>
        ///// <param name="email">Email of the user. The user must be logged in.</param>
        ///// <param name="boardName">The name of the board</param>
        ///// <param name="title">Title of the new task</param>
        ///// <param name="dueDate">The due date if the new task</param>
        ///// <returns>Response with user-email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        //public Response<string> AddTask(string email, string boardName, string title, DateTime dueDate) // for task without description
        //{
        //    try
        //    {
        //        bc.AddTask(email, boardName, title, dueDate);
        //        log.Debug("Adding Task (without description) was executed!");
        //        return new Response<String>(email);
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error(e.Message);
        //        return new Response<string>(e);
        //    }
        //}

        /// <summary>
        /// this method tests Requirement 13
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<string> AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                bc.AdvanceTask(email, boardName, columnOrdinal, taskId);
                log.Debug("Advance task was executed!");
                return new Response<String>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);

            }
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
        public Response<string> UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                bc.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
                log.Debug("Update tasks dueDate was executed!");
                return new Response<String>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
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

        public Response<string> UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                bc.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                log.Debug("Update tasks Title was executed!");
                return new Response<String>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
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
        public Response<string> UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                bc.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                log.Debug("Update tasks description was executed!");
                return new Response<String>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public Response<List<Task>> InProgressTasks(string email)
        {
            try
            {
                List<Task> InProgressTasks=bc.InProgressTasks(email);
                log.Debug("return InProgress tasks !");
                return new Response<List<Task>>(InProgressTasks);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<List<Task>>(e);
            }

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
        public Response<string> AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                bc.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                log.Debug("Assign task was executed!");
                return new Response<string>();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response<string>(e);
            }
        }
    }
}
