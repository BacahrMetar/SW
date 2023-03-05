using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Moq;
using NUnit.Framework;

namespace NUnitTestProject1
{
    public class NUintBoardTest
    {
        UserService us;
        UserController uc1;
        BoardService bs;
        Mock<UserController> Mock1;
        Mock<UserController> Mock2;
        [SetUp]
        public void Setup()
        {
            us = new UserService();
            uc1 = new UserController();
            bs = new BoardService(new BoardController(uc1));
            Mock1 = new Mock<UserController>();
            Mock2 = new Mock<UserController>();
        }

        //[Test]
        public void TestAddBoard_ValidParameters_returnPositive()
        {
            //Arrange
            String email = "test_add_board@gmail.com";
            String boardName = "board1";

            //Mock.Setup(m => m.isExist(email)).Returns(true);

            //Act

            //Assert
        }
    }
}