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
        public SalesDashboardWindow()
        {
            this.InitializeComponent();

			this.Title = "Sales";

			

			MainFrame.Navigate(typeof(SalesMainPage));
		}

		private void CustomerPageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void OffertePageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ContactPageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void NotePageButton_Click(object sender, RoutedEventArgs e)
		{

		}

		
	}
}
