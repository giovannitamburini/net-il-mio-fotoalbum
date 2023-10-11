using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // modificato la riga per l'autenticazione/autorizzazione
            builder.Services.AddDbContext<PhotoPortfolioContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<PhotoPortfolioContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // dependency
            builder.Services.AddScoped<PhotoPortfolioContext, PhotoPortfolioContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // aggiunta per l'autenticazione
            app.UseAuthentication();

            app.UseAuthorization();

            // rotta di defualt quando avviamo il server
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Photo}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}