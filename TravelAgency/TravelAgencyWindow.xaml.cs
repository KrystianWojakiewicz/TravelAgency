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

        void initializeOffersFromDB()
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                Offers = new ObservableCollection<Offer>(db.Offer.ToList());
            }
            offersDataGrid.ItemsSource = Offers;
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

        private void OffersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fetchWeatherData();
        }

        public void fetchWeatherData()
        {

            string MY_API_KEY = "9f97a9c4acee2b7917ae2b12a1f7a700/";
            string LAT_LONG = Offers[offersDataGrid.SelectedIndex].Coordinates;

            string responseText;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.darksky.net/forecast/" + MY_API_KEY + LAT_LONG + "?units=si");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    responseText = responseStream.ReadToEnd();
                }
            }
            var data = (JObject)JsonConvert.DeserializeObject(responseText);

            updateWeatherTextBox(data);
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
    }
}
