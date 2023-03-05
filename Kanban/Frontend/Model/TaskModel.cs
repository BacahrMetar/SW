using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Frontend;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private int _id;
        private string _description;
        private DateTime _dueDate;
        private string _assignee;
        private DateTime _creationTime;
        private string _title;
        /// <summary>
        /// gettrer for task's id
        /// </summary>
        public int Id 
        {
            get => _id;
        }
        /// <summary>
        ///  gettrer for task's title
        /// </summary>
        public string Title 
        {  
            get => _title;
        }

        /// <summary>
        ///  gettrer for task's decription
        /// </summary>
        public string Description
        {
            get => _description;
          
        }
        /// <summary>
        ///  gettrer for task's dueDate
        /// </summary>
        public DateTime DueDate
        {
            get => _dueDate;
        }
        /// <summary>
        ///  gettrer for task's assignee
        /// </summary>
        public string Assignee
        {
            get => _assignee;
        }
        /// <summary>
        ///  gettrer for task's creation time
        /// </summary>
        public DateTime CreationTime
        {
               get => _creationTime;
        }

        public TaskModel(BackendController controller ,Task t) : base(controller)
        {
            this._id = t.Id;
            this._title = t.Title;
            this._description = t.Description;
            this._dueDate = t.DueDate;
            this._assignee = t.Assignee;
            this._creationTime = t.CreationTime;

        }
        /// <summary>
        /// to string method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_assignee))
            {
                return "ID : " + Id + "\nTitle : " + Title + "\nDescription : " + Description + "\nDueDate : " + DueDate;
            }
            else
            {
                return "ID : " + Id + "\nTitle : " + Title + "\nDescription : " + Description + "\nAssignee : " + Assignee + "\nDueDate : " + DueDate;
            }
        }

      



    }
}
