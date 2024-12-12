using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudWorkOrderCreatePage : Page
	{
		private int _appointmentId;
		private int _currentUserId;
		private List<Product> _availableProducts;
		private int _currentPage = 0;
		private const int PageSize = 10;
		private bool _isLoading = false;
		public OnderhoudWorkOrderCreatePage()
		{
			this.InitializeComponent();
			_currentUserId = User.LoggedInUser.Id;
		}

		private async Task InitializePageAsync()
		{
			using(var db = new AppDbContext())
			{
				var workOrder = await db.WorkOrders.FirstOrDefaultAsync(wo => wo.AppointmentId == _appointmentId);

				if(workOrder != null)
				{
					descriptionTextBox.Text = workOrder.Description;
				}
			}
		}

		private async Task LoadProductsAsync()
		{
			if(_isLoading)
				return;
			_isLoading = true;

			loadingTextBlock.Text = $"Loading more products... ({_availableProducts?.Count ?? 0} loaded)";
			loadingTextBlock.Visibility = Visibility.Visible;

			using(var db = new AppDbContext())
			{
				var products = await db.Products
					.Where(p => p.IsStock)
					.OrderBy(p => p.Id)
					.Skip(_currentPage * PageSize)
					.Take(PageSize)
					.ToListAsync();

				if(products.Any())
				{
					if(_availableProducts == null)
					{
						_availableProducts = new List<Product>();
					}

					_availableProducts.AddRange(products);
					productsListView.ItemsSource = null;
					productsListView.ItemsSource = _availableProducts;

					_currentPage++;
				}
				else
				{
					loadingTextBlock.Text = "All products are loaded.";
				}
			}

			loadingTextBlock.Visibility = Visibility.Collapsed;
			_isLoading = false;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is int appointmentId)
			{
				_appointmentId = appointmentId;
				await InitializePageAsync();
				await LoadProductsAsync();
			}
		}


		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if(sender is CheckBox checkBox)
			{
				var parentPanel = checkBox.Parent as StackPanel;
				if(parentPanel != null)
				{
					var quantityBox = parentPanel.Children.OfType<TextBox>().FirstOrDefault();
					if(quantityBox != null)
					{
						quantityBox.IsEnabled = false;
						quantityBox.Text = string.Empty;
					}
				}
			}
		}
		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if(sender is CheckBox checkBox)
			{
				var parentPanel = checkBox.Parent as StackPanel;
				if(parentPanel != null)
				{
					var quantityBox = parentPanel.Children.OfType<TextBox>().FirstOrDefault();
					if(quantityBox != null)
					{
						quantityBox.IsEnabled = true;
					}
				}
			}
		}

		private void OnScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			var scrollViewer = sender as ScrollViewer;

			if(scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 50)
			{
				_ = LoadProductsAsync();
			}
		}

		private async void SaveWorkOrderButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedProducts = new List<WorkOrderProduct>();

			foreach(var item in productsListView.Items)
			{
				var container = productsListView.ContainerFromItem(item) as ListViewItem;
				if(container != null)
				{
					var panel = container.ContentTemplateRoot as StackPanel;
					var checkBox = panel.Children[0] as CheckBox;
					var quantityBox = panel.Children[1] as TextBox;

					if(checkBox.IsChecked == true && int.TryParse(quantityBox.Text, out int quantity) && quantity > 0)
					{
						var product = checkBox.Tag as Product;
						selectedProducts.Add(new WorkOrderProduct
						{
							ProductId = product.Id,
							Quantity = quantity
						});
					}
				}
			}

			if(!selectedProducts.Any())
			{
				await new MessageDialog("Selecteer minstens één product en vul een geldige hoeveelheid in.").ShowAsync();
				return;
			}

			var workOrder = new WorkOrder
			{
				AppointmentId = _appointmentId,
				UserId = _currentUserId,
				Date_Created = DateTime.Now,
				Description = descriptionTextBox.Text,
				WorkOrderProducts = selectedProducts
			};

			using(var db = new AppDbContext())
			{
				db.WorkOrders.Add(workOrder);
				db.SaveChanges();
			}

			await new MessageDialog("Werkbon succesvol opgeslagen!").ShowAsync();
		}
	}
}
