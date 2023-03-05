using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserBoardMapper : DalController //// A TABLE TO USER - BOARD (ONLY MEMBERS OF BOARD ARE INCLUDED)
    {
        
        private const string UsersBoardsTableName = "UsersBoards";

        private readonly UsersDalController _usersDalController;

        public UserBoardMapper() : base(UsersBoardsTableName)
        {
            this._usersDalController = new UsersDalController();
        }

        /// <summary>
        /// Select all board members for specific board id
        /// </summary>
        /// <param name="id">The id of the wanted board</param>
        /// <returns> List with the Users who are members of board with the id</returns>
        public List<UserDTO> SelectAllBoardMembers(int id)
        {
            List<UserDTO> results = new List<UserDTO>();
            List<string> membersEmails = new List<string>();
            using (var connection = new SQLiteConnection(_usersDalController.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT DISTINCT Email, Password FROM UsersBoards as ub,Users as u " +
                    $"WHERE boardId = {id} and ub.userEmail = u.Email;";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        UserDTO user = _usersDalController.ConvertReaderToObject(dataReader);
                        results.Add(user);
                    }
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
                return results;
            }
        }

        /// <summary>
        /// Converts a reader to UserBoardDTO object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public override UserBoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserBoardDTO UBDTO = new UserBoardDTO(reader.GetInt32(0), reader.GetString(1));
            return UBDTO;
        }

        /// <summary>
        /// Inserting a new user-board object to table
        /// </summary>
        /// <param name="userBoard"> BoardDTO parameter</param>
        /// <returns></returns>
        public bool Insert(UserBoardDTO userBoard)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UsersBoardsTableName} ({UserBoardDTO.BoardsIDColumnName},{UserBoardDTO.UsersEmailColumnName}) " +
                        $"VALUES (@boardIDParam,@EmailParam);";

                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardIDParam", userBoard.BoardID);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"EmailParam", userBoard.UserEmail);

                    command.Parameters.Add(boardIDParam);
                    command.Parameters.Add(EmailParam);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Inserting a user-board object to data failed");
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
        /// Deletes a member from a board
        /// </summary>
        /// <param name="ubDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeleteMember(UserBoardDTO ubDto)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            { 
                connection.Open();
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {UsersBoardsTableName} where {UserBoardDTO.BoardsIDColumnName} = @boardId and {UserBoardDTO.UsersEmailColumnName} = @email;"
                };
                try
                {
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", ubDto.BoardID);
                    SQLiteParameter emailParam = new SQLiteParameter(@"email", ubDto.UserEmail);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(emailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Delete a member from board failed");
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
        /// removes board from UserBoards table
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
                    command.CommandText = $"DELETE FROM {UsersBoardsTableName} WHERE {UserBoardDTO.BoardsIDColumnName} = @boardIdVal";


                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", boardID);
                    command.Parameters.Add(boardIdParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Delete all user-board connections from table succeed");

                }
                catch (Exception e)
                {
                    log.Error("Failed to delete a user-board from table");
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

    }
}


