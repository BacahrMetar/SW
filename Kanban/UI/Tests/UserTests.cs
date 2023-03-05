using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackTests.Tests
{
    public class UserTests
    {
        private readonly UserService us;
        private readonly GradingService gs; 
        public UserTests()
        {
            this.gs = new GradingService();
            this.us = gs.getUs();
        }


        public void RunAllTests()
        {
            RegisterRun();
            LoginRun();
            LogoutRun();
        }
        public void RegisterRun()
        {
            Console.WriteLine("-----------Register Test-----------");

            try //registering new user 
            {
                Console.WriteLine(gs.Register("disposable++styleemailwithsymbol@example.com", "asdAbcccc1"));
               
                //Console.WriteLine("User registered successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //register exisiting email
            {
                Console.WriteLine(gs.Register("disposable++styleemailwithsymbol@example.com", "Abc1234"));
                //Console.WriteLine("User registered successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Null email
            {
                Console.WriteLine(gs.Register(null, "Abc1234"));
                //Console.WriteLine("User registered successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //No Uppercase char in password
            {
                Console.WriteLine(gs.Register("passwordcheck@gmail.com", "abc1234"));
                //Console.WriteLine("User registered successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //No Number in password
            {
                Console.WriteLine(gs.Register("passwordcheck@gmail.com", "Abbba"));
                //Console.WriteLine("User registered successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Too short password
            {
                Console.WriteLine(gs.Register("passwordcheck@gmail.com", "Ab12"));
                //Console.WriteLine("User registered successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            us.cleanUsers();
        }

        public void LoginRun()
        {
            Console.WriteLine("-----------Login Test-----------");

            Console.WriteLine(gs.Register("logintest@gmail.com", "Abc1234")); //registering a user 
            try //already logged in user
            {
                Console.WriteLine(gs.Login("logintest@gmail.com", "Abc1234"));
                //Console.WriteLine("User logged in successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Logout("logintest@gmail.com")); //logged out the user

            try //logging not logged in existing user
            {
                Console.WriteLine(gs.Login("logintest1@gmail.com", "Abc1234"));
                //Console.WriteLine("User logged in successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Logout("logintest@gmail.com")); //logged out the user

            try //user exist but wrong password
            {
                Console.WriteLine(gs.Login("logintest@gmail.com", "Abc12123123534"));
                //Console.WriteLine("User logged in successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //user doesn't exist
            {
                Console.WriteLine(gs.Login("kanban04@gmail.com", "Abc1234"));
                //Console.WriteLine("User logged in successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("UpperCaseEmail@gmail.com", "Abc1234")); //registering a user (when user registers he is logged in)
            Console.WriteLine(gs.Logout("UpperCaseEmail@gmail.com"));
            try // user with upper case email
            {
                Console.WriteLine(gs.Login("UpperCaseEmail@gmail.com", "Abc1234"));
                //Console.WriteLine("User logged in successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            us.cleanUsers();
        }

        public void LogoutRun()
        {
            Console.WriteLine("-----------Logout Test-----------");

            Console.WriteLine(gs.Register("logouttest@gmail.com", "Abc1234")); //registering  a user
            Console.WriteLine(gs.Login("logouttest@gmail.com", "Abc1234"));

            try //logout connected user
            {
                Console.WriteLine(gs.Logout("logouttest@gmail.com"));
                //Console.WriteLine("User logged out successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //logout not connected user
            {
                Console.WriteLine(gs.Logout("logouttest@gmail.com"));
                // Console.WriteLine("User logged out successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //logout not existing user
            {
                us.Logout("notexist@gmail.com");
                //Console.WriteLine("User logged out successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            us.cleanUsers();
        }

        public void AddOwnedBoardRun1()
        {
            Console.WriteLine("-----------Add Owned Board test-----------");

            Console.WriteLine(gs.Register("add_owned_board@gmail.com", "Abc1234")); //registering a user
            Console.WriteLine(gs.Login("add_owned_board@gmail.com", "Abc1234"));

            try // adding new board to owned user board
            {
                Console.WriteLine(gs.AddBoard("logouttest@gmail.com","newBoard"));
                //Console.WriteLine("User logged out successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        //need to check if it should be also in grading  Service?

        public void AddOwnedBoardRun()
        {
            Console.WriteLine("-----------Add Owned Board test-----------");

            Console.WriteLine(gs.Register("add_owned_board@gmail.com", "Abc1234")); //registering a user
            Console.WriteLine(gs.Login("add_owned_board@gmail.com", "Abc1234"));

            try //user's Email is not logged in
            {
               // Console.WriteLine(u.AddBoard("testaddboard@gmail.com", "BoardTest")); //NO FINISHED
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("testaddboard@gmail.com", "Abc1234"));

            try //adding board after user logged in - legal
            {

                Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Board name is already exist for this user
            {

                Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Email is null
            {

                Console.WriteLine(gs.AddBoard(null, "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Board name is null
            {

                Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            us.cleanUsers();
        }

        public void LoadDataRun()
        {
            Console.WriteLine("-----------Load Data Test-----------");
        }

        public void DeleteDataRun()
        {
            Console.WriteLine("-----------Delete Data Test-----------");
        }
    }
}
