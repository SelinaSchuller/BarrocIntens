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
        public int? ProductId { get; set; }
        public int AppointmentId { get; set; }
        public int RequestId { get; set; }
        public User User { get; set; }
        public Product? Product { get; set; }
        public Appointment Appointment { get; set; }
        public ServiceRequest Request { get; set; }
    }
}
