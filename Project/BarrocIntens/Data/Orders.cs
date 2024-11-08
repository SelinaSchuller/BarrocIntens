using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class Orders
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }
        public Companies Company { get; set; }
        public Products Products { get; set; }

    }
}
