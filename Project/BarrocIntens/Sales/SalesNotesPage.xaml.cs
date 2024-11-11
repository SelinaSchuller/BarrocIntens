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
	public sealed partial class SalesNotesPage : Page
	{
		private SalesDashboardWindow _parentWindow;
		private int EmployeeId { get; set; }
		private List<Note> NotitieLijst { get; set; }
		public SalesNotesPage()
		{
			this.InitializeComponent();

			using(var db = new AppDbContext())
			{
				NotitieLijst = db.Notes
					.Include(n => n.Customer)
					.Include(n => n.Employee)
					.ThenInclude(e => e.Name)
					.OrderBy(n => n.Date_Created)
					.ToList();

				notesListView.ItemsSource = NotitieLijst;
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			_parentWindow = e.Parameter as SalesDashboardWindow;
		}

		public void CreateNoteButton_Click(object sender, RoutedEventArgs e)
		{
			_parentWindow?.NavigateToCreateNotePage();
		}

	}

}
