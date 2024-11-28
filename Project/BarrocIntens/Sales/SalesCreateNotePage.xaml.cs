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
		private List<Customer> KlantenLijst { get; set; }
		private int EmployeeId { get; set; }
		private List<string> NoteTypes { get; set; }
		private string SelectedType { get; set; }
		private bool IsComboBoxEnabled { get; set; } = true;
		private bool IsNewTypeTextBoxEnabled { get; set; } = true;
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
				EmployeeId = _parentWindow.EmployeeId;
				System.Diagnostics.Debug.WriteLine($"SalesCreateNotePage: Employee Id is {EmployeeId}");
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
				KlantenLijst = db.Customers
					.OrderBy(c => c.Name)
					.ToList();
				customerInput.ItemsSource = KlantenLijst;

				NoteTypes = db.Notes
				.Select(n => n.Type)
				.Distinct()
				.OrderBy(type => type)
				.ToList();

				NoteTypes.Insert(0, "-- Voeg eigen type toe --");
			}
		}
		private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(typeComboBox.SelectedItem.ToString() == "-- Voeg eigen type toe --")
			{
				newTypeTextBox.Visibility = Visibility.Visible;
				SelectedType = string.Empty;
			}
			else
			{
				newTypeTextBox.Visibility = Visibility.Collapsed;
				SelectedType = typeComboBox.SelectedItem.ToString();
			}
		}
		private void SaveNoteButton_Click(object sender, RoutedEventArgs e)
		{
			if((string.IsNullOrWhiteSpace(titleTextBox.Text)) || ((string.IsNullOrWhiteSpace(newTypeTextBox.Text) && string.IsNullOrWhiteSpace(SelectedType))))
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
				string Type = string.Empty;
				if(IsNewTypeTextBoxEnabled && !string.IsNullOrWhiteSpace(newTypeTextBox.Text))
				{
					Type = newTypeTextBox.Text.Trim();

					if(!NoteTypes.Contains(Type))
					{
						NoteTypes.Add(Type);
						NoteTypes = NoteTypes.OrderBy(type => type).ToList();
					}
				}
				else if(!string.IsNullOrEmpty(SelectedType) && SelectedType != "-- Voeg eigen type toe --")
				{
					Type = SelectedType;
				}

				var newNote = new Note
				{
					Title = titleTextBox.Text,
					Type = Type,
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

