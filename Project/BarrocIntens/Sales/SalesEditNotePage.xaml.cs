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
	public sealed partial class SalesEditNotePage : Page
	{
		private SalesDashboardWindow _parentWindow;
		private List<Note> _notitiesLijst { get; set; }
		private Note _note { get; set; }
		private string _selectedType { get; set; }
		private bool _isNewTypeTextBoxEnabled { get; set; } = true;
		private int _noteId { get; set; }
		private List<string> _noteTypes { get; set; }
		public SalesEditNotePage()
		{
			this.InitializeComponent();

		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is SalesDashboardWindow parentWindow)
			{
				_parentWindow = parentWindow;
				_noteId = _parentWindow.noteId;
				System.Diagnostics.Debug.WriteLine($"SalesEditNotePage: Note Id is {_noteId}");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("SalesEditNotePage: No valid SalesDashboardWindow received.");
			}

			LoadData();
			_selectedType = _note.Type;
			newTypeTextBox.Text = string.Empty;
			newTypeTextBox.Visibility = Visibility.Collapsed;
			typeComboBox.SelectedItem = _selectedType;
		}


		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				_notitiesLijst = db.Notes
					.Include(n => n.Customer)
					.ToList();
				_note = db.Notes.SingleOrDefault(n => n.Id == _noteId);
				titleTextBox.Text = _note.Title.ToString();
				descriptionTextBox.Text = _note.Description.ToString();
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
					Title = "Titel vereist",
					Content = "Voer een titel in voor de notitie voordat u deze opslaat.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};
				titleErrorDialog.ShowAsync();
				return;
			}
			else
			{
				using(var db = new AppDbContext())
				{
					var existingNote = db.Notes.SingleOrDefault(n => n.Id == _note.Id);
					existingNote.Title = titleTextBox.Text;
					existingNote.Description = descriptionTextBox.Text;
					if(_isNewTypeTextBoxEnabled && !string.IsNullOrWhiteSpace(newTypeTextBox.Text))
					{
						string newType = newTypeTextBox.Text.Trim();
						existingNote.Type = newType;

						if(!_noteTypes.Contains(newType))
						{
							_noteTypes.Add(newType);
							_noteTypes = _noteTypes.OrderBy(type => type).ToList();
						}
					}
					else if(!string.IsNullOrEmpty(_selectedType) && _selectedType != "-- Voeg eigen type toe --")
					{
						existingNote.Type = _selectedType;
					}

					db.Notes.Update(existingNote);
					db.SaveChanges();
				}
				_parentWindow.NavigateToNotesPage();
			}
		}

		private async void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
		{
			if(_note == null)
				return;

			// Show confirmation dialog
			ContentDialog deleteDialog = new ContentDialog
			{
				Title = "Bevestiging verwijderen",
				Content = $"Weet u zeker dat u de notitie \"{_note.Title}\" wilt verwijderen?",
				PrimaryButtonText = "Ja",
				CloseButtonText = "Nee",
				XamlRoot = this.XamlRoot
			};

			var result = await deleteDialog.ShowAsync();

			if(result == ContentDialogResult.Primary)
			{
				// Delete the note from the database
				using(var db = new AppDbContext())
				{
					var note = db.Notes.SingleOrDefault(n => n.Id == _note.Id);
					if(note != null)
					{
						db.Notes.Remove(note);
						db.SaveChanges();
						_parentWindow.NavigateToNotesPage();
					}
				}
			}
		}

	}
}
