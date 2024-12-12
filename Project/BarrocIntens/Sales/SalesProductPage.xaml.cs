using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarrocIntens.Sales
{
    public sealed partial class SalesProductPage : Page
    {
        private List<Product> ProductList { get; set; } = new List<Product>();
        private List<ProductCategory> CategoryList { get; set; } = new List<ProductCategory>();

        public SalesProductPage()
        {
            this.InitializeComponent();
            LoadProductsAsync();
        }

        private async void LoadProductsAsync()
        {
            using (var db = new AppDbContext())
            {
                ProductList = db.Products.Include(p => p.Category)
                                         .Where(p => p.VisibleForCustomers && p.Category.Id != 1)
                                         .OrderBy(p => p.Id)
                                         .ToList();
                ProductListView.ItemsSource = ProductList;

                CategoryList = db.ProductCategories
                                 .Where(c => c.Id != 1)
                                 .OrderBy(c => c.Name)
                                 .ToList();

                CategoryList.Insert(0, new ProductCategory { Id = 0, Name = "Geen categorie" });
                CategoryDropdown.ItemsSource = CategoryList;
                CategoryDropdown.SelectedIndex = 0;
            }

            ProductListView.ItemsSource = ProductList;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox)?.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                ProductListView.ItemsSource = ProductList;
            }
            else
            {
                ProductListView.ItemsSource = ProductList
                    .Where(c => c.Name != null && c.Name.ToLower().Contains(searchText))
                    .ToList();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SalesMainPage));
        }

        private async void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                using (var db = new AppDbContext())
                {
                    var productDetails = await db.Products
                                                    .Include(c => c.Category)
                                                    .Where(c => c.Id == selectedProduct.Id)
                                                    .OrderBy(c => c.Id)
                                                    .ToListAsync();

                    ProductInfoListView.ItemsSource = productDetails;
                }             
            }
        }

        private void CategoryDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryDropdown.SelectedItem is ProductCategory selectedCategory)
            {
                if (selectedCategory.Id == 0)
                {
                    ProductListView.ItemsSource = ProductList;
                }
                else
                {
                    ProductListView.ItemsSource = ProductList
                        .Where(p => p.Category.Id == selectedCategory.Id)
                        .ToList();
                }
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryDropdown.SelectedIndex = 0;
            ProductListView.ItemsSource = ProductList;
        }
    }
}
