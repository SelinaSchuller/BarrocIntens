using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarrocIntens.Sales
{
    public sealed partial class SalesProductPage : Page
    {
        private List<Product> ProductList { get; set; } = new List<Product>();
        private List<ProductCategory> CategoryList { get; set; } = new List<ProductCategory>();

        private int _currentPage = 0;
        private const int PageSize = 10;
        private bool _isLoading = false;

        public SalesProductPage()
        {
            this.InitializeComponent();
            ProductListView.Loaded += ProductListView_Loaded;
            InitializePageAsync();
        }

        private async void InitializePageAsync()
        {
            await LoadCategoriesAsync();
            await LoadProductsAsync();
        }

        private void ProductListView_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetScrollViewer(ProductListView);
            if (scrollViewer != null)
            {
                scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            }
        }

        private ScrollViewer GetScrollViewer(DependencyObject root)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(root);

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                if (child is ScrollViewer viewer)
                {
                    return viewer;
                }

                var descendant = GetScrollViewer(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            System.Diagnostics.Debug.WriteLine($"VerticalOffset: {scrollViewer.VerticalOffset}, ScrollableHeight: {scrollViewer.ScrollableHeight}");

            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 50)
            {
                _ = LoadProductsAsync();
            }
        }


        private async Task LoadCategoriesAsync()
        {
            using (var db = new AppDbContext())
            {
                CategoryList = db.ProductCategories
                                 .Where(c => c.Id != 1)
                                 .OrderBy(c => c.Name)
                                 .ToList();

                CategoryList.Insert(0, new ProductCategory { Id = 0, Name = "Geen categorie" });
                CategoryDropdown.ItemsSource = CategoryList;
                CategoryDropdown.SelectedIndex = 0;
            }
        }

        private async Task LoadProductsAsync(int? categoryId = null)
        {
            if (_isLoading)
                return;

            _isLoading = true;

            // Get ScrollViewer from ListView
            ScrollViewer scrollViewer = GetScrollViewer(ProductListView);
            double? currentVerticalOffset = scrollViewer?.VerticalOffset;

            using (var db = new AppDbContext())
            {
                IQueryable<Product> query = db.Products.Where(p => p.VisibleForCustomers && p.Category.Id != 1);

                // Apply category filter if provided
                if (categoryId.HasValue && categoryId.Value != 0)
                {
                    query = query.Where(p => p.Category.Id == categoryId.Value);
                }

                // Fetch the products based on current page and filter
                var newProducts = await query
                    .OrderBy(p => p.Id)
                    .Skip(_currentPage * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                if (newProducts.Any())
                {
                    ProductList.AddRange(newProducts);
                    _currentPage++;
                }
            }

            // Temporarily set ItemsSource to null, then update
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = ProductList;

            // Restore scroll position
            if (scrollViewer != null && currentVerticalOffset.HasValue)
            {
                scrollViewer.ChangeView(null, currentVerticalOffset.Value, null, true);
            }

            _isLoading = false;
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SalesMainPage));
        }

        private void ProductListView_ScrollChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 50)
            {
                _ = LoadProductsAsync();
            }
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

        private async void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                using (var db = new AppDbContext())
                {
                    var productDetails = await db.Products
                                                  .Include(c => c.Category)
                                                  .Where(c => c.Id == selectedProduct.Id)
                                                  .FirstOrDefaultAsync();
                    ProductInfoListView.ItemsSource = new List<Product> { productDetails };
                }
            }
        }

        private async void CategoryDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryDropdown.SelectedItem is ProductCategory selectedCategory)
            {
                _currentPage = 0; // Reset page number for new category filtering
                ProductList.Clear(); // Clear the existing list for fresh loading

                // Load products based on the selected category
                await LoadProductsAsync(selectedCategory.Id); // Pass selected category Id to filter
            }
        }


        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryDropdown.SelectedIndex = 0;
            ProductListView.ItemsSource = ProductList;
        }
    }
}
