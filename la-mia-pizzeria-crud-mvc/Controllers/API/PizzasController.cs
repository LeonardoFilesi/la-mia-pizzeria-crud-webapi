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
        private PizzaContext _myDB;
        public PizzasController(PizzaContext myDB)
        {
            _myDB = myDB;
        }

        [HttpGet]
        public IActionResult GetPizzas() 
        { 
            using(PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizzas = db.Pizzas.Include(pizza => pizza.Ingredientis).ToList();
                return Ok(pizzas);
            }
        }
        [HttpGet("{id}")]
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

        [HttpGet]
        public IActionResult SearchPizzaById(int id)
        {

            using (PizzaContext db = new PizzaContext())
            {
                Pizza? pizza = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();
                
                if (id != null)
                {
                    return Ok(pizza);
                } else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]Pizza newPizza)
        {
            try
            {
                _myDB.Pizzas.Add(newPizza);
                _myDB.SaveChanges();
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

            
    }
}
