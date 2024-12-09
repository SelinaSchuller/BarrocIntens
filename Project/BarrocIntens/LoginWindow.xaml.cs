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
using BarrocIntens.Data;
using BarrocIntens.Services;
using Windows.UI.ViewManagement;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using System.Text;
using System.Security.Cryptography;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        private int _userId { get; set; }
        public LoginWindow()
        {
            this.InitializeComponent();
            this.Title = "Login Pagina";
            Fullscreen fullscreenService = new Fullscreen();
            fullscreenService.SetFullscreen(this);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                string email = mailTextBox.Text;
                string password = PasswordTextBox.Password;
                string hashedPassword = HashPassword(password); // Hash the entered password

                // Compare the hashed password with the stored hashed password
                var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);

                if (user != null)
                {
                    int departmentId = user.DepartmentId;
                    int userId = user.Id;
                    _userId = userId;

                    switch (departmentId)
                    {
                        case 1:
                            var salesDashboard = new Sales.SalesDashboardWindow(userId);
                            System.Diagnostics.Debug.WriteLine($"User: {userId}");
                            salesDashboard.Activate();
                            break;

                        case 2:
                            var onderhoudDashboard = new Onderhoud.OnderhoudBaseWindow(userId);
                            onderhoudDashboard.Activate();
                            break;

                        case 3:
                            var financeDashboard = new Financiën.FinanciënMainWindow(userId);
                            this.Close();
                            financeDashboard.Activate();
                            break;

                        case 4:
                            var inkoopDashboard = new Inkoop.InkoopDashboardWindow();
                            inkoopDashboard.Activate();
                            break;

                        case 6:
                            var plannerDashboard = new Onderhoud.OnderhoudBaseWindow(userId);
                            plannerDashboard.Activate();
                            break;

                        default:
                            ErrorTextBlock.Text = "Er is geen Department aan deze user gekoppeld";
                            return;
                    }
                }
                else
                {
                    ErrorTextBlock.Text = "E-mail of wachtwoord is onjuist";
                }
            }
        }
    }
}
