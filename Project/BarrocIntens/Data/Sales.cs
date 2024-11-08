using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class Sales
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public Products Product { get; set; }
        public Customers Customer { get; set; }
    }
}
