using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Column
    {
        private string colName;
        private int ordinal;
        private List<Task> tasks;
        private int tasksLimit;
        private const int UNLIMITED_TASKS = -1;
        private readonly int board_id;
        private TaskDalController _task_dal_controller;
        private ColumnDTO dto;

        /// <summary>
        /// Constructor to Column
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="ordinal"></param>
        public Column(string colName, int ordinal, int BoardID)
        {
            tasks = new List<Task>();
            tasksLimit = UNLIMITED_TASKS;
            this.colName = colName;
            this.ordinal = ordinal;
            this.board_id= BoardID;
            dto = new ColumnDTO(board_id,ordinal,tasksLimit);
        }
        /// <summary>
        /// Constructor to column
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="ordinal"></param>
        /// <param name="tasksLimit"></param>
        public Column(string colName, int ordinal, int tasksLimit, int BoardID)
        {
            tasks = new List<Task>();
            this.tasksLimit = tasksLimit;
            this.colName = colName;
            this.ordinal = ordinal;
            this.board_id = BoardID;
            dto = new ColumnDTO(board_id, ordinal, tasksLimit);
        }

        /// <summary>
        /// Constructor to column
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="ordinal"></param>
        /// <param name="tasksLimit"></param>
        public Column(ColumnDTO columnDTO)
        {
            tasks = new List<Task>();
            this.tasksLimit = columnDTO.TasksLimit;
            this.ordinal = columnDTO.Ordinal;
            this.board_id = columnDTO.BoardID;
            this._task_dal_controller = new TaskDalController();
            List<TaskDTO> columnsTaskDTO = _task_dal_controller.SelectAllColumnsTasksForBoardID(board_id,ordinal);
            foreach (TaskDTO task in columnsTaskDTO)
            {
                Task t = new Task(task);     
                tasks.Add(t);
            }
            DTO = columnDTO;
            //this.task=GET TASKS FOR DB
        }

        [JsonConstructor]
        public Column(int ordinal, int tasksLimit, List<Task> tasks)
        {
            this.ordinal = ordinal;
            this.tasks = tasks;
            this.tasksLimit = tasksLimit;
        }

        /// <summary>
        /// getter for board's id
        /// </summary>
        [JsonIgnore]
        public int BoardID { get { return board_id; } }

        /// <summary>
        /// getter for columns ordinal
        /// </summary>
        public int Ordinal { get { return ordinal; } set { ordinal = value; } }

        /// <summary>
        /// getter for tasks limit
        /// </summary>
        public int TasksLimit { get { return tasksLimit; } set { tasksLimit = value; } }

        /// <summary>
        /// getter for columns tasks
        /// </summary>
        public List<Task> Tasks { get { return tasks; } set { tasks = value; } }

        /// <summary>
        /// getter for dto object
        /// </summary>
        [JsonIgnore]
        public ColumnDTO DTO { get { return dto; } set { dto = value; } }


        /// <summary>
        /// tasl limit getters
        /// </summary>
        /// <returns>int representing the task limit</returns>
        public int GetTasksLimit()
        {
            return tasksLimit;
        }
      
        /// <summary>
        /// method to add task
        /// </summary>
        /// <param name="t"></param>
        /// <exception cref="Exception"></exception>
        public void AddTask(Task t)
        {
            if (isFull())
                throw new Exception("Column is full - limited to " + tasksLimit + " tasks!");
            tasks.Add(t);
        }
      /// <summary>
      /// getters to a task
      /// </summary>
      /// <param name="taskId"></param>
      /// <returns>Task represent the desire task</returns>
        public Task GetTask(int taskId)
        {
            bool flag = true;
            Task t = null;
            for(int i = 0; i < tasks.Count && flag; i++)
            {
                if (tasks[i].Id == taskId)
                {
                     t = tasks[i];
                    flag = false;
                }
            }

            if (flag)
                return null;
            else
                return t;
            
        }
      
        /// <summary>
        /// getter to column name
        /// </summary>
        /// <returns>string representing the column name</returns>
        public String GetColumnName()
        {
             return colName;
        }
      
        /// <summary>
        /// method to limit the number of task in the column
        /// </summary>
        /// <param name="limit"></param>
        /// <exception cref="Exception"></exception>
        public void LimitColumn(int limit) 
        {
            if (limit != UNLIMITED_TASKS && limit < 0)
                throw new Exception("Negative limit value!");
            if (limit != UNLIMITED_TASKS && tasks.Count > limit)
                throw new Exception("More tasks than limit value!");
            this.tasksLimit = limit;

        }
      
        /// <summary>
        /// method to get a the task in the column
        /// </summary>
        /// <returns>List of taks representing the tasks in the column</returns>
        public List<Task> GetTasks()
        {
            return this.tasks;
        }
      
        /// <summary>
        ///method to remove specific task from the column 
        /// </summary>
        /// <param name="t"></param>
        public void Removetask(Task t)
        {
            int index = tasks.IndexOf(t);
            tasks.RemoveAt(index);
        }
      
        /// <summary>
        ///method to recive the amount of tasks in thr column 
        /// </summary>
        /// <returns>int -representing the current amount of task in the column</returns>
        public int GetTasksSize()
        {
            return tasks.Count;
        }
      
        /// <summary>
        /// method to check if the column has reach to its limit
        /// </summary>
        /// <returns>bollean- representing if the column is full </returns>
        public bool isFull()
        {
            if (tasksLimit == UNLIMITED_TASKS)
                return false;
            return tasks.Count == tasksLimit;
        }

       
    }
}
