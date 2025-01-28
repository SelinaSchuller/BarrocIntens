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
    /// An esmpty page that can be used on its own or navigated to within a Frame.
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

			if(e.Parameter is int productId)
			{
				_productId = productId;
				using(var db = new AppDbContext())
				{
					var product = db.Products.FirstOrDefault(p => p.Id == productId);
					var inventory = db.ProductInventories.FirstOrDefault(i => i.ProductId == productId);

					if(product != null)
					{
						NaamInput.Text = product.Name;
						DescInput.Text = product.Description;
						VoorraadCheckBox.IsChecked = product.IsStock;
						ZichtbaarheidCheckBox.IsChecked = product.VisibleForCustomers;
						PrijsInput.Text = Math.Round(product.Price, 2).ToString();
						CategoryComboBox.SelectedValue = product.CategoryId;

						if(inventory != null)
						{
							VoorraadInput.Text = inventory.InStock.ToString();
							VoorraadInput.Visibility = product.IsStock ? Visibility.Visible : Visibility.Collapsed;
						}
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
			VoorraadError.Visibility = Visibility.Collapsed;

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
					VoorraadError.Visibility = Visibility.Visible;
					validatieErrors += 1;
				}
			}

			prijsOutput = Math.Round(prijsOutput, 2);

			if(validatieErrors == 0)
			{
				using(var db = new AppDbContext())
				{
					var product = db.Products.FirstOrDefault(p => p.Id == _productId);
					var inventory = db.ProductInventories.FirstOrDefault(i => i.ProductId == _productId);

					if(product != null)
					{
						product.Name = NaamInput.Text;
						product.Description = DescInput.Text;
						product.Price = prijsOutput;
						product.IsStock = VoorraadCheckBox.IsChecked == true;
						product.VisibleForCustomers = ZichtbaarheidCheckBox.IsChecked == true;
						product.CategoryId = (int)CategoryComboBox.SelectedValue;

						if(product.IsStock == true)
						{
							if(inventory == null)
							{
								inventory = new ProductInventory
								{
									ProductId = product.Id,
									InStock = int.Parse(VoorraadInput.Text),
									AmountOrdered = 0
								};
								db.ProductInventories.Add(inventory);
							}
							else
							{
								inventory.InStock = int.Parse(VoorraadInput.Text);
							}
						}
						else if(inventory != null)
						{
							db.ProductInventories.Remove(inventory);
						}
					}

					db.SaveChanges();
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
