using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UserManagementApplication.Client.Data;
using UserManagementApplication.Client.Enumerations;
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
        #region Properties
        
        public event EventHandler<IView> OnViewLoaded;

        public SessionData SessionToken { get; set; }

        public UserData CurrentUserData
        {
            get
            {
                return this.ListView.SelectedItem as UserData;
            }
        }

        public string ViewTitle
        {
            get
            {
                return this.Title;
            }
            set
            {
                this.Title = value;
            }
        }

        public string FirstName
        {
            get
            {
                return this.textBoxFirstName.Text;
            }
            set
            {
                this.textBoxFirstName.Text = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.textBoxLastName.Text;
            }
            set
            {
                this.textBoxLastName.Text = value;
            }
        }

        protected UserManagementPresenter Presenter { get; set; } 
        
        #endregion

        #region Constructors
        
        public UserManagementView()
        {
            InitializeComponent();
            Presenter = new UserManagementPresenter(this);
        } 

        #endregion

        #region Methods
        
        public void UpdateData(IList<UserData> userData)
        {
            ListView.Items.Clear();

            foreach (var data in userData)
            {
                ListView.Items.Add(data);
            }
        }

        public void HandleException(string message)
        {
            MessageBox.Show(this, message, this.Title);
        }

        public void HandleLogout()
        {
            LoginView loginView = new LoginView();

            loginView.Show();
        }

        public void EnableAdd(bool value)
        {
            buttonAdd.IsEnabled = value;
        }

        public void EnableEdit(bool value)
        {
            buttonEdit.IsEnabled = value;
        }

        public void EnableDelete(bool value)
        {
            buttonDelete.IsEnabled = value;
        }

        public void EnableUnlock(bool value)
        {
            buttonUnlock.IsEnabled = value;
        }

        public void OnAddUser(UserData userData)
        {
            UserAddEditView userAddEdit = new UserAddEditView()
            {
                ViewOperation = ViewOperation.Add,
                UserData = userData,
                SessionToken = this.SessionToken,
                Owner = this
            };

            userAddEdit.ShowDialog();
        }

        public void OnEditUser(UserData userData)
        {
            UserAddEditView userAddEdit = new UserAddEditView()
            {
                ViewOperation = ViewOperation.Edit,
                UserData = userData,
                SessionToken = this.SessionToken,
                Owner = this
            };

            userAddEdit.ShowDialog();
        }

        #endregion

        #region MyRegion
        
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Presenter.Logout();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var eventHandler = OnViewLoaded;

            if (eventHandler != null)
            {
                OnViewLoaded(this, this);
            }
        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Presenter.FindAllUsers();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Presenter.SecureControls();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Presenter.AddUser();
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            Presenter.EditUser();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            Presenter.DeleteUser();
        }

        private void buttonUnlock_Click(object sender, RoutedEventArgs e)
        {
            Presenter.UnlockUser();
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            Presenter.FindUsers();
        } 

        #endregion
    }
}
