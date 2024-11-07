using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class ServiceRequests
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date_Reported { get; set; }
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public Customers Customer { get; set; }
        public Products Product { get; set; }
    }
}
