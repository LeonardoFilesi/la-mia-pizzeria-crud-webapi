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
        [HttpGet]
        public IActionResult SearchPizzas(string? search)
        {
            if(search == null)
            {
                return BadRequest(new {Message = "Non hai inserito una stringa di ricerca"});
            }
            
            using(PizzaContext db = new PizzaContext())
            {
                List<Pizza> foundedPizzas = db.Pizzas.Where(pizza => pizza.Name.ToLower().Contains(search.ToLower())).ToList();
                return Ok(foundedPizzas);
            }
        }
    }
}
