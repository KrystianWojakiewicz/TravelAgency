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

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        //check if there exists a user with given username/password
        private bool queryUser(TravelAgencyDBEntities db)
        {
            var user = db.User
              .Where(u => u.Username == usernameTextBox.Text)
              .Where(u => u.Password == passwordTextBox.Text)
              .Select(u => u);

            User result = user.FirstOrDefault();
            if (result == null)
            {
                MessageBox.Show("Invalid Username or Password");
                return false;
            }
            else
            {
                MessageBox.Show("Login Successful", "alert", MessageBoxButton.OK);
                
                TravelAgencyWindow taWindow = new TravelAgencyWindow(result);
                App.Current.MainWindow = taWindow;
                this.Close();
                taWindow.Show();

                if (result.isAdmin == true)
                {
                    AdminWindow adminWin = new AdminWindow();
                    adminWin.Show();
                }
                return true;            
            }
        }

        private void clearInputBoxes()
        {
            usernameTextBox.Clear();
            passwordTextBox.Clear();
        }

        private void GoToRegisterWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            this.Hide();
            registerWindow.Show();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                if (!String.IsNullOrEmpty(usernameTextBox.Text) && !String.IsNullOrEmpty(passwordTextBox.Text))
                {
                    queryUser(db);
                }
                else
                {
                    MessageBox.Show("Please enter your credentials");
                }
            }              
            clearInputBoxes();
        }
    }
}
