using BarrocIntens.Data;
using BarrocIntens.Sales;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudAfsprakenCreatePage : Page
	{
		private OnderhoudBaseWindow _parentWindow;
		public OnderhoudAfsprakenCreatePage()
		{
			this.InitializeComponent();
			LoadData();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if(e.Parameter is OnderhoudBaseWindow parentWindow)
			{
				_parentWindow = parentWindow;
			}
		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				var users = db.Users
					.Where(u => u.DepartmentId == 2)
					.Select(u => new { u.Id, u.Name })
					.ToList();
				UserComboBox.ItemsSource = users;
				UserComboBox.DisplayMemberPath = "Name";
				UserComboBox.SelectedValuePath = "Id";

				var customers = db.Customers
					.OrderBy(c => c.Name)
					.Select(c => new { c.Id, c.Name })
					.ToList();
				CustomerComboBox.ItemsSource = customers;
				CustomerComboBox.DisplayMemberPath = "Name";
				CustomerComboBox.SelectedValuePath = "Id";

				var serviceRequests = db.ServiceRequests
					.Where(sr => sr.Status == 1)
					.OrderBy(sr => sr.Date_Reported)
					.Select(sr => new
					{
						sr.Id,
						DisplayText = $"{sr.Description} - {sr.FormattedDateReported}"
					})
					.ToList();
				ServiceRequestComboBox.ItemsSource = serviceRequests;
				ServiceRequestComboBox.DisplayMemberPath = "DisplayText";
				ServiceRequestComboBox.SelectedValuePath = "Id";
			}
		}

		private async void SaveAppointmentButton_Click(object sender, RoutedEventArgs e)
		{
			if(string.IsNullOrWhiteSpace(DescriptionTextBox.Text) || DatePicker.SelectedDate == null ||
				UserComboBox.SelectedValue == null || CustomerComboBox.SelectedValue == null)
			{
				ContentDialog errorDialog = new ContentDialog
				{
					Title = "Veld leeg",
					Content = "Alle velden moeten worden ingevuld.",
					CloseButtonText = "Ok",
					XamlRoot = this.XamlRoot
				};
				errorDialog.ShowAsync();
				return;
			}

			using(var db = new AppDbContext())
			{
				var appointment = new Appointment
				{
					Description = DescriptionTextBox.Text.Trim(),
					Date = DatePicker.SelectedDate.Value.DateTime,
					UserId = (int)UserComboBox.SelectedValue,
					CustomerId = (int)CustomerComboBox.SelectedValue
				};

				db.Appointments.Add(appointment);
				db.SaveChanges();

				// If a ServiceRequest is selected, create a WorkOrder
				if(ServiceRequestComboBox.SelectedValue != null)
				{
					int serviceRequestId = (int)ServiceRequestComboBox.SelectedValue;

					var serviceRequest = db.ServiceRequests.SingleOrDefault(sr => sr.Id == serviceRequestId);
					if(serviceRequest != null)
					{
						serviceRequest.Status = 2; // Update the status to 2
					}
					db.ServiceRequests.Update(serviceRequest);

					var workOrder = new WorkOrder
					{
						Description = appointment.Description,
						UserId = appointment.UserId,
						WorkOrderProducts = null,
						AppointmentId = appointment.Id,
						RequestId = (int)ServiceRequestComboBox.SelectedValue
					};

					db.WorkOrders.Add(workOrder);
					db.SaveChanges();
				}
			}
			ContentDialog successDialog = new ContentDialog
			{
				Title = "Succes",
				Content = "Afspraak succesvol opgeslagen.\nWilt u terugkeren naar de planning of nog een nieuwe afspraak maken?",
				PrimaryButtonText = "Ga naar planning",
				SecondaryButtonText = "Nieuwe afspraak",
				XamlRoot = this.XamlRoot
			};

			var result = await successDialog.ShowAsync();

			if(result == ContentDialogResult.Primary)
			{
				_parentWindow.NavigateToPlanningPage();
			}
			else if(result == ContentDialogResult.Secondary)
			{
				ResetForm();
			}
		}
		private void ResetForm()
		{
			DescriptionTextBox.Text = string.Empty;
			DatePicker.SelectedDate = null;
			UserComboBox.SelectedIndex = -1;
			CustomerComboBox.SelectedIndex = -1;
			ServiceRequestComboBox.SelectedIndex = -1;
		}

	}
}
