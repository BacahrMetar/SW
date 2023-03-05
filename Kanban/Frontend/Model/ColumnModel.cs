using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace IntroSE.Kanban.Frontend.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }
        private int _ordinal;
        public ColumnModel(BackendController controller, Column c) : base(controller)
        {
            Tasks = new ObservableCollection<TaskModel>();
            List<Task> tasks = c.GetTasks();
            this._ordinal = c.Ordinal;
            foreach (Task t in tasks)
            {
                TaskModel tm = new TaskModel(controller, t);
                Tasks.Add(tm);
            }
        }
        public int ORDINAL
        {
            get { return _ordinal; }
        }
    }
}
