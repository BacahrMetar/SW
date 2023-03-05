using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        private readonly int _id;
        readonly DateTime _creationTime;
        private string _title;
        private string _description;
        private DateTime _dueDate;
        private string _columnName;
        private string _assignee;
        private const int MAX_DESCRIPTION_LENGHT = 300;
        private const int MAX_TITLE_LENGHT = 50;
        private const string BACKLOG_COLUMN_NAME = "backlog";
        private const string INPROGRESS_COLUMN_NAME = "in progress";
        private const string DONE_COLUMN_NAME = "done";
        private TaskDTO t_DTO;
        private readonly int _boardId;

        /// <summary>
        /// Constructor for task
        /// </summary>
        /// <param name="title">Title of the task (must not be empty, up to 50 chars)</param>
        /// <param name="description">Description of the task (must not be empty, up to 300 chars)</param>
        /// <param name="dueDate">Due date of the task</param>
        /// <param name="id">Id of the new task</param>
        public Task(string title, string description, DateTime dueDate, int id, int boardId)
        {
            _creationTime = DateTime.Now;
            if(LegalDueDate(dueDate))
                this._dueDate = dueDate;
            if(LegalTitle(title))
                this._title = title;
            if (LegalDescription(description))
                this._description = description;
            _columnName = BACKLOG_COLUMN_NAME;
            this._id = id;
            this._boardId = boardId;
            _assignee = null;
            this.t_DTO = new TaskDTO(id, boardId, 0 ,title, description, dueDate, _creationTime, null);
        }


        /// <summary>
        /// constructor from dto object
        /// </summary>
        /// <param name="taskdto"></param>
        public Task(TaskDTO taskdto)
        {
            this._id = taskdto.TaskId;
            this._boardId = taskdto.BoardId;
            this._title = taskdto.TaskTitle;
            this._description= taskdto.TaskDescription;
            this.DueDate = taskdto.DueDate;
            this._creationTime = taskdto.CreationTime;
            this._assignee = taskdto.Assignee;
            int i = taskdto.ColumnOrdinal;
            if (i == 0)
                this._columnName = BACKLOG_COLUMN_NAME;
            else if (i == 1)
                this._columnName = INPROGRESS_COLUMN_NAME;
            else
                this._columnName = DONE_COLUMN_NAME;
            this.t_DTO = taskdto;
        }
        [JsonConstructor]
        public Task(int id, DateTime creationTime, string title, string description, DateTime dueDate, string assignee)
        {
            _id = id;
            _creationTime = creationTime;
            _title = title;
            Description = description;
            DueDate = dueDate;
            Assignee = assignee;
        }

        /// <summary>
        /// getters and setters
        /// </summary>
        public int Id { get { return _id; } }
        public DateTime CreationTime { get { return _creationTime; } }
        public string Title { get { return _title; } set { _title = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public DateTime DueDate { get { return _dueDate; } set { _dueDate = value; } }
     
        public string Assignee { get { return _assignee; } set { _assignee = value; } }
        [JsonIgnore]
        public TaskDTO DTO { get { return t_DTO; } set { t_DTO = value; } }

        /// <summary>
        /// method to update task's title (only by assigne)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="actor"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateTitle(string title,string actor)
        {
            if (actor==null || !actor.Equals(Assignee)) // if assignee is null task cannot be updated
            {
                throw new Exception("User that is not assignee cannot update task!");
            }
            if (GetColumName().Equals(DONE_COLUMN_NAME))
            {
                throw new Exception("Cannot update task in done column!");
            }
            if (LegalTitle(title))
            {
                this._title = title;
                t_DTO.TaskTitle = title;               
            }
            else
                throw new Exception("Not legal title!");
        }
        /// <summary>
        /// method to update task's description
        /// </summary>
        /// <param name="description"></param>
        /// <param name="actor"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateDescription(string description,string actor)
        {
            if (actor == null || !actor.Equals(Assignee)) // if assignee is null task cannot be updated
            {
                throw new Exception("User that is not assigne cannot update description!");
            }
            if (GetColumName().Equals(DONE_COLUMN_NAME))
            {
                throw new Exception("Cannot update task in done column!");
            }
            if (LegalDescription(description))
            {
                this._description = description;
                t_DTO.TaskDescription = description;
            }
            else
                throw new Exception("Not legal description!");
        }

        /// <summary>
        /// method to update task's duedate
        /// </summary>
        /// <param name="dueDate"></param>
        /// <param name="actor"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateTaskDueDate(DateTime dueDate, string actor)
        {
            if (actor == null || !actor.Equals(Assignee)) // if assignee is null task cannot be updated
            {
                throw new Exception("User that is not assigne cannot update due date!");
            }
            if (GetColumName().Equals(DONE_COLUMN_NAME))
            {
                throw new Exception("Cannot update task in done column!");
            }
            if (LegalDueDate(dueDate))
            {
                this._dueDate = dueDate;
                t_DTO.DueDate = dueDate;
            }
            else
                throw new Exception("Not legal dueDate!");
           
        }
        /// <summary>
        /// method to check if task's title is legal
        /// </summary>
        /// <param name="title"></param>
        /// <returns>boolean representing if the title is legal or not </returns>
        /// <exception cref="Exception"></exception>
        public bool LegalTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_TITLE_LENGHT || title.Length == 0)
                throw new Exception("Not legal title!");
            return true;
        }
        /// <summary>
        /// method to check if task's description is legal
        /// </summary>
        /// <param name="description"></param>
        /// <returns>boolean representing if the descripotion is legal or not</returns>
        /// <exception cref="Exception"></exception>
        public bool LegalDescription(string description)
        {
            if ((description == null || (description.Length != 0 && string.IsNullOrWhiteSpace(description))) || description.Length > MAX_DESCRIPTION_LENGHT)
            {
                throw new Exception("Not legal description!");
            }
            
            return true;
        }

        /// <summary>
        /// method to check if task's due date is legal
        /// </summary>
        /// <param name="dueDate"></param>
        /// <returns>boolean representing if the due date is legal or not</returns>
        /// <exception cref="Exception"></exception>
        public bool LegalDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.Now || dueDate<_creationTime)
                throw new Exception("Not legal dueDate!");
            return true;
        }
        /// <summary>
        /// method to get the column's name
        /// </summary>
        /// <returns>string representing the column's name</returns>
        public String GetColumName()
        {
            return this._columnName;
        }
       
        /// <summary>
        /// method to set column's name
        /// </summary>
        /// <param name="columnName"></param>
        public void SetColumnName(string columnName)
        {
            this._columnName = columnName;
        }
        

    }
}
