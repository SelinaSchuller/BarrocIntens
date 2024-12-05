using BarrocIntens.Data;
using Bogus.DataSets;
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
	public sealed partial class SalesNotesPage : Page
	{
		private SalesDashboardWindow _parentWindow;
		private List<Note> _notitieLijst { get; set; }
		public SalesNotesPage()
		{
			this.InitializeComponent();

			using(var db = new AppDbContext())
			{
				_notitieLijst = db.Notes
					.Include(n => n.Customer)
					.Include(n => n.Employee)
					.OrderBy(n => n.Date_Created)
					.ToList();

				notesListView.ItemsSource = _notitieLijst;
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			_parentWindow = e.Parameter as SalesDashboardWindow;
		}

		public void CreateNoteButton_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"Navigating to SalesCreateNotePage. EmployeeId: {_parentWindow.employeeId}");
			_parentWindow?.NavigateToCreateNotePage();
		}

		private void EditNoteButton_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var note = button?.DataContext as Note;

			if(note != null)
			{
				System.Diagnostics.Debug.WriteLine($"Note: {note.Title}");
				_parentWindow?.NavigateToEditNotePage(note.Id);
			}
		}
	}

}
