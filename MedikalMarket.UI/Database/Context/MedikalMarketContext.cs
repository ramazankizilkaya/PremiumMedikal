using MedikalMarket.UI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MedikalMarket.UI.Database.Context
{
    public class MedikalMarketContext : DbContext   /*IdentityDbContext*/
    {
        public MedikalMarketContext(DbContextOptions<MedikalMarketContext> options)
           : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdProduct> AdProducts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ContactUs> ContactUsMessages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EmailNewsletter> EmailNewsletters { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<MiddleCategory> MiddleCategories { get; set; }
        public DbSet<MiniSlider> MiniSliders { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<TopCategory> TopCategories { get; set; }
        public DbSet<Slider> Sliders { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.NoAction;
            //}

        }

    }
}
