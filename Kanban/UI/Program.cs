using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.BackTests.Tests;
using System.Text.RegularExpressions;
using System.Text.Json;
using Task = IntroSE.Kanban.Backend.BusinessLayer.Task;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.BackTests
{
    public class Program
    {

        static void Main(string[] args)
        {
            //GradingService grading = new GradingService();
            //Console.WriteLine(grading.DeleteData());

            // RegisterTests(grading);
            // LoginTests(grading);
            // LogoutTests(grading);
            // AddBoardTests(grading);
            // AddTaskTests(grading);
            // GetColumnNameTests(grading);
            // GetColumnLimitTests(grading);
            // LimitColumnTests(grading);
            //AssignTaskTests(grading);
            //UpdateTaskDescriptionTests(grading);
            // UpdateTaskDueDateTests(grading);
            //UpdateTaskTitleTests(grading);
            //AdvanceTaskTests(grading);
            //GetColumnTests(grading);
            // RemoveBoardTests(grading);
            // InProgressTasksTests(grading);
            // GetUserBoardsTests(grading);
            // JoinBoardTests(grading);
            // LeaveBoardTests(grading);
            // TransferOwnershipTests(grading);


            // legal register
            //Console.WriteLine(grading.Register("Register1@email.com", "Register1"));

            //// email already exists
            //Console.WriteLine(grading.Register("Register1@email.com", "Register1"));

            //// illegal emails
            //Console.WriteLine(grading.Register("noShtroudle", "Password1"));
            //Console.WriteLine(grading.Register("tooManyCharacters" +
            //                                    "tooManyCharacters" +
            //                                    "tooManyCharacters" +
            //                                    "tooManyCharacters" +
            //                                    "tooManyCharacters" +
            //                                    "tooManyCharacters" +
            //                                    "tooManyCharacters" +
            //                                    "@email.com", "Password1"));

            // illegal password
            //Console.WriteLine(/*grading*/.Register("metar@email.com", "Aa123456"));
            //Console.WriteLine(grading.Register("email2@email.com", "Aa1Aa1Aa1Aa1Aa1Aa1Aa1"));
            //Console.WriteLine(grading.Register("email3@email.com", "password3"));
            //Console.WriteLine(grading.Register("email4@email.com", "PASSWORD3"));
            //Console.WriteLine(grading.Register("email5@email.com", "Password"));

            GradingService gs = new GradingService();


            //UserService us = new UserService();
            // BoardService bs = us.GetBoardService();
            //TaskService ts = bs.GetTaskService();

            //UserTests ut = new UserTests();
            //BoardTests bt = new BoardTests();
            //TasksTests tt = new TasksTests();


            // ut.RunAllTests();
            //  bt.RunAllTests();
            //  tt.RunAllTests();

            //ut.RunAllTests();
            //bt.RunAllTests();
            //tt.RunAllTests();


            //Console.writeline(gs.register("vladik@gmail.com", "ab1234653"));
            //gs.Logout("vladik@gmail.com");
            //Receiver a = JsonSerializer.Deserialize<Receiver>(gs.Login("vladik@gmail.com", "Ab1234653"));

            Console.WriteLine(gs.Logout("vladik@gmail.com"));
            //Console.WriteLine(gs.Login("vladik@gmail.com", "Abc2123"));
            //Console.WriteLine(gs.AddBoard("vladik@gmail.com", "NewBoard"));
            //Console.WriteLine(gs.GetColumnName("vladik@gmail.com", "NewBoard6", 0));
            ////Console.WriteLine(gs.RemoveBoard("vladik@gmail.com", "NewBoard"));
            //Console.WriteLine(gs.AddTask("vladik@gmail.com", "NewBoard", "Title", "desc", new DateTime(2023, 4, 27)));
            //Console.WriteLine(gs.AddTask("vladik@gmail.com", "NewBoard", "Title2", "desc2", new DateTime(2023, 4, 27)));
            //Console.WriteLine(gs.GetColumn("vladik@gmail.com", "NewBoard", 0));
            //Console.WriteLine(gs.GetColumnName("vladik@gmail.com", "NewBoard", 0));
            //Console.WriteLine(gs.GetColumnLimit("vladik@gmail.com", "NewBoard", 0));
            //Console.WriteLine(gs.LimitColumn("vladik@gmail.com", "NewBoard", 0, 5));
            //Console.WriteLine(gs.GetColumnLimit("vladik@gmail.com", "NewBoard", 0));
            //Console.WriteLine(gs.AdvanceTask("vladik@gmail.com", "NewBoard", 0, 1));
            //Console.WriteLine(gs.GetColumn("vladik@gmail.com", "NewBoard", 0));
            //Console.WriteLine(gs.GetColumn("vladik@gmail.com", "NewBoard", 1));

            //Console.WriteLine(gs.UpdateTaskDescription("vladik@gmail.com", "NewBoard", 1, 1, "WOW NEW DESCRIPTION"));
            //Console.WriteLine(gs.UpdateTaskDueDate("vladik@gmail.com", "NewBoard", 1, 1, new DateTime(5555, 4, 27)));
            //Console.WriteLine(gs.UpdateTaskTitle("vladik@gmail.com", "NewBoard", 1, 1, "WOW NEW TITLE"));

            //Console.WriteLine(gs.GetColumn("vladik@gmail.com", "NewBoard", 1));
            //s
            //Console.WriteLine(gs.AddBoard("vladik@gmail.com", "NewBoard2"));
            //Console.WriteLine(gs.AddTask("vladik@gmail.com", "NewBoard2", "Title3", "desc3", new DateTime(2023, 4, 27)));
            //Console.WriteLine(gs.AdvanceTask("vladik@gmail.com", "NewBoard2", 0, 2));
            //Console.WriteLine(gs.AddTask("vladik@gmail.com", "NewBoard2", "Title4", "desc5", new DateTime(2023, 4, 27)));
            //Console.WriteLine(gs.AdvanceTask("vladik@gmail.com", "NewBoard2", 0, 3));
            //Console.WriteLine(gs.LimitColumn("vladik@gmail.com", "NewBoard2", 1, 1));

            //Console.WriteLine(gs.InProgressTasks("vladik@gmail.com"));




            //Console.WriteLine(gs.Register("vladik@gmail.com", "Abc123"));
            //ut.RegisterRun();
            //ut.RunAllTests();
            //bt.RunAllTests();
            //tt.RunAllTests();
            //User u = new User("vladik@gmail.com", "Abc123");
            //Response<User> r2 = new Response<User>(u);
            //Task t = new Task("Task1", new DateTime(2023, 4, 27), 5);
            // Board b = new Board("NewBoard");
            //b.AddTask(t);


            //string json1 = JsonSerializer.Serialize(r2);
            //Console.WriteLine(json1);
            //string json2 = JsonSerializer.Serialize(b);
            //Console.WriteLine(json);
            //Console.WriteLine(json2);
            //User d = JsonSerializer.Deserialize<User>(json);
            //Console.WriteLine("" + d.Password);
            //Response<User> r = new Response<User>(u);
            //string json = JsonSerializer.Serialize(r);
            //Console.WriteLine(json);

            //DATA LAYER
            /*
           UsersDalController usersController = new UsersDalController();
           UserDTO user = new UserDTO(0, "Our Forum");
           bool ans = true;// forumController.Delete(forum);
           List<ForumDTO> forums = forumController.SelectAllForums();
           foreach (ForumDTO forumDTO in forums)
           {
               Console.WriteLine(forumDTO.Name);
           }
           Console.WriteLine(ans);
           ans = forumController.Insert(forum);
           Console.WriteLine(ans);

           MessageDalController messageController = new MessageDalController();
           MessageDTO message = new MessageDTO(1, "Hello Hi2", "This is my body", forum.Id);
           messageController.Delete(message);
           messageController.Insert(message);

           List<MessageDTO> messages = messageController.SelectAllMessages();
           foreach (MessageDTO m in messages)
           {
               Console.WriteLine(m.Title);
           }
            */


            // us.LoadData();

            //us.DeleteDataDB();  //Delete Data from all database tables 
            //us.DeleteData();
            //Console.WriteLine(gs.Register("TestRegister@gmail.com", "Abc1234"));

            //Console.WriteLine(gs.DeleteData());
            //Console.WriteLine(gs.Register("jony@gmail.com", "Abc1234"));
            //Console.WriteLine(gs.Register("Metar@gmail.com", "Abc1234"));

            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board1"));
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board2"));
            //Console.WriteLine(gs.AddTask("jony@gmail.com", "Board1", "Task1", "", DateTime.MaxValue));
            //Console.WriteLine(gs.JoinBoard("metar@gmail.com", 0));
            //Console.WriteLine(gs.LeaveBoard("metar@gmail.com", 0));
            //gs.DeleteData();




            //Console.WriteLine(gs.AddTask("jony@gmail.com", "Board1", "Task2", "", DateTime.MaxValue));
            //Console.WriteLine(gs.GetColumn("jony@gmail.com", "Board1", 0));
            //Console.WriteLine(gs.GetUserBoards("jony@gmail.com"));
            //Console.WriteLine(gs.GetUserBoards("jony@gmail.com"));


            //Console.WriteLine(gs.DeleteData());
            //Console.WriteLine(gs.Register("jony@gmail.com", "Abc1234"));
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board1"));
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board2"));
            //Console.WriteLine(gs.AddTask("jony@gmail.com", "Board1","Task1","",DateTime.MaxValue));

            //Console.WriteLine(gs.AddTask("jony@gmail.com", "Board1", "Task2", "", DateTime.MaxValue));
            //Console.WriteLine(gs.GetColumn("jony@gmail.com", "Board1",0));
            //Console.WriteLine(gs.GetUserBoards("jony@gmail.com"));



            //===========TESTS======================================================================================
            //Console.WriteLine(gs.DeleteData());
            //Console.WriteLine(gs.LoadData());

            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board1"));

            //////Console.WriteLine(gs.LoadData());
            //Console.WriteLine(gs.Register("jony@gmail.com", "Abc1234"));
            ////Console.WriteLine(gs.Logout("jony@gmail.com"));
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board1"));
            ////Console.WriteLine(gs.DeleteData());


            ////Console.WriteLine(gs.Login("jony@gmail.com", "Abc1234"));

            //////null email
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board1"));

            //////null boardname
            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", ""));

            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board1"));

            //////not existing board 
            ////Console.WriteLine(gs.RemoveBoard("jony@gmail.com", "not"));





            //////Console.WriteLine(gs.Login("jony@gmail.com", "Abc1234"));
            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board1"));
            ////Console.WriteLine(gs.AddTask("jony@gmail.com", "Board1","title1","description1",DateTime.MaxValue));
            ////Console.WriteLine(gs.AssignTask("jony@gmail.com", "Board1", 0, 0, "jony@gmail.com"));
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board2"));
            ////Console.WriteLine(gs.AddTask("jony@gmail.com", "Board2", "title12","", DateTime.MaxValue));
            ////Console.WriteLine(gs.AssignTask("jony@gmail.com", "Board2", 0, 1, "jony@gmail.com"));
            ////Console.WriteLine(gs.AdvanceTask("jony@gmail.com", "Board2", 0, 1));
            ////Console.WriteLine(gs.AdvanceTask("jony@gmail.com", "Board1", 0, 0));
            ////Console.WriteLine(gs.InProgressTasks("jony@gmail.com"));
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board3"));

            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board4"));

            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board5"));

            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board6"));
            ////Console.WriteLine(gs.AddBoard("jony12@gmail.com", "Board6"));


            //////Console.WriteLine(gs.RemoveBoard("jony@gmail.com", "Board3"));
            //////user is LOgout
            ////Console.WriteLine(gs.Logout("jony@gmail.com"));
            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board7"));
            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board8"));

            ////Console.WriteLine(gs.Login("jony@gmail.com", "Abc1234"));

            ////Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board7"));
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board8"));

            ////Boardn Name is null
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", ""));
            ////boardName is alredy exist
            //Console.WriteLine(gs.AddBoard("jony@gmail.com", "Board8"));
            ////no such a username
            //Console.WriteLine(gs.GetUserBoards("jony11@gmail.com"));
            //Console.WriteLine(gs.GetUserBoards("jony@gmail.com"));


            //===========TESTS======================================================================================


            //Console.WriteLine(gs.RemoveBoard("samanta@gmail.com", "another board"));
            //Console.WriteLine(gs.AssignTask("ofek@gmail.com", "another board",1,3, "samanta@gmail.com"));
            //Console.WriteLine(gs.AdvanceTask("jony@gmail.com", "my favorite board", 0, 7));
            //Console.WriteLine(gs.AssignTask("kobi@gmail.com", "another board",0,1, "ofek@gmail.com"));
            //Console.WriteLine(gs.Register())
            //Console.WriteLine(gs.Register("dory2@gmail.com", "Aa123456"));
            // Console.WriteLine(gs.AddBoard("dory2@gmail.com", "Dorys board"));


            //Console.WriteLine(gs.Login("kobi@gmail.com", "Aa123456789"));
            //Console.WriteLine(gs.TransferOwnership("kobi@gmail.com", "metar@gmail.com", "my favorite board"));

            // Console.WriteLine(gs.Login("metar@gmail.com", "Aa12341234"));
            //Console.WriteLine(gs.Login("metar@gmail.com", "Aa12341234"));
            //Console.WriteLine(gs.LeaveBoard("kobi@gmail.com", "metar@gmail.com", "my favorite board"));
            //  Console.WriteLine(gs.LeaveBoard("metar@gmail.com",1));
            //Console.WriteLine(gs.LeaveBoard("metar@gmail.com",1));

            //Console.WriteLine(gs.AssignTask("kobi@gmail.com", "another board",0,1, "ofek@gmail.com"));
            //Console.WriteLine(gs.Register())
            //Console.WriteLine(gs.Register("dory2@gmail.com", "Aa123456"));
            // Console.WriteLine(gs.AddBoard("dory2@gmail.com", "Dorys board"));\
            //DateTime thisDate1 = new DateTime(2023, 6, 10);
            //Console.WriteLine(gs.UpdateTaskDueDate("kobi@gmail.com", "my favorite board", 0, 1, thisDate1));

            //Console.WriteLine(gs.DeleteData());

            //GradingService gs = new GradingService();
            //Console.WriteLine(gs.LoadData());
            //Console.WriteLine(gs.Login("dory@gmail.com", "Aa123456"));
            //Console.WriteLine(gs.AddBoard("dory@gmail.com", "another board"));
            //Console.WriteLine(gs.JoinBoard("dory@gmail.com", 2));

            //GradingService gs = new GradingService();
            //Console.WriteLine(gs.LoadData());
            //Console.WriteLine(gs.Login("dory@gmail.com", "Aa123456"));
            //Console.WriteLine(gs.AddBoard("dory@gmail.com", "another board"));
            //Console.WriteLine(gs.JoinBoard("dory@gmail.com", 2));


            //    Console.WriteLine("-----------Update Task Title Test-----------");

            //string TooLongTitle = "";

            //for (int i = 0; i < 10; i++)
            //{
            //    TooLongTitle += "newTitle"; //new Title with length of 80 chars (max should be 50) 
            //}

            //try //username does not exist
            //{
            //    Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "BoardTest", 0, 0, "newTitle"));
            //    Console.WriteLine("Task title has been updated successfuly!");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //Console.WriteLine(gs.Register("testupdatetasktitle@gmail.com", "Ab12345")); //registered user is logged out
            Console.WriteLine("test");
        }
    }
}
