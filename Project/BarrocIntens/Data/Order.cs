using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public Product Products { get; set; }

    }
}
