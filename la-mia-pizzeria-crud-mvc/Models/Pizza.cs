using la_mia_pizzeria_crud_mvc.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_crud_mvc.Models
{
    public class Pizza
    {
        [Key] public int Id { get; set; }

        //------------------------------------------------------------------------------------------------------------------------
        [Required(ErrorMessage = "il nome della Pizza è obbligatorio")] // AGGIUNTA per la VALIDAZIONE
        [MaxLength(100, ErrorMessage = "La lunghezza massima è di 100 caratteri")]  // AGGIUNTA se si vogliono specificare ancora di più le colonne
        public string Name { get; set; }

        //------------------------------------------------------------------------------------------------------------------------
        [Pizza5Words]
        [Required(ErrorMessage = "La descrizione della Pizza è obbligatoria")] // AGGIUNTA per la VALIDAZIONE
        [Column(TypeName = "text")]  // AGGIUNTA se si vogliono specificare ancora di più le colonne
        public string Description { get; set; }
        
        //------------------------------------------------------------------------------------------------------------------------
        public int Price { get; set; }

        //------------------------------------------------------------------------------------------------------------------------
        [Url(ErrorMessage ="Devi inserire un link valido per l'immagine della Pizza")] // AGGIUNTA per la VALIDAZIONE
        [MaxLength(500, ErrorMessage ="Il link non può essere più lungo di 500 caratteri")] // AGGIUNTA per la VALIDAZIONE
        public string Image {  get; set; }


        // AGGIUNGERE CategoryId e la relazione con 1 a N CATEGORY
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }


        // RELAZIONE N a N con gli Ingredienti
        public List<Ingredienti>? Ingredientis { get; set; }


        //COSTRUTTORE===================================================================================================================
        public Pizza(string name, string description, int price, string image)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Image = image;
        }

        public Pizza()  // SEMPRE AGGIUNGERE COSTRUTTORE VUOTO
        {

        }
    }
}
