using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using BarrocIntens.Onderhoud;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Inkoop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InkoopDashboardWindow : Window
    {
        public InkoopDashboardWindow()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(ProductenPage));
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(ProductenPage));
        }

        private void BestelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
