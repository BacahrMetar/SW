using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class TaskDTO : DTO
    {
        public const string TasksIdColumnName = "ID";
        public const string TasksBoardId = "BoardId";
        public const string TasksColumnOrdinal = "ColumnOrdinal";
        public const string TasksTitleColumnName = "Title";
        public const string TasksDescriptionColumnName = "Description";
        public const string TasksDueDateColumnName = "Due_Date";
        public const string TasksCreationTimeColumnName = "Creation_Time";
        public const string TasksAssignedColumnName = "Assignee";

        private TaskDalController _task_dal_controller;


        private int _taskId;
        private int _boardId;
        private int _columnOrdinal;
        private string _taskTitle;
        private string _taskDescription;
        private DateTime _dueDate;
        private DateTime _creationTime;
        private string _assignee;

        /// <summary>
        /// TaskDto constructor  with description
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskTitle"></param>
        /// <param name="taskDescription"></param>
        /// <param name="dueDate"></param>
        /// <param name="creationTime"></param>
        /// <param name="assignee"></param>
        public TaskDTO(int id,int boardId,int columnOrdinal, string taskTitle, string taskDescription, DateTime dueDate, DateTime creationTime, string assignee) : base(new TaskDalController())
        {
            _taskId = id;
            _boardId = boardId;
            _columnOrdinal = columnOrdinal;
            _taskTitle = taskTitle;
            _taskDescription = taskDescription;
            _dueDate = dueDate;
            _creationTime = creationTime;
            _assignee = assignee;
            _task_dal_controller = (TaskDalController)_controller;
        }


        /// <summary>
        /// gettters and Setters
        /// </summary>
        public int TaskId { get => _taskId; }
        public int BoardId { get => _boardId; }
        public int ColumnOrdinal { get => _columnOrdinal; set { _columnOrdinal = value; _task_dal_controller.UpdateOrdinalValue(_taskId, value); } }
        public string TaskTitle { get => _taskTitle; set { _taskTitle = value; _task_dal_controller.UpdateTaskFields(_taskId, TasksTitleColumnName, value); } }
        public string TaskDescription { get => _taskDescription; set { _taskDescription = value; _task_dal_controller.UpdateTaskFields(_taskId, TasksDescriptionColumnName, value); } }
        public DateTime DueDate { get => _dueDate; set { _dueDate = value; _task_dal_controller.UpdateTaskDates(_taskId, TasksDueDateColumnName, value); } }
        public DateTime CreationTime { get => _creationTime; set {_creationTime = value; _task_dal_controller.UpdateTaskDates(_taskId, TasksCreationTimeColumnName, value); } }
        public string Assignee { get => _assignee; set { _assignee = value; _task_dal_controller.UpdateAssignee(_taskId, value); } }

      
    }
       
}
