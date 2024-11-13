using BarrocIntens.Data;
using BarrocIntens.Services;
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
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Financiën
{
    public sealed partial class FinanciënMainWindow
    {

        public FinanciënMainWindow()
        {
            this.InitializeComponent();

            Fullscreen fullscreenService = new Fullscreen();
            fullscreenService.SetFullscreen(this);

            using (var db = new AppDbContext())
            {
                contractListView.ItemsSource = db.LeaseContracts.Include(c => c.Company);
            }
        }
    }

}
