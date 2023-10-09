using Microsoft.AspNetCore.Mvc.Rendering;

namespace la_mia_pizzeria_crud_mvc.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set; }

        // NUOVE PROPRIETA' serve per gestire "Select" che seleziona istanze multiple nelle viste (Multiple per la relazione N a N)
        public List<SelectListItem>? Ingredienti { get; set; }
        public List<string>? SelectedIngredientiId { get; set; }
    }
}
