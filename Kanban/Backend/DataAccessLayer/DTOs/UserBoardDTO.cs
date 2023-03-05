using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class UserBoardDTO : DTO
    {
        public const string BoardsIDColumnName = "boardId";
        public const string UsersEmailColumnName = "userEmail";
        private UserBoardMapper _usersBoardsMapper;

        private int _boardId;
        private string _userEmail;

        public UserBoardDTO(int boardID, string userEmail) : base(new UserBoardMapper())
        {
            _boardId = boardID;
            _userEmail = userEmail;
            this._usersBoardsMapper = new UserBoardMapper();
        }

        public int BoardID { get => _boardId;}
        public string UserEmail { get => _userEmail; set { _userEmail = value; _usersBoardsMapper.Update(_boardId, UsersEmailColumnName, value); } }
        public string usersEmailColumnName { get => UsersEmailColumnName; }

    }
}
