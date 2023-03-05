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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;
        private UserModel userModel;
        
        public BoardView(BoardModel b,UserModel u)
        {
           
            InitializeComponent();
            this.viewModel = new BoardViewModel(b);
            this.userModel = u;
            this.DataContext = viewModel;
        }

        /// <summary>
        /// button to return the use's boards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void go_Back_Click(object sender, RoutedEventArgs e)
        {
            BoardsWindow boardsView = new BoardsWindow(userModel);
            boardsView.Show();
            this.Close();
        }

        
    }
}
