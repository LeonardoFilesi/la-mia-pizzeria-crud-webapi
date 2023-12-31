using la_mia_pizzeria_crud_mvc.CustomLoggers;
using la_mia_pizzeria_crud_mvc.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace la_mia_pizzeria_crud_mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // var connectionString = builder.Configuration.GetConnectionString("PizzaContextConnection") ?? throw new InvalidOperationException("Connection string 'PizzaContextConnection' not found.");
            // CODICE RIMOSSO COME DA ISTRUZIONI SLIDE PER AUTHENTICATION
            // builder.Services.AddDbContext<PizzaContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<PizzaContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<PizzaContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Codice aggiunto per evitare che la chiamata Api, vada il loop
            // Codice di configurazione per il serializzatore Json
            // in modo che ignori completamente le dipendenze cicliche di eventuali relazioni N:N o 1:N
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //DEPENDECY INJECTION
            builder.Services.AddScoped<ICustomLogger, CustomFileLogger>();
            builder.Services.AddScoped<PizzaContext, PizzaContext>();
            
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Pizza}/{action=UserIndex}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}