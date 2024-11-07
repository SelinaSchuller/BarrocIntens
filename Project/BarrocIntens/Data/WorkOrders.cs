using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class WorkOrders
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int AppointmentId { get; set; }
        public int RequestId { get; set; }
        public Users User { get; set; }
        public Products Product { get; set; }
        public Appointments Appointment { get; set; }
        public ServiceRequests Request { get; set; }
    }
}
