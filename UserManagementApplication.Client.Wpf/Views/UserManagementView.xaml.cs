using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        public event EventHandler<IView> OnViewLoaded;

        public string SessionToken { get; set; }

        public UserData CurrentUserData
        {
            get
            {
                return this.ListView.SelectedItem as UserData;
            }
        }


        protected UserManagementPresenter Presenter { get; set; }

        public UserManagementView()
        {
            InitializeComponent();
            Presenter = new UserManagementPresenter(this);
        }

        public void UpdateData(IList<UserData> userData)
        {
            ListView.Items.Clear();

            foreach(var data in userData)
            {
                ListView.Items.Add(data);
            }
        }

        public void HandleException(string message)
        {
            MessageBox.Show(message, this.Title);
        }

        public void HandleLogout()
        {
            LoginView loginView = new LoginView();

            loginView.Show();

            this.Close();
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

        public void OnAddItem(UserData userData)
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

        public void OnEditItem(UserData userData)
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
            Presenter.Logout();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Presenter.RefreshData();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Presenter.SecureControls(ListView.SelectedIndex);
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Presenter.AddItem();
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            Presenter.EditItem();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            Presenter.DeleteItem();
        }

        
    }
}
