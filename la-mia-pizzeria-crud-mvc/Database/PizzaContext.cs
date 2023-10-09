using Microsoft.EntityFrameworkCore;  // non generato, aggiunto a mano da me
using la_mia_pizzeria_crud_mvc.Models;   // non generato, aggiunto a mano da me
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace la_mia_pizzeria_crud_mvc.Database
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }  // AGGIUNTO QUESO FARE MIGRATION E AGGIORNARE DATABASE
        public DbSet<Ingredienti> Ingredientis { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=PizzaDB; TrustServerCertificate=True; Integrated Security=True");
        }
        // QUESTO METODO ^^^^^^^^^ IMPOSTA LA STRINGA DI CONNESSIONE importante:  TrustServerCertificate=True;
    }
}
