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
using BarrocIntens.Sales;

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudBaseWindow : Window
	{
		private List<ServiceRequest> _storingenLijst { get; set; }
		public int userId { get; set; }
		private User _loggedInUser { get; set; }
		public OnderhoudBaseWindow(int? employeeId)
		{
			this.InitializeComponent();
			this.Title = "Onderhoud";
			Fullscreen fullscreenService = new Fullscreen();
			fullscreenService.SetFullscreen(this);

			if(employeeId != null)
			{
				userId = employeeId.Value;

				using(var db = new AppDbContext())
				{
					_loggedInUser = db.Users.FirstOrDefault(u  => u.Id == userId);
				}
			}
			if(userId == 7 && _loggedInUser.Email == "hoofdonderhoud@barrocintens.nl")
			{
				LoadData();
				StoringIcon.Visibility = Visibility.Visible;
				AfspraakCreateButton.Visibility = Visibility.Collapsed;
			}
			else if (_loggedInUser.DepartmentId == 6)
			{
				AfspraakCreateButton.Visibility = Visibility.Visible;
			}
			else if (_loggedInUser.DepartmentId == 2)
			{
				AfspraakCreateButton.Visibility = Visibility.Collapsed;
				StoringIcon.Visibility = Visibility.Collapsed;
			}

			MainFrame.Navigate(typeof(OnderhoudMainPage));

			SetButtonVisibility();
		}

		private void SetButtonVisibility()
		{
			PlanningButton.Visibility = Visibility.Visible;

			if(_loggedInUser.DepartmentId == 6)
			{
				AfspraakCreateButton.Visibility = Visibility.Visible;
			}

			if(MainFrame.SourcePageType == typeof(OnderhoudMainPage))
			{
				PlanningButton.Visibility = Visibility.Collapsed;
			}
			else if(MainFrame.SourcePageType == typeof(OnderhoudAfsprakenCreatePage))
			{
				AfspraakCreateButton.Visibility = Visibility.Collapsed;
			}
		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				_storingenLijst = db.ServiceRequests
					.Where(s => s.Status == 1)
					.ToList();
			}

			if(_storingenLijst != null)
			{
				StoringenBadgeText.Text = _storingenLijst.Count.ToString();
			}
			else
			{
				StoringenBadgeText.Visibility = Visibility.Collapsed;
			}

			MeldingIconImage.PointerEntered += MeldingIconImage_PointerEntered;
			MeldingIconImage.PointerExited += MeldingIconImage_PointerExited;
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

		private void PlanningButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(OnderhoudMainPage));
			SetButtonVisibility();
		}

		private void AfspraakCreateButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(OnderhoudAfsprakenCreatePage), this);
			SetButtonVisibility();
		}

		private void WorkOrdersButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(OnderhoudWorkOrdersPage), this);
			SetButtonVisibility();
		}

		public void NavigateToPlanningPage()
		{
			MainFrame.Navigate(typeof(OnderhoudMainPage));
			SetButtonVisibility();
		}
	}
}
