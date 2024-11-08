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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoginWindow : Window
	{
		public LoginWindow()
		{
			this.InitializeComponent();
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			using (var db = new AppDbContext())
			{
				string username = NameTextBox.Text;
				string password = PasswordTextBox.Password;
				if (db.Users.Any(u => u.Name == username && u.Password == password))
				{
					int departmentId = db.Users.Where(u => u.Name == username && u.Password == password).Select(u => u.DepartmentId).FirstOrDefault();
					if (departmentId == 1)
					{
						//Opens een nieuwe window (SalesDashboard) en closed de huidige window (LoginWindow)
						var salesDashboard = new Sales.SalesDashboard();
						this.Close();
						salesDashboard.Activate();
					}
					else if (departmentId == 2)
					{
						var onderhoudDashboard = new Onderhoud.OnderhoudBaseWindow();
						this.Close();
						onderhoudDashboard.Activate();
					}
					else if (departmentId == 3)
					{
						// Verander dit naar de juiste window wanneer deze is aangemaakt
						var financeDashboard = new Financiën.FinanciënMainPage();
						this.Close();
						//financeDashboard.Activate();
					}
					else if (departmentId == 4)
					{
						var inkoopDashboard = new Inkoop.InkoopDashboardWindow();
						this.Close();
						inkoopDashboard.Activate();
					}
					else if (departmentId == null)
					{
						ErrorTextBlock.Text = "Er is geen Department aan deze user gekoppelt";
						return;
                    }
                }
				else
				{
                    ErrorTextBlock.Text = "Gebruikersnaam of wachtwoord is onjuist";	
                }

            }
		}
	}
}
