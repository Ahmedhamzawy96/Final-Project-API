using Microsoft.EntityFrameworkCore;

namespace API_and_DataBase.Models
{
    public class CompanyContext:DbContext
    {
         public CompanyContext(DbContextOptions<CompanyContext> options):base(options)
        {


        }

        public CompanyContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImportProduct>()
                .HasKey(I => new { I.ReceiptID, I.ProductID });
            modelBuilder.Entity<ExportProduct>()
                .HasKey(E => new { E.ReceiptID, E.ProductID });
            modelBuilder.Entity<CarProduct>()
                .HasKey(C => new { C.CarID, C.ProductID });

        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarProduct> CarProducts { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public  virtual DbSet<Customer> Customers { get; set; }
        public  virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<ExportProduct> ExportProducts { get; set; }
        public virtual DbSet<ExportReciept> ExportReciepts{ get; set; }
        public virtual DbSet<ImportProduct> ImportProducts { get; set; }
        public virtual DbSet<ImportReciept> ImportReciepts{ get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }



    }
}
