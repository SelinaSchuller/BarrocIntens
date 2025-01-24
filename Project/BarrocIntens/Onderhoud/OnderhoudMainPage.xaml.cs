using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Linq;

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudMainPage : Page
	{
		private OnderhoudBaseWindow _parentWindow;
		private ContentDialog _currentDialog;
		private ScheduleAppointmentCollection _scheduleAppointments;

		public OnderhoudMainPage()
		{
			this.InitializeComponent();

			using(var db = new AppDbContext())
			{
				Schedule.DaysViewSettings.TimeRulerFormat = "HH:mm";
				Schedule.TimelineViewSettings.EndHour = 24;
				Schedule.Language.Equals("nl-NL");

				_scheduleAppointments = new ScheduleAppointmentCollection();

				foreach(var appointment in db.Appointments)
				{
					_scheduleAppointments.Add(new ScheduleAppointment
					{
						StartTime = appointment.Date,
						EndTime = appointment.Date.AddHours(1),
						Subject = appointment.Description,
						Id = appointment.Id,
					});
				}

				this.Schedule.ItemsSource = _scheduleAppointments;
			}
			Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is OnderhoudBaseWindow parentWindow)
			{
				_parentWindow = parentWindow;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("OnderhoudMainPage: No valid OnderhoudBaseWindow received.");
			}
		}

		private async void Schedule_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
		{
			e.Cancel = true;

			if(_currentDialog != null)
			{
				return;
			}

			try
			{
				if(e.Appointment is ScheduleAppointment appointment)
				{
					using(var db = new AppDbContext())
					{
						var dbAppointment = db.Appointments
							.Include(a => a.Customer)
							.ThenInclude(c => c.Company)
							.Include(a => a.User)
							.FirstOrDefault(a => a.Id == (int)appointment.Id);

						if(dbAppointment == null)
						{
							await new ContentDialog
							{
								Title = "Fout",
								Content = "Afspraak niet gevonden in de database.",
								CloseButtonText = "Sluiten",
								XamlRoot = this.XamlRoot
							}.ShowAsync();
							return;
						}

						var serviceRequest = db.ServiceRequests
							.Include(sr => sr.Product)
							.FirstOrDefault(sr => sr.CustomerId == dbAppointment.CustomerId);

						AppointmentDescriptionTextBlock.Text = dbAppointment.Description;
						AppointmentDateTextBlock.Text = dbAppointment.Date.ToString("dd/MM/yyyy");
						AppointmentStartTimeTextBlock.Text = dbAppointment.Date.ToString("HH:mm");
						CustomerNameTextBlock.Text = dbAppointment.Customer.Name;
						CustomerAddressTextBlock.Text = dbAppointment.Customer.Address;
						CustomerCompanyTextBlock.Text = dbAppointment.Customer.Company?.Name ?? "Geen bedrijf";

						if(serviceRequest != null)
						{
							ServiceRequestStackPanel.Visibility = Visibility.Visible;
							ServiceRequestDescriptionTextBlock.Text = serviceRequest.Description;
							ServiceRequestDateReportedTextBlock.Text = serviceRequest.FormattedDateReported;
							ServiceRequestProductTextBlock.Text = serviceRequest.Product?.Name ?? "Geen product";
						}
						else
						{
							ServiceRequestStackPanel.Visibility = Visibility.Collapsed;
						}

						UserNameTextBlock.Text = dbAppointment.User.Name;

						AppointmentDetailsDialog.Tag = appointment;

						_currentDialog = AppointmentDetailsDialog;
						await AppointmentDetailsDialog.ShowAsync();
					}
				}
			}
			finally
			{
				_currentDialog = null;
			}
		}

		private void CreateWorkOrderButton_Click(object sender, RoutedEventArgs e)
		{
			if(_currentDialog != null && _currentDialog.Tag is ScheduleAppointment appointment)
			{
				_currentDialog.Hide();
				CreateWorkOrder(appointment);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("No valid appointment selected for work order creation.");
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
