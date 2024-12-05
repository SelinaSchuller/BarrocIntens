using BarrocIntens.Data;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Sales
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalesProductPage : Page
    {
        private List<Product> ProductList { get; set; } = new List<Product>();

        public SalesProductPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                ProductList = db.Products.Include(p => p.Category)
                                         .Where(p => p.VisibleForCustomers)
                                         .OrderBy(p => p.Id)
                                         .ToList();
                ProductListView.ItemsSource = ProductList;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox)?.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                ProductListView.ItemsSource = ProductList;
            }
            else
            {
                ProductListView.ItemsSource = ProductList
                    .Where(c => c.Name != null && c.Name.ToLower().Contains(searchText))
                    .ToList();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SalesMainPage));
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                using (var db = new AppDbContext())
                {
                    var productDetails = db.Products
                                           .Include(c => c.Category)
                                           .Where(c => c.Id == selectedProduct.Id)
                                           .OrderBy(c => c.Id)
                                           .ToList();

                    ProductInfoListView.ItemsSource = productDetails;
                }
            }
        }
    }
}
