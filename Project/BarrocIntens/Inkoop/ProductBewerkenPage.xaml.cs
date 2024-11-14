using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using BarrocIntens.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Inkoop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductBewerkenPage : Page
    {
        private int _productId;

        public ProductBewerkenPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                var categories = db.ProductCategories.ToList();
                CategoryComboBox.ItemsSource = categories;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int productId)
            {
                _productId = productId;
                using (var db = new AppDbContext())
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        NaamInput.Text = product.Name;
                        DescInput.Text = product.Description;
                        VoorraadCheckBox.IsChecked = product.IsStock;
                        ZichtbaarheidCheckBox.IsChecked = product.VisibleForCustomers;
                        PrijsInput.Text = Math.Round(product.Price, 2).ToString();
                        CategoryComboBox.SelectedValue = product.CategoryId;
                    }
                }

            }
        }
        private void TerugButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductenPage));
        }

        private void OpslaanButton_Click(object sender, RoutedEventArgs e)
        {
            NaamError.Visibility = Visibility.Collapsed;
            DescError.Visibility = Visibility.Collapsed;
            PrijsError.Visibility = Visibility.Collapsed;
            CategoryError.Visibility = Visibility.Collapsed;

            int validatieErrors = 0;

            if (NaamInput.Text.Length == 0)
            {
                NaamError.Visibility = Visibility.Visible;
                validatieErrors += 1;
            }

            if (DescInput.Text.Length == 0)
            {
                DescError.Visibility = Visibility.Visible;
                validatieErrors += 1;
            }


            if (decimal.TryParse(PrijsInput.Text, out decimal prijsOutput))
            {

            }
            else
            {
                PrijsError.Visibility = Visibility.Visible;
                validatieErrors += 1;
            }

            if (CategoryComboBox.SelectedValue == null)
            {
                CategoryError.Visibility = Visibility.Visible;
                validatieErrors += 1;
            }

            prijsOutput = Math.Round(prijsOutput, 2);

            if (validatieErrors == 0)
            {
                using (var db = new AppDbContext())
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == _productId);
                    if (product != null)
                    {
                        product.Name = NaamInput.Text;
                        product.Description = DescInput.Text;
                        product.Price = (decimal)prijsOutput;
                        product.IsStock = VoorraadCheckBox.IsChecked == true;
                        product.VisibleForCustomers = ZichtbaarheidCheckBox.IsChecked == true;
                        product.CategoryId = (int)CategoryComboBox.SelectedValue;
                    };

                    db.SaveChanges();
                    Frame.Navigate(typeof(ProductenPage));
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}
