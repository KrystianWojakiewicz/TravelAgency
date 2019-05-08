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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for TravelAgencyWindow.xaml
    /// </summary>
    public partial class TravelAgencyWindow : Window
    {
        ObservableCollection<Offer> Offers { get; set; }

        void initializeOffersFromDB()
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                Offers = new ObservableCollection<Offer>(db.Offer.ToList());
            }
            offersDataGrid.ItemsSource = Offers;
            //offersDataGrid.DataContext = Offers;
        }


        public TravelAgencyWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            initializeOffersFromDB();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Login loginWin = new Login();
            loginWin.Show();
            this.Close();
        }
    }
}
