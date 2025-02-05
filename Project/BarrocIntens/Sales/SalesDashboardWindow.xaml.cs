using BarrocIntens.Financiën;
using BarrocIntens.Onderhoud;
using BarrocIntens.Services;
using Microsoft.UI.Windowing;
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
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Sales
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalesDashboardWindow : Window
	{
		public int employeeId { get; set; }
		public int noteId { get; set; }
		public int OfferteId { get; set; }

		public SalesDashboardWindow(int? employeeId)
        {
            this.InitializeComponent();
			this.Title = "Sales";
			Fullscreen fullscreenService = new Fullscreen();
			fullscreenService.SetFullscreen(this);

			if(employeeId != null)
			{
				this.employeeId = employeeId.Value;
				System.Diagnostics.Debug.WriteLine($"SalesDashboardWindow initialized with EmployeeId: {this.employeeId}");

			}
			MainFrame.Navigate(typeof(SalesMainPage));

			SetButtonVisibility();
		}

		private void SetButtonVisibility()
		{
			CustomerPageButton.Visibility = Visibility.Visible;
			CompanyPageButton.Visibility = Visibility.Visible;
			OffertePageButton.Visibility = Visibility.Visible;
            NotePageButton.Visibility = Visibility.Visible;
			CreateServiceRequestPageButton.Visibility = Visibility.Visible;
            NotePageButton.Content = "Notities";

            if(MainFrame.SourcePageType == typeof(SalesCreateNotePage) || MainFrame.SourcePageType == typeof(SalesEditNotePage))
			{
				NotePageButton.Content = "Terug";
			}

		}

		private void CustomerPageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(SalesMainPage));
			SetButtonVisibility();
		}

		private void CompanyPageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(SalesCompanyPage));
			SetButtonVisibility();
		}

		private void OffertePageButton_Click(object sender, RoutedEventArgs e)
		{

			MainFrame.Navigate(typeof(SalesOffertesPage), this);
			SetButtonVisibility();
        }

		private void NotePageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(SalesNotesPage), this);
			SetButtonVisibility();
		}
		
		private void CreateServiceRequestPageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(SalesStoringAanvraagCreatePage), this);
			SetButtonVisibility();
		}

		public void NavigateToCreateNotePage()
		{
			NotePageButton.Visibility = Visibility.Visible;
			NotePageButton.Content = "Terug";
			MainFrame.Navigate(typeof(SalesCreateNotePage), this);
			SetButtonVisibility();
		}

		public void NavigateToEditNotePage(int NoteId)
		{
			this.noteId = NoteId;
			NotePageButton.Visibility = Visibility.Visible;
			NotePageButton.Content = "Terug";
			MainFrame.Navigate(typeof(SalesEditNotePage), this);
			SetButtonVisibility();
		}

		public void NavigateToNotesPage()
		{
			System.Diagnostics.Debug.WriteLine("Navigating to SalesNotesPage after saving note.");
			MainFrame.Navigate(typeof(SalesNotesPage), this);
			SetButtonVisibility();
		}
		public void NavigateToMainPage()
		{
			System.Diagnostics.Debug.WriteLine("Navigating to SalesMainPage after saving storing.");
			MainFrame.Navigate(typeof(SalesMainPage), this);
			SetButtonVisibility();
		}

		public void NavigateToOfferteAanmakenPage()
		{
			MainFrame.Navigate(typeof(OfferteAanmakenPage));
			SetButtonVisibility();
		}
		public void NavigateToOfferteBewerkenPage(int OfferteId)
		{
			this.OfferteId = OfferteId;
            MainFrame.Navigate(typeof(SalesOfferteEditPage), this);
            SetButtonVisibility();
		}
	}
}
