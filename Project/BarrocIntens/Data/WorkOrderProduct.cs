using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{

	internal class WorkOrderProduct
	{
		public int WorkOrderId { get; set; }
		public WorkOrder WorkOrder { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }

		public int Quantity { get; set; }
	}
}
