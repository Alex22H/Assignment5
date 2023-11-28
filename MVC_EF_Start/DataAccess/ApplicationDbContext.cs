using Microsoft.EntityFrameworkCore;
using MVC_EF_Start.Models;

namespace MVC_EF_Start.DataAccess
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<DRegion> DRegions { get; set; }
        public DbSet<DCounty> DCounties { get; set; }
        public DbSet<DMake> DMakes { get; set; }
        public DbSet<DModel> DModels { get; set; }
        public DbSet<DVehicle> DVehicles { get; set; }

      /*  
        public DbSet<State> States { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Range> Ranges { get; set; }


        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // State and County relationship


             modelBuilder.Entity<DRegion>()
             .HasMany(r => r.Counties)       // DRegion has many DCountries
             .WithOne(c => c.Region)         // DCountry has one DRegion
             .HasForeignKey(c => c.RegionID) // Foreign key property in DCountry
             .IsRequired();                  // Assuming RegionID is required in DCountry


            base.OnModelCreating(modelBuilder);
        }

       
       

    }
}