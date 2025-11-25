using System.ComponentModel.DataAnnotations;
using SknC.Web.Core.Enums;

namespace SknC.Web.Core.Entities
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string InciName { get; set; } = string.Empty; // Ej: "Niacinamide"

        [StringLength(200)]
        public string? CommonName { get; set; } // Ej: "Vitamin B3"

        [StringLength(1000)]
        public string? Description { get; set; }

        public IngredientFunction Function { get; set; }

        // Navigation Property
        public ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
    }
}