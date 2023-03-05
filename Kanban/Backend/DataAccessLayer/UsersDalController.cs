using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UsersDalController : DalController
    {
        private const string UsersTableName = "Users";

        /// <summary>
        /// constructor
        /// </summary>
        public UsersDalController() : base(UsersTableName) { }


        /// <summary>
        /// Selects all users from users table
        /// </summary>
        /// <returns></returns>
        public List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();

            return result;
        }


        /// <summary>
        /// Inserts a user to Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Insert(UserDTO user)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UsersTableName} ({UserDTO.UsersEmailColumnName} ,{UserDTO.UsersPasswordColumnName}) " +
                        $"VALUES (@emailVal,@passwordVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug("Adding user to users table succeed");
                }
                catch(Exception e)
                {
                    log.Error("Attempted inserting a new user to table");
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
        /// Converts a reader to UserDTO object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public override UserDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1));
            return result;

        }
    }
}

