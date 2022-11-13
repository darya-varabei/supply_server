using Microsoft.EntityFrameworkCore;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.Models.Login;

namespace SupplyIO.SupplyIO.Services.Models.Context
{
    public class MetalContext : DbContext
    {
        public MetalContext(DbContextOptions<MetalContext> options)
            : base(options)
        {

            //Database.EnsureDeleted();
            //Database.Migrate();
        }

        public virtual DbSet<Certificate> Certificate { get; set; } = null!;
        public virtual DbSet<ChemicalComposition> ChemicalComposition { get; set; } = null!;
        public virtual DbSet<ImpactStrength> ImpactStrengths { get; set; } = null!;
        public virtual DbSet<Package> Package { get; set; } = null!;
        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<Size> Size { get; set; } = null!;
        public virtual DbSet<Weight> Weight { get; set; } = null!;
        public virtual DbSet<Status> Status { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfo { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionUrl = /*Environment.GetEnvironmentVariable("DATABASE_URL");*/ "postgres://pcgeuxxctxtdrg:4389638e9b54fbea09186de13cc5b2563f9120bcfd3afe245d4cacf2cdcf9a75@ec2-52-212-228-71.eu-west-1.compute.amazonaws.com:5432/d7bm7ameohjgqa";

        //    connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
        //    var userPassSide = connectionUrl.Split("@")[0];
        //    var hostSide = connectionUrl.Split("@")[1];

        //    var user = userPassSide.Split(":")[0];
        //    var password = userPassSide.Split(":")[1];
        //    var host = hostSide.Split("/")[0];
        //    var database = hostSide.Split("/")[1].Split("?")[0];

        //    var defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";

        //    optionsBuilder.UseNpgsql(defaultConnectionString);

        //    //optionsBuilder.UseNpgsql("Host=localhost;Database=MetalManagment;Username=postgres;Password=admin");

        //    //Database.EnsureDeleted();
        //    //Database.Migrate();
        //}
    }
}
