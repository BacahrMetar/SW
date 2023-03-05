using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        public ObservableCollection<ColumnModel> Columns { get; set; }
        private int id;
        private string name;
        private string owner;


        /// <summary>
        /// getter for board's id
        /// </summary>
        public int ID { get { return id; } set { id = value; } }

        /// <summary>
        /// getter for board's name
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// getter for board's owner email
        /// </summary>
        public string Owner { get { return owner; } set { owner = value; } }


        public BoardModel(BackendController controller, Board board) : base(controller)
        {
            Columns = new ObservableCollection<ColumnModel>();
            Dictionary<string, Column> boardColumns = board.GetColumns();
            ID = board.ID;
            Name = board.Name;
            Owner = board.Owner;

            foreach (KeyValuePair<string, Column> entry in boardColumns)
            {
                Column c = entry.Value;
                ColumnModel cm = new ColumnModel(controller, c);
                Columns.Add(cm);
            }

        }
        /// <summary>
        /// to string method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ID: " + ID + "    Board Name: " + Name + "    Owner: " + Owner;
        }


        /// <summary>
        /// method to get the board's columnModel by columnOrdinal
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        public ColumnModel getColumn(int columnOrdinal)
        {
            for (int i = 0; i < Columns.Count; i++)
            {

                if (Columns.ElementAt(i).ORDINAL == columnOrdinal)
                {
                    return Columns.ElementAt(i);

                }
            }
            return null;
        }
    }
}
       

        
    

    

  