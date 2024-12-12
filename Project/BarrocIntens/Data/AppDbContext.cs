using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using System.Configuration;
using System.Security.Cryptography;


namespace BarrocIntens.Data
{

	internal class AppDbContext : DbContext
	{
		public DbSet<Department> Departments { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<LeaseContract> LeaseContracts { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<InvoiceItem> InvoicesItems { get; set; }
		public DbSet<ServiceRequest> ServiceRequests { get; set; }
		public DbSet<ProductInventory> ProductInventories { get; set; }
		public DbSet<WorkOrder> WorkOrders { get; set; }
		public DbSet<WorkOrderProduct> WorkOrderProducts { get; set; }
		public DbSet<Sales> Sales { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(
				ConfigurationManager.ConnectionStrings["BarrocIntens"].ConnectionString,
				ServerVersion.Parse("8.0.30")
				);
		}

		private static string HashPassword(string password)
		{
			using(SHA256 sha256 = SHA256.Create())
			{
				byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				StringBuilder builder = new StringBuilder();
				foreach(byte b in bytes)
				{
					builder.Append(b.ToString("x2"));
				}
				return builder.ToString();
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var faker = new Faker();
			modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.HasColumnType("decimal(18,2)");

			modelBuilder.Entity<ProductCategory>().HasData(
				new ProductCategory { Id = 1, Name = "Part" },
				new ProductCategory { Id = 2, Name = "Accessory" },
				new ProductCategory { Id = 3, Name = "Service" },
				new ProductCategory { Id = 4, Name = "Software" },
				new ProductCategory { Id = 5, Name = "Hardware" }
			);

			// Departments
			modelBuilder.Entity<Department>().HasData(
				new Department { Id = 1, Name = "Sales" },
				new Department { Id = 2, Name = "Onderhoud" },
				new Department { Id = 3, Name = "Finance" },
				new Department { Id = 4, Name = "Inkoop" },
				new Department { Id = 5, Name = "Hoofd Inkoop" },
				new Department { Id = 6, Name = "Planner" }

			);

			// Users
			modelBuilder.Entity<User>().HasData(
				new User { Id = 1, Name = "Emma de Vries", Email = "sales@barrocintens.nl", Password = HashPassword("sales"), Active = true, DepartmentId = 1 },
				new User { Id = 2, Name = "Liam Jansen", Email = "onderhoud@barrocintens.nl", Password = HashPassword("onderhoud"), Active = true, DepartmentId = 2 },
				new User { Id = 3, Name = "Sophie Bakker", Email = "finance@barrocintens.nl", Password = HashPassword("finance"), Active = true, DepartmentId = 3 },
				new User { Id = 4, Name = "Lucas Visser", Email = "inkoop@barrocintens.nl", Password = HashPassword("inkoop"), Active = true, DepartmentId = 4 },
				new User { Id = 5, Name = "Mila Smit", Email = "hoofdinkoop@barrocintens.nl", Password = HashPassword("hoofdinkoop"), Active = true, DepartmentId = 4 },
				new User { Id = 6, Name = "Noah van Dijk", Email = "planner@barrocintens.nl", Password = HashPassword("planner"), Active = true, DepartmentId = 6 },
				new User { Id = 7, Name = "Richard Van Vlieger", Email = "hoofdonderhoud@barrocintens.nl", Password = HashPassword("hoofdonderhoud"), Active = true, DepartmentId = 2 }
			);

			// Companies
			var companies = new Faker<Company>()
				.RuleFor(c => c.Id, f => f.IndexFaker + 1)
				.RuleFor(c => c.Name, f => f.Name.FullName())
				.RuleFor(c => c.Bkr, f => f.Random.Bool())
				.Generate(150); // Inactive Customers
			modelBuilder.Entity<Company>().HasData(companies);
			// Customers
			var customers = new Faker<Customer>()
				.RuleFor(c => c.Id, f => f.IndexFaker + 1)
				.RuleFor(c => c.Name, f => f.Name.FullName())
				.RuleFor(c => c.Address, f => f.Address.StreetAddress())
				.RuleFor(c => c.Email, f => f.Internet.Email())
				.RuleFor(c => c.PhoneNumber, f => f.Random.Replace("(###) ###-####"))
				.RuleFor(c => c.CompanyId, f => f.Random.Int(1, 150))
				.Generate(150);


			modelBuilder.Entity<Customer>().HasData(customers);


			// Products
			modelBuilder.Entity<Product>()
						.HasOne(p => p.Category)
						.WithMany(c => c.Products)
						.HasForeignKey(p => p.CategoryId);

			base.OnModelCreating(modelBuilder);

			var products = new Faker<Product>()
				.RuleFor(p => p.Id, f => f.IndexFaker + 1)
				.RuleFor(p => p.Name, f => f.Commerce.ProductName())
				.RuleFor(p => p.Price, f => Math.Round((decimal)f.Finance.Amount(1, 1000), 2)) // Round to 2 decimal places
				.RuleFor(p => p.CategoryId, f => f.Random.Int(1, 5)) // Random CategoryId from 1 to 5
				.RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
				.RuleFor(p => p.IsStock, f => f.Random.Bool())
				.RuleFor(p => p.VisibleForCustomers, f => true)
				.Generate(500);

			modelBuilder.Entity<Product>().HasData(products);


			// Invoices
			var invoices = new Faker<Invoice>()
				.RuleFor(i => i.Id, f => f.IndexFaker + 1)
				.RuleFor(i => i.ContractId, f => f.Random.Int(1, 150))
				.RuleFor(i => i.DateCreated, f => f.Date.Recent())
				.RuleFor(i => i.TotalPrice, (decimal)0)
				.RuleFor(i => i.Paid, f => f.Random.Bool()) // 120 invoices with payment delay
				.Generate(500);

			modelBuilder.Entity<Invoice>().HasData(invoices);

			// Lease Contracts
			var leaseContracts = new Faker<LeaseContract>()
				.RuleFor(l => l.Id, f => f.IndexFaker + 1)
				.RuleFor(l => l.CompanyId, f => f.Random.Int(1, 150))
				.RuleFor(l => l.Start_Date, f => f.Date.Past(1))
				.RuleFor(l => l.Contract_Type, f => f.PickRandom(new[] { "Repeat", "One-time" }))
				.RuleFor(l => l.End_Date, f => f.Date.Future(1)).Rules((f, l) =>
				{
					if(l.Contract_Type == "Repeat")
					{
						l.End_Date = null;
					}
				})
				.Generate(150);

			modelBuilder.Entity<LeaseContract>().HasData(leaseContracts);

			//ServiceRequests(storingen)
			var serviceRequests = new Faker<ServiceRequest>()
				.RuleFor(w => w.Id, f => f.IndexFaker + 7)
				.RuleFor(w => w.Description, f => f.Lorem.Sentence())
				.RuleFor(w => w.Date_Reported, f => f.Date.Past(1))
				.RuleFor(w => w.Status, f => f.Random.Int(1, 3))
				.RuleFor(w => w.CustomerId, f => f.Random.Int(1, 150))
				.RuleFor(w => w.ProductId, f => f.Random.Int(1, 500))
				.Generate(75);

			modelBuilder.Entity<ServiceRequest>().HasData(serviceRequests);

			modelBuilder.Entity<ServiceRequest>().HasData(
				new ServiceRequest
				{
					Id = 1,
					Description = "Er lekt water uit de achterkant van de machine",
					Date_Reported = DateTime.Today,
					Status = 1,
					CustomerId = 1,
					ProductId = 1,
				},
				new ServiceRequest
				{
					Id = 2,
					Description = "Is ineens gestopt en gaat niet meer aan. Is ineens gestopt en gaat niet meer aan. Is ineens gestopt en gaat niet meer aan. Is ineens gestopt en gaat niet meer aan.",
					Date_Reported = DateTime.Today.AddDays(-1),
					Status = 1,
					CustomerId = 2,
					ProductId = 2,
				},
				new ServiceRequest
				{
					Id = 3,
					Description = "Koffie komt er uit maar is niet goed gemengd.",
					Status = 2,
					CustomerId = 3,
					ProductId = 3,
				},
				new ServiceRequest
				{
					Id = 4,
					Description = "Storingscode 404 op display",
					Status = 2,
					CustomerId = 4,
					ProductId = 1,
				},
				new ServiceRequest
				{
					Id = 5,
					Description = "Hoge temperatuuralarm",
					Status = 3,
					CustomerId = 5,
					ProductId = 2,
				},
				new ServiceRequest
				{
					Id = 6,
					Description = "Water is op error, maar water is niet op.",
					Status = 3,
					CustomerId = 6,
					ProductId = 3,
				}

			);

			//Relation workorder en workorderProduct:
			modelBuilder.Entity<WorkOrderProduct>()
				.HasKey(wop => new { wop.WorkOrderId, wop.ProductId });

			modelBuilder.Entity<WorkOrderProduct>()
				.HasOne(wop => wop.WorkOrder)
				.WithMany(wo => wo.WorkOrderProducts)
				.HasForeignKey(wop => wop.WorkOrderId);

			modelBuilder.Entity<WorkOrderProduct>()
				.HasOne(wop => wop.Product)
				.WithMany()
				.HasForeignKey(wop => wop.ProductId);

			// Work Orders
			modelBuilder.Entity<WorkOrder>().HasData(
				new WorkOrder
				{
					Id = 1,
					Description = "Installatie van nieuwe machine op locatie.",
					UserId = 1,
					Date_Created = DateTime.Today.AddDays(-5),
					AppointmentId = 1,
					RequestId = null
				},
				new WorkOrder
				{
					Id = 2,
					Description = "Onderhoud uitvoeren volgens jaarlijks schema.",
					UserId = 2,
					Date_Created = DateTime.Today.AddDays(-10),
					AppointmentId = 2,
					RequestId = 3
				},
				new WorkOrder
				{
					Id = 3,
					Description = "Vervanging van beschadigde onderdelen.",
					UserId = 3,
					Date_Created = DateTime.Today.AddDays(-2),
					AppointmentId = 3,
					RequestId = 2
				},
				new WorkOrder
				{
					Id = 4,
					Description = "Kalibratie van het apparaat na grote reparatie.",
					UserId = 4,
					Date_Created = DateTime.Today,
					AppointmentId = 4,
					RequestId = null
				},
				new WorkOrder
				{
					Id = 5,
					Description = "Software-update en controle van connectiviteit.",
					UserId = 5,
					Date_Created = DateTime.Today.AddDays(-15),
					AppointmentId = 5,
					RequestId = 1
				},
				new WorkOrder
				{
					Id = 6,
					Description = "Diagnose uitvoeren vanwege herhaaldelijke storingen.",
					UserId = 6,
					Date_Created = DateTime.Today.AddDays(-3),
					AppointmentId = 6,
					RequestId = 4
				}
			);



			//        var workOrders = new Faker<WorkOrder>()
			//            .RuleFor(w => w.Id, f => f.IndexFaker + 1)
			//            .RuleFor(w => w.RequestId, f => f.Random.Int(1, 75))
			//            .RuleFor(w => w.Description, f => f.Lorem.Sentence())
			//.RuleFor(w => w.Date_Created, f => f.Date.Past(1))
			//.RuleFor(w => w.AppointmentId, f => f.Random.Int(1, 75))
			//            .RuleFor(w => w.ProductId, f => f.Random.Int(1, 500))
			//            .RuleFor(w => w.UserId, f => f.Random.Int(1, 4))
			//            .Generate(75);

			//        // Adding routine and maintenance work orders
			//        var routineWorkOrders = workOrders.Take(35).Select(w =>
			//        {
			//            w.Description = "Routine Maintenance: " + w.Description;
			//            return w;
			//        }).ToList();

			//        var emergencyWorkOrders = workOrders.Skip(35).ToList();

			//        modelBuilder.Entity<WorkOrder>().HasData(routineWorkOrders);
			//        modelBuilder.Entity<WorkOrder>().HasData(emergencyWorkOrders);

			//WorkOrderProducts:
			modelBuilder.Entity<WorkOrderProduct>().HasData(
				new WorkOrderProduct
				{
					WorkOrderId = 1,
					ProductId = 1,
					Quantity = 5
				},
				new WorkOrderProduct
				{
					WorkOrderId = 2,
					ProductId = 2,
					Quantity = 3
				},
				new WorkOrderProduct
				{
					WorkOrderId = 3,
					ProductId = 4,
					Quantity = 2
				},
				new WorkOrderProduct
				{
					WorkOrderId = 4,
					ProductId = 3,
					Quantity = 1
				},
				new WorkOrderProduct
				{
					WorkOrderId = 5,
					ProductId = 1,
					Quantity = 10
				},
				new WorkOrderProduct
				{
					WorkOrderId = 6,
					ProductId = 2,
					Quantity = 6
				}
			);


			// Appointments
			var appointments = new Faker<Appointment>()
				.RuleFor(a => a.Id, f => f.IndexFaker + 1)
				.RuleFor(a => a.Date, f => f.Date.Future(1))
				.RuleFor(a => a.UserId, f => f.Random.Int(1, 4))
				.RuleFor(a => a.CustomerId, f => f.Random.Int(1, 150))
				.RuleFor(a => a.Description, f => f.Lorem.Sentence())
				.Generate(75);

			modelBuilder.Entity<Appointment>().HasData(appointments);


			// Product Categories

			//Notes
			modelBuilder.Entity<Note>().HasData(
				new Note { Id = 1, Title = "System Checkup", Type = "Terug bellen", Description = "Performing a full system diagnostic.", Date_Created = new DateTime(2024, 2, 12), CustomerId = 1, EmployeeId = 1 },
				new Note { Id = 2, Title = "Issue Report", Type = "Terug bellen", Description = "Reported issue with water leakage.", Date_Created = new DateTime(2024, 2, 14), CustomerId = 2, EmployeeId = 1 },
				new Note { Id = 3, Title = "Maintenance Scheduled", Type = "Afspraak nog te maken", Description = "Scheduled maintenance for next week.", Date_Created = new DateTime(2024, 2, 15), CustomerId = 3, EmployeeId = 2 },
				new Note { Id = 4, Title = "Customer Feedback", Type = "Afspraak nog te maken", Description = "Received feedback on product performance.", Date_Created = new DateTime(2024, 2, 18), CustomerId = 4, EmployeeId = 2 },
				new Note { Id = 5, Title = "Installation", Type = "Afspraak is gemaakt", Description = "Completed installation of new hardware.", Date_Created = new DateTime(2024, 2, 20), CustomerId = 5, EmployeeId = 3 },
				new Note { Id = 6, Title = "Error Code 404", Type = "Afspraak is gemaakt", Description = "Investigated error code 404 in system.", Date_Created = new DateTime(2024, 2, 22), CustomerId = 6, EmployeeId = 3 },
				new Note { Id = 7, Title = "System Upgrade", Type = "Is afgerond", Description = "Upgraded system firmware to latest version.", Date_Created = new DateTime(2024, 2, 25), CustomerId = 7, EmployeeId = 4 },
				new Note { Id = 8, Title = "Training Session", Type = "Is afgerond", Description = "Provided training for customer staff.", Date_Created = new DateTime(2024, 2, 26), CustomerId = 8, EmployeeId = 4 },
				new Note { Id = 9, Title = "Follow-up Visit", Type = "Overig", Description = "Scheduled follow-up visit for customer support.", Date_Created = new DateTime(2024, 2, 28), CustomerId = 9, EmployeeId = 1 },
				new Note { Id = 10, Title = "Routine Maintenance", Type = "Overig", Description = "Routine check-up performed.", Date_Created = new DateTime(2024, 3, 1), CustomerId = 10, EmployeeId = 1 },
				new Note { Id = 11, Title = "Product Demonstration", Type = "Terug bellen", Description = "Gave product demonstration to new customer.", Date_Created = new DateTime(2024, 3, 3), CustomerId = 11, EmployeeId = 2 },
				new Note { Id = 12, Title = "Warranty Claim", Type = "Afspraak nog te maken", Description = "Processed warranty claim for customer.", Date_Created = new DateTime(2024, 3, 4), CustomerId = 12, EmployeeId = 2 },
				new Note { Id = 13, Title = "Diagnostic Test", Type = "Afspraak is gemaakt", Description = "Conducted diagnostic tests on equipment.", Date_Created = new DateTime(2024, 3, 6), CustomerId = 13, EmployeeId = 3 },
				new Note { Id = 14, Title = "System Alert", Type = "Is afgerond", Description = "Resolved system alert for temperature issues.", Date_Created = new DateTime(2024, 3, 7), CustomerId = 14, EmployeeId = 3 },
				new Note { Id = 15, Title = "Software Update", Type = "Overig", Description = "Installed latest software update for system.", Date_Created = new DateTime(2024, 3, 9), CustomerId = 15, EmployeeId = 4 }
			);

			// Productinventory
			var productInventories = new Faker<ProductInventory>()
				.RuleFor(p => p.Id, f => f.IndexFaker + 1)
				.RuleFor(p => p.ProductId, f => f.IndexFaker + 1)
				.RuleFor(p => p.InStock, f => f.Random.Int(1, 20))
				.RuleFor(p => p.AmountOrdered, f => f.Random.Int(0, 50))
				.Generate(500);

			modelBuilder.Entity<ProductInventory>().HasData(productInventories);


		}
	}
}
