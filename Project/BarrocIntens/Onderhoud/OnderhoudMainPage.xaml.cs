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

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudMainPage : Page
	{
		private int _appointmentId;
		private OnderhoudBaseWindow _parentWindow;

		List<Appointment> appointments2 = new List<Appointment>();
		public OnderhoudMainPage()
		{
			this.InitializeComponent();

			using (var db = new AppDbContext())
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
                        Subject = appointment.Description
                    });
                }

                this.Schedule.ItemsSource = scheduleAppointmentCollection;
            }
            Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
        }

		private void CreateWorkOrder_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;

			if(button != null)
			{
				var appointment = button.DataContext as Appointment;

				if(appointment != null)
				{
					int appointmentId = appointment.Id;
                    _parentWindow.NavigateToCreateWorkOrderPage(appointmentId);

					System.Diagnostics.Debug.WriteLine($"Appointment ID: {appointmentId}");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("No Appointment found in DataContext.");
				}
			}
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
        }
    }
}
