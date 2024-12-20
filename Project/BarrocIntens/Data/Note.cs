﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
	internal class Note
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public DateTime Date_Created { get; set; }
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
		public int EmployeeId { get; set; }
		public User Employee { get; set; }

		public string FormattedDateCreated => Date_Created.ToString("dd/MM/yyyy");

	}
}
