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
		private bool isDeleted { get; set; }
		private ObservableCollection<Product> Products { get; set; }
		public ProductenPage()
		{
			this.InitializeComponent();

			LoadProducts();

		}

		private void LoadProducts()
		{
			using(var db = new AppDbContext())
			{
				var products = db.Products.Include(p => p.Category).OrderBy(p => p.Id).ToList();
				Products = new ObservableCollection<Product>(products);
				ProductListView.ItemsSource = Products;
			}
		}

		private void ZoekButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = SearchTextBox.Text?.Trim().ToLower();

			using(var db = new AppDbContext())
			{
				var filteredProducts = string.IsNullOrEmpty(searchText)
					? db.Products.Include(p => p.Category).OrderBy(p => p.Id).ToList()
					: db.Products
						.Include(p => p.Category)
						.Where(p => p.Name.ToLower().Contains(searchText) ||
									(p.Category != null && p.Category.Name.ToLower().Contains(searchText)))
						.OrderBy(p => p.Id)
						.ToList();

				Products.Clear();
				foreach(var product in filteredProducts)
				{
					Products.Add(product);
				}
			}
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
			if(sender is Button button && button.Tag is int id)
			{
				Frame.Navigate(typeof(ProductBewerkenPage), id);
			}

		}

		private async void VerwijderButton_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Start VerwijderButton_Click");

			var button = sender as Button;
			if(button != null && button.DataContext is Product product)
			{
				// Initial confirmation dialog
				var initialDialog = new ContentDialog
				{
					Title = "Bevestiging",
					Content = "Weet je zeker dat je dit product wilt verwijderen?",
					PrimaryButtonText = "Ja",
					CloseButtonText = "Nee",
					XamlRoot = this.XamlRoot
				};

				var initialResult = await initialDialog.ShowAsync();

				if(initialResult != ContentDialogResult.Primary)
					return; // User canceled the deletion

				using(var db = new AppDbContext())
				{
					// Check for active ServiceRequests using this product
					var activeServiceRequests = db.ServiceRequests
						.Where(sr => sr.ProductId == product.Id && (sr.Status == 1 || sr.Status == 2))
						.ToList();

					if(activeServiceRequests.Any())
					{
						// Warning dialog if ServiceRequests are using the product
						var warningDialog = new ContentDialog
						{
							Title = "Product wordt nog gebruikt",
							Content = $"Dit product wordt nog gebruikt in {activeServiceRequests.Count} storings aanvraag(aanvragen) met status (1)'Nog te doen' of (2)'Onderweg'. Als u doorgaat, wordt het product in deze servicerequests leeggemaakt. Wilt u doorgaan?",
							PrimaryButtonText = "Ja",
							SecondaryButtonText = "Nee",
							XamlRoot = this.XamlRoot
						};

						var warningResult = await warningDialog.ShowAsync();

						if(warningResult != ContentDialogResult.Primary)
							return; // User chose to keep the product

						// Set ProductId to null for all affected ServiceRequests
						foreach(var serviceRequest in activeServiceRequests)
						{
							serviceRequest.ProductId = null;
						}

						db.ServiceRequests.UpdateRange(activeServiceRequests);
						await db.SaveChangesAsync();
					}

					// Check for references in other tables (e.g., WorkOrders)
					var relatedWorkOrders = db.WorkOrders.Where(wo => wo.ProductId == product.Id).ToList();
					if(relatedWorkOrders.Any())
					{
						foreach(var workOrder in relatedWorkOrders)
						{
							workOrder.ProductId = null;
						}

						db.WorkOrders.UpdateRange(relatedWorkOrders);
						await db.SaveChangesAsync();
					}

					// Delete the product
					if(activeServiceRequests.Any())
					{
						foreach(var serviceRequest in activeServiceRequests)
						{
							serviceRequest.ProductId = null;
						}
						db.ServiceRequests.UpdateRange(activeServiceRequests);

						if(!await SaveChangesWithLogging(db))
						{
							return; // Exit on failure
						}
					}

					var productToDelete = db.Products.FirstOrDefault(p => p.Id == product.Id);
					if(productToDelete != null)
					{
						db.Products.Remove(productToDelete);

						if(!await SaveChangesWithLogging(db))
						{
							return; // Exit on failure
						}

						// Reload products only after successful deletion
						ReloadProducts();
					}
				}
			}

			System.Diagnostics.Debug.WriteLine("End VerwijderButton_Click");
		}

		private void ReloadProducts()
		{
			System.Diagnostics.Debug.WriteLine("Start ReloadProducts");
			using(var db = new AppDbContext())
			{
				Products.Clear(); // Ensure existing data is cleared
				var updatedProducts = db.Products.Include(p => p.Category).OrderBy(p => p.Id).ToList();
				foreach(var updatedProduct in updatedProducts)
				{
					Products.Add(updatedProduct);
				}
			}
			System.Diagnostics.Debug.WriteLine("End ReloadProducts");
		}

		private async Task<bool> SaveChangesWithLogging(AppDbContext db)
		{
			try
			{
				await db.SaveChangesAsync();
				return true;
			}
			catch(Microsoft.EntityFrameworkCore.DbUpdateException ex)
			{
				// Log inner exception for debugging
				System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.InnerException?.Message}");
				return false;
			}
		}
	}
}
