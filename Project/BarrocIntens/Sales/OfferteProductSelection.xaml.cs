using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarrocIntens.Sales
{
    public sealed partial class OfferteProductSelection : UserControl
    {
        public List<int> SelectedProductIds { get; set; } = new();

        public OfferteProductSelection()
        {
            this.InitializeComponent();
            LoadProductsAsync();
        }

        private async void LoadProductsAsync()
        {
            using (var db = new AppDbContext())
            {
                var products = await db.Products.ToListAsync();
                ProductListView.ItemsSource = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = (decimal)p.Price,
                    IsSelected = false
                }).ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedProductIds.Clear();

            foreach (var item in ProductListView.Items)
            {
                var productViewModel = item as ProductViewModel;
                if (productViewModel != null && productViewModel.IsSelected)
                {
                    SelectedProductIds.Add(productViewModel.Id);
                }
            }

            var parentDialog = this.Parent as ContentDialog;
            parentDialog?.Hide();
        }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsSelected { get; set; }
    }
}