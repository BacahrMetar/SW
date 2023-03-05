using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskDalController : DalController
    {
        private const string TasksTableName = "Tasks";

        public TaskDalController() : base(TasksTableName) { }

        /// <summary>
        /// Select all Board From DataBase
        /// </summary>
        /// <returns> List with the Boards</returns>
        public List<TaskDTO> SelectAllBoards()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();

            return result;
        }
        /// <summary>
        /// inserting new board
        /// </summary>
        /// <param name="board"> BoardDTO parameter</param>
        /// <returns></returns>
        public bool Insert(TaskDTO task)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TasksTableName} ({TaskDTO.TasksIdColumnName},{TaskDTO.TasksBoardId},{TaskDTO.TasksColumnOrdinal},{TaskDTO.TasksTitleColumnName} ,{TaskDTO.TasksDescriptionColumnName},{TaskDTO.TasksDueDateColumnName},{TaskDTO.TasksCreationTimeColumnName},{TaskDTO.TasksAssignedColumnName}) " +
                        $"VALUES (@idVal,@boardIdVal,@columnOrdinalVal,@titleVal,@decriptionVal,@dueDateVal,@creatonTimeVal,@assignedVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.TaskId);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", task.BoardId);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", task.ColumnOrdinal);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.TaskTitle);
                    SQLiteParameter decriptionParam = new SQLiteParameter(@"decriptionVal", task.TaskDescription);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate);
                    SQLiteParameter creatonTimeParam = new SQLiteParameter(@"creatonTimeVal",task.CreationTime );
                    SQLiteParameter assignedParam = new SQLiteParameter(@"assignedVal",task.Assignee );


                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(decriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(creatonTimeParam);
                    command.Parameters.Add(assignedParam);


                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Inserting task to data succeed");
                }
                catch(Exception e)
                {
                    log.Error("Failed inserting a new task to table");
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
        /// conver Dto to object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>TaskDto</returns>
        public override TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result;
            if (!reader.IsDBNull(7))
            {
                result = new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetDateTime(6), reader.GetString(7));
            }
            else
            {
                result = new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetDateTime(6), null);
            }
            return result;
        }

        /// <summary>
        /// Updating date time object in data base
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool Update(long id, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TasksTableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    log.Error($"Failed updating date time object in {TasksTableName}");
                    throw new Exception(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            log.Debug($"Succeed updating date time object in {TasksTableName}");
            return res > 0;
        }

        /// <summary>
        /// Selects all the tasks of a column
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public List<TaskDTO> SelectAllColumnsTasksForBoardID(int boardId,int ordinal)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
              
                command.CommandText = $"SELECT * FROM Tasks WHERE {TaskDTO.TasksBoardId} = {boardId} And {TaskDTO.TasksColumnOrdinal} = {ordinal};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    var exist = command.ExecuteScalar();
                    if (exist != null)
                    {
                        dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            TaskDTO task = ConvertReaderToObject(dataReader);
                            results.Add(task);
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Failed selecting all tasks of a column in {TasksTableName}");
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
                log.Debug($"Succeed selecting all tasks of a column in {TasksTableName}");
                return results;
            }
        }

        /// <summary>
        /// Updates a task field that are strings
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="ColumnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateTaskFields(int taskId,string ColumnName,string value)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TasksTableName} set [{ColumnName}]=@value where {TaskDTO.TasksIdColumnName} = {taskId};"
                };

                try
                {
                    connection.Open();
                    SQLiteParameter valueParam = new SQLiteParameter(@"value", value);
                    command.Parameters.Add(valueParam);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Task's {ColumnName} column update in data attempted but failed!");
                    throw new Exception(e.Message);

                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            log.Debug($"Task's {ColumnName} column update in data succeed!");
            return res > 0;
        }
        /// <summary>
        /// Update for tasks dates values
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="TasksTitleColumnName"></param>
        /// <param name="value">The new date time</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateTaskDates(int taskId, string ColumnName, DateTime value)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TasksTableName} set [{ColumnName}]=@value where {TaskDTO.TasksIdColumnName} = {taskId};"
                };

                try
                {
                    connection.Open();
                    SQLiteParameter valueParam = new SQLiteParameter(@"value", value);
                    command.Parameters.Add(valueParam);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Task's {ColumnName} column update in data attempted but failed!");
                    throw new Exception(e.Message);

                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            log.Debug($"Task's {ColumnName} column update in data succeed!");
            return res > 0;
        }

        /// <summary>
        /// Update the task's ordinal (used in advance task)
        /// </summary>
        /// <param name="taskID">The id of the task in table</param>
        /// <param name="newOrdinal">The new ordinal of the task</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateOrdinalValue(int taskID, int newOrdinal)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TasksTableName} set [{TaskDTO.TasksColumnOrdinal}]={newOrdinal} where {TaskDTO.TasksIdColumnName} = {taskID};"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Error($"Updated task ordinal succeed");
                }
                catch (Exception e)
                {
                    log.Error($"Attempted to update ordinal of a task in tasks table but faild");
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
        /// Updates the assignee of a task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="newOrdinal"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateAssignee(int taskID, string newAssignee)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TasksTableName} set [{TaskDTO.TasksAssignedColumnName}]=@assignee where {TaskDTO.TasksIdColumnName} = {taskID};"
                };
                try
                {
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assignee", newAssignee);
                    command.Parameters.Add(assigneeParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Error($"Updated task assginee succeed");
                }
                catch (Exception e)
                {
                    log.Error($"Attempted to update assginee of a task in tasks table but faild");
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
        /// removes board from tasks table
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
                    command.CommandText = $"DELETE FROM {TasksTableName} WHERE {TaskDTO.TasksBoardId} = @boardIdVal";


                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", boardID);
                    command.Parameters.Add(boardIdParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Delete all board tasks from table succeed");

                }
                catch (Exception e)
                {
                    log.Error("Failed to delete all board tasks from table");
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
        /// Getter for tasks table max task id
        /// </summary>
        /// <returns>Max task id in table</returns>
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
                    command.CommandText = $"SELECT MAX({TaskDTO.TasksIdColumnName}) FROM {TasksTableName};";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        maxInd = dataReader.GetInt32(0);
                    }
                }
                catch (Exception e)
                {
                    log.Error("Attempted to get max task id in tasks table");
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


    }
}
