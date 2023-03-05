using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDalController : DalController
    {
        private const string BoardsTableName = "Boards";
        public BoardDalController() : base(BoardsTableName) { }
       
       /// <summary>
       /// Select all Board From DataBase
       /// </summary>
       /// <returns> List with the Boards</returns>
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();

            return result;
        }
        /// <summary>
        /// Inserting new board to data base
        /// </summary>
        /// <param name="board"> BoardDTO parameter</param>
        /// <returns></returns>
         public bool Insert(BoardDTO board)
         {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardsTableName} ({BoardDTO.BoardsIDColumnName},{BoardDTO.BoardsNameColumnName} ,{BoardDTO.BoardsOwnerNameColumnName}) " +
                        $"VALUES (@idVal,@boardNameVal,@ownerNameVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.ID);
                    SQLiteParameter boardNameParam = new SQLiteParameter(@"boardNameVal", board.Name);
                    SQLiteParameter ownerNameParam = new SQLiteParameter(@"ownerNameVal", board.Owner);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardNameParam);
                    command.Parameters.Add(ownerNameParam);
                    
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Inserting a new board to data succeed");
                }
                catch(Exception e)
                {
                    log.Error("Inserting a new board to data failed");
                    throw new Exception(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        /// <summary>
        /// removes board from boards table
        /// </summary>
        /// <param name="boardID">The board's id</param>
        /// <returns></returns>
        public bool DeleteBoard(int boardID)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {BoardsTableName} WHERE {BoardDTO.BoardsIDColumnName} = @boardIdVal";


                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", boardID);
                    command.Parameters.Add(boardIdParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Delete board from table succeed");

                }
                catch(Exception e)
                {
                    log.Error("failed to delete a board from table");
                    throw new Exception(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        /// <summary>
        /// Getter for boards table max board id
        /// </summary>
        /// <returns>Max board id in table</returns>
        /// <exception cref="Exception"></exception>
        public int GetMaxIndex()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader dataReader = null;
                int maxInd = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"SELECT MAX({BoardDTO.BoardsIDColumnName}) FROM {BoardsTableName}; ";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        maxInd=dataReader.GetInt32(0);
                    }
                }
                catch(Exception e)
                {
                    log.Error("Attempted to get max board id in boards table");
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                }
                return maxInd;
            }
        }

        /// <summary>
        /// Update any board field (Found by id)
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="BoardColumnName">The column to update</param>
        /// <param name="value">The new value</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateBoardField(int boardId, string BoardColumnName, string value)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {BoardsTableName} set [{BoardColumnName}]=@value where {BoardDTO.BoardsIDColumnName} = {boardId};"
                };

                try
                {
                    SQLiteParameter valueParam = new SQLiteParameter(@"value", value);
                    command.Parameters.Add(valueParam);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                    log.Error("board's "+ BoardColumnName+" update in data attempted but failed!");
                    throw new Exception(e.Message);

                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }


        /// <summary>
        /// convert Dto to object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>BoardDto</returns>
        public override BoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
            return result;

        }

    }
}
