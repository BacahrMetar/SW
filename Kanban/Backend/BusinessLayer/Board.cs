using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Board
    {
        private string name;
        private Dictionary<string, Column> columns;
        private const string BACKLOG_COLUMN_NAME = "backlog";
        private const string INPROGRESS_COLUMN_NAME = "in progress";
        private const string DONE_COLUMN_NAME = "done";
        private string [] colNames = { BACKLOG_COLUMN_NAME, INPROGRESS_COLUMN_NAME, DONE_COLUMN_NAME };
        private const int MAX_COLUMN_ORDINAL = 2;
        private const int MIN_COLUMN_ORDINAL = 0;
        private readonly int id;
        private string owner;
        private List<string> members;
        private BoardDTO dto;
        private UserBoardMapper _users_boards_Mapper;
        private ColumnDalController _columns_dal_controller;



        /// <summary>
        /// Constructor for Board
        /// </summary>
        /// <param name="name">The board name</param>
        ///  <param name="id">The board id</param>
        ///  <param name="owner">The board owner</param>
        public Board(string name, int id, string owner)
        {
            this.name = name;
            columns = new Dictionary<string, Column>();
            columns[BACKLOG_COLUMN_NAME] = new Column(BACKLOG_COLUMN_NAME, 0 , id);
            columns[INPROGRESS_COLUMN_NAME] = new Column(INPROGRESS_COLUMN_NAME, 1, id);
            columns[DONE_COLUMN_NAME] = new Column(DONE_COLUMN_NAME, 2, id);
            this.id = id;
            this.owner = owner;
            this.members = new List<string>();
            dto = new BoardDTO(id,name,owner);
            _users_boards_Mapper = new UserBoardMapper();
        }

        /// <summary>
        /// Constructor to Board from a dto object
        /// </summary>
        /// <param name="boardDTO">The dto object of board</param>
        public Board(BoardDTO boardDTO)
        {
            this.id = boardDTO.ID;
            this.name = boardDTO.Name;
            this.owner = boardDTO.Owner;
            _users_boards_Mapper = new UserBoardMapper();
            this.members = new List<string>();
            List<UserDTO> membersDTO = _users_boards_Mapper.SelectAllBoardMembers(id);
            foreach (UserDTO memeber in membersDTO)
            {
                members.Add(memeber.Email);
            }
            this.dto = boardDTO;
            this.columns = new Dictionary<string, Column>();
            this._columns_dal_controller = new ColumnDalController();
            List<ColumnDTO> boardsColumnsDTO = _columns_dal_controller.SelectAllColumnsForBoardID(id);
            if (boardsColumnsDTO.Count > 0) //if board exist it already has columns
            {
                foreach (ColumnDTO column in boardsColumnsDTO)
                {
                    Column c = new Column(column);
                    string columnName = colNames[c.Ordinal];
                    columns.Add(columnName, c);
                }
            }
            else //we build 3 new columns and add to board
            {
                int counter = 0;
                foreach(string columnName in colNames)
                {
                    Column c = new Column(columnName, counter, id);
                    _columns_dal_controller.Insert(c.DTO); //added to data
                    columns.Add(columnName, c); //added to RAM
                    counter++;
                }
            }
        }

        [JsonConstructor]
        public Board(int id, string name, string owner, Dictionary<string, Column> columns)
        {
            this.name = name;
            this.columns = columns;
            this.id = id;
            this.owner = owner;
        }

        /// <summary>
        /// getter for board's id
        /// </summary>
        public int ID { get { return id; } }

        /// <summary>
        /// getter for board's name
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// getter for board's owner email
        /// </summary>
        public string Owner { get { return owner; } set { owner = value; } }

        /// <summary>
        /// getter for accessed users list
        /// </summary>
        [JsonIgnore]
        public List<string> Members { get { return members; }}

        /// <summary>
        /// getter for the columns
        /// </summary>
        public Dictionary<string, Column> Columns { get { return columns; } }

        /// <summary>
        /// getter for the dto
        /// </summary>
        [JsonIgnore]
        public BoardDTO DTO { get { return dto; } set { dto = value; } }

        /// <summary>
        /// getter for board's column
        /// </summary>
        /// <returns>Dictionary- representing the board columns</returns>
        public Dictionary<string, Column> GetColumns() 
        {
            return columns;
        }

       /// <summary>
       /// getter for a column's limit by column's ordinal
       /// </summary>
       /// <param name="ordinal"></param>
       /// <returns>int- representing the column's limit</returns>
       /// <exception cref="Exception"></exception>
        public int GetColumnLimit(int ordinal)
        {
            if(ordinal < MIN_COLUMN_ORDINAL || ordinal > MAX_COLUMN_ORDINAL)
            {
                throw new Exception("ordinal value cant be "+ordinal);
            }

            if (ordinal == MIN_COLUMN_ORDINAL)
                return columns[BACKLOG_COLUMN_NAME].GetTasksLimit();
            if (ordinal == MAX_COLUMN_ORDINAL)
                return columns[DONE_COLUMN_NAME].GetTasksLimit();
            else
            {
                return columns[INPROGRESS_COLUMN_NAME].GetTasksLimit();
            }
        }
        /// <summary>
        /// getter for a column's limit by column's name
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns>int- representing the column's limit</returns>
        /// <exception cref="Exception"></exception>
        public int GetColumnLimit(string columnName)
        {
            if (!colNames.Contains(columnName))
                throw new Exception("Column name is illegal!");
            return columns[columnName].GetTasksLimit();
        }
        /// <summary>
        /// method for limiting colum size 
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="limit"></param>
        /// <exception cref="Exception"></exception>
        public void LimitColumn(int columnOrdinal, int limit)
        {
            try
            {
                Column c = columns[colNames[columnOrdinal]];
                c.LimitColumn(limit);
                c.DTO.TasksLimit = limit; 
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Getter for a column name by ordinal
        /// </summary>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetColumnName(int ordinal)
        {
            if(ordinal < MIN_COLUMN_ORDINAL || ordinal > MAX_COLUMN_ORDINAL)
                throw new Exception("ordinal value cant be " + ordinal);
            if(ordinal == MIN_COLUMN_ORDINAL)
                return BACKLOG_COLUMN_NAME;
            if (ordinal == MAX_COLUMN_ORDINAL)
                return DONE_COLUMN_NAME;
            else
                return INPROGRESS_COLUMN_NAME;
        }
        /// <summary>
        /// Method for adding a task to board
        /// </summary>
        /// <param name="t"></param>
        /// <exception cref="Exception"></exception>
        public void AddTask(Task t)
        {
            try
            {
                columns[BACKLOG_COLUMN_NAME].AddTask(t);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Method for moving a task from one column to another
        /// </summary>
        /// <param name="t"></param>
        /// <param name="columnName">The column to move the task to</param>
        /// <exception cref="Exception"></exception>
        public void MoveTask(Task t,string columnName)
        {
            if (!colNames.Contains(columnName))
                throw new Exception("Column name is illegal!");
            columns[columnName].AddTask(t);
        }

        /// <summary>
        /// Getter for column by column name
        /// </summary>
        /// <param name="Column"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Column GetColumn(string Column)
        {
            if (!colNames.Contains(Column))
                throw new Exception("Column name is illegal!");
            return columns[Column];
        }
      
        /// <summary>
        /// Getter for column by column ordinal
        /// </summary>
        /// <param name="Column"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Column GetColumn(int ordinal)
        {
            return columns[GetColumnName(ordinal)];
        }

        /// <summary>
        /// Getter for a task by taskId
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task GetTask(int taskId)
        {
            
            for (int i = 0; i < colNames.Length; i++)
            {
                Task temp = columns[colNames[i]].GetTask(taskId);
                if (temp != null)
                {
                    return temp;
                }

            }
            throw new Exception("Task id is not exist"); 
        }
        /// <summary>
        /// getter for in progress tasks
        /// </summary>
        /// <returns>list-representing the in progress task</returns>
        public List<Task> GetInProgress()
        {
            return columns[INPROGRESS_COLUMN_NAME].GetTasks();
        }
        /// <summary>
        /// method for removing a task
        /// </summary>
        /// <param name="t"></param>
        /// <param name="coulumnName"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveTask(Task t, string coulumnName)
        {
            if (!colNames.Contains(coulumnName))
                throw new Exception("Column name is illegal!");
            columns[coulumnName].Removetask(t);
        }
        /// <summary>
        /// Adds the user to the member list of the board
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="Exception">If user is already member of the board</exception>
        public void AddMember(string email)
        {

            if (members.Contains(email)){
                throw new Exception("User already member of this board");
            }
            try
            {
                UserBoardDTO userBoard = new UserBoardDTO(ID, email);
                _users_boards_Mapper.Insert(userBoard);//insert to data
                members.Add(email);// insert to ram
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Removes the user from the member list of the board
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="Exception">If user is not a member of the board</exception>
        public void RemoveMember(string email)
        {

            if (!members.Contains(email))
            {
                throw new Exception("User is not a member of this board");
            }
            try
            {
                UserBoardDTO userBoard = new UserBoardDTO(ID, email);
                _users_boards_Mapper.DeleteMember(userBoard);//remove from data
                members.Remove(email); //update ram
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Method for reciving all the tasks that a user is assignee of them.
        /// </summary>
        /// <param name="email">The email of the assignee</param>
        /// <returns>All the tasks that are not "done", that the user is assigned to</returns>
        public List<Task> GetAssignedTasks(string email)
        {
            List<Task> backlogTasks = columns[BACKLOG_COLUMN_NAME].GetTasks();
            List<Task> inprogressTasks = columns[INPROGRESS_COLUMN_NAME].GetTasks();
            List<Task> allTasks = backlogTasks.Concat(inprogressTasks).ToList();
            List<Task> userAssignedTasks = new List<Task>();
            foreach (Task t in allTasks)
            {
                if (t.Assignee!=null && t.Assignee.Equals(email))
                {
                    userAssignedTasks.Add(t);
                }
            }
            return userAssignedTasks;
        }

        /// <summary>
        /// Un assign all tasks of a user
        /// </summary>
        /// <param name="email">The user to unassign from</param>
        public void UnAssignAllTasks(string email)
        {
            foreach (Task t in GetAssignedTasks(email))
            {
                t.Assignee = null;
                t.DTO.Assignee = null;
            }
        }

        /// <summary>
        /// Check if user is member of the board
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsMember(string email)
        {
            return members.Contains(email);
        }

        /// <summary>
        /// Checks if user is the owner of a board
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsOwner(string email)
        {
            return owner.Equals(email);
        }




    }

}
