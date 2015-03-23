using System;
using System.Windows;
using UserManagementApplication.Client.Data;
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
        #region Properties

        public SessionData SessionToken { get; set; }
        public event EventHandler<IView> OnViewLoaded;
        protected LoginPresenter Presenter { get; set; }
        
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

        #endregion

        #region Constructors
        
        public LoginView()
        {
            InitializeComponent();
            Presenter = new LoginPresenter(this);
            DataContext = this;
        }

        #endregion

        #region Methods
        
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

        #endregion

        #region Functions
        
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

        #endregion
    }
}
