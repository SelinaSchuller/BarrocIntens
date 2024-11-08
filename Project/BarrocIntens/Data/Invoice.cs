using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class Invoice
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public DateTime DateCreated { get; set; }
        public double TotalPrice { get; set; }
        public bool Paid { get; set; }
    }
}
