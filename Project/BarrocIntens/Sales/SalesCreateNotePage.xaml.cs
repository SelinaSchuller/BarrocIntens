using BarrocIntens.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;

namespace BarrocIntens.Sales
{
	public sealed partial class SalesCreateNotePage : Page
	{
		private SalesDashboardWindow _parentWindow;
		private List<Customer> _klantenLijst { get; set; }
		private int _employeeId { get; set; }
		private List<string> _noteTypes { get; set; }
		private string _selectedType { get; set; }
		private bool _isComboBoxEnabled { get; set; } = true;
		private bool _isNewTypeTextBoxEnabled { get; set; } = true;
		public SalesCreateNotePage()
		{
			this.InitializeComponent();
			
			LoadData();
			newTypeTextBox.Text = string.Empty;
			newTypeTextBox.Visibility = Visibility.Collapsed;
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is SalesDashboardWindow parentWindow)
			{
				_parentWindow = parentWindow;
				_employeeId = _parentWindow.employeeId;
				System.Diagnostics.Debug.WriteLine($"SalesCreateNotePage: Employee Id is {_employeeId}");
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
				customerInput.ItemsSource = _klantenLijst;

				_noteTypes = db.Notes
				.Select(n => n.Type)
				.Distinct()
				.OrderBy(type => type)
				.ToList();

				_noteTypes.Insert(0, "-- Voeg eigen type toe --");
			}
		}
		private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(typeComboBox.SelectedItem.ToString() == "-- Voeg eigen type toe --")
			{
				newTypeTextBox.Visibility = Visibility.Visible;
				_selectedType = string.Empty;
			}
			else
			{
				newTypeTextBox.Visibility = Visibility.Collapsed;
				_selectedType = typeComboBox.SelectedItem.ToString();
			}
		}
		private void SaveNoteButton_Click(object sender, RoutedEventArgs e)
		{
			if((string.IsNullOrWhiteSpace(titleTextBox.Text)) || ((string.IsNullOrWhiteSpace(newTypeTextBox.Text) && string.IsNullOrWhiteSpace(_selectedType))))
			{
				ContentDialog titleErrorDialog = new ContentDialog
				{
					Title = "Een of meerdere velden is leeg",
					Content = "Voer alle velden in voor de notitie voordat u deze opslaat.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};
				titleErrorDialog.ShowAsync();
				return;
			}
			else if(customerInput.SelectedItem is Customer selectedCustomer)
			{
				string type = string.Empty;
				if(_isNewTypeTextBoxEnabled && !string.IsNullOrWhiteSpace(newTypeTextBox.Text))
				{
					type = newTypeTextBox.Text.Trim();

					if(!_noteTypes.Contains(type))
					{
						_noteTypes.Add(type);
						_noteTypes = _noteTypes.OrderBy(type => type).ToList();
					}
				}
				else if(!string.IsNullOrEmpty(_selectedType) && _selectedType != "-- Voeg eigen type toe --")
				{
					type = _selectedType;
				}

				var newNote = new Note
				{
					Title = titleTextBox.Text,
					Type = type,
					Description = descriptionTextBox.Text,
					Date_Created = DateTime.Now,
					CustomerId = selectedCustomer.Id,
					EmployeeId = _employeeId
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

