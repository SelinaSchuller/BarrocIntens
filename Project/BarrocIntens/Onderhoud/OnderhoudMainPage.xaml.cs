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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Syncfusion.UI.Xaml.Scheduler;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Appointments;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Onderhoud
{
<<<<<<< Updated upstream
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
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
                List<CalenderInfo> calenderInfos1 = new List<CalenderInfo>();
                List<CalenderInfo> calenderInfos = new List<CalenderInfo>();
                DateTime date = DateTime.Now;
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                var appointments = db.Appointments.OrderBy(a => a.Date).ToArray();
				List<List<Appointment>> appointments1 = new List<List<Appointment>>();
                for (int i = 0; i <= 6; i++)
                {
                    appointments1.Add(new List<Appointment>());
                }
				
                bool AreFallingInSameWeek(DateTime date1, DateTime date2, DayOfWeek weekStartsOn)
                {
                    return date1.AddDays(-GetOffsetedDayofWeek(date1.DayOfWeek, (int)weekStartsOn)) == date2.AddDays(-GetOffsetedDayofWeek(date2.DayOfWeek, (int)weekStartsOn));
                }
=======
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnderhoudMainPage : Page
    {
        public OnderhoudMainPage()
        {
            this.InitializeComponent();
>>>>>>> Stashed changes

            using (var db = new AppDbContext())
            {
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(@"@32382e302e30PUUwxaozoM6FanVOT3CLZUo5J9zXHglhwaQLMKyqkOs=\r\n");
                this.Schedule.DaysViewSettings.TimeRulerFormat = "hh:mm";
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
                Schedule.ItemsSource = scheduleAppointmentCollection;
            }
        }

            private void ButtonBack_Click(object sender, RoutedEventArgs e)
            {

            }
    

<<<<<<< Updated upstream
        private void ButtonWednesday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Wednesday).OrderBy(a => a.Date);
            }
        }

        private void ButtonThursday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Thursday).OrderBy(a => a.Date);
            }
        }

        private void ButtonFriday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Friday).OrderBy(a => a.Date);
            }
        }

        private void ButtonSaturday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Saturday).OrderBy(a => a.Date);
            }
        }

        private void ButtonSunday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Sunday).OrderBy(a => a.Date);
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Visible;
            dayCalander.Visibility = Visibility.Collapsed;
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
	}
=======
    }
>>>>>>> Stashed changes
}
