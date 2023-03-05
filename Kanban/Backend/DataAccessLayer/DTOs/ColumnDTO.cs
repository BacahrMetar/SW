using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class ColumnDTO : DTO
    {
        public const string BoardIDColumnName = "boardId";
        public const string OrdinalColumnName = "Ordinal";
        public const string TasksLimitColumnName = "tasksLimit";

        private int _boardId;
        private int _ordinal;
        private int _tasksLimit;
        private ColumnDalController column_dal_controller=new ColumnDalController();
        public ColumnDTO(DalController controller) : base(controller) { }

        public ColumnDTO(int boardId, int ordinal, int limit) : base(new ColumnDalController())
        {
            _boardId = boardId;
            _ordinal = ordinal;
            _tasksLimit = limit;
        }

        public int BoardID { get => _boardId; set { column_dal_controller.Update(_boardId, BoardIDColumnName, value); _boardId = value; } }
        public int Ordinal { get => _ordinal; set { _ordinal = value; column_dal_controller.Update(_boardId, OrdinalColumnName, value); } }
        public int TasksLimit { get => _tasksLimit; set { _tasksLimit = value; column_dal_controller.UpdateTaskLimit(_boardId, BoardIDColumnName, _ordinal, OrdinalColumnName, TasksLimitColumnName, value); } }
    }
}
