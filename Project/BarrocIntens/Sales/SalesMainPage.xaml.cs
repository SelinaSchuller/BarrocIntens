using BarrocIntens.Data;
using BarrocIntens.Inkoop;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace BarrocIntens.Sales
{
    public sealed partial class SalesMainPage : Page
    {
        private List<Customer> CustomerList { get; set; } = new List<Customer>();

        public SalesMainPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                CustomerList = db.Customers.OrderBy(p => p.Id).ToList();
                customersListView.ItemsSource = CustomerList;
            }
        }

        private void NewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SalesKlantAanmakenPage));
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox)?.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                customersListView.ItemsSource = CustomerList;
            }
            else
            {
                customersListView.ItemsSource = CustomerList
                    .Where(c => c.Name != null && c.Name.ToLower().Contains(searchText))
                    .ToList();
            }
        }

        private void customersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customersListView.SelectedItem is Customer selectedCustomer)
            {
                using (var db = new AppDbContext())
                {
                    var customerDetails = db.Customers
                                             .Include(c => c.Company)
                                             .Where(c => c.Id == selectedCustomer.Id)
                                             .OrderBy(c => c.Id)
                                             .ToList();

                    customerInfoListView.ItemsSource = customerDetails;
                }
            }
        }

        private void ProductenButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SalesProductPage));
        }
    }

}
