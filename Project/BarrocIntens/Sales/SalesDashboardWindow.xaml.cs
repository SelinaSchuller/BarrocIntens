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
		public int EmployeeId { get; set; }
		public int NoteId { get; set; }
		public SalesDashboardWindow(int? employeeId)
        {
            this.InitializeComponent();
			this.Title = "Sales";
			Fullscreen fullscreenService = new Fullscreen();
			fullscreenService.SetFullscreen(this);

			if(employeeId != null)
			{
				EmployeeId = employeeId.Value;
			}
			MainFrame.Navigate(typeof(SalesMainPage));

			SetButtonVisibility();
		}

		private void SetButtonVisibility()
		{
			CustomerPageButton.Visibility = Visibility.Visible;
			CompanyPageButton.Visibility = Visibility.Visible;
			OffertePageButton.Visibility = Visibility.Visible;
			ContactPageButton.Visibility = Visibility.Visible;
			NotePageButton.Visibility = Visibility.Visible;

			if(MainFrame.SourcePageType == typeof(SalesMainPage))
			{
				CustomerPageButton.Visibility = Visibility.Collapsed;
			}
			else if(MainFrame.SourcePageType == typeof(SalesCompanyPage))
			{
				CompanyPageButton.Visibility = Visibility.Collapsed;
			}
			else if(MainFrame.SourcePageType == typeof(SalesMainPage))
			{
				OffertePageButton.Visibility = Visibility.Collapsed;
			}
			else if(MainFrame.SourcePageType == typeof(SalesMainPage))
			{
				ContactPageButton.Visibility = Visibility.Collapsed;
			}
			else if(MainFrame.SourcePageType == typeof(SalesNotesPage))
			{
				NotePageButton.Visibility = Visibility.Collapsed;
				NotePageButton.Content = "Notities";
			}
			else if(MainFrame.SourcePageType == typeof(SalesCreateNotePage) || MainFrame.SourcePageType == typeof(SalesEditNotePage))
			{
				NotePageButton.Visibility = Visibility.Visible;
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
			SetButtonVisibility();
		}

		private void ContactPageButton_Click(object sender, RoutedEventArgs e)
		{
			SetButtonVisibility();
		}

		private void NotePageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(SalesNotesPage), this);
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
			this.NoteId = NoteId;
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

	}
}