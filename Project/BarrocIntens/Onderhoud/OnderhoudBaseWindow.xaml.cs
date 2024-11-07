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

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudBaseWindow : Window
	{
		public List<OnderhoudMainPage.StoringItem> StoringenLijst { get; set; }

		public OnderhoudBaseWindow()
		{
			this.InitializeComponent();
			this.Title = "Onderhoud";

			CreateHardcodeList();

			StoringenBadgeText.Text = StoringenLijst.Count.ToString();

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
			// Just animate scale to indicate hover
			AnimateScale(1.2, 0.5);
		}

		private void MeldingIconImage_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			// Reset scale back to normal when not hovering
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


		public void CreateHardcodeList()
		{
			// Hardcoded data for "Storingen"
			StoringenLijst = new List<StoringItem>
			{
				new StoringItem { KlantNaam = "Jan van Dijk", Status = 0, Date = new DateTime(2024, 10, 8) },
				new StoringItem { KlantNaam = "Pieter de Jong", Status = 0, Date = DateTime.Today.AddDays(-1) },
				new StoringItem { KlantNaam = "Klaas Bakker", Status = 0, Date = DateTime.Today }
			};
		}
	}
}
