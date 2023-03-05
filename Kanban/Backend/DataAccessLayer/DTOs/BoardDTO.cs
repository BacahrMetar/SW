using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class BoardDTO :DTO
    {
        public const String BoardsIDColumnName = "ID";
        public const string BoardsNameColumnName = "Name";
        public const string BoardsOwnerNameColumnName = "Owner";
        private BoardDalController _board_dal_controller;


        private int _boardID;
        private string _boardName;
        private string _boardOwnerName;
        

        public BoardDTO(int id,string boardName,string ownerName) : base(new BoardDalController())
        {
            _boardID = id;
            _boardName = boardName;
            _boardOwnerName = ownerName;
            _board_dal_controller = (BoardDalController)_controller;
        }

        public int ID { get => _boardID; }
        public string Name { get => _boardName; set { _boardName = value; _controller.Update(ID,BoardsNameColumnName, value); }  }     
        public string Owner { get => _boardOwnerName; set { _boardOwnerName = value; _board_dal_controller.UpdateBoardField(ID, BoardsOwnerNameColumnName, value); } }
       

    }
}
