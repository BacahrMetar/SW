using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email;
        public ObservableCollection<BoardModel> Boards { get; set; }
        /// <summary>
        ///  gettrer for user's email
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
            Boards = controller.GetAllUserBoards(email);
        }

    }
}
