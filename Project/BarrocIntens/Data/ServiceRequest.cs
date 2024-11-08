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
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }

		public string FormattedDateReported => Date_Reported.ToString("dd/MM/yyyy");

	}
}
