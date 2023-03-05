using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Moq;
using NUnit.Framework;

namespace NUnitTestProject1
{
    public class NUintUserTest
    {
        UserService us;
        UserController uc1;
        BoardService bs;
        Mock<UserController> userMock;
        Mock<UserController> userMock2;
        [SetUp]
        public void Setup()
        {
            us = new UserService();
            uc1 = new UserController();
            bs = new BoardService(new BoardController(uc1));
            userMock = new Mock<UserController>();
            userMock2 = new Mock<UserController>();
        }

        //===========================================================
        //=                  registration tests                     =
        //===========================================================       
        /// <summary>
        /// Test checking that registertion with valid parameters pass
        /// </summary>
        [Test]
        public void TestRegistertion_ValidParameters_returnPositive()
        {
            //Arrange
            String email = "testregistertion@gmail.com";
            string password = "Ab123456!";
            //Act

            //Assert
            Assert.DoesNotThrow(() => uc1.Register(email, password));

        }
        /// <summary>
        /// Test checking that registertion with Invalid password fail
        /// </summary>
        [Test]
        public void TestRegistertion_InValidPassword_returnNegative()
        {
            //Arrange
            String email = "testregistertion@gmail.com";
            string password = "a";
            //Act

            //Assert
            Assert.Throws<Exception>(() => uc1.Register(email, password));
         }
        /// <summary>
        /// Test checking that registertion with Invalid email fail
        /// </summary>
        [Test]
        public void TestRegistertion_EmailIsWrong_returnNegative()
        {
            //Arrange
            String email = "T.... estregistertion@gmail.com";
            string password = "Ab123456!";
            //Act

            //Assert
            Assert.Throws<Exception>(() => uc1.Register(email, password));
        }
        /// <summary>
        /// Test checking that registertion 2 user with same email is fail 
        /// </summary>
        [Test]
        public void TestRegistertion_EmailExist_returnNegative()
        {
            //Arrange
            String email = "testregistertion@gmail.com";
            string password = "Ab123456!";
            //Act
            uc1.Register(email, password);
            //Assert
            Assert.Throws<Exception>(() => uc1.Register(email, password));
        }

        //===========================================================
        //=                  Login tests                            =
        //===========================================================   

        /// <summary>
        /// Test checking that login new user with valid parametrs pass
        /// </summary>
        //[Test]
        //public void TestLogin22_ValidParameters_returnPositive()
        //{
        //    //Arrange
        //    String email = "testregistertion@gmail.com";
        //    string password = "Ab123456!";
        //    //Act
        //    uc1 = new UserController<userMock.object>();
        //    uc1.Register(email, password);
        //    uc1.Login(email, password);
        //    //Assert
        //    //uc1.Login(email,password);
        //    Assert.IsTrue(uc1.getUsers()[email].getIsLogged());

        //}
        [Test]
        public void TestLogin_ValidParameters_returnPositive()
        {
            //Arrange
            String email = "testregistertion@gmail.com";
            string password = "Ab123456!";
            //Act
            //uc1.Register(email, password);
            //var userLogged = userMock.Setup(m => m.IsLogged(email)).Returns(true);
            uc1.Register(email, password);
            uc1.Login(email, password);
            //Assert
            //uc1.Login(email,password);
            Assert.IsTrue(uc1.getUsers()[email].getIsLogged());
    
        }
        /// <summary>
        /// Test checking that login new user with non existing email fail
        /// </summary>
        [Test]
        public void TestLogin_NonExistingEmail_returnNegative()
        {
            //Arrange
            String email = "non_existing_email@gmail.com";
            string password = "Ab123456!";
            //Act
           
            //userMock.Setup(m => m.isExist(email)).Returns(true);
           
            //Assert
            //uc1.Login(email,password);
            Assert.Throws<Exception>(() => uc1.Login(email, password));
        }
        /// <summary>
        /// Test checking that login already logged in user fail
        /// </summary>
        [Test]
        public void TestLogin_AlreadyLoggedIn_returnNegative()
        {
            //Arrange
            String email = "alreadyloggedin@gmail.com";
            string password = "Ab123456!";
            //Act
            //userMock.Setup(m => m.Register(email,password));
            //userMock2.Setup(m => m.isLogged(email)).Returns(false);
            uc1.Register(email, password);
            uc1.Login(email, password);
            //Assert
            //uc1.Login(email,password);
            Assert.Throws<Exception>(() => uc1.Login(email, password));
        }

        //===========================================================
        //=                  LogOut tests                            =
        //=========================================================== 

        /// <summary>
        /// Test checking that logout user pass
        /// </summary>
        [Test]
        public void TestLogout__returnPositive()
        {
            //Arrange
            String email = "testlogout@gmail.com";
            string password = "Ab123456!";
            //Act
            //uc1.Register(email, password);
            //userMock.Setup(m => m.isExist(email)).Returns(true);
            //userMock.Setup(m => m.isExist(email)).Returns(true);
            uc1.Register(email, password);
            uc1.Login(email, password);
            uc1.Logout(email);
            
            //Assert

            Assert.IsFalse(uc1.getUsers()[email].getIsLogged());
        }

        /// <summary>
        /// Test checking that login new user with non existing email fail
        /// </summary>
        [Test]
        public void TestLogout_AlreadyLoggedOut_returnNegative()
        {
            //Arrange
            String email = "non_existing_email@gmail.com";
            string password = "Ab123456!";
            //Act

            //userMock.Setup(m => m.isExist(email)).Returns(true);
            uc1.Register(email, password);
            uc1.Login(email, password);
            uc1.Logout(email);
            //Assert

            Assert.Throws<Exception>(() => uc1.Logout(email));
        }

        //===========================================================
        //=                  IsLogged tests                            =
        //=========================================================== 

        /// <summary>
        /// Test checking that logged in user pass
        /// </summary>
        [Test]
        public void TestIsLogged__returnPositive()
        {
            //Arrange
            String email = "test_is_logged@gmail.com";
            string password = "Ab123456!";
            //Act

            //userMock.Setup(m => m.isExist(email)).Returns(true);
            uc1.Register(email, password);
            uc1.Login(email, password);
            bool check = uc1.isLogged(email);
            //Assert

            Assert.That(check == true);
        }
        /// <summary>
        /// Test checking that non login user fail
        /// </summary>
        [Test]
        public void TestIsLogged_noLogged_returnNegative()
        {
            //Arrange
            String email = "test_is_logged@gmail.com";
            string password = "Ab123456!";
            //Act

            //userMock.Setup(m => m.isExist(email)).Returns(true);
            uc1.Register(email, password);    
            bool check = uc1.isLogged(email);
            //Assert

            Assert.That(check == false);
        }
        //===========================================================
        //=                  IsExist tests                            =
        //=========================================================== 
        /// <summary>
        /// Test checking that an existing user exisit in thw user list pass
        /// </summary>
        [Test]
        public void TestIsExist__returnPositive()
        {
            //Arrange
            String email = "test_is_exist@gmail.com";
            string password = "Ab123456!";
            //Act

            //userMock.Setup(m => m.isExist(email)).Returns(true);
            uc1.Register(email, password);
            //uc1.Login(email, password);
            bool check = uc1.isExist(email);
            //Assert
  
            Assert.That(check == true);
        }
        /// <summary>
        /// Test checking that an non existing user  not in the user list fail
        /// </summary>
        [Test]
        public void TestIsExist__returnNegative()
        {
            //Arrange
            String email = "test_is_exist@gmail.com";
            //Act       
            bool check = uc1.isExist(email);
            //Assert
          
            Assert.That(check == false);
        }

    }
}