
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DalController
    {
        protected readonly string _connectionString;
        private readonly string _tableName;
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }

        public string ConnectionString { get { return _connectionString; } }

        /// <summary>
        /// Update that uses the id of the wanted object
        /// Used to string values to update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Update(long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeValue} where id={id}"
                };
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    res = command.ExecuteNonQuery();
                    log.Debug($"Update in {_tableName} commited successfuly!");
                }
                catch (Exception e)
                {
                    log.Error($"Update in {_tableName} failed!");
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
        /// Update the uses the primary key of an object. Used to Users table with string primary key.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool Update(string primaryKey, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    //CommandText = $"update {_tableName} set [{attributeName}]=@{attributeValue} where {attributeName} = {primaryKey};"
                     CommandText = $"update {_tableName} set [{attributeName}]=@attributeValue where {attributeName} = {primaryKey};"
                };
                try
                {
                    connection.Open();
                    SQLiteParameter valueParam = new SQLiteParameter(@"value", attributeValue);
                    command.Parameters.Add(valueParam);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    //command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    //res = command.ExecuteNonQuery();
                    log.Debug($"Update in {_tableName} commited successfuly!");
                }
                catch (Exception e)
                {
                    log.Error($"Update in {_tableName} failed!");
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
        /// Update that uses the id of the wanted object
        /// Used to int values to update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Update(int primaryKey, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]={attributeValue} where {attributeName} = {primaryKey};"
                };
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    res = command.ExecuteNonQuery();
                    log.Debug($"Update in {_tableName} commited successfuly!");
                }
                catch(Exception e)
                {
                    log.Error($"Update in {_tableName} failed!");
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
        /// Select all rows from specific table
        /// </summary>
        /// <returns></returns>
        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);         
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Select all row from {_tableName} failed!");
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (dataReader != null){
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }
                
            }
            log.Debug($"Select all row from {_tableName} succeed!");
            return results;
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public bool DeleteDataDB()
        {
          int res = -1;
            foreach(string tableName in GetTables()) 
            {
                
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    try
                    {
                        connection.Open();
                        command.CommandText = $"Delete From {tableName} ";
                        res = command.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        log.Error("faild Attempted to delete all " + tableName + " table");
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }
                
            }
            log.Debug("Delete data from all tables succeed!");
            return res > 0;
        }

        /// <summary>
        /// Gets all tables in data base
        /// </summary>
        /// <returns>A list of all table names of data base</returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetTables()
        {
            List<string> list = new List<string>();

            // executes query that select names of all tables in master table of the database
            string query = "SELECT name FROM sqlite_master " +
                    "WHERE type = 'table'" +
                    "ORDER BY 1";
            try
            {

                DataTable table = GetDataTable(query);

                // Return all table names in the List

                foreach (DataRow row in table.Rows)
                {
                    list.Add(row.ItemArray[0].ToString());
                }
            }
            catch (Exception e)
            {
                log.Error("GetTable Attempt failed");
               throw new Exception(e.Message);
            }
            return list;
        }

        /// <summary>
        /// Gets a data table
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var c = new SQLiteConnection(_connectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Converts a reader to dto object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public abstract DTO ConvertReaderToObject(SQLiteDataReader reader);


    }
}