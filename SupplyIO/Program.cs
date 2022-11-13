using Microsoft.EntityFrameworkCore;
using SupplyIO.SupplyIO.DataAccess;
using SupplyIO.SupplyIO.DataAccess.PostgreSQL;
using SupplyIO.SupplyIO.Services;
using SupplyIO.SupplyIO.Services.Logic;
using SupplyIO.SupplyIO.Services.Models.Context;
using SupplyIO.SupplyIO.Services.Models.Login;

var builder = WebApplication.CreateBuilder(args);

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MetalContext>(opt =>
    opt.UseNpgsql(defaultConnectionString));

var serviceProvider = builder.Services.BuildServiceProvider();

//try
//{
//    var dbContext = serviceProvider.GetRequiredService<MetalContext>();
//    dbContext.Database.EnsureDeleted();
//    dbContext.Database.Migrate();

//    var user = new User()
//    {
//        Login = "user",
//        Password = "user",
//        UserInfo = new UserInfo()
//        {
//            FirstName = "Глеб",
//            LastName = "Караулов",
//            Position = "worker"
//        }
//    };

//    var admin = new User()
//    {
//        Login = "admin",
//        Password = "admin",
//        UserInfo = new UserInfo()
//        {
//            FirstName = "Антон",
//            LastName = "Вальев",
//            Position = "manager"
//        }
//    };

//    dbContext.User.AddRange(user, admin);
//    dbContext.SaveChanges();
//}
//catch
//{
//}

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMetalService, MetalService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMetalAccess, MetalAccess>();
builder.Services.AddScoped<IAuthAccess, AuthAccess>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStatusCodePages();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
