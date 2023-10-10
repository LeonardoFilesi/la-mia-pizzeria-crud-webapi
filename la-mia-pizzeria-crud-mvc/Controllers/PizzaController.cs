using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_crud_mvc.Models;  // Questa classe che richiamo, la sto artificialmente richiamando, non è stata scritta automaticamente
using System.Diagnostics;  // Questa classe che richiamo, la sto artificialmente richiamando, non è stata scritta automaticamente
using la_mia_pizzeria_crud_mvc.Database;
using Microsoft.Identity.Client;
using la_mia_pizzeria_crud_mvc.CustomLoggers;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace la_mia_pizzeria_crud_mvc.Controllers
{
   
    public class PizzaController : Controller
    {
        private ICustomLogger _mylogger;
        private PizzaContext _myDatabase;
        public PizzaController(PizzaContext db, ICustomLogger logger)
        {
            _mylogger = logger;
            _myDatabase = db;
        }
        // AGGIUNGE UN PEZZO DI CODICE CHE SEMPLIFICA LA SINTASSI DI TUTTO IL CONTROLLER "_myDatabase"

        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            _mylogger.WriteLog("L'utente è arrivato sulla pagina Pizza > Index");

            List<Pizza> pizzas = _myDatabase.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredientis).ToList<Pizza>();
            return View("Index", pizzas);

        }

        [Authorize(Roles = "ADMIN, USER")]
        public IActionResult UserIndex()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizzas = db.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredientis).ToList<Pizza>();
                return View("UserIndex", pizzas);
            }
        }

        [Authorize(Roles = "ADMIN, USER")]
        public IActionResult Details(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza? foundedPizza = db.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredientis).FirstOrDefault();

                if (foundedPizza == null)
                {
                    return NotFound($"La pizza con id:{id} non è stato trovato!");
                }
                else
                {
                    return View("Details", foundedPizza);
                }
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> categories = _myDatabase.Categories.ToList();

            // OPERAZIONE NECESSARIA per passare al nuovo PizzaFormModel solo le informazioni string Name e int Id delle istanze di Ingredienti
            List<SelectListItem> allIngredientisSelectList = new List<SelectListItem>();
            List<Ingredienti> databaseAllIngredientis = _myDatabase.Ingredientis.ToList();
            foreach (Ingredienti ingredienti in databaseAllIngredientis)
            {
                allIngredientisSelectList.Add(
                    new SelectListItem
                    {
                        Text = ingredienti.Name,
                        Value = ingredienti.Id.ToString()
                    });
            }

            PizzaFormModel model = new PizzaFormModel{ Pizza = new Pizza(), Categories = categories, Ingredienti = allIngredientisSelectList};
           
            return View("Create", model);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<SelectListItem> allIngredientisSelectList = new List<SelectListItem>();
                List<Ingredienti> databaseAllIngredientis = _myDatabase.Ingredientis.ToList();
                foreach (Ingredienti ingredienti in databaseAllIngredientis)
                {
                    allIngredientisSelectList.Add(new SelectListItem { Text = ingredienti.Name, Value = ingredienti.Id.ToString() });
                }
                data.Ingredienti = allIngredientisSelectList;
                
                return View("Create", data);
            }

            data.Pizza.Ingredientis = new List<Ingredienti>();

            if (data.SelectedIngredientiId != null)
            {
                foreach(string ingredientiSelectedid in data.SelectedIngredientiId)
                {
                    int intIngredientiSelectedId = int.Parse(ingredientiSelectedid);

                    Ingredienti? ingredientiInDataBase = _myDatabase.Ingredientis.Where(ingredienti => ingredienti.Id == intIngredientiSelectedId).FirstOrDefault();

                    if(ingredientiInDataBase != null)
                    {
                        data.Pizza.Ingredientis.Add(ingredientiInDataBase);
                    }
                }
            }

            _myDatabase.Pizzas.Add(data.Pizza);
            _myDatabase.SaveChanges();
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(int id)
        {

            Pizza? pizzaToEdit = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredientis).FirstOrDefault();

            if (pizzaToEdit == null)
            {
                return NotFound("La pizza non è stata trovata");
            }
            else
            {
                List<Category> categories = _myDatabase.Categories.ToList();

                List<SelectListItem> allIngredientisSelectList = new List<SelectListItem>();
                List<Ingredienti> databaseAllIngredientis = _myDatabase.Ingredientis.ToList();
                foreach (Ingredienti ingredienti in databaseAllIngredientis)
                {
                    allIngredientisSelectList.Add(new SelectListItem
                    {
                        Value = ingredienti.Id.ToString(),
                        Text = ingredienti.Name,
                        Selected = pizzaToEdit.Ingredientis.Any(ingredientiAssociated => ingredientiAssociated.Id == ingredienti.Id)
                    });
                }

                PizzaFormModel model = new PizzaFormModel { Pizza = pizzaToEdit, Categories = categories, Ingredienti = allIngredientisSelectList };
                return View("Update", model);
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<SelectListItem> allIngredientisSelectList = new List<SelectListItem>();
                List<Ingredienti> databaseAllIngredientis = _myDatabase.Ingredientis.ToList();
                foreach (Ingredienti ingredienti in databaseAllIngredientis)
                {
                    allIngredientisSelectList.Add(new SelectListItem
                    {
                        Value = ingredienti.Id.ToString(),
                        Text = ingredienti.Name,
                    });
                }
                data.Ingredienti = allIngredientisSelectList;

                return View("Update", data);
            }
            data.Pizza.Id = id;
            Pizza? pizzaToUpdate = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredientis).FirstOrDefault();

            if (pizzaToUpdate != null)
            {
                data.Pizza.Ingredientis = new List<Ingredienti>();
                EntityEntry<Pizza> entryEntity = _myDatabase.Entry(pizzaToUpdate);
                
                if(data.SelectedIngredientiId != null) 
                {
                    foreach (string ingredientiSelectedid in data.SelectedIngredientiId)
                    {
                        int intIngredientiSelectedId = int.Parse(ingredientiSelectedid);

                        Ingredienti? ingredientiInDataBase = _myDatabase.Ingredientis.Where(ingredienti => ingredienti.Id == intIngredientiSelectedId).FirstOrDefault();

                        if (ingredientiInDataBase != null)
                        {
                            pizzaToUpdate.Ingredientis.Add(ingredientiInDataBase);
                        }
                    }
                }
                entryEntity.CurrentValues.SetValues(data.Pizza);

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi spiace, non sono state trovate Pizze da aggiornare");
            }

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            Pizza? pizzaToDelete = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _myDatabase.Pizzas.Remove(pizzaToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La Pizza da eliminare non è stata trovata");
            }

        }

    }
}
