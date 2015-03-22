using System;
using System.Windows;
using System.Windows.Controls;
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

        public event EventHandler<IView> OnViewLoaded;

        public string SessionToken { get; set; }
        public string Username
        {
            get
            {
                return this.UsernameTextBox.Text;
            }
            set
            {
                this.UsernameTextBox.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return this.PasswordBox.Password;
            }
            set
            {
                this.PasswordBox.Password = value;
            }
        }

        public LoginView()
        {
            InitializeComponent();
            Presenter = new LoginPresenter(this);
            DataContext = this;
        }

        public void HandleException(string message)
        {
            MessageBox.Show(this, message, this.Title);
        }

        public void HandleSuccessfulLogin()
        {
            UserManagementView userManagementView = new UserManagementView() { SessionToken = SessionToken };

            userManagementView.Show();
            this.Close();
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
            Presenter.Login();
        }
    }
}
