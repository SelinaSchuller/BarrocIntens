using BarrocIntens.Onderhoud;
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
		private int EmployeeId { get; set; }
		public SalesDashboardWindow(int? employeeId)
        {
            this.InitializeComponent();
			this.Title = "Sales";

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
			OffertePageButton.Visibility = Visibility.Visible;
			ContactPageButton.Visibility = Visibility.Visible;
			NotePageButton.Visibility = Visibility.Visible;

			if(MainFrame.SourcePageType == typeof(SalesMainPage))
			{
				CustomerPageButton.Visibility = Visibility.Collapsed;
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

			}
		}

		private void CustomerPageButton_Click(object sender, RoutedEventArgs e)
		{
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
			NotePageButton.Content = "Terug naar Notities";
			MainFrame.Navigate(typeof(SalesCreateNotePage), EmployeeId);
			SetButtonVisibility();
		}

		public void CreateNotePageButton_Click(object sender, RoutedEventArgs e)
		{
			NotePageButton.Visibility = Visibility.Visible;
			NotePageButton.Content = "Terug naar Notities";
			MainFrame.Navigate(typeof(SalesCreateNotePage), EmployeeId);
			SetButtonVisibility();
		}




	}
}
