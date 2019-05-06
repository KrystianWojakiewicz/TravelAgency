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
        }

        public AdminWindow()
        {
            InitializeComponent();

            initializeUsersFromDB();
            usersListBox.ItemsSource = Users;
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
