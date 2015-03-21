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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserManagementApplication.Client.Presenters;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Client.Wpf.Views;

namespace UserManagementApplication.Client.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView : Window, ILoginView
    {
        protected LoginPresenter Presenter { get; set; }

        public LoginView()
        {
            InitializeComponent();
            Presenter = new LoginPresenter(this);
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Presenter.Login();
        }

        public string SessionToken { get; set; }
        public string Username { get; set; }
        public string Password
        {
            get
            {
                return PasswordBox.Password;
            }
            set
            {
                PasswordBox.Password = value;
            }
        }

        public void HandleLoginMessage(string message)
        {
            MessageBox.Show(message, this.Name);
        }

        public void HandleSuccessfulLogin()
        {
            UserManagementView userManagementView = new UserManagementView();

            userManagementView.Show();
            this.Close();
        }
    }
}
