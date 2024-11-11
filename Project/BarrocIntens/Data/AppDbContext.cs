using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;


namespace BarrocIntens.Data
{
    
    internal class AppDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LeaseContract> LeaseContracts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoicesItems { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=;database=BarrocIntens",
                ServerVersion.Parse("8.0.30")
                );
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var faker = new Faker();

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
                new Department { Id = 4, Name = "Inkoop" }
            );

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Sales", Email = "sales@barrocintens.nl", Password = "sales", Active = true, DepartmentId = 1 },
                new User { Id = 2, Name = "Onderhoud", Email = "onderhoud@barrocintens.nl", Password = "onderhoud", Active = true, DepartmentId = 2 },
                new User { Id = 3, Name = "Finance", Email = "finance@barrocintens.nl", Password = "finance", Active = true, DepartmentId = 3 },
                new User { Id = 4, Name = "Inkoop", Email = "inkoop@barrocintens.nl", Password = "inkoop", Active = true, DepartmentId = 4 }
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
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.CompanyId, f => f.Random.Int(1, 150))
                .Generate(150); // Active Customers


            modelBuilder.Entity<Customer>().HasData(customers);


            // Products
            var products = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => (double)f.Finance.Amount(100, 1000))
                .RuleFor(p => p.CategoryId, f => f.Random.Int(1, 5))
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.IsStock, f => f.Random.Bool())
                .RuleFor(p => p.VisibleForCustomers, f => true)
                .Generate(500);

            modelBuilder.Entity<Product>().HasData(products);

            // Invoices
            Func<Faker, double> value = f => (double)f.Finance.Amount(100, 5000);
            var invoices = new Faker<Invoice>()
                .RuleFor(i => i.Id, f => f.IndexFaker + 1)
                .RuleFor(i => i.ContractId, f => f.Random.Int(1, 150))
                .RuleFor(i => i.DateCreated, f => f.Date.Recent())
                .RuleFor(i => i.TotalPrice, value)
                .RuleFor(i => i.Paid, f => f.Random.Bool()) // 120 invoices with payment delay
                .Generate(500);

            modelBuilder.Entity<Invoice>().HasData(invoices);

            // Lease Contracts
            var leaseContracts = new Faker<LeaseContract>()
                .RuleFor(l => l.Id, f => f.IndexFaker + 1)
                .RuleFor(l => l.CompanyId, f => f.Random.Int(1, 3))
                .RuleFor(l => l.Start_Date, f => f.Date.Past(1))
                .RuleFor(l => l.Contract_Type, f => f.PickRandom(new[] { "Repeat", "One-time" }))
                .RuleFor(l => l.End_Date, f => f.Date.Future(1)).Rules((f, l) =>
                {
                    if (l.Contract_Type == "Repeat")
                    {
                        l.End_Date = null;
                    }
                })
                .Generate(150);

            modelBuilder.Entity<LeaseContract>().HasData(leaseContracts);

            //ServiceRequests(storingen)

            //Ik heb dit ff temp gezet omdat ik aan het testen ben:
            var serviceRequests = new Faker<ServiceRequest>()
                .RuleFor(w => w.Id, f => f.IndexFaker + 7)
                .RuleFor(w => w.Description, f => f.Lorem.Sentence())
                .RuleFor(w => w.Date_Reported, f => f.Date.Past(1))
                .RuleFor(w => w.Status, f => f.Random.Int(1, 3))
                .RuleFor(w => w.CustomerId, f => f.Random.Int(1, 150))
                .RuleFor(w => w.ProductId, f => f.Random.Int(1, 500))
                .Generate(75);

            //var serviceRequests = new Faker<ServiceRequest>()
            //	.RuleFor(w => w.Id, f => f.IndexFaker + 7)
            //	.RuleFor(w => w.Description, f => f.Lorem.Sentence())
            //	.RuleFor(w => w.Status, f => f.Random.Int(3, 3))
            //	.RuleFor(w => w.CustomerId, f => f.Random.Int(1, 150))
            //	.RuleFor(w => w.ProductId, f => f.Random.Int(1, 500))
            //	.Generate(75);

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

            // Work Orders
            var workOrders = new Faker<WorkOrder>()
                .RuleFor(w => w.Id, f => f.IndexFaker + 1)
                .RuleFor(w => w.RequestId, f => f.Random.Int(1, 75))
                .RuleFor(w => w.Description, f => f.Lorem.Sentence())
                .RuleFor(w => w.AppointmentId, f => f.Random.Int(1, 75))
                .RuleFor(w => w.ProductId, f => f.Random.Int(1, 500))
                .RuleFor(w => w.UserId, f => f.Random.Int(1, 4))
                .Generate(75);

            // Adding routine and maintenance work orders
            var routineWorkOrders = workOrders.Take(35).Select(w =>
            {
                w.Description = "Routine Maintenance: " + w.Description;
                return w;
            }).ToList();

            var emergencyWorkOrders = workOrders.Skip(35).ToList();

            modelBuilder.Entity<WorkOrder>().HasData(routineWorkOrders);
            modelBuilder.Entity<WorkOrder>().HasData(emergencyWorkOrders);

            // Appointments
            var appointments = new Faker<Appointment>()
                .RuleFor(a => a.Id, f => f.IndexFaker + 1)
                .RuleFor(a => a.Date, f => f.Date.Future(1))
                .RuleFor(a => a.UserId, f => f.Random.Int(1, 4))
                .RuleFor(a => a.CustomerId, f => f.Random.Int(1, 150))
                .RuleFor(a => a.Description, f => f.Lorem.Sentence())
                .Generate(75);

            modelBuilder.Entity<Appointment>().HasData(appointments);

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
