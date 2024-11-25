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
	public sealed partial class SalesMainPage : Page
	{
		public SalesMainPage()
		{
			this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                customersListView.ItemsSource = db.Customers.OrderBy(p => p.Id).ToList();
            }
        }

		private void NewCustomerButton_Click(object sender, RoutedEventArgs e)
		{

		}
		private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{			

        }
        private void customersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customersListView.SelectedItem is Customer selectedCustomer)
            {
                using (var db = new AppDbContext())
                {
                    var customerDetails = db.Customers
                                             .Include(c => c.Company)
                                             .Where(c => c.Id == selectedCustomer.Id)
                                             .OrderBy(c => c.Id)
                                             .ToList();

                    customerInfoListView.ItemsSource = customerDetails;
                }
            }
        }
    }
}
