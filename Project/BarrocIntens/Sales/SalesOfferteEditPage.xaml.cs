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
using System.Linq;

namespace BarrocIntens.Sales
{
    public sealed partial class SalesOfferteEditPage : Page
    {
        private int offerteId { get; set; }
        private List<InvoiceItem> selectedInvoiceItems = new List<InvoiceItem>();
        private Invoice currentInvoice;
        private SalesDashboardWindow _parentWindow;

        public SalesOfferteEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is SalesDashboardWindow parentWindow)
            {
                _parentWindow = parentWindow;
                offerteId = _parentWindow.OfferteId;
                System.Diagnostics.Debug.WriteLine($"SalesEditOffertePage: Offerte Id is {offerteId}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SalesEditOffertePage: No valid SalesDashboardWindow received.");
            }

            LoadInvoice();
        }
        private void LoadInvoice()
        {
            using (var db = new AppDbContext())
            {
                // Load the invoice
                currentInvoice = db.Invoices.Find(offerteId);

                if (currentInvoice == null)
                {
                    throw new InvalidOperationException("Offerte not found");
                }

                this.DataContext = currentInvoice;
                var products = db.InvoicesItems
                    .Include(i => i.Product)
                    .Where(i => i.InvoiceId == currentInvoice.Id)
                    .ToList();

                foreach (var product in products)
                {
                    selectedInvoiceItems.Add(product);
                }

                UpdateProductsListView();
                UpdateTotalPriceTextBlock();
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

            dialog.XamlRoot = this.XamlRoot;

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
                    var invoiceItem = new InvoiceItem
                    {
                        Amount = 1,
                        ProductId = product.Id,
                        InvoiceId = currentInvoice.Id
                    };
                    selectedInvoiceItems.Add(invoiceItem);
                    currentInvoice.TotalPrice += product.Price;
                    db.InvoicesItems.Add(invoiceItem);
                }

                db.SaveChanges();
                UpdateProductsListView();
                UpdateTotalPriceTextBlock();
            }
        }

        private void UpdateProductsListView()
        {
            OfferteProductenListView.ItemsSource = null;

            List<InvoiceItem> InvoiceProducts;
            using (var db = new AppDbContext())
            {
                InvoiceProducts = db.InvoicesItems
                    .Include(i => i.Product)
                    .Where(i => i.InvoiceId == currentInvoice.Id)
                    .ToList();
            }

            OfferteProductenListView.ItemsSource = InvoiceProducts;
        }

        private void UpdateTotalPriceTextBlock()
        {
            TotaalPriceTextBlock.Text = currentInvoice.TotalPrice.ToString("0.00");
        }

        private void OfferteOpslaanButton_Click(object sender, RoutedEventArgs e)
        {
            var totalPrices = TotaalPriceTextBlock.Text;
            decimal totalPrice = decimal.Parse(totalPrices);
            using (var db = new AppDbContext())
            {
                var invoice = db.Invoices.Find(currentInvoice.Id);
                invoice.TotalPrice = decimal.Parse(totalPrices);
                db.SaveChanges();
            }
            var dialog = new ContentDialog
            {
                Title = "Bevestiging",
                Content = "Offerte succesvol aangepast!",
                PrimaryButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            dialog.ShowAsync();
            dialog.PrimaryButtonClick += (s, args) =>
            {
                Frame.GoBack();
            };

        }

        private void AantalProducten_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox AantalTextBox = sender as TextBox;
            if (AantalTextBox == null) return;
            else if (AantalTextBox.Text == "") return;
            else if (AantalTextBox.Text.Length > 1 && AantalTextBox.Text[0] == '0') AantalTextBox.Text = AantalTextBox.Text.Remove(0, 1);
            else if (AantalTextBox.Text.Length > 1 && AantalTextBox.Text[0] == '-') AantalTextBox.Text = AantalTextBox.Text.Remove(0, 1);
            else if (AantalTextBox.Text.Length > 1 && !int.TryParse(AantalTextBox.Text, out int result)) AantalTextBox.Text = AantalTextBox.Text.Remove(AantalTextBox.Text.Length - 1);
            else if (AantalTextBox.Text.Length == 1 && !int.TryParse(AantalTextBox.Text, out int result2)) AantalTextBox.Text = "";
            else if (AantalTextBox.Text.Length > 9) AantalTextBox.Text = AantalTextBox.Text.Remove(AantalTextBox.Text.Length - 1);
            else if (AantalTextBox.Text.Length > 1 && int.Parse(AantalTextBox.Text) > 999999999) AantalTextBox.Text = AantalTextBox.Text.Remove(AantalTextBox.Text.Length - 1);



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
                        int previousQuantity = invoiceItem.Amount;

                        totalPrice -= previousQuantity * prijsDecimal;
                        invoiceItem.Amount = newQuantity;
                        totalPrice += newQuantity * prijsDecimal;
                    }

                    db.SaveChanges();
                    currentInvoice.TotalPrice = totalPrice;
                    UpdateTotalPriceTextBlock();
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

                    var invoiceItem = db.InvoicesItems
                        .Include(i => i.Product)
                        .FirstOrDefault(i => i.InvoiceId == currentInvoice.Id && i.ProductId == productId);

                    if (invoiceItem != null)
                    {
                        if (invoiceItem.Product != null)
                        {
                            decimal productPrice = invoiceItem.Product.Price;
                            currentInvoice.TotalPrice -= invoiceItem.Amount * productPrice;

                            db.InvoicesItems.Remove(invoiceItem);
                            db.SaveChanges();

                            UpdateProductsListView();
                            UpdateTotalPriceTextBlock();
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
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
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