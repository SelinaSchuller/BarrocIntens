using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarrocIntens.Sales
{
    public sealed partial class OfferteAanmakenPage : Page
    {
        private List<InvoiceItem> selectedInvoiceItems = new List<InvoiceItem>();
        private Invoice currentInvoice;

        public OfferteAanmakenPage()
        {
            this.InitializeComponent();
            using (var db = new AppDbContext())
            {
                currentInvoice = new Invoice
                {
                    ContractId = new Random().Next(500, 9999),
                    DateCreated = DateTime.Now,
                    TotalPrice = 0,
                    Paid = false
                };
                db.Invoices.Add(currentInvoice);
                db.SaveChanges(); // Save the new invoice to the database
            }
        }

        private async void ProductAddButton_Click(object sender, RoutedEventArgs e)
        {
            var productSelectionDialog = new OfferteProductSelection();
            var dialog = new ContentDialog
            {
                Content = productSelectionDialog,
                CloseButtonText = "Cancel",
            };

            dialog.XamlRoot = this.XamlRoot; // Set the XamlRoot explicitly

            var result = await dialog.ShowAsync();
            AddSelectedProductIds(productSelectionDialog.SelectedProductIds);
        }

        public void AddSelectedProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                var products = db.Products.Where(p => productIds.Contains(p.Id)).ToList();

                foreach (var product in products)
                {
                    // Create a new InvoiceItem with default amount of 1
                    var invoiceItem = new InvoiceItem
                    {
                        Amount = 1, // Default to 1 for the first addition
                        ProductId = product.Id,
                        InvoiceId = currentInvoice.Id // Use the correct invoice ID
                    };
                    selectedInvoiceItems.Add(invoiceItem); // Add the invoice item to the list
                    currentInvoice.TotalPrice += product.Price; // Update the total price
                    db.InvoicesItems.Add(invoiceItem); // Add the invoice item to the database
                }

                db.SaveChanges(); // Commit changes to the database
                UpdateProductsListView();
                UpdateTotalPriceTextBlock(); // Update the total price display
            }
        }

        private void UpdateProductsListView()
        {
            OfferteProducten.ItemsSource = null; // Reset the ItemsSource

            // Get InvoiceItems from the database using the current invoice ID
            List<InvoiceItem> InvoiceProducts;
            using (var db = new AppDbContext())
            {
                InvoiceProducts = db.InvoicesItems
                    .Include(i => i.Product) // Include Product details if needed
                    .Where(i => i.InvoiceId == currentInvoice.Id) // Use the correct invoice ID
                    .ToList();
            }

            OfferteProducten.ItemsSource = InvoiceProducts; // Set the new ItemsSource
        }

        private void UpdateTotalPriceTextBlock()
        {
            TotaalPriceTextBlock.Text = currentInvoice.TotalPrice.ToString("€ 0.00"); // Update the total price display
        }

        private void OfferteOpslaanButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement save functionality if needed
        }

        private void AantalProducten_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox AantalTextBox = sender as TextBox;
            if (AantalTextBox == null) return;
            else if (AantalTextBox.Text == "") return;

            ListViewItem listViewItem = FindParent<ListViewItem>(AantalTextBox);
            if (listViewItem != null)
            {
                using (var db = new AppDbContext())
                {
                    TextBlock productIdTextBlock = FindChild<TextBlock>(listViewItem, "ProductId");
                    int productId = int.Parse(productIdTextBlock.Text);

                    var invoiceItem = db.InvoicesItems.FirstOrDefault(i => i.InvoiceId == currentInvoice.Id && i.ProductId == productId);

                    TextBlock prijsTextBlock = FindChild<TextBlock>(listViewItem, "ProductPrijs");
                    string prijs = prijsTextBlock.Text;

                    string aantal = AantalTextBox.Text;
                    int newQuantity;
                    int.TryParse(aantal, out newQuantity);

                    decimal prijsDecimal = decimal.Parse(prijs);
                    decimal totalPrice = currentInvoice.TotalPrice;

                    if (invoiceItem != null)
                    {
                        // Calculate the price change based on the previous amount
                        int previousQuantity = invoiceItem.Amount;

                        // Update the total price based on the change in quantity
                        totalPrice -= previousQuantity * prijsDecimal; // Remove the price of the previous quantity
                        invoiceItem.Amount = newQuantity; // Update the amount in the invoice item
                        totalPrice += newQuantity * prijsDecimal; // Add the price of the new quantity
                    }

                    db.SaveChanges(); // Commit changes to the database
                    currentInvoice.TotalPrice = totalPrice; // Update the current invoice total price
                    UpdateTotalPriceTextBlock(); // Update the total price display
                }
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton == null) return;

            ListViewItem listViewItem = FindParent<ListViewItem>(deleteButton);
            if (listViewItem != null)
            {
                using (var db = new AppDbContext())
                {
                    TextBlock productIdTextBlock = FindChild<TextBlock>(listViewItem, "ProductId");
                    int productId = int.Parse(productIdTextBlock.Text);

                    // Find the invoice item to delete with eager loading of the Product
                    var invoiceItem = db.InvoicesItems
                        .Include(i => i.Product) // Eager load the Product
                        .FirstOrDefault(i => i.InvoiceId == currentInvoice.Id && i.ProductId == productId);

                    if (invoiceItem != null)
                    {
                        // Check if Product is not null before accessing its properties
                        if (invoiceItem.Product != null)
                        {
                            // Update the total price by subtracting the price of the product
                            decimal productPrice = invoiceItem.Product.Price; // Get the price of the product
                            currentInvoice.TotalPrice -= invoiceItem.Amount * productPrice;

                            // Remove the item from the database
                            db.InvoicesItems.Remove(invoiceItem);
                            db.SaveChanges(); // Commit changes to the database

                            // Update the UI
                            UpdateProductsListView(); // Refresh the ListView
                            UpdateTotalPriceTextBlock(); // Update the total price display
                        }
                    }
                }
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }

        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent is valid.
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the requested type child
                T childType = child as T;
                if (childType != null && !string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
            }
            return foundChild;
        }
    }
}