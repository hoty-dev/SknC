using System.ComponentModel.DataAnnotations;

namespace SknC.Web.Core.Entities
{
    public class ProductIngredient
    {
        // Composite Key Part 1
        public int ProductReferenceId { get; set; }
        public ProductReference? ProductReference { get; set; }

        // Composite Key Part 2
        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }

        // Payload
        [StringLength(50)]
        public string? Concentration { get; set; } // Ej: "10%", "500ppm"
    }
}