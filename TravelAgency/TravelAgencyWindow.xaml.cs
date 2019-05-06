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
            TravelAgencyDBEntities db = new TravelAgencyDBEntities();
            foreach (Offer offer in db.Offer)
            {
                Offers.Add(offer);
            }
        }


        public TravelAgencyWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            initializeOffersFromDB();
            offerListBox.ItemsSource = Offers;
        }
    }
}
