using BarrocIntens.Data;
using BarrocIntens.Converters;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BarrocIntens.Sales
{
    public sealed partial class SalesOffertesPage : Page
    {
        private SalesDashboardWindow _parentWindow;
        private List<Invoice> OffertesLijst { get; set; }

        public SalesOffertesPage()
        {
            this.InitializeComponent();
            LoadOffertes();
        }

        private async void LoadOffertes()
        {
            using (var db = new AppDbContext())
            {
                OffertesLijst = await db.Invoices
                    .OrderBy(n => n.DateCreated)
                    .ToListAsync();

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
            _parentWindow?.NavigateToOfferteAanmakenPage();
        }

        private void EditOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow?.NavigateToOfferteBewerkenPage();
            // finish later
        }

        private void DeleteOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOfferte = offerteListView.SelectedItem as Invoice;
            if (selectedOfferte == null)
            {
                return;
            }
            using (var db = new AppDbContext())
            {
                var offerte = db.Invoices.Find(selectedOfferte.Id);
                db.Invoices.Remove(offerte);
                db.SaveChanges();
            }
            LoadOffertes();
        }
    }
}