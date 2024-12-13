using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
	internal class WorkOrder
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public int UserId { get; set; }
		public DateTime Date_Created { get; set; }
		public int AppointmentId { get; set; }
		public int? RequestId { get; set; }
		public User User { get; set; }
		public Appointment Appointment { get; set; }
		public ServiceRequest? Request { get; set; }

		// Nieuwe eigenschap voor meerdere producten
		public ICollection<WorkOrderProduct>? WorkOrderProducts { get; set; }

		public string FormattedDateReported => Date_Created.ToString("dd/MM/yyyy");
	}
}
