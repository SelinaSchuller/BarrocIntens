using BarrocIntens.Data;
using BarrocIntens.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
		private Appointment appointment { get; set; }
		private Customer customer { get; set; }
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

			loadingTextBlock.Text = $"Producten aan het laden... ({_availableProducts?.Count ?? 0} geladen)";
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
					loadingTextBlock.Text = "Alle producten zijn geladen.";
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
			string emailBody = "";

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

					var workOrderTemp = db.WorkOrders
						.Include(wo => wo.Appointment)
						.ThenInclude(a => a.Customer)
						.FirstOrDefault(wo => wo.AppointmentId == _appointmentId);

					if(workOrderTemp != null)
					{
						appointment = workOrderTemp.Appointment;
						customer = appointment.Customer;
					}
				}

				emailBody = $"Beste Hoofd Maintenance,\n\n" +
							$"Er is een nieuwe werkbron aangemaakt.\n" +
							$"Afspraak: {appointment.Description}\n" +
							$"Afspraak datum: {appointment.Date.ToString()}\n" +
							$"Klant: {customer.Name}\n\n" +
							$"Omschrijving: {descriptionTextBox.Text}\n\n" +
							$"Geen producten nodig voor deze reparatie.\n\n" +
							$"Met vriendelijke groet,\n{User.LoggedInUser.Name} van Onderhoud";

				SendEmailToMaintenanceHead("Nieuwe Werkbron Aangemaakt", emailBody);

				ContentDialog saveDialog = new ContentDialog
				{
					Title = "Succes",
					Content = "Werkbron succesvol opgeslagen!",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};

				await saveDialog.ShowAsync();

				_parentWindow.NavigateToPlanningPage();
				return;
			}

			using(var db = new AppDbContext())
			{
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

							var inventory = db.ProductInventories.FirstOrDefault(pi => pi.ProductId == product.Id);
							if(inventory != null)
							{
								if(inventory.InStock >= quantity)
								{
									inventory.InStock -= quantity;
									selectedProducts.Add(new WorkOrderProduct
									{
										ProductId = product.Id,
										Quantity = quantity
									});
								}
								else
								{
									ContentDialog stockDialog = new ContentDialog
									{
										Title = "Onvoldoende voorraad",
										Content = $"Product '{product.Name}' heeft onvoldoende voorraad. Beschikbaar: {inventory.InStock}, gevraagd: {quantity}.",
										CloseButtonText = "Ok",
										XamlRoot = this.XamlRoot
									};

									await stockDialog.ShowAsync();
									return;
								}
							}
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

				db.WorkOrders.Add(workOrderWithProducts);
				db.SaveChanges();

				var workOrderTemp = db.WorkOrders
					.Include(wo => wo.Appointment)
					.ThenInclude(a => a.Customer)
					.FirstOrDefault(wo => wo.AppointmentId == _appointmentId);

				if(workOrderTemp != null)
				{
					appointment = workOrderTemp.Appointment;
					customer = appointment.Customer;
				}
			}

			emailBody = $"Beste Hoofd Maintenance,\n\n" +
						$"Er is een nieuwe werkbron aangemaakt.\n" +
						$"Afspraak: {appointment.Description}\n" +
						$"Afspraak datum: {appointment.Date.ToString()}\n" +
						$"Klant: {customer.Name}\n\n" +
						$"Omschrijving: {descriptionTextBox.Text}\n\n" +
						$"Producten voor deze werkbron:\n";

			foreach(var product in selectedProducts)
			{
				var productDetails = _availableProducts.FirstOrDefault(p => p.Id == product.ProductId);
				emailBody += $"- {productDetails?.Name}: {product.Quantity}\n";
			}

			emailBody += $"\nMet vriendelijke groet,\n{User.LoggedInUser.Name} van Onderhoud";

			SendEmailToMaintenanceHead("Nieuwe Werkbron Aangemaakt", emailBody);

			ContentDialog dialog = new ContentDialog
			{
				Title = "Succes",
				Content = "Werkbron succesvol opgeslagen!",
				CloseButtonText = "Ok",
				XamlRoot = this.XamlRoot
			};

			await dialog.ShowAsync();

			_parentWindow.NavigateToPlanningPage();
		}

		private void SendEmailToMaintenanceHead(string subject, string body)
		{
			var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io")
			{
				Port = 587,
				Credentials = new NetworkCredential("9557a28d5f84d0", "8e0fed3dd4aab8"),
				EnableSsl = true,
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress("onderhoud@barrocintens.nl"),
				Subject = subject,
				Body = body,
				IsBodyHtml = false,
			};

			string maintenanceHeadEmail = "hoofdmaintenance@barrocintens.nl";
			mailMessage.To.Add(maintenanceHeadEmail);

			try
			{
				smtpClient.Send(mailMessage);
				System.Diagnostics.Debug.WriteLine("E-mail succesvol verzonden naar hoofd maintenance.");
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Fout bij het verzenden van e-mail: {ex.Message}");
			}
		}
	}
}
