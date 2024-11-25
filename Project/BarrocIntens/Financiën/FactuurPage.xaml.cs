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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FactuurPage : Page
    {

        public FactuurPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                var customers = db.Customers.Select(c => c.Email).ToList();
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

            if (IsValidEmail(Email.Text))
            {
                SuccesText.Text = "E-Mail Send!";
                mailMessage.To.Add(Email.Text);

                smtpClient.Send(mailMessage);

                ErrorText.Text = "";

            }
            else
            {
                ErrorText.Text = "Invalid Email";
            }

        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
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
    }
}
