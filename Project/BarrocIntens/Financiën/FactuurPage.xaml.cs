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
using BarrocIntens.Financiën;
using Bogus.DataSets;
using System.Net.Mail;
using System.Net;
using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;

namespace BarrocIntens
{
    public sealed partial class FactuurPage : Page
    {

        public FactuurPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                var customers = db.Customers.OrderBy(customer => customer.Name).Select(c => c.Email).ToList();
                CustomerComboBox.ItemsSource = customers;
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var yourMessage = Bericht.Text;
            var subject = Subject.Text;

            var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io")
            {
                Port = 587,
                Credentials = new NetworkCredential("9e0ffd8dda7ba5", "aeb22de8794ddd"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("FinanceTest@Email.com"),
                Subject = subject,
                Body = yourMessage,
                IsBodyHtml = false,
            };

            if (CustomerComboBox.SelectedItem != null)
            {
                if (IsValidEmail(CustomerComboBox.SelectedItem.ToString()))
                {
                    SuccesText.Text = "E-Mail Send!";
                    mailMessage.To.Add(CustomerComboBox.SelectedItem.ToString());

                    smtpClient.Send(mailMessage);

                    ErrorText.Text = "";

                }
                else
                {
                    ErrorText.Text = "Email niet correct";
                }
            }
            else
            {
                ErrorText.Text = "Kies een Email!";
            }
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (AutoCheckbox.IsChecked == true)
            {
                Subject.Text = "Automatische Factuur";
                Subject.IsEnabled = false;
                if (CustomerComboBox.SelectedItem != null)
                {
                    Bericht.Text = $"Beste {new AppDbContext().Customers.Where(c => c.Email == CustomerComboBox.SelectedItem.ToString()).First().Name}, \n\n \n\n Met vriendelijke groet, \n\n Finance";
                }
                else
                {
                    Bericht.Text = $"Beste Klant, \n\n \n\n Met vriendelijke groet, \n\n Finance";
                }
            }
            else
            {
                Subject.Text = "";
                Subject.IsEnabled = true;
                Bericht.Text = "";
            }
        }

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerComboBox.SelectedItem != null && AutoCheckbox.IsChecked == true)
            {
                Bericht.Text = $"Beste {new AppDbContext().Customers.Where(c => c.Email == CustomerComboBox.SelectedItem.ToString()).First().Name}, \n\n \n\n Met vriendelijke groet, \n\n Finance";
            }
        }
    }
}
