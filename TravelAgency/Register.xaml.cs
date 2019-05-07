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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
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
                    isAdmin = false
                };
                db.User.Add(newUser);
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Please enter your credentials");
            }
 
            MessageBox.Show("User has been successfully registered");
        }

        private void clearInputBoxes()
        {
            usernameTextBox.Clear();
            passwordTextBox.Clear();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                addNewUser(db);
            }  
            clearInputBoxes();

            this.Close();
            App.Current.MainWindow.Show();
        }
    }
}
