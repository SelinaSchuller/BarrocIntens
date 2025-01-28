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
using System.Threading.Tasks;

namespace BarrocIntens.Sales
{
    public sealed partial class SalesOffertesPage : Page
    {
		private SalesDashboardWindow _parentWindow;
		private List<Invoice> _offertesLijst;
		private int _currentPage = 0;
		private const int PageSize = 50;
		private bool _isLoading = false;

		public SalesOffertesPage()
        {
            this.InitializeComponent();
            _offertesLijst = new List<Invoice>();
		}

		private async Task LoadOffertesAsync()
		{
			if(_isLoading)
				return;

			_isLoading = true;
			InfoMessageTextBlockLoading.Text = $"Offertes aan het laden... ({_offertesLijst?.Count ?? 0} geladen)";
			InfoMessageTextBlockLoading.Visibility = Visibility.Visible;

			using(var db = new AppDbContext())
			{
				var offertes = await db.Invoices
					.OrderByDescending(n => n.DateCreated)
					.Skip(_currentPage * PageSize)
					.Take(PageSize)
					.ToListAsync();

				if(offertes.Any())
				{
					_offertesLijst.AddRange(offertes);
					offerteListView.ItemsSource = null;
					offerteListView.ItemsSource = _offertesLijst;

					_currentPage++;
				}
				else
				{
					InfoMessageTextBlockLoading.Text = "Alle offertes zijn geladen.";
				}
			}

			InfoMessageTextBlockLoading.Visibility = Visibility.Collapsed;
			_isLoading = false;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			_parentWindow = e.Parameter as SalesDashboardWindow;

			_currentPage = 0;
			_offertesLijst.Clear();
			await LoadOffertesAsync();
		}

		public void CreateOfferteButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow?.NavigateToOfferteAanmakenPage();
        }

		private async void OnScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			var scrollViewer = sender as ScrollViewer;

			if(scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 50)
			{
				await LoadOffertesAsync();
			}
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
					var deletedOfferte = _offertesLijst.FirstOrDefault(o => o.Id == offerteId);
					if(deletedOfferte != null)
					{
						_offertesLijst.Remove(deletedOfferte);
						offerteListView.ItemsSource = null;
						offerteListView.ItemsSource = _offertesLijst;
					}
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