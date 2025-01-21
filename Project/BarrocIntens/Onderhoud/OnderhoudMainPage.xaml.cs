using BarrocIntens.Data;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using BarrocIntens.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Onderhoud
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class OnderhoudMainPage : Page
	{
		private int _appointmentId;
		private OnderhoudBaseWindow _parentWindow;
		private ContentDialog _currentDialog;
		private Button _workOrderButton;

		public OnderhoudMainPage()
		{
			this.InitializeComponent();

			using(var db = new AppDbContext())
			{
				Schedule.DaysViewSettings.TimeRulerFormat = "HH:mm";
				Schedule.TimelineViewSettings.EndHour = 24;

				var scheduleAppointmentCollection = new ScheduleAppointmentCollection();

				foreach(var appointment in db.Appointments)
				{
					scheduleAppointmentCollection.Add(new ScheduleAppointment
					{
						StartTime = appointment.Date,
						EndTime = appointment.Date.AddHours(1),
						Subject = appointment.Description,
						Id = appointment.Id
					});
				}

				this.Schedule.ItemsSource = scheduleAppointmentCollection;
			}
			Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is OnderhoudBaseWindow parentWindow)
			{
				_parentWindow = parentWindow;

				_appointmentId = _parentWindow.appointmentId;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("OnderhoudWorkOrderCreatePage: No valid OnderhoudBaseWindow received.");
			}
		}

		private void Schedule_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
		{
			e.Cancel = false;

			if(_workOrderButton != null)
			{
				(this.Content as Grid)?.Children.Remove(_workOrderButton);
			}

			var appointment = e.Appointment as ScheduleAppointment;
			if(appointment == null)
				return;

			_workOrderButton = new Button
			{
				Content = "Werkbron aanmaken",
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(300, 0, 0, 250),
				Width = 200,
				Height = 40
			};

			_workOrderButton.Click += (s, args) => CreateWorkOrder(appointment);

			if(this.Content is Grid mainGrid)
			{
				mainGrid.Children.Add(_workOrderButton);
				Canvas.SetZIndex(_workOrderButton, 99);
			}
		}

		private void CreateWorkOrder(ScheduleAppointment appointment)
		{
			if(appointment?.Id != null && int.TryParse(appointment.Id.ToString(), out int appointmentId))
			{
				_parentWindow?.NavigateToCreateWorkOrderPage(appointmentId);
				System.Diagnostics.Debug.WriteLine($"Navigated to create work order for Appointment ID: {appointmentId}");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Invalid Appointment ID.");
			}
		}
	}
}
