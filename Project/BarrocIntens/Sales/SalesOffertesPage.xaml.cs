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
    public sealed partial class SalesOffertesPage : Page
    {
        private SalesDashboardWindow _parentWindow;
        private int EmployeeId { get; set; }
        private List<Invoice> OffertesLijst { get; set; }
        public SalesOffertesPage()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                OffertesLijst = db.Invoices
                    .OrderBy(n => n.DateCreated)
                    .ToList();

                offerteListView.ItemsSource = OffertesLijst;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _parentWindow = e.Parameter as SalesDashboardWindow;
        }

        public void CreateOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.NavigateToOfferteAanmakenPage();
        }

        private void EditOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var note = button?.DataContext as Note;

            if (note != null)
            {
               // System.Diagnostics.Debug.WriteLine($"Note: {note.Title}");
                _parentWindow.NavigateToOfferteBewerkenPage();
            }
            // finish later
        }
    }

}
