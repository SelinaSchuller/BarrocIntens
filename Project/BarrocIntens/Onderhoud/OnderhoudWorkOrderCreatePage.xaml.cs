using BarrocIntens.Data;
using BarrocIntens.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WinRT.Interop;

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
		private OnderhoudBaseWindow _parentWindow;
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

			if(e.Parameter is OnderhoudBaseWindow parentWindow)
			{
				_parentWindow = parentWindow;

				_appointmentId = _parentWindow.appointmentId;
				await InitializePageAsync();
				await LoadProductsAsync();
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("OnderhoudWorkOrderCreatePage: No valid OnderhoudBaseWindow received.");
			}
		}


		private void NoProductCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			foreach(var item in productsListView.Items)
			{
				var container = productsListView.ContainerFromItem(item) as ListViewItem;
				if(container != null)
				{
					var panel = container.ContentTemplateRoot as StackPanel;
					var checkBox = panel.Children[0] as CheckBox;
					if(checkBox != null)
					{
						checkBox.IsEnabled = false;
						checkBox.IsChecked = false;
					}
				}
			}
		}

		private void NoProductCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			foreach(var item in productsListView.Items)
			{
				var container = productsListView.ContainerFromItem(item) as ListViewItem;
				if(container != null)
				{
					var panel = container.ContentTemplateRoot as StackPanel;
					var checkBox = panel.Children[0] as CheckBox;
					if(checkBox != null)
					{
						checkBox.IsEnabled = true;
					}
				}
			}
		}

		private void ProductCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			noProductCheckBox.IsChecked = false;
			noProductCheckBox.IsEnabled = false;
		}

		private void ProductCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			bool anyChecked = false;

			foreach(var item in productsListView.Items)
			{
				var container = productsListView.ContainerFromItem(item) as ListViewItem;
				if(container != null)
				{
					var panel = container.ContentTemplateRoot as StackPanel;
					var checkBox = panel.Children[0] as CheckBox;
					if(checkBox != null && checkBox.IsChecked == true)
					{
						anyChecked = true;
						break;
					}
				}
			}

			if(!anyChecked)
			{
				noProductCheckBox.IsEnabled = true;
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

			if(noProductCheckBox.IsChecked == true)
			{
				var workOrder = new WorkOrder
				{
					AppointmentId = _appointmentId,
					UserId = _currentUserId,
					Date_Created = DateTime.Now,
					Description = descriptionTextBox.Text,
					WorkOrderProducts = null
				};

				using(var db = new AppDbContext())
				{
					db.WorkOrders.Add(workOrder);
					db.SaveChanges();
				}

				ContentDialog saveDialog = new ContentDialog
				{
					Title = "Succes",
					Content = "Werkbon succesvol opgeslagen!",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};

				await saveDialog.ShowAsync();

				_parentWindow.NavigateToPlanningPage();
				return;
			}

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
				ContentDialog errorDialog = new ContentDialog
				{
					Title = "Leeg veld",
					Content = "Selecteer minstens één product met een aantal of vink 'Geen product nodig' aan.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};

				await errorDialog.ShowAsync();
				return;
			}

			var workOrderWithProducts = new WorkOrder
			{
				AppointmentId = _appointmentId,
				UserId = _currentUserId,
				Date_Created = DateTime.Now,
				Description = descriptionTextBox.Text,
				WorkOrderProducts = selectedProducts
			};

			using(var db = new AppDbContext())
			{
				db.WorkOrders.Add(workOrderWithProducts);
				db.SaveChanges();
			}

			ContentDialog dialog = new ContentDialog
			{
				Title = "Succes",
				Content = "Werkbon succesvol opgeslagen!",
				CloseButtonText = "Ok",
				XamlRoot = this.XamlRoot
			};

			await dialog.ShowAsync();

			_parentWindow.NavigateToPlanningPage();
		}
	}
}
