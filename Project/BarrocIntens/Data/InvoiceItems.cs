using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class InvoiceItems
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public Invoices Invoice { get; set; }
        public Products Product { get; set; }
    }
}
