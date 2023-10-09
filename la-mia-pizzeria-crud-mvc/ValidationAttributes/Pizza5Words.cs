using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_crud_mvc.ValidationAttributes
{
    public class Pizza5Words : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string)
            {
                string inputValue = (string) value;

                if(inputValue == null || inputValue.Split(' ').Length <= 5) 
                {
                    return new ValidationResult("Il campo non contiene più di 5 parole!!!");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Il campo inserito non è di tipo stringa");

        }
    }
}
