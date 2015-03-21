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
using UserManagementApplication.Client.Presenters;
using UserManagementApplication.Client.ViewData;
using UserManagementApplication.Client.ViewDefinitions;

namespace UserManagementApplication.Client.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserManagementView : Window, IUserManagementView
    {
        public event EventHandler<IView> OnViewLoaded;

        public string SessionToken { get; set; }

        protected UserManagementPresenter Presenter { get; set; }

        public UserManagementView()
        {
            InitializeComponent();
            Presenter = new UserManagementPresenter(this);
        }

        public void UpdateData(IList<UserData> userData)
        {
            foreach(var data in userData)
            {
                ListView.Items.Add(data);
            }
        }

        public void HandleException(string message)
        {
            throw new NotImplementedException();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var eventHandler = OnViewLoaded;

            if (eventHandler != null)
            {
                OnViewLoaded(this, this);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }   
    }
}
