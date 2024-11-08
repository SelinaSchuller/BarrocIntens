using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsStock { get; set; }
        public bool VisibleForCustomers { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }
    }
}
