using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        ObservableCollection<User> Users { get; set; }
 
        
        void initializeUsersFromDB()
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                Users = new ObservableCollection<User>(db.User.ToList());
            }
            usersListBox.ItemsSource = Users;
        }

        public AdminWindow()
        {
            InitializeComponent();

            initializeUsersFromDB();
        }

        private void addNewUser(TravelAgencyDBEntities db)
        {
            if (!String.IsNullOrEmpty(usernameTextBox.Text) && !String.IsNullOrEmpty(passwordTextBox.Text))
            {
                User newUser = new User()
                {
                    UserID = Guid.NewGuid(),
                    Username = usernameTextBox.Text,
                    Password = passwordTextBox.Text,
                    Saldo = 0,
                    isAdmin = isAdminCheckBox.IsChecked
                };
                db.User.Add(newUser);
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Please enter profile credentials");
            }

            MessageBox.Show("User has been successfully registered");
        }

        private void editUser(TravelAgencyDBEntities db)
        {
            if (!String.IsNullOrEmpty(usernameTextBox.Text) && !String.IsNullOrEmpty(passwordTextBox.Text))
            {
                int userIndex = usersListBox.SelectedIndex;
                Users[userIndex].Username = usernameTextBox.Text;
                Users[userIndex].Password = passwordTextBox.Text;

                db.Entry(Users[userIndex]).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Please enter profile credentials");
            }

            MessageBox.Show("User has been successfully edited");
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            using(TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                var userToDelete = db.User.Find(Users[usersListBox.SelectedIndex].UserID);
                db.User.Remove(userToDelete);
                db.SaveChanges();
                initializeUsersFromDB(); 
            }
        }

        private void clearInputBoxes()
        {
            usernameTextBox.Clear();
            passwordTextBox.Clear();
            isAdminCheckBox.IsChecked = false;
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                addNewUser(db);
            }
            clearInputBoxes();
            initializeUsersFromDB();
        }

        private void UsersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersListBox.SelectedIndex == -1) { return; }

            int userIndex = usersListBox.SelectedIndex;
            usernameTextBox.Text = Users[userIndex].Username;
            passwordTextBox.Text = Users[userIndex].Password;
        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
           // if(usersListBox.SelectedIndex == -1) { return; }

            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                editUser(db);
            }
            clearInputBoxes();
            initializeUsersFromDB();
            usersListBox.ItemsSource = Users;
        }
    }
}
