using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Input;
using static BarrocIntens.Onderhoud.OnderhoudMainPage;
using Windows.UI.Core;
using Microsoft.UI.Windowing;
using BarrocIntens.Services;
using BarrocIntens.Data;
using System.Linq;

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudBaseWindow : Window
	{
		private List<ServiceRequest> StoringenLijst { get; set; }

		public OnderhoudBaseWindow()
		{
			this.InitializeComponent();
			this.Title = "Onderhoud";
			Fullscreen fullscreenService = new Fullscreen();
			fullscreenService.SetFullscreen(this);

			using(var db = new AppDbContext())
			{
				StoringenLijst = db.ServiceRequests
					.Where(s => s.Status == 1)
					.ToList();
			}

			if(StoringenLijst != null)
			{
				StoringenBadgeText.Text = StoringenLijst.Count.ToString();
			}
			else
			{
				StoringenBadgeText.Visibility = Visibility.Collapsed;
			}

			MeldingIconImage.PointerEntered += MeldingIconImage_PointerEntered;
			MeldingIconImage.PointerExited += MeldingIconImage_PointerExited;

			MainFrame.Navigate(typeof(OnderhoudMainPage));
		}

		private void MeldingIcon_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var storingenWindow = new OnderhoudIngekomenStoringen();
			storingenWindow.Activate();
		}

		//Volgende 3 functies is voor animatie van de meldingen knop/image
		private void MeldingIconImage_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			AnimateScale(1.2, 0.5);
		}

		private void MeldingIconImage_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			AnimateScale(1.0, 0.5);
		}

		private void AnimateScale(double targetScale, double durationSeconds)
		{
			var storyboard = new Storyboard();

			var scaleXAnimation = new DoubleAnimation
			{
				To = targetScale,
				Duration = new Duration(TimeSpan.FromSeconds(durationSeconds)),
				EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
			};
			Storyboard.SetTarget(scaleXAnimation, ImageScaleTransform);
			Storyboard.SetTargetProperty(scaleXAnimation, "ScaleX");

			var scaleYAnimation = new DoubleAnimation
			{
				To = targetScale,
				Duration = new Duration(TimeSpan.FromSeconds(durationSeconds)),
				EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
			};
			Storyboard.SetTarget(scaleYAnimation, ImageScaleTransform);
			Storyboard.SetTargetProperty(scaleYAnimation, "ScaleY");

			storyboard.Children.Add(scaleXAnimation);
			storyboard.Children.Add(scaleYAnimation);

			storyboard.Begin();
		}

	}
}
