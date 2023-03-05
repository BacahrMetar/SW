using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class BoardViewModel :NotifiableObject
    {
        private BackendController controller;
        private BoardModel board;

        public string Title { get; private set; }
        public ColumnModel COL0 { get; private set; }
        public ColumnModel COL1 { get; private set; }
        public ColumnModel COL2 { get; private set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="board"></param>
        public BoardViewModel(BoardModel board)
        {
            this.controller = board.Controller;
            this.board = board;
            Title = "Tasks for " + board.Name;
            COL0 = board.getColumn(0);
            COL1 = board.getColumn(1);
            COL2 = board.getColumn(2);
        
        }

    }
}
