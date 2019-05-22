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
    /// Interaction logic for SCartWindow.xaml
    /// </summary>
    public partial class SCartWindow : Window
    {
        ObservableCollection<Offer> ShoppingList { get; set; }
        User myUser;

        void initializeShoppingList(List<Offer> shoppingList)
        {
            if(shoppingList == null) { return; }

            ShoppingList = new ObservableCollection<Offer>(shoppingList);
            shoppingDataGrid.ItemsSource = ShoppingList;
        }

        public SCartWindow(List<Offer> shoppingList, User user)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            myUser = user;
            saldoLabel.Content = myUser.Saldo;
            initializeShoppingList(shoppingList);
        }

        private void CloseCartBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ShoppingList == null) { return; }
            if (myUser.Saldo >= getSummedPrice())
            {
                ShoppingList.Clear();
            }
            else
            {
                MessageBox.Show("Not enough Money on your Account!");
            }
        }

        private int getSummedPrice()
        {
            if(ShoppingList == null) { return -1; }

            int totalSum = 0;
            foreach(Offer off in ShoppingList) 
            {
                totalSum += (int)off.Price;
            }
            return totalSum;
        }

        private void SaldoBtn_Click(object sender, RoutedEventArgs e)
        {
            using (TravelAgencyDBEntities db = new TravelAgencyDBEntities())
            {
                var query = db.User.Where(u => u.Username == myUser.Username);
                var queryResult = query.First();
                int parseInt = -1;
                if(Int32.TryParse(valueTextBox.Text, out parseInt))
                {
                    Decimal newSaldo = queryResult.Saldo += parseInt;
                    db.Entry(queryResult).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    myUser.Saldo = newSaldo;
                    saldoLabel.Content = newSaldo;
                    MessageBox.Show("Saldo successfully updated");
                } 
            }
        }
    }
}
