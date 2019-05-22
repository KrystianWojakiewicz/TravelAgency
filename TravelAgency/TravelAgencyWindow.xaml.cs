using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        List<Offer> shoppingList { get; set; } = new List<Offer>();
        User myUser;

        void initializeOffersFromDB()
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                Offers = new ObservableCollection<Offer>(db.Offer.ToList());
            }
            offersDataGrid.ItemsSource = Offers;
        }


        public TravelAgencyWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            myUser = user;
            initializeOffersFromDB();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Login loginWin = new Login();
            loginWin.Show();
            this.Close();
        }

        private void OffersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(offersDataGrid.SelectedIndex == -1) { return; }
            fetchWeatherData();
        }

        public void fetchWeatherData()
        {

            string MY_API_KEY = "9f97a9c4acee2b7917ae2b12a1f7a700/";
            string LAT_LONG = Offers[offersDataGrid.SelectedIndex].Coordinates;

            string responseText;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.darksky.net/forecast/" + MY_API_KEY + LAT_LONG + "?units=si");
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                    {
                        responseText = responseStream.ReadToEnd();
                    }
                }

                try
                {
                    var data = (JObject)JsonConvert.DeserializeObject(responseText);
                    updateWeatherTextBox(data);
                }
                catch(JsonException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch(WebException ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        private void updateWeatherTextBox(JObject data)
        {
            weatherInfoTextBox.Text = "----WELCOME TO OUR WEATHER FORECAST----\n";

            string temperature = data["currently"]["temperature"].Value<string>();
            weatherInfoTextBox.Text += "Temperature: " + temperature + "\n";

            string windSpeed = data["currently"]["windSpeed"].Value<string>();
            weatherInfoTextBox.Text += "Wind Speed: " + windSpeed + "\n";

            string humidity = data["currently"]["humidity"].Value<string>();
            weatherInfoTextBox.Text += "Humidity: " + humidity + "\n";

            string summary = data["currently"]["summary"].Value<string>();
            weatherInfoTextBox.Text += "Summary: " + summary + "\n";
        }

        private void addToCart_Click(object sender, RoutedEventArgs e)
        {
            int offerIndex = offersDataGrid.SelectedIndex;
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                if (Offers[offerIndex].Quantity <= 0)
                {
                    MessageBox.Show("No offers left");
                }
                else
                {
                    Offers[offerIndex].Quantity = Offers[offerIndex].Quantity - 1;

                    db.Entry(Offers[offerIndex]).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    initializeOffersFromDB();
                    shoppingList.Add(Offers[offerIndex]);
                }
            }
        }

        private void shoppingCartBtn_Click(object sender, RoutedEventArgs e)
        {
            SCartWindow shopCartWindow = new SCartWindow(shoppingList, myUser);
            shopCartWindow.Show();
            this.Hide();
        }
    }
}
