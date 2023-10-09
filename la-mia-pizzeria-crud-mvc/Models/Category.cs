using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_crud_mvc.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Il titolo della categoria è obbligatorio!")]
        [StringLength(50, ErrorMessage ="Il link non può essere lungo più di 50 caratteri")]
        public string Name { get; set; }    

        //RELAZIONE 1 A n TRA PIZZA E CATEGORIA

        public List<Pizza>? Pizzas { get; set; }
        public Category() 
        { 
        
        }

    }
}
