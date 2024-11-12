using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class InvoiceItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public Invoice Invoice { get; set; }
        public Product Product { get; set; }
    }
}
