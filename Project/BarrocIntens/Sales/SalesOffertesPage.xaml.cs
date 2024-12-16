using BarrocIntens.Data;
using BarrocIntens.Converters;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;

namespace BarrocIntens.Sales
{
    public sealed partial class SalesOffertesPage : Page
    {
        private SalesDashboardWindow _parentWindow;
        private List<Invoice> OffertesLijst { get; set; }

        public SalesOffertesPage()
        {
            this.InitializeComponent();

        }

        private async void LoadOffertes()
        {
            using (var db = new AppDbContext())
            {
                OffertesLijst = await db.Invoices
                    .OrderByDescending(n => n.DateCreated)
                    .ToListAsync();

                offerteListView.ItemsSource = OffertesLijst;
                InfoMessageTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _parentWindow = e.Parameter as SalesDashboardWindow;
            LoadOffertes();
        }

        public void CreateOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow?.NavigateToOfferteAanmakenPage();
        }

        private void EditOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var stackPanel = FindParent<StackPanel>(button);
            var offerteIdTextBlock = stackPanel?.Children
                .OfType<TextBlock>()
                .FirstOrDefault(tb => tb.Name == "OfferteIdTextBlock");
            if (offerteIdTextBlock == null)
            {
                return;
            }
            int offerteId = int.Parse(offerteIdTextBlock.Text);
            _parentWindow?.NavigateToOfferteBewerkenPage(offerteId);
        }
        private void DeleteOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Bevestiging",
                Content = "Weet u zeker dat u deze offerte wilt verwijderen?",
                PrimaryButtonText = "Ja",
                CloseButtonText = "Nee",
                XamlRoot = this.XamlRoot
            };
            dialog.ShowAsync();
            dialog.CloseButtonClick += (s, args) =>
            {
                Debug.WriteLine("Dialog closed");
                return;
            };
            dialog.PrimaryButtonClick += (s, args) =>
            {
                var button = sender as Button;
                var stackPanel = FindParent<StackPanel>(button);
                var offerteIdTextBlock = stackPanel?.Children
                    .OfType<TextBlock>()
                    .FirstOrDefault(tb => tb.Name == "OfferteIdTextBlock");

                if (offerteIdTextBlock != null)
                {
                    int offerteId = int.Parse(offerteIdTextBlock.Text);

                    using (var db = new AppDbContext())
                    {
                        var offerte = db.Invoices.Find(offerteId);

                        if (offerte != null)
                        {
                            db.Invoices.Remove(offerte);
                            db.SaveChanges();
                        }

                    }
                    LoadOffertes();
                }
            };
        }

        // Methode om een parent te vinden van een bepaald type als textblock niet gevonden worden
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }
    }
}