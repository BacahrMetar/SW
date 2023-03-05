using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnDalController : DalController
    {
        private const string ColumnsTableName = "Columns";
        public ColumnDalController() : base(ColumnsTableName) { }

        public override ColumnDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO result = new ColumnDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
            return result;
        }


        /// <summary>
        /// Select all board members for specific board id
        /// </summary>
        /// <param name="id">The id of the wanted board</param>
        /// <returns> List with the Users who are members of board with the id</returns>
        public List<ColumnDTO> SelectAllColumnsForBoardID(int id)
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
            List<string> membersEmails = new List<string>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(null, connection);             
                command.CommandText = $"SELECT * FROM Columns WHERE boardId = {id} ;";
                SQLiteDataReader dataReader = null;
                try
                {
                    var exist = command.ExecuteScalar(); //checks if columns exist in data base
                    if (exist!=null)
                    {
                        dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            ColumnDTO column = ConvertReaderToObject(dataReader);
                            results.Add(column);
                        }
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
        /// Tasks limit update for a specific column
        /// </summary>
        /// <param name="BoardID">The board id</param>
        /// <param name="BoardIDColumnName">The column name</param>
        /// <param name="ColumnOrdinal"></param>
        /// <param name="OrdinalColumnName"></param>
        /// <param name="TasksLimitColumnName"></param>
        /// <param name="newLimitValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateTaskLimit(int BoardID, string BoardIDColumnName, int ColumnOrdinal, string OrdinalColumnName, string TasksLimitColumnName, int newLimitValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {ColumnsTableName} set [{TasksLimitColumnName}]={newLimitValue} where {BoardIDColumnName} = {BoardID} and {OrdinalColumnName} = {ColumnOrdinal};"
                };

                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    log.Error("Limit task update in data attempted but failed!");
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
        /// Inserting new column to data base
        /// </summary>
        /// <param name="column"> ColumnDTO parameter</param>
        /// <returns></returns>
        public bool Insert(ColumnDTO column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnsTableName} ({ColumnDTO.BoardIDColumnName},{ColumnDTO.OrdinalColumnName} ,{ColumnDTO.TasksLimitColumnName}) " +
                        $"VALUES (@boardIdVal,@ordinalVal,@limitVal);";


                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", column.BoardID);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", column.Ordinal);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.TasksLimit);

                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(limitParam);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Inserting a new column to data succeed");
                }
                catch (Exception e)
                {
                    log.Error("Inserting a new column to data failed");
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
        /// removes board from Columns table
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
                    command.CommandText = $"DELETE FROM {ColumnsTableName} WHERE {ColumnDTO.BoardIDColumnName} = @boardIdVal";


                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", boardID);
                    command.Parameters.Add(boardIdParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Delete all board columns from table succeed");

                }
                catch (Exception e)
                {
                    log.Error("Failed to delete all boards columns from table");
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
