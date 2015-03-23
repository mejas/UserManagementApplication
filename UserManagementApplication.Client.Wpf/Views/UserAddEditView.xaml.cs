using System;
using System.Windows;
using UserManagementApplication.Client.Data;
using UserManagementApplication.Client.Enumerations;
using UserManagementApplication.Client.Presenters;
using UserManagementApplication.Client.ViewData;
using UserManagementApplication.Client.ViewDefinitions;

namespace UserManagementApplication.Client.Wpf.Views
{
    /// <summary>
    /// Interaction logic for UserAddEditView.xaml
    /// </summary>
    public partial class UserAddEditView : Window, IUserAddEditView
    {
        #region Properties

        public string ViewTitle { get; set; }
        public UserData UserData { get; set; }
        public SessionData SessionToken { get; set; }
        public ViewOperation ViewOperation { get; set; }
        public event EventHandler<IView> OnViewLoaded;

        public string Username
        {
            get
            {
                return this.UsernameControl.Text;
            }
            set
            {
                this.UsernameControl.Text = value;
            }
        }

        public string FirstName
        {
            get
            {
                return this.FirstNameControl.Text;
            }
            set
            {
                this.FirstNameControl.Text = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.LastNameControl.Text;
            }
            set
            {
                this.LastNameControl.Text = value;
            }
        }

        public DateTime Birthdate
        {
            get
            {
                if (this.BirthdateControl.SelectedDate == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return (DateTime)this.BirthdateControl.SelectedDate;
                }
            }
            set
            {
                this.BirthdateControl.SelectedDate = value;
            }
        }

        public string Password
        {
            get
            {
                return PasswordControl.Password;
            }
            set
            {
                this.PasswordControl.Password = value;
            }
        }

        protected UserAddEditPresenter Presenter { get; set; }

        #endregion

        #region Constructors
        
        public UserAddEditView()
        {
            InitializeComponent();

            Presenter = new UserAddEditPresenter(this);
        }

        #endregion

        #region Methods
        
        public void HandleValidationResults(ValidationResults validationResults)
        {
            foreach (var result in validationResults.ErrorDictionary)
            {
                MessageBox.Show(this, result.Value, this.Title);
            }
        }

        public void HandleException(string message)
        {
            MessageBox.Show(this, message, this.Title);
        }

        public void HandleCommitSuccess()
        {
            this.Close();
        }

        #endregion

        #region Functions
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var eventHandler = OnViewLoaded;

            if (eventHandler != null)
            {
                OnViewLoaded(this, this);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Presenter.Save();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 

        #endregion
    }
}
