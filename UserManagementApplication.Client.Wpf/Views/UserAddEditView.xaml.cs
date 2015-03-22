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
        public string ViewTitle { get; set; }
        public UserData UserData { get; set; }
        public string SessionToken { get; set; }
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
                this.FirstNameControl.Text = value;
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

        public UserAddEditView()
        {
            InitializeComponent();

            Presenter = new UserAddEditPresenter(this);
        }

        public void HandleValidationResults(ValidationResults validationResults)
        {
            foreach (var result in validationResults.ErrorDictionary)
            {
                MessageBox.Show(result.Value, this.Title);
            }
        }

        public void HandleException(string message)
        {
            MessageBox.Show(message, this.Title);
        }

        public void HandleCommitSuccess()
        {
            this.Close();
        }

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
    }
}
