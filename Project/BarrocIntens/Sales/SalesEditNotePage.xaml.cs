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
		private Customer Klant { get; set; }
		private List<Note> NotitiesLijst { get; set; }
		private Note _note { get; set; }
		private string SelectedType { get; set; }
		private bool IsComboBoxEnabled { get; set; } = true;
		private bool IsNewTypeTextBoxEnabled { get; set; } = true;
		private int _noteId { get; set; }
		private List<string> NoteTypes { get; set; }
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
				_noteId = _parentWindow.NoteId;
				System.Diagnostics.Debug.WriteLine($"SalesEditNotePage: Note Id is {_noteId}");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("SalesEditNotePage: No valid SalesDashboardWindow received.");
			}

			LoadData();

			SelectedType = _note.Type;
			newTypeTextBox.Text = string.Empty;
			newTypeTextBox.Visibility = Visibility.Collapsed;
			typeComboBox.SelectedItem = SelectedType;
		}


		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				NotitiesLijst = db.Notes
					.Include(n => n.Customer)
					.ToList();
				_note = db.Notes.SingleOrDefault(n => n.Id == _noteId);
				titleTextBox.Text = _note.Title.ToString();
				descriptionTextBox.Text = _note.Description.ToString();

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
				//SelectedType = null;
			}
			else
			{
				newTypeTextBox.Visibility = Visibility.Collapsed;
			}
		}

		


		private void SaveNoteButton_Click(object sender, RoutedEventArgs e)
		{
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
			else
			{
				using(var db = new AppDbContext())
				{
					var existingNote = db.Notes.SingleOrDefault(n => n.Id == _note.Id);
					existingNote.Title = titleTextBox.Text;
					existingNote.Description = descriptionTextBox.Text;

					if(IsNewTypeTextBoxEnabled && !string.IsNullOrWhiteSpace(newTypeTextBox.Text))
					{
						string newType = newTypeTextBox.Text.Trim();
						existingNote.Type = newType;

						if(!NoteTypes.Contains(newType))
						{
							NoteTypes.Add(newType);
							NoteTypes = NoteTypes.OrderBy(type => type).ToList();
						}
					}
					else if(!string.IsNullOrEmpty(SelectedType) && SelectedType != "-- Voeg eigen type toe --")
					{
						existingNote.Type = SelectedType;
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
