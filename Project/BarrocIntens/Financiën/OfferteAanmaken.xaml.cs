using BarrocIntens.Data; // Ensure you have this namespace for your Product model
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarrocIntens.Financiën
{
    public sealed partial class OfferteAanmaken : Page
    {
        private List<Product> selectedProducts = new List<Product>();

        public OfferteAanmaken()
        {
            this.InitializeComponent();
        }

        public void TerugButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back
            // this.Frame.Navigate(typeof(Financiën.Financiën));
        }

        private async void ProductAddButton_Click(object sender, RoutedEventArgs e)
        {
            var productSelectionDialog = new OfferteProductSelection();
            var dialog = new ContentDialog
            {
                Content = productSelectionDialog,
                CloseButtonText = "Cancel",
            };

            // Set the XamlRoot explicitly to avoid the ArgumentException
            dialog.XamlRoot = this.XamlRoot;

            // Show the dialog and wait for user action
            var result = await dialog.ShowAsync();
            AddSelectedProductIds(productSelectionDialog.SelectedProductIds);
        }

        public void AddSelectedProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                var products = db.Products.Where(p => productIds.Contains(p.Id)).ToList();
                selectedProducts.AddRange(products);
                UpdateProductsListView();
            }
        }

        private void UpdateProductsListView()
        {
            OfferteProducten.ItemsSource = null; // Reset the ItemsSource
            OfferteProducten.ItemsSource = selectedProducts; // Set the new ItemsSource
        }
    }
}