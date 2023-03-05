
using IntroSE.Kanban.Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;


namespace IntroSE.Kanban.Frontend.ViewModel

{
    public class BoardsViewModel : NotifiableObject
    {
        private BackendController controller;
        
       
        public UserModel User { get; private set; }
        private BoardModel _selectedBoard;



        public BoardsViewModel(UserModel user)
        {
            this.controller = user.Controller;
            User = user;

        }

        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }
        
        private bool _enableForward = false;
       /// <summary>
       /// getter
       /// </summary>
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
        
        /// <summary>
        /// method to logout the user from the system
        /// </summary>
        public void Logout()
        {
            try
            {
                controller.Logout(User.Email);
                MessageBox.Show("Logged out successfully.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot Logout the user. " + e.Message);
            }
        }
    }
}