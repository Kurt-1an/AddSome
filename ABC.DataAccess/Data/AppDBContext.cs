using ABC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ABC.DataAccess.Data
{
    public class AppDBContext : IdentityDbContext<IdentityUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) 
            : base(options) 
        {
            
        }

        //Create Database
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<UserManagement> UsersManagement { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            ////Pushed Data into Category Database
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "CCTV", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Printers", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Computer Accesories", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Cables & Tools", DisplayOrder = 4 }
                );

            //Pushed Data into UserMAnagement Database
            modelBuilder.Entity<UserManagement>().HasData(
                new UserManagement { Id = 1, FirstName = "Kurt", ContactNumber = 09568271611, AccessLevel = "Admin", Address = "Taytay Rizal", Email = "neiljejomar@gmail.com", LastName = "Alarcos", Password = "Hatdog", DateCreated = DateTime.Now }
                );

            //Pushed Data into PurchaseOrder Database
            modelBuilder.Entity<PurchaseOrder>().HasData(
				new PurchaseOrder { Id = 1, SupplierName = "Kurt", LocationDelivery = "Pasig", PaymentTerm = "Cash", ExpectedDeliveryDate = new DateTime(2023, 10, 21), EmployeeName = "Neil", ContactNumber = 09568271611, ShipmentPreference = "Cash On Delivery", AdditionalNote="additional note goes in here" }
				);

            //Pushed Data into Customer Database
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FirstName = "Kurt", LastName = "Betonio", EmailAddress = "neiljejomar@gmail.com", ContactNumber = 09568271611, Type = "Walk in", ApSuUn = "Unit 1234", StreetorSubd = "Jasmine St.", Barangay = "Batman", City = "Antipolo", Province = "Rizal", ZipCode = 1870 }
                );

            //Pushed Data into Product Database
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Barcode = 0832175698,
                    SKU = "printer-AllInOne-XYZ123",
                    productName = "XYZ123 All-in-One Printer",
                    CategoryId = 1,
                    Brand = "HP",
                    Warehouse = "Makati",
                    Description = "Versatile all-in-one printer for printing, copying, and scanning",
                    CostPrice = 800,
                    RetailPrice = 1299,
                    StockQuantity = 20,
                    MinimumStockQuantity = 5,
                    Type = "Extended Warranty",
                    Duration = "12 months from date of purchase",
                    Provider = "Third-Party Warranty Company",
                    addNotes = "Additional Notes is here color touchscreen interface ",
                    SupplierId = 2,
                    ImageUrl = ""

                },
                new Product
                {
                    Id = 2,
                    Barcode = 0954532414,
                    SKU = "cctv-SmartCam-360",
                    productName = "SmartCam 360 Security Camera",
                    CategoryId = 2,
                    Brand = "Samsung",
                    Warehouse = "Pasig",
                    Description = "Panoramic view with motion detection",
                    CostPrice = 1200,
                    RetailPrice = 1999,
                    StockQuantity = 15,
                    MinimumStockQuantity = 4,
                    Type = "Manufacturers Warranty",
                    Duration = "7 days from date of purchase",
                    Provider = "Manufacturer",
                    SupplierId = 1,
                    ImageUrl = ""
                });

            //Pushed Data into Supplier Database
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    Id = 1,
                    supplierCompanyName = "Addvert",
                    supplierContactNumber = 09651232235,
                    supplierEmail = "addvert214@gmail.com",
                    supplierStatus = "Active",
                    supplierDescription = "N/A",
                    supplierLotBlk = "c4 l5",
                    supplierStreetSubdv = "E. Corazon",
                    supplierBarangay = "Maybancal",
                    supplierCity = "Tanay",
                    supplierProvince = "Rizal",
                    supplierZipCode = 1870,
                    supplierNote = "My supplier"
                },

                new Supplier
                {
                    Id = 2,
                    supplierCompanyName = "Addvert",
                    supplierContactNumber = 09651232235,
                    supplierEmail = "addvert214@gmail.com",
                    supplierStatus = "Active",
                    supplierDescription = "N/A",
                    supplierLotBlk = "c4 l5",
                    supplierStreetSubdv = "E. Corazon",
                    supplierBarangay = "Maybancal",
                    supplierCity = "Tanay",
                    supplierProvince = "Rizal",
                    supplierZipCode = 1870,
                    supplierNote = "My supplier"
                });


        }
	}
}
