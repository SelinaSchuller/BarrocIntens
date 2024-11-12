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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Inkoop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductenPage : Page
    {
        public ProductenPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                ProductListView.ItemsSource = db.Products.Include(p => p.Category).OrderBy(p => p.Id).ToList();
            };
           
        }

        private void ZoekButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NieuwProductButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductAanmaakPage));
        }

        private void BewerkButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                Frame.Navigate(typeof(ProductBewerkenPage), id);
            }
               
        }

        private async void VerwijderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var product = button.DataContext as Product;

                if (product != null)
                {
                    bool isDeleted = await DeleteProductFromDatabase(product);

                    if (isDeleted)
                    {
                        var productList = ProductListView.ItemsSource as ObservableCollection<Product>;
                        if (productList != null)
                        {
                            productList.Remove(product);
                        }
                    }
                }
            }
        }

        private async Task<bool> DeleteProductFromDatabase(Product product)
        {
            using (var db = new AppDbContext())
            {
                var productToDelete = await db.Products
                                                    .FirstOrDefaultAsync(p => p.Id == product.Id);

                if (productToDelete != null)
                {
                    db.Products.Remove(productToDelete);
                    await db.SaveChangesAsync();
                    ProductListView.ItemsSource = db.Products.Include(p => p.Category).OrderBy(p => p.Id).ToList();
                    return true;
                }
            }

            return false;
        }

    }
}
