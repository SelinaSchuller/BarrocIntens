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
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Companies> Company { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<LeaseContracts> LeaseContracts { get; set; }
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<InvoiceItems> InvoicesItems { get; set; }
        public DbSet<ServiceRequests> ServiceRequests { get; set; }
        public DbSet<ProductInventories> ProductInventories { get; set; }
        public DbSet<WorkOrders> WorkOrders { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<ProductCategories> ProductCategories { get; set; }


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

            modelBuilder.Entity<ProductCategories>().HasData(
                new ProductCategories { Id = 1, Name = "Part" },
                new ProductCategories { Id = 2, Name = "Accessory" },
                new ProductCategories { Id = 3, Name = "Service" },
                new ProductCategories { Id = 4, Name = "Software" },
                new ProductCategories { Id = 5, Name = "Hardware" }
            );

            // Departments
            modelBuilder.Entity<Departments>().HasData(
                new Departments { Id = 1, Name = "Management" },
                new Departments { Id = 2, Name = "Sales" },
                new Departments { Id = 3, Name = "Onderhoud" },
                new Departments { Id = 4, Name = "Finance" },
                new Departments { Id = 5, Name = "Inkoop" }
            );

            // Users
            modelBuilder.Entity<Users>().HasData(
                new Users { Id = 1, Name = "Admin", Email = "admin@barrocintens.nl", Password = "admin", Active = true, DepartmentId = 1 },
                new Users { Id = 2, Name = "Sales", Email = "sales@barrocintens.nl", Password = "sales", Active = true, DepartmentId = 2 },
                new Users { Id = 3, Name = "Onderhoud", Email = "onderhoud@barrocintens.nl", Password = "onderhoud", Active = true, DepartmentId = 3 },
                new Users { Id = 4, Name = "Finance", Email = "finance@barrocintens.nl", Password = "finance", Active = true, DepartmentId = 4 },
                new Users { Id = 5, Name = "Inkoop", Email = "inkoop@barrocintens.nl", Password = "inkoop", Active = true, DepartmentId = 5 }
            );

            // Companies
            var companies = new Faker<Companies>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Bkr, f => f.IndexFaker < 50) // 10 with BKR registration
                .Generate(150); // Inactive Customers
            modelBuilder.Entity<Companies>().HasData(companies);
            // Customers
            var customers = new Faker<Customers>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Address, f => f.Address.StreetAddress())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.CompanyId, f => f.Random.Int(1, 150))
                .Generate(150); // Active Customers


            modelBuilder.Entity<Customers>().HasData(customers);


            // Products
            modelBuilder.Entity<Products>()
                        .HasOne(p => p.Category)
                        .WithMany(c => c.Products)
                        .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);

            var products = new Faker<Products>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => f.Finance.Amount(100, 1000))
                .RuleFor(p => p.CategoryId, f => f.Random.Int(1, 5)) // Random CategoryId from 1 to 5
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.IsStock, f => f.Random.Bool())
                .RuleFor(p => p.VisibleForCustomers, f => true)
                .Generate(500);

            modelBuilder.Entity<Products>().HasData(products);


            // Invoices
            Func<Faker, double> value = f => (double)f.Finance.Amount(100, 5000);
            var invoices = new Faker<Invoices>()
                .RuleFor(i => i.Id, f => f.IndexFaker + 1)
                .RuleFor(i => i.ContractId, f => f.Random.Int(1, 150))
                .RuleFor(i => i.DateCreated, f => f.Date.Recent())
                .RuleFor(i => i.TotalPrice, value)
                .RuleFor(i => i.Paid, f => f.IndexFaker < 120) // 120 invoices with payment delay
                .Generate(500);

            modelBuilder.Entity<Invoices>().HasData(invoices);

            // Lease Contracts
            var leaseContracts = new Faker<LeaseContracts>()
                .RuleFor(l => l.Id, f => f.IndexFaker + 1)
                .RuleFor(l => l.CompanyId, f => f.Random.Int(1, 3))
                .RuleFor(l => l.Start_Date, f => f.Date.Past(1))
                .RuleFor(l => l.End_Date, f => f.Date.Future(1))
                .RuleFor(l => l.Contract_Type, f => f.PickRandom(new[] { "Repeat", "One-time" }))
                .Generate(150);

            modelBuilder.Entity<LeaseContracts>().HasData(leaseContracts);

            // Work Orders
            var serviceRequests = new Faker<ServiceRequests>()
                .RuleFor(w => w.Id, f => f.IndexFaker + 1)
                .RuleFor(w => w.Description, f => f.Lorem.Sentence())
                .RuleFor(w => w.Status, f => f.Random.Int(1, 3))
                .RuleFor(w => w.CustomerId, f => f.Random.Int(1, 150))
                .RuleFor(w => w.ProductId, f => f.Random.Int(1, 500))
                .Generate(75);

            modelBuilder.Entity<ServiceRequests>().HasData(serviceRequests);


            var workOrders = new Faker<WorkOrders>()
                .RuleFor(w => w.Id, f => f.IndexFaker + 1)
                .RuleFor(w => w.RequestId, f => f.Random.Int(1, 75))
                .RuleFor(w => w.Description, f => f.Lorem.Sentence())
                .RuleFor(w => w.AppointmentId, f => f.Random.Int(1, 75))
                .RuleFor(w => w.ProductId, f => f.Random.Int(1, 500))
                .RuleFor(w => w.UserId, f => f.Random.Int(1, 5))
                .Generate(75);

            // Adding routine and maintenance work orders
            var routineWorkOrders = workOrders.Take(35).Select(w =>
            {
                w.Description = "Routine Maintenance: " + w.Description;
                return w;
            }).ToList();

            var emergencyWorkOrders = workOrders.Skip(35).ToList();

            modelBuilder.Entity<WorkOrders>().HasData(routineWorkOrders);
            modelBuilder.Entity<WorkOrders>().HasData(emergencyWorkOrders);

            // Appointments
            var appointments = new Faker<Appointments>()
                .RuleFor(a => a.Id, f => f.IndexFaker + 1)
                .RuleFor(a => a.Date, f => f.Date.Future(1))
                .RuleFor(a => a.UserId, f => f.Random.Int(1, 5))
                .RuleFor(a => a.CustomerId, f => f.Random.Int(1, 150))
                .RuleFor(a => a.Description, f => f.Lorem.Sentence())
                .Generate(75);

            modelBuilder.Entity<Appointments>().HasData(appointments);

            // Product Categories

        }
    }
}
