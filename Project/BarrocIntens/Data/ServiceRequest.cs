using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class ServiceRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date_Reported { get; set; }
        public int Status { get; set; } //1 = UnAssigned; 2 = In Progress; 3 = Done;
        public int CustomerId { get; set; }
        public int? ProductId { get; set; }
        public Customer Customer { get; set; }
        public Product? Product { get; set; }
      
      //Dit is zodat je zo de Date kan ophalen zonder de tijd erbij:
      public string FormattedDateReported => Date_Reported.ToString("dd/MM/yyyy");

	}
}
