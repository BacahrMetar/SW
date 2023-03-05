 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackTests.Tests
{
    public class BoardTests
    {
        private readonly BoardService bs;
        private readonly UserService us;
        private readonly TaskService ts;
        private readonly GradingService gs;

        public BoardTests()
        {
            this.gs = new GradingService();
            this.us = gs.getUs();
            this.bs = gs.getBs();
            this.ts = gs.getTs();
        }

        public void RunAllTests()
        {
            AddBoardRun();
            RemoveBoardRun();
            GetColumnRun();
            GetColumnNameRun();
            GetColumnLimitRun();
            LimitColumnRun();
            GetUserBoardsRun();
            GetBoardNameRun();
            JoinBoardRunTest();
            LeaveBoardRunTest();
            TransferOwnershipRunTest();
        }

        public void AddBoardRun()
        {
            Console.WriteLine("-----------Add Board Test-----------");          
            try //Email is null
            {

                Console.WriteLine(gs.AddBoard(null, "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

            try //username does not exist
            {

                Console.WriteLine(gs.AddBoard("testaddboard1@gmail.com", "name"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("testaddboard@gmail.com", "Abc1234"));
            Console.WriteLine(gs.Logout("testaddboard@gmail.com"));

            try //Board name is null
            {

                Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //user's Email is not logged in
            {
                Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("testaddboard@gmail.com", "Abc1234"));
            Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", "BoardTest"));

            try //Board name is already exist for this user
            {
                Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding board after user logged in - legal
            {
                Console.WriteLine(gs.AddBoard("testaddboard@gmail.com", "NewBoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        }

        public void RemoveBoardRun()
        {
            Console.WriteLine("-----------Remove Board Test-----------");

            try //username is null
            {

                Console.WriteLine(gs.RemoveBoard(null, "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //username does not exist
            {

                Console.WriteLine(gs.RemoveBoard("NOnExist@gmail.com", "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("testremoveboard@gmail.com", "Abc1234"));
            Console.WriteLine(gs.AddBoard("testremoveboard@gmail.com", "BoardTest"));

            try //boardName is null
            {

                Console.WriteLine(gs.RemoveBoard("testremoveboard@gmail.com", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Logout("testremoveboard@gmail.com"));
            try //user's Email is not logged in
            {

                Console.WriteLine(gs.RemoveBoard("testremoveboard@gmail.com", "BoardTest"));
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("testremoveboard@gmail.com", "Abc1234"));

            try //Board's name does not exist
            {

                Console.WriteLine(gs.RemoveBoard("testremoveboard@gmail.com", "NotExist"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(gs.Register("noOwner@gmail.com", "Abc1234"));
            Console.WriteLine(gs.JoinBoard("noOwner@gmail.com", 0));

            try //not board owner trying to remove a board
            {

                Console.WriteLine(gs.RemoveBoard("noOwner@gmail.com", "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //All condition are good and the board has removed
            {

                Console.WriteLine(gs.RemoveBoard("testremoveboard@gmail.com", "BoardTest"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        }

        public void GetColumnRun()
        {
            Console.WriteLine("-----------Get Column Test-----------");

            Console.WriteLine(gs.Register("testgetcolumn@gmail.com", "Ab12345"));
            Console.WriteLine(gs.AddBoard("testgetcolumn@gmail.com", "BoardTest"));

            try //username is null
            {

                Console.WriteLine(gs.GetColumn(null, "BoardTest", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

             try //board name is null
            {

                Console.WriteLine(gs.GetColumn("notexist.com", null, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //username does not exist
            {

                Console.WriteLine(gs.GetColumn("notexist.com", "BoardTest", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Logout("testgetcolumn@gmail.com"));

            try //user's Email is not logged in
            {

                Console.WriteLine(gs.GetColumn("testgetcolumn@gmail.com", "BoardTest", 2));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("testgetcolumn@gmail.com", "Ab12345"));

            try //Board's name does not exist
            {

                Console.WriteLine(gs.GetColumn("testgetcolumn@gmail.com", "NotExist", 2));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //column oridnal is illegal (should be 0 or 1 or 2)
            {

                Console.WriteLine(gs.GetColumn("testgetcolumn@gmail.com", "BoardTest", 4));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //legal input
            {

                Console.WriteLine(gs.GetColumn("testgetcolumn@gmail.com", "BoardTest", 2));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();

        }

        public void GetColumnNameRun()
        {
            Console.WriteLine("-----------Get Column Name Test-----------");

            try //username does not exist
            {
                Console.WriteLine(gs.GetColumnName("testgetcolumnname@gmail.com", "BoardTest", 3));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("testgetcolumnname@gmail.com", "Ab12345"));

            try //user's Email is not logged in
            {

                Console.WriteLine(gs.GetColumnName("testgetcolumnname@gmail.com", "BoardTest", 3));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("testgetcolumnname@gmail.com", "Ab12345"));

            try //Board's name does not exist
            {

                Console.WriteLine(gs.GetColumnName("testgetcolumnname@gmail.com", "BoardTest", 3));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AddBoard("testgetcolumnname@gmail.com", "BoardTest"));

            try //column is not illegal (should be 0-2)
            {

                Console.WriteLine(gs.GetColumnName("testgetcolumnname@gmail.com", "BoardTest", 3));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //changing column Ordinal
            {

                Console.WriteLine(gs.GetColumnName("testgetcolumnname@gmail.com", "BoardTest", 1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        }

        public void GetColumnLimitRun()
        {
            Console.WriteLine("-----------Get Column Limit Test-----------");

            try //username does not exist
            {

                Console.WriteLine(gs.GetColumnLimit("testgetcolumnlimit@gmail.com", "BoardTest", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("testgetcolumnlimit@gmail.com", "Ab12345"));
            Console.WriteLine(gs.Logout("testgetcolumnlimit@gmail.com"));

            try //user's Email is not logged in
            {

                Console.WriteLine(gs.GetColumnLimit("testgetcolumnlimit@gmail.com", "BoardTest", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(gs.Login("testgetcolumnlimit@gmail.com", "Ab12345"));

            try //Board's name does not exist
            {

                Console.WriteLine(gs.GetColumnLimit("testgetcolumnlimit@gmail.com", "nonExistingBoard", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AddBoard("testgetcolumnlimit@gmail.com", "BoardTest"));

            try //column oridnal is illegal (should be 0, 1 or 2)
            {

                Console.WriteLine(gs.GetColumnLimit("testgetcolumnlimit@gmail.com", "BoardTest", 4));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //legal request
            {

                Console.WriteLine(gs.GetColumnLimit("testgetcolumnlimit@gmail.com", "BoardTest", 2));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        
        }


        public void LimitColumnRun()
        {
            Console.WriteLine("-----------Limit Column Test-----------");

            Console.WriteLine(gs.Register("limitcolumntest@gmail.com", "Abc1234")); //registering a user (when user registers he is logged out)            
            Console.WriteLine(gs.AddBoard("limitcolumntest@gmail.com", "LimitColumnTestBoard")); //adding the user a board

            try //limiting backlog column to 3 task and adding 3 tasks to this column
            {

                Console.WriteLine(gs.LimitColumn("limitcolumntest@gmail.com", "LimitColumnTestBoard", 0, 3));
                Console.WriteLine(gs.AddTask("limitcolumntest@gmail.com", "LimitColumnTestBoard", "NEW TASK",
                    "A task made for testing limit column option", new DateTime(2023, 4, 27))); //adding a task to backlog column (task id is 1)
                Console.WriteLine(gs.AddTask("limitcolumntest@gmail.com", "LimitColumnTestBoard", "NEW TASK 2",
                    "A task made for testing limit column option", new DateTime(2023, 4, 27))); //adding a new task to backlog column (task id is 2)
                Console.WriteLine(gs.AddTask("limitcolumntest@gmail.com", "LimitColumnTestBoard", "NEW TASK 3",
                    "A task made for testing limit column option", new DateTime(2023, 4, 27))); //adding a new task to backlog column (task id is 3)
                Console.WriteLine("3 Tasks added successfully to backlog!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //changing limit to 1 (with already existing 3 tasks) - shouldn't be possible
            {

                Console.WriteLine(gs.LimitColumn("limitcolumntest@gmail.com", "LimitColumnTestBoard", 0, 1));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding another task to a full column (backlog)
            {

                Console.WriteLine(gs.AddTask("limitcolumntest@gmail.com", "LimitColumnTestBoard", "NEW TASK 4",
                    "A task made for testing limit column option", new DateTime(2023, 4, 27)));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Limit works!");
            }

            try //setting limit to -1 (no limit) and adding two more tasks
            {

                Console.WriteLine(gs.LimitColumn("limitcolumntest@gmail.com", "LimitColumnTestBoard", 0, -1));
                //adding a new task to backlog column (task id is 4)
                Console.WriteLine(gs.AddTask("limitcolumntest@gmail.com", "LimitColumnTestBoard", "NEW TASK 4",
                    "A task made for testing limit column option", new DateTime(2023, 4, 27)));
                //adding a new task to backlog column (task id is 5)
                Console.WriteLine(gs.AddTask("limitcolumntest@gmail.com", "LimitColumnTestBoard", "NEW TASK 5",
                   "A task made for testing limit column option", new DateTime(2023, 4, 27)));
                //Console.WriteLine("Task added successfully to backlog!");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Limit works!");
            }

            try //setting a limit with null email
            {

                Console.WriteLine(gs.LimitColumn(null, "LimitColumnTestBoard", 0, 5));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //setting a limit with null board name
            {
                Console.WriteLine(gs.LimitColumn("limitcolumntest@gmail.com", null, 0, 5));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //setting a limit to wrong column ordinal
            {

                Console.WriteLine(gs.LimitColumn("limitcolumntest@gmail.com", "LimitColumnTestBoard", 10, 1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //setting a negative limit
            {

                Console.WriteLine(gs.LimitColumn("limitcolumntest@gmail.com", "LimitColumnTestBoard", 0, -5));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

         
            gs.DeleteData();
        }
        public void GetUserBoardsRun()
        {
            Console.WriteLine("-----------Get User Boards Test-----------");

            Console.WriteLine(gs.Register("getuserboardstest@gmail.com", "Abc1234")); //registering a user (when user registers he is logged out)           
            Console.WriteLine(gs.AddBoard("getuserboardstest@gmail.com", "GetUserBoardsTestBoard1")); //adding the user a board
            Console.WriteLine(gs.AddBoard("getuserboardstest@gmail.com", "GetUserBoardsTestBoard2")); //adding the user a board

            try //null email
            {
                Console.WriteLine(gs.GetUserBoards(null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //user name does not exist
            {
                Console.WriteLine(gs.GetUserBoards("notexistinguser@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Logout("getuserboardstest@gmail.com"));

            try //user is not logged in
            {
                Console.WriteLine(gs.GetUserBoards("getuserboardstest@gmail.com"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("getuserboardstest@gmail.com", "Abc1234"));

            Console.WriteLine(gs.Register("getuserboardstest2@gmail.com", "Abc1234")); //registering another user


            try //user does not have boards
            {

                Console.WriteLine(gs.GetUserBoards("getuserboardstest2@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //valid 
            {
                Console.WriteLine(gs.GetUserBoards("getuserboardstest@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
         
        }
        public void GetBoardNameRun()
        {

            Console.WriteLine("-----------Get Board Name Test-----------");

            Console.WriteLine(gs.Register("getboardname@gmail.com", "Abc1234")); //registering a user         
            Console.WriteLine(gs.AddBoard("getboardname@gmail.com", "GetBoardNameTest")); //adding the user a board - (id=0)

            Console.WriteLine(gs.Logout("getboardname@gmail.com"));

          
            try //Board's id does not exist
            {
                Console.WriteLine(gs.GetBoardName(10));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Negative board id
            {
                Console.WriteLine(gs.GetBoardName(-21));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Valid
            {
                Console.WriteLine(gs.GetBoardName(0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //bs.cleanBoards();
            gs.DeleteData();
        }

        public void JoinBoardRunTest()
        {
            Console.WriteLine("-----------Join Board Test-----------");

            Console.WriteLine(gs.Register("joinboard@gmail.com", "Abc1234"));
            
            Console.WriteLine(gs.AddBoard("joinboard@gmail.com", "JoinBoardTest")); //adding the user a board (id=0)

            try //username to join is null
            {
                Console.WriteLine(gs.JoinBoard(null, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //username to join does not exist
            {
                Console.WriteLine(gs.JoinBoard("usertojoin@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("usertojoin@gmail.com", "Abc1234"));
            Console.WriteLine(gs.Logout("usertojoin@gmail.com"));

            try //user to join is not logged in
            {
                Console.WriteLine(gs.JoinBoard("usertojoin@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("usertojoin@gmail.com", "Abc1234"));

            try //Board's Id is illegal
            {
                Console.WriteLine(gs.JoinBoard("usertojoin@gmail.com", -5));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Board's Id does not exist
            {
                Console.WriteLine(gs.JoinBoard("usertojoin@gmail.com", 30));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(gs.JoinBoard("usertojoin@gmail.com", 0));

            try //User try to join a board he already member
            {
                Console.WriteLine(gs.JoinBoard("usertojoin@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Email is null
            {
                Console.WriteLine(gs.JoinBoard(null, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

          
            gs.DeleteData();
        }

        public void LeaveBoardRunTest()

        {
         
            Console.WriteLine("-----------Leave Board Test-----------");
          

            Console.WriteLine(gs.Register("leaveboard@gmail.com", "Abc1234"));
            Console.WriteLine(gs.AddBoard("leaveboard@gmail.com", "LeaveBoardTest")); //adding the user a board (id=0)

            try //username null
            {
                Console.WriteLine(gs.LeaveBoard(null, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //username does not exist
            {
                Console.WriteLine(gs.LeaveBoard("nonExistingUser@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.WriteLine(gs.Register("anotheruser@gmail.com", "Abc1234"));
            Console.WriteLine(gs.JoinBoard("anotheruser@gmail.com", 0)); //joining a user to board
            Console.WriteLine(gs.Logout("anotheruser@gmail.com"));

            try //user to leave is not logged in
            {
                Console.WriteLine(gs.LeaveBoard("anotheruser@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("anotheruser@gmail.com", "Abc1234"));

            try //Board's Id does not exist
            {
                Console.WriteLine(gs.LeaveBoard("anotheruser@gmail.com", 30));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try //Board's Id is illegal 
            {
                Console.WriteLine(gs.LeaveBoard("anotheruser@gmail.com", -1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("notbelong@gmail.com", "Abc1234"));
           

            try //user name does not belong to this board
            {
                Console.WriteLine(gs.LeaveBoard("notbelong@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try //Ownership try to leave a board
            {
                Console.WriteLine(gs.LeaveBoard("leaveboard@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try // all conditions are good and user has leaved the board succsessfuly
            {
                Console.WriteLine(gs.LeaveBoard("anotheruser@gmail.com", 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
          
            gs.DeleteData();

        }

        public void TransferOwnershipRunTest()
        {
           
            Console.WriteLine("-----------Transfer Ownership Test-----------");
           

            Console.WriteLine(gs.Register("from@gmail.com", "Abc1234"));            
            Console.WriteLine(gs.AddBoard("from@gmail.com", "transfer")); //adding the user a board (id=0)

            try //Curren owner username is null
            {
                Console.WriteLine(gs.TransferOwnership(null, "notexist@gmail.com", "transfer"));

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //new owner username is null
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", null, "transfer"));

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //username to transer does not exist
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "notexist@gmail.com", "transfer"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Logout("from@gmail.com"));

            try //username that transer does not Logged In
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "notexist@gmail.com", "transfer"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("to@gmail.com", "Abc1234"));
            Console.WriteLine(gs.Logout("to@gmail.com"));
            Console.WriteLine(gs.Login("from@gmail.com", "Abc1234"));



            try //board to transer is null
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "to@gmail.com", null));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //board to transer does not exist
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "to@gmail.com", "NotExist"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //user Try to transfer Ownership to himself
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "from@gmail.com", "transfer"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //user attempt to transfer ownership of a board he is not owner
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "to@gmail.com", "noOwneredByUser"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.WriteLine(gs.Login("to@gmail.com", "Abc1234"));
            Console.WriteLine(gs.AddBoard("from@gmail.com", "testSameBoard"));
            Console.WriteLine(gs.AddBoard("to@gmail.com", "testSameBoard"));

            try// from user try to transfe to use a board he is already has
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "to@gmail.com", "testSameBoard"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }      

            try// valid parameters
            {
                Console.WriteLine(gs.TransferOwnership("from@gmail.com", "to@gmail.com", "testTransferBoard"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        
            gs.DeleteData();
        }


    }

    
}
