using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class IngredientAllergen
    {
        public int IngredientId { get; set; }
        public int AllergenId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;
        public Allergen Allergen { get; set; } = null!;
    }
}
