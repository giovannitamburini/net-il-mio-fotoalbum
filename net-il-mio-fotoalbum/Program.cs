using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using System.Text.Json.Serialization;

namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // modificato la riga per l'autenticazione/autorizzazione
            builder.Services.AddDbContext<PhotoPortfolioContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<PhotoPortfolioContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // codice di configurazione per il serializzatore JSON, in modo che ignori le dipendenze cicliche le eventuali relazioni 1:N o N:N presenti nel JSON risultante
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Dependency Injection
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