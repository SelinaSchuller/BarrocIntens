using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class User
    {
		public static User LoggedInUser { get; set; }
		public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
