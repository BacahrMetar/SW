using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardsWindow.xaml
    /// </summary>
    public partial class BoardsWindow : Window
    {
        private BoardsViewModel viewModel;
        private UserModel userModel;

        public BoardsWindow(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardsViewModel(u);
            this.userModel = u;
            this.DataContext = viewModel;
        }

        /// <summary>
        /// log out button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

       
        /// <summary>
        /// button to enter the user's board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Board(object sender, RoutedEventArgs e)
        {
            bool isSelected = viewModel.EnableForward;
            if (isSelected)
            {
                BoardView boardView = new BoardView(viewModel.SelectedBoard,userModel);
                boardView.Show();
                this.Close();
            }
        }
    }
}
