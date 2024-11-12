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
	public sealed partial class SalesCompanyPage : Page
	{
		private List<Company> CompanyList { get; set; }
		private List<Customer> CustomerList { get; set; }
		private int CompanyId { get; set; }
		private Company SelectedCompany { get; set; }
		public SalesCompanyPage()
		{
			this.InitializeComponent();

			LoadData();

		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				CompanyList = db.Companies
					.OrderBy(n => n.Name)
					.ToList();

				companiesListView.ItemsSource = CompanyList;
			}
		}

		private void NewCompanyButton_Click(object sender, RoutedEventArgs e)
		{

		}
		private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string searchText = (sender as TextBox)?.Text.ToLower();

			if(string.IsNullOrWhiteSpace(searchText))
			{
				companiesListView.ItemsSource = CompanyList;
			}
			else
			{
				companiesListView.ItemsSource = CompanyList
					.Where(c => c.Name.ToLower().Contains(searchText))
					.ToList();
			}
		}

		private void CompaniesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			if(companiesListView.SelectedItem is Company selectedCompany)
			{
				SelectedCompany = selectedCompany;

				CompanyId = selectedCompany.Id;
				CompanyNameTextBlock.Text = selectedCompany.Name;
				CompanyBkrIcon.Visibility = selectedCompany.Bkr ? Visibility.Visible : Visibility.Collapsed;

				using(var db = new AppDbContext())
				{
					CustomerList = db.Customers
						.Where(c => c.CompanyId == CompanyId)
						.OrderBy(n => n.Name)
						.ToList();

					customersListView.ItemsSource = CustomerList;
				}
			}
		}

		private void ToggleBkr_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if(SelectedCompany != null)
			{
				SelectedCompany.Bkr = !SelectedCompany.Bkr;

				CompanyBkrIcon.Visibility = SelectedCompany.Bkr ? Visibility.Visible : Visibility.Collapsed;

				using(var db = new AppDbContext())
				{
					var company = db.Companies.SingleOrDefault(c => c.Id == SelectedCompany.Id);
					if(company != null)
					{
						company.Bkr = SelectedCompany.Bkr;
						db.SaveChanges();
					}
				}

				LoadData();

				companiesListView.SelectedItem = CompanyList.FirstOrDefault(c => c.Id == SelectedCompany.Id);
			}
		}

	}
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is bool boolValue)
			{
				return boolValue ? Visibility.Visible : Visibility.Collapsed;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value is Visibility visibility && visibility == Visibility.Visible;
		}
	}
}
