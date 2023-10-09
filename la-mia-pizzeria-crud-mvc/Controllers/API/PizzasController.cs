using la_mia_pizzeria_crud_mvc.Database;
using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_crud_mvc.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPizzas() 
        { 
            using(PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizzas = db.Pizzas.Include(pizza => pizza.Ingredientis).ToList();
                return Ok(pizzas);
            }
        }    
    }
}
