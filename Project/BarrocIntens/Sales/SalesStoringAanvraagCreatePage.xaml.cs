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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Sales
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SalesStoringAanvraagCreatePage : Page
	{
		private List<Customer> _klantenLijst;
		private List<Product> _productenLijst;
		private SalesDashboardWindow _parentWindow;

		public SalesStoringAanvraagCreatePage()
		{
			this.InitializeComponent();
			LoadData();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is SalesDashboardWindow parentWindow)
			{
				_parentWindow = parentWindow;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("SalesCreateNotePage: No valid SalesDashboardWindow received.");
			}
		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				_klantenLijst = db.Customers
					.OrderBy(c => c.Name)
					.ToList();
				CustomerComboBox.ItemsSource = _klantenLijst;

				_productenLijst = db.Products
					.OrderBy(c => c.Name)
					.ToList();
				ProductComboBox.ItemsSource = _productenLijst;
			}
		}

		private async void SaveServiceRequest_Click(object sender, RoutedEventArgs e)
		{
			if(string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
			{
				ContentDialog titleErrorDialog = new ContentDialog
				{
					Title = "Vul een beschrijving in",
					Content = "Voer een beschrijving in voordat u de storings aanvraag opslaat.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};
				titleErrorDialog.ShowAsync();
				return;
			}
			else if(CustomerComboBox.SelectedItem is Customer selectedCustomer)
			{
				if(ProductComboBox.SelectedItem is Product selectedProduct)
				{
					System.Diagnostics.Debug.WriteLine($"Title: {DescriptionTextBox.Text} Type: {selectedCustomer.Name} Description: {selectedProduct.Name}");

					var newRequest = new ServiceRequest
					{
						Description = DescriptionTextBox.Text,
						Date_Reported = DateTime.Now,
						Status = 1, // UnAssigned
						CustomerId = selectedCustomer.Id,
						ProductId = selectedProduct.Id
					};

					using(var db = new AppDbContext())
					{
						db.ServiceRequests.Add(newRequest);
						db.SaveChanges();
					}

					var dialog = new ContentDialog
					{
						Title = "Bevestiging",
						Content = "Storing aanvraag succesvol aangemaakt!",
						PrimaryButtonText = "OK",
						XamlRoot = this.XamlRoot
					};
					dialog.PrimaryButtonClick += (s, args) =>
					{
						_parentWindow.NavigateToMainPage();
					};

					await dialog.ShowAsync();
				}
				else
				{
					ContentDialog productErrorDialog = new ContentDialog
					{
						Title = "Selecteer een product",
						Content = "Kies een product uit de lijst voordat u de storing aanvraag opslaat.",
						CloseButtonText = "Ok",
						XamlRoot = this.XamlRoot
					};
					productErrorDialog.ShowAsync();
				}
			}
			else
			{
				ContentDialog customerErrorDialog = new ContentDialog
				{
					Title = "Selecteer een klant",
					Content = "Kies een klant uit de lijst voordat u de storing aanvraag opslaat.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};
				customerErrorDialog.ShowAsync();
			}
		}

	}
}
