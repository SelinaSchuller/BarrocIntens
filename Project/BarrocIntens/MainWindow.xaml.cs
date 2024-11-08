using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using BarrocIntens.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using BarrocIntens.Onderhoud;
using BarrocIntens.Inkoop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }


            //Als login werkt:
            //var baseWindow = new LoginWindow();
            //baseWindow.Activate();

            //Tijdelijk om gelijk naar je pagina te kijken:
            var baseWindow = new OnderhoudBaseWindow();
            baseWindow.Activate();
            DispatcherQueue.TryEnqueue(() =>
            {
                this.Close();
            });
        }
		private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
		{
			SelectorBarItem selectedItem = sender.SelectedItem;
			if(selectedItem == SalesDashboardSelector)
			{
				MainFrame.Navigate(typeof(SalesDashboard));
			}
		}
		/// <summary>
		/// An empty window that can be used on its own or navigated to within a Frame.
		/// </summary>

	}
}
