using BarrocIntens.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudWorkOrderCreatePage : Page
	{
		private int _appointmentId;
		private int _currentUserId;
		private List<Product> _availableProducts;

		public OnderhoudWorkOrderCreatePage()
		{
			this.InitializeComponent();
			_currentUserId = User.LoggedInUser.Id;

			InitializePage();
		}

		private void InitializePage()
		{
			using(var db = new AppDbContext())
			{
				var workOrder = db.WorkOrders.FirstOrDefault(wo => wo.AppointmentId == _appointmentId);

				if(workOrder != null)
				{
					descriptionTextBox.Text = workOrder.Description;
				}
			}

			LoadProducts();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is int appointmentId)
			{
				_appointmentId = appointmentId;
				InitializePage();
			}
		}


		private void LoadProducts()
		{
			using(var db = new AppDbContext())
			{
				_availableProducts = db.Products.Where(p => p.IsStock).ToList();
			}

			foreach(var product in _availableProducts)
			{
				var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

				var checkBox = new CheckBox
				{
					Content = product.Name,
					Tag = product
				};
				row.Children.Add(checkBox);

				var quantityBox = new TextBox
				{
					PlaceholderText = "Aantal",
					Width = 50,
					IsEnabled = false
				};

				checkBox.Checked += (s, e) => quantityBox.IsEnabled = true;
				checkBox.Unchecked += (s, e) =>
				{
					quantityBox.IsEnabled = false;
					quantityBox.Text = "";
				};
				row.Children.Add(quantityBox);

				productsPanel.Children.Add(row);
			}
		}

		private async void SaveWorkOrderButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedProducts = new List<WorkOrderProduct>();

			foreach(var child in productsPanel.Children)
			{
				if(child is StackPanel panel)
				{
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
