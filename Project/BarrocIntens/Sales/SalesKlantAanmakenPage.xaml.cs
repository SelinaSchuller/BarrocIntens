using BarrocIntens.Data;
using BarrocIntens.Inkoop;
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
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Sales
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalesKlantAanmakenPage : Page
    {
        public SalesKlantAanmakenPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                var companies = db.Companies.ToList();
                CompanyComboBox.ItemsSource = companies;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SalesMainPage));
        }

		private void PhoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = sender as TextBox;

			string filteredText = new string(textBox.Text.Where(char.IsDigit).ToArray());
			if(textBox.Text != filteredText)
			{
				int cursorPosition = textBox.SelectionStart - (textBox.Text.Length - filteredText.Length);
				textBox.Text = filteredText;
				textBox.SelectionStart = Math.Max(cursorPosition, 0);
			}

			if(textBox.Text.Length > 10)
			{
				textBox.Text = textBox.Text.Substring(0, 10);
				textBox.SelectionStart = 10;
			}
		}


		private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int validationErrors = 0;
            
            validationErrors = ValidateInputs(validationErrors);
            if (validationErrors == 0)
            {
				var selectedCountryCode = (CountryCodeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
				var phoneNumberInput = selectedCountryCode + PhoneNumberTextBox.Text;
				System.Diagnostics.Debug.WriteLine($"Phonenumberinput: {phoneNumberInput}");

				using(var db = new AppDbContext())
                {
                    db.Customers.Add(new Customer
                    {
                        Name = NameInput.Text,
                        Address = AdressInput.Text,
                        Email = EmailInput.Text,
                        PhoneNumber = phoneNumberInput,
                        CompanyId = (int)CompanyComboBox.SelectedValue
                        
                    });

                    db.SaveChanges();

                    Frame.Navigate(typeof(SalesMainPage));
                    return;
                }
            }
            else
            {
                return;
            }
        }

        public int ValidateInputs(int validationErrors)
        {
			var selectedCountryCode = (CountryCodeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
			var phoneNumberInput = selectedCountryCode + PhoneNumberTextBox.Text;
			System.Diagnostics.Debug.WriteLine($"Phonenumberinput: {phoneNumberInput} Count: {phoneNumberInput.Count()}");

			NameError.Visibility = Visibility.Collapsed;
            AdressError.Visibility = Visibility.Collapsed;
            EmailError.Visibility = Visibility.Collapsed;
            TelError.Visibility = Visibility.Collapsed;
            CompanyError.Visibility = Visibility.Collapsed;

            if (NameInput.Text.Length == 0)
            {
                NameError.Visibility = Visibility.Visible;
                validationErrors++;
            }

            if (AdressInput.Text.Length == 0)
            {
                AdressError.Visibility = Visibility.Visible;
                validationErrors++;
            }

            if (!IsValidEmail(EmailInput.Text))
            {
                EmailError.Visibility = Visibility.Visible;
                validationErrors++;
            }

            if (!IsValidPhoneNumber(phoneNumberInput))
            {
                TelError.Visibility = Visibility.Visible;
                validationErrors++;
            }
            else if(string.IsNullOrWhiteSpace(selectedCountryCode) || string.IsNullOrEmpty(PhoneNumberTextBox.Text) || PhoneNumberTextBox.Text.Count() != 8)
            {
				TelError.Visibility = Visibility.Visible;
				validationErrors++;
			}

            if (CompanyComboBox.SelectedValue == null)
            {
                CompanyError.Visibility = Visibility.Visible;
                validationErrors++;
            }

            if (validationErrors > 0)
            {
                return validationErrors;
            }

            return validationErrors;
        }

        bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$");
        }

        bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
