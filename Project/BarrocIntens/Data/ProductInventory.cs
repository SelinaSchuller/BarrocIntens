using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class ProductInventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int InStock { get; set; }
        public int AmountOrdered { get; set; }
        public Product Product { get; set; }
    }
}
