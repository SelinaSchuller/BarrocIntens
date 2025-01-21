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

namespace BarrocIntens.Sales
{ 
	public sealed partial class SalesCompanyPage : Page
	{
		private List<Company> _companyList { get; set; }
		private List<Customer> _customerList { get; set; }
		private int _companyId { get; set; }
		private Company _selectedCompany { get; set; }
		public SalesCompanyPage()
		{
			this.InitializeComponent();

			LoadData();

		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				_companyList = db.Companies
					.OrderBy(n => n.Name)
					.ToList();

				companiesListView.ItemsSource = _companyList;
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
				companiesListView.ItemsSource = _companyList;
			}
			else
			{
				companiesListView.ItemsSource = _companyList
					.Where(c => c.Name.ToLower().Contains(searchText))
					.ToList();
			}
		}

		private void CompaniesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			if(companiesListView.SelectedItem is Company selectedCompany)
			{
				_selectedCompany = selectedCompany;

				_companyId = selectedCompany.Id;
				CompanyNameTextBlock.Text = selectedCompany.Name;
				CompanyBkrIcon.Visibility = selectedCompany.Bkr ? Visibility.Visible : Visibility.Collapsed;

				using(var db = new AppDbContext())
				{
					_customerList = db.Customers
						.Where(c => c.CompanyId == _companyId)
						.OrderBy(n => n.Name)
						.ToList();

					customersListView.ItemsSource = _customerList;
				}
			}
		}

		private void ToggleBkr_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if(_selectedCompany != null)
			{
				_selectedCompany.Bkr = !_selectedCompany.Bkr;

				using(var db = new AppDbContext())
				{
					var company = db.Companies.SingleOrDefault(c => c.Id == _selectedCompany.Id);
					if(company != null)
					{
						company.Bkr = _selectedCompany.Bkr;
						db.SaveChanges();
					}
				}

				LoadData();
				companiesListView.SelectedItem = _companyList.FirstOrDefault(c => c.Id == _selectedCompany.Id);
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

	public class BooleanToOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is bool boolValue)
			{
				return boolValue ? 1.0 : 0.0;
			}
			return 0.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value is double opacity && opacity == 1.0;
		}
	}

}
