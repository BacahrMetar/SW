using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackTests.Tests
{
    public class TasksTests
    {
        private BoardService bs;
        private UserService us;
        private TaskService ts;
        private readonly GradingService gs;

        public TasksTests()
        {
            this.gs = new GradingService();
            this.us = gs.getUs();
            this.bs = gs.getBs();
            this.ts = gs.getTs();
        }

        public void RunAllTests()
        {
            AddTaskRun();
            AdvanceTaskRun();
            UpdateDescriptionRun();
            UpdateTaskDueDateRun();
            UpdateTaskTitleRun();
            InProgressTasksRun();
            AssignTaskRun();
        }

        public void AddTaskRun()
        {
            Console.WriteLine("-----------Add Task Test-----------");
            string LongTitle = "";
            string LongDescription = "";
            for (int i = 0; i < 60; i++)
            {
                LongTitle += "a"; //will end up with 60 characters (Max 50)
                LongDescription += "abcdef"; //will end up with 360 characters (Max 300)
            }

            Console.WriteLine(gs.Register("addtasktest@gmail.com", "Abc1234")); //registering a user 
            Console.WriteLine(gs.AddBoard("addtasktest@gmail.com", "AddTaskTestBoard")); //adding the user a board

            try //adding task to existing board (to backlog column)
            {
                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", "NEW TASK", "A task made for testing", new DateTime(2023, 4, 27)));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.LimitColumn("addtasktest@gmail.com", "AddTaskTestBoard", 0, 1); //setting limit on backlog column

            try //adding a task to full column board
            {
                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", "NEW TASK 2", "A task made for testing", new DateTime(2023, 4, 27)));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.LimitColumn("addtasktest@gmail.com", "AddTaskTestBoard", 0, 10)); //increasing the limit of column

            try //adding a task with too long title
            {
                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", LongTitle, "Long Title test", new DateTime(2023, 4, 27)));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task with empty title
            {

                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", "", "Empty Title test", new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task with too long description
            {

                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", "New Task 3", LongDescription, new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //adding a task to non existing email
            {

                Console.WriteLine(gs.AddTask("nonExisting@gmail.com", "AddTaskTestBoard", "NEW TASK 4", "Null email test", new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try //adding a task to null email
            {

                Console.WriteLine(gs.AddTask(null, "AddTaskTestBoard", "NEW TASK 4", "Null email test", new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task to non existing board name
            {

                Console.WriteLine(gs.AddTask("nonExisting@gmail.com", "NonExistingBoard", "NEW TASK 4", "Null email test", new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task to null board name
            {

                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", null, "NEW TASK 5", "Null board name test", new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task with null title
            {

                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", null, "Null title test", new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task with null description
            {

                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", "NEW TASK 6", null, new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task with expired date 
            {

                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", "NEW TASK 6", null, DateTime.MinValue));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //adding a task with Valid paramenters
            {

                Console.WriteLine(gs.AddTask("addtasktest@gmail.com", "AddTaskTestBoard", "NEW TASK 6", "Description", new DateTime(2023, 4, 27)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            gs.DeleteData();
        }

        public void AdvanceTaskRun()
        {
            Console.WriteLine("-----------Advance Task Test-----------");
            Console.WriteLine(gs.Register("advancetasktest@gmail.com", "Abc1234")); //registering a user (when user registers he is logged out)
            Console.WriteLine(gs.AddBoard("advancetasktest@gmail.com", "AdvanceTaskTestBoard")); //adding the user a board
            Console.WriteLine(gs.AddTask("advancetasktest@gmail.com", "AdvanceTaskTestBoard", "NEW TASK", "A task made for testing advance option"
                , new DateTime(2023, 4, 27))); //adding a task to backlog column (task id is 0)

            try //advance task from backlog to in progress
            {

                Console.WriteLine(gs.AdvanceTask("advancetasktest@gmail.com", "AdvanceTaskTestBoard", 0, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.LimitColumn("advancetasktest@gmail.com", "AdvanceTaskTestBoard", 2, 0)); //limiting the done column so it can't take tasks at all

            try //advance task from in progress to done ("full" column)
            {

                Console.WriteLine(gs.AdvanceTask("advancetasktest@gmail.com", "AdvanceTaskTestBoard", 1, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.LimitColumn("advancetasktest@gmail.com", "AdvanceTaskTestBoard", 2, 10)); //extending done limit

            try //advance task from in progress to done
            {

                Console.WriteLine(gs.AdvanceTask("advancetasktest@gmail.com", "AdvanceTaskTestBoard", 1, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //advance task from done (shouldn't be possible)
            {

                Console.WriteLine(gs.AdvanceTask("advancetasktest@gmail.com", "AdvanceTaskTestBoard", 2, 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AddTask("advancetasktest@gmail.com", "AdvanceTaskTestBoard", "NEW TASK 1", "A task made for testing advance option",
                new DateTime(2023, 4, 27))); //adding a new task to backlog column (task id is 1)

            try //advance task with null email
            {

                Console.WriteLine(gs.AdvanceTask(null, "AdvanceTaskTestBoard", 0, 1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //advance task with null boardName
            {

                Console.WriteLine(gs.AdvanceTask("advancetasktest@gmail.com", null, 0, 1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //advance task with not existing task id
            {

                Console.WriteLine(gs.AdvanceTask("advancetasktest@gmail.com", "AdvanceTaskTestBoard", 0, 10));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            bs.cleanBoards();
        }

        public void UpdateDescriptionRun()
        {
            Console.WriteLine("-----------Update Task Description Test-----------");

            string TooLongDescription = "";

            for (int i = 0; i < 30; i++)
            {
                TooLongDescription += "NewDescription"; //new Description with lenght of 420 chars(max should be 300) 
            }


            try //username does not exist
            {

                Console.WriteLine(gs.UpdateTaskDescription("testupdatetaskdescription@gmail.com", "BoardTest", 0, 0, "newDescription"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("testupdatetaskdescription@gmail.com", "Ab12345")); //registered user is logged out
            Console.WriteLine(gs.AddBoard("testupdatetaskdescription@gmail.com", "newBoard")); //adding board to user
            Console.WriteLine(gs.AddTask("testupdatetaskdescription@gmail.com", "newBoard", "NEW TASK", "A task made for testing", new DateTime(2023, 4, 27))); //adding task (id 0)

            try //Board's name does not exist
            {

                Console.WriteLine(gs.UpdateTaskDescription("testupdatetaskdescription@gmail.com", "NotExistingBoard", 0, 0, "newDescription"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Task's ID is not exist
            {

                Console.WriteLine(gs.UpdateTaskDescription("testupdatetaskdescription@gmail.com", "newBoard", 0, 10, "newDescription"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //description's length is more than max length (300 chars)
            {

                Console.WriteLine(gs.UpdateTaskDescription("testupdatetaskdescription@gmail.com", "newBoard", 0, 0, TooLongDescription));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //legal update
            {

                Console.WriteLine(gs.UpdateTaskDescription("testupdatetaskdescription@gmail.com", "newBoard", 0, 0, "updatedDescription"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AdvanceTask("testupdatetaskdescription@gmail.com", "newBoard", 0, 0)); // advancing task "BackLog -> In Progress"
            Console.WriteLine(gs.AdvanceTask("testupdatetaskdescription@gmail.com", "newBoard", 1, 0)); // advancing task "In Progress -> Done"

            try //update a task in done column is illegal
            {
                Console.WriteLine(gs.UpdateTaskDescription("testupdatetaskdescription@gmail.com", "newBoard", 2, 0, "updatedDescription2"));
                //Console.WriteLine("Task description has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //email is null
            {
                Console.WriteLine(gs.UpdateTaskDescription(null, "newBoard", 0, 0, "newDescription"));
                // Console.WriteLine("Task description has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //board name is null
            {
                Console.WriteLine(gs.UpdateTaskDescription("TestUpdateTaskDescription@gmail.com", null, 0, 0, "newDescription"));
                //Console.WriteLine("Task description has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //new description is null
            {
                Console.WriteLine(gs.UpdateTaskDescription("TestUpdateTaskDescription@gmail.com", "newBoard", 0, 0, null));
                //Console.WriteLine("Task description has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        }

        public void UpdateTaskDueDateRun()
        {
            Console.WriteLine("-----------Update Task Due Date Test-----------");

            try //username does not exist
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", "BoardTest", 0, 0, new DateTime(2023, 4, 27)));
                //Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try //username is null
            {
                Console.WriteLine(gs.UpdateTaskDueDate(null, "BoardTest", 0, 0, new DateTime(2023, 4, 27)));
                //Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("updateduedatetest@gmail.com", "Abc1234")); //registering a user
            Console.WriteLine(gs.AddBoard("updateduedatetest@gmail.com", "UpdateDueDateTestBoard")); //adding the user a board
            Console.WriteLine(gs.AddTask("updateduedatetest@gmail.com", "UpdateDueDateTestBoard", "NEW TASK", "A task made for testing due date change", new DateTime(2023, 4, 27))); //adding a task

            try //Board's name is null
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", null, 0, 0, new DateTime(2023, 4, 27)));
                // Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Board's name does not exist
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", "NotExistingBoard", 0, 0, new DateTime(2023, 4, 27)));
                // Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Task's ID is not exist
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", "UpdateDueDateTestBoard", 0, 10, new DateTime(2023, 4, 27)));
                //Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //changing due date to earlier date (date that was already passed)
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", "UpdateDueDateTestBoard", 0, 0, new DateTime(1990, 2, 20)));
                //Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AdvanceTask("updateduedatetest@gmail.com", "UpdateDueDateTestBoard", 0, 0)); //advancing task backlog -> in progress
            Console.WriteLine(gs.AdvanceTask("updateduedatetest@gmail.com", "UpdateDueDateTestBoard", 1, 0)); //advancing task in progress -> done

            try //changing due date in done column (user cannot edit this column)
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", "UpdateDueDateTestBoard", 2, 0, new DateTime(2024, 5, 31)));
                Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //changing due date with null email
            {
                Console.WriteLine(gs.UpdateTaskDueDate(null, "UpdateDueDateTestBoard", 0, 0, new DateTime(2024, 5, 31)));
                Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //changing due date with null board name
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", null, 0, 0, new DateTime(2024, 5, 31)));
                Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Valid Paramters
            {
                Console.WriteLine(gs.UpdateTaskDueDate("updateduedatetest@gmail.com", "UpdateDueDateTestBoard", 0, 0, new DateTime(2024, 5, 31)));
                Console.WriteLine("Task due date updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        }

        public void UpdateTaskTitleRun()
        {
            Console.WriteLine("-----------Update Task Title Test-----------");

            string TooLongTitle = "";

            for (int i = 0; i < 10; i++)
            {
                TooLongTitle += "newTitle"; //new Title with length of 80 chars (max should be 50) 
            }

            try //username does not exist
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "BoardTest", 0, 0, "newTitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("testupdatetasktitle@gmail.com", "Ab12345")); //registered user is logged out
            Console.WriteLine(gs.AddBoard("testupdatetasktitle@gmail.com", "newBoard")); //adding board to user
            Console.WriteLine(gs.AddTask("testupdatetasktitle@gmail.com", "newBoard", "NEW TASK", "A task made for testing", new DateTime(2023, 4, 27))); //adding task (id 0)


            try //Board's name is null
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", null, 0, 0, "newTitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Board's name does not exist
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "NotExistingBoard", 0, 0, "newTitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Task's ID is not exist
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "newBoard", 0, 10, "newTitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //title's length is more than max length (80 chars)
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "newBoard", 0, 0, TooLongTitle));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //legal update
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "newBoard", 0, 0, "updatedtitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AdvanceTask("testupdatetasktitle@gmail.com", "newBoard", 0, 0)); // advancing task "BackLog -> In Progress"
            Console.WriteLine(gs.AdvanceTask("testupdatetasktitle@gmail.com", "newBoard", 1, 0)); // advancing task "In Progress -> Done"

            try //update a task in done column is illegal
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "newBoard", 2, 0, "updatedtitle2"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //email is null
            {
                Console.WriteLine(gs.UpdateTaskTitle(null, "newBoard", 2, 0, "newTitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //board name is null
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", null, 2, 0, "newTitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //new title is null
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "newBoard", 2, 0, null));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //valid parametrs 
            {
                Console.WriteLine(gs.UpdateTaskTitle("testupdatetasktitle@gmail.com", "newBoard", 2, 0, "newTitle"));
                Console.WriteLine("Task title has been updated successfuly!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        }

        public void InProgressTasksRun()
        {
            Console.WriteLine("-----------In Progress Tasks Test-----------");

            try //username does not exist
            {
                Console.WriteLine(gs.InProgressTasks("testinprogresstasks@gmail.com"));
                //Console.WriteLine("In Progress tasks has been returned successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("testinprogresstasks@gmail.com", "Ab12345"));
            //Console.WriteLine(gs.Login("testinprogresstasks@gmail.com", "Ab12345"));

            try //user doesnt have any board
            {
                Console.WriteLine(gs.InProgressTasks("testinprogresstasks@gmail.com"));
                //Console.WriteLine("In Progress tasks has been returned successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AddBoard("testinprogresstasks@gmail.com", "TestBoard"));

            try //empty in progress list
            {
                Console.WriteLine(gs.InProgressTasks("testinprogresstasks@gmail.com"));
                //Console.WriteLine("In Progress tasks has been returned successfully!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //Email is null
            {
                Console.WriteLine(gs.InProgressTasks(null));
                //Console.WriteLine("In Progress tasks has been returned successfully!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.AddTask("testinprogresstasks@gmail.com", "TestBoard", "newTask", "desc1", new DateTime(2023, 5, 27))); // task id is 0
            Console.WriteLine(gs.AddTask("testinprogresstasks@gmail.com", "TestBoard", "newTask2", "desc2", new DateTime(2023, 5, 27))); //task id is 1
            Console.WriteLine(gs.AddTask("testinprogresstasks@gmail.com", "TestBoard", "newTask3", "desc3", new DateTime(2023, 5, 27))); //task id is 2
            Console.WriteLine(gs.AdvanceTask("testinprogresstasks@gmail.com", "TestBoard", 0, 0));
            Console.WriteLine(gs.AdvanceTask("testinprogresstasks@gmail.com", "TestBoard", 0, 1));
            Console.WriteLine(gs.AdvanceTask("testinprogresstasks@gmail.com", "TestBoard", 0, 2));

            Console.WriteLine(gs.AddBoard("testinprogresstasks@gmail.com", "TestBoard2"));
            Console.WriteLine(gs.AddTask("testinprogresstasks@gmail.com", "TestBoard2", "newTask4", "desc4", new DateTime(2023, 5, 27))); //task id is 3
            Console.WriteLine(gs.AdvanceTask("testinprogresstasks@gmail.com", "TestBoard2", 0, 3));
            try //4 tasks in progress from 2 different boards
            {
                Console.WriteLine(gs.InProgressTasks("testinprogresstasks@gmail.com"));
                //Console.WriteLine("In Progress tasks has been returned successfully!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



            gs.DeleteData();

        }

        public void AssignTaskRun()
        {
            Console.WriteLine("-----------Assign Task Test-----------");

            Console.WriteLine(gs.Register("assigntest@gmail.com", "Ab12345"));
            Console.WriteLine(gs.AddBoard("assigntest@gmail.com", "assignBoard"));
            Console.WriteLine(gs.AddTask("assigntest@gmail.com", "assignBoard", "newTask", "desc1", new DateTime(2023, 5, 27))); // task id is 0

            try //assigned email is null
            {
                Console.WriteLine(gs.AssignTask(null, "assignBoard", 0, 0, "assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //user trying to assign task to null userEmail
            {
                Console.WriteLine(gs.AssignTask("assigntest@gmail.com", "assignBoard", 0, 0, null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //username does not exist
            {
                Console.WriteLine(gs.AssignTask("assigntest@gmail.com", "assignBoard",0,0,"assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Register("assinged@gmail.com", "Ab12345"));
            Console.WriteLine(gs.JoinBoard("assinged@gmail.com", 0));
            Console.WriteLine(gs.Logout("assinged@gmail.com"));
            


            try //assigned user is not logged in
            {
                Console.WriteLine(gs.AssignTask("assigntest@gmail.com", "assignBoard", 0, 0, "assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(gs.Login("assinged@gmail.com", "Ab12345"));

            try //board is null
            {
                Console.WriteLine(gs.AssignTask("assigntest@gmail.com", null, 0, 0, "assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            try //board not exist
            {
                Console.WriteLine(gs.AssignTask("assigntest@gmail.com", "NotExist", 0, 0, "assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //assigned user has no boards 
            {
                Console.WriteLine(gs.AssignTask("assigntest@gmail.com", "NotExist", 0, 0, "assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(gs.AddBoard("assinged@gmail.com", "newAssignBoard"));
            Console.WriteLine(gs.AssignTask("assigntest@gmail.com", "newAssignBoard", 0, 0, "assinged@gmail.com"));
            Console.WriteLine(gs.Register("notAssignee@gmail.com", "Ab12345"));


            try //not assignee user try to assign a task
            {
                Console.WriteLine(gs.AssignTask("notAssignee@gmail.com", "newAssignBoard", 1, 0, "assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try //trying to assign a task to a user that not exist in the board
            {
                Console.WriteLine(gs.AssignTask("assinged@gmail.com", "newAssignBoard", 1, 0, "notAssignee@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try //id not exist
            {
                Console.WriteLine(gs.AssignTask("assigntest@gmail.com", "assignBoard", 0, 30, "assinged@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            Console.WriteLine(gs.JoinBoard("notAssignee@gmail.com", 1));


            try //Valid parametres
            {
                Console.WriteLine(gs.AssignTask("assinged@gmail.com", "newAssignBoard", 1, 0, "notAssignee@gmail.com"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gs.DeleteData();
        }
    }
}
