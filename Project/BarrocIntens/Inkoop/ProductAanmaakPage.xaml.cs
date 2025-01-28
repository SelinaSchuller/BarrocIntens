using BarrocIntens.Data;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Inkoop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductAanmaakPage : Page
    {
        public ProductAanmaakPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                var categories = db.ProductCategories.ToList();
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.SelectedIndex = 0;
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
			VoorrraadError.Visibility = Visibility.Collapsed;

			int validatieErrors = 0;

			if(NaamInput.Text.Length == 0)
			{
				NaamError.Visibility = Visibility.Visible;
				validatieErrors += 1;
			}

			if(DescInput.Text.Length == 0)
			{
				DescError.Visibility = Visibility.Visible;
				validatieErrors += 1;
			}

			if(!decimal.TryParse(PrijsInput.Text, out decimal prijsOutput))
			{
				PrijsError.Visibility = Visibility.Visible;
				validatieErrors += 1;
			}

			if(VoorraadCheckBox.IsChecked == true)
			{
				if(!int.TryParse(VoorraadInput.Text, out int inStock) || inStock <= 0)
				{
					VoorrraadError.Visibility = Visibility.Visible;
					validatieErrors += 1;
				}
			}

			if(validatieErrors == 0)
			{
				using(var db = new AppDbContext())
				{
					var newProduct = new Product
					{
						Name = NaamInput.Text,
						Description = DescInput.Text,
						Price = prijsOutput,
						IsStock = VoorraadCheckBox.IsChecked == true,
						VisibleForCustomers = ZichtbaarheidCheckBox.IsChecked == true,
						CategoryId = (int)CategoryComboBox.SelectedValue
					};

					db.Products.Add(newProduct);
					db.SaveChanges();

					if(VoorraadCheckBox.IsChecked == true && int.TryParse(VoorraadInput.Text, out int inStock))
					{
						var inventory = new ProductInventory
						{
							ProductId = newProduct.Id,
							InStock = inStock,
							AmountOrdered = 0
						};

						db.ProductInventories.Add(inventory);
						db.SaveChanges();
					}

					Frame.Navigate(typeof(ProductenPage));
					return;
				}
			}
		}

		private void VoorraadCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			VoorraadInput.Visibility = Visibility.Visible;
		}

		private void VoorraadCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			VoorraadInput.Visibility = Visibility.Collapsed;
			VoorraadInput.Text = string.Empty;
		}
	}
}
