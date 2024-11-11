using BarrocIntens.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarrocIntens.Sales
{
	public sealed partial class SalesCreateNotePage : Page
	{
		private SalesDashboardWindow _parentWindow;
		private List<Customer> KlantenLijst { get; set; }
		private int EmployeeId { get; set; }

		public SalesCreateNotePage()
		{
			this.InitializeComponent();
			
			LoadCustomers();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is SalesDashboardWindow parentWindow)
			{
				_parentWindow = parentWindow;
				EmployeeId = _parentWindow.EmployeeId;
				System.Diagnostics.Debug.WriteLine($"SalesCreateNotePage: Employee Id is {EmployeeId}");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("SalesCreateNotePage: No valid SalesDashboardWindow received.");
			}
		}


		private void LoadCustomers()
		{
			using(var db = new AppDbContext())
			{
				KlantenLijst = db.Customers
					.OrderBy(c => c.Name)
					.ToList();
				customerInput.ItemsSource = KlantenLijst;
			}
		}

		private void SaveNoteButton_Click(object sender, RoutedEventArgs e)
		{
			// Check if the Title field is empty
			if(string.IsNullOrWhiteSpace(titleTextBox.Text))
			{
				ContentDialog titleErrorDialog = new ContentDialog
				{
					Title = "Titel vereist",
					Content = "Voer een titel in voor de notitie voordat u deze opslaat.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};
				titleErrorDialog.ShowAsync();
				return;
			}

			if(customerInput.SelectedItem is Customer selectedCustomer)
			{
				

				var newNote = new Note
				{
					Title = titleTextBox.Text,
					Description = descriptionTextBox.Text,
					Date_Created = DateTime.Now,
					CustomerId = selectedCustomer.Id,
					EmployeeId = EmployeeId
				};

				using(var db = new AppDbContext())
				{
					db.Notes.Add(newNote);
					db.SaveChanges();
				}
				_parentWindow.NavigateToNotesPage();
			}
			else
			{
				ContentDialog customerErrorDialog = new ContentDialog
				{
					Title = "Selecteer een klant",
					Content = "Kies een klant uit de lijst voordat u de notitie opslaat.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};
				customerErrorDialog.ShowAsync();
			}
		}


	}
}

