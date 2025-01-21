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
using BarrocIntens.Services;

namespace BarrocIntens.Inkoop
{
    public sealed partial class InkoopDashboardWindow : Window
    {
        public InkoopDashboardWindow()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(ProductenPage));
            Fullscreen fullscreenService = new Fullscreen();
            fullscreenService.SetFullscreen(this);
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
