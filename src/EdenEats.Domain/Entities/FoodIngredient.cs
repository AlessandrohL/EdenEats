using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class FoodIngredient
    {
        public Guid FoodId { get; set; }
        public int IngredientId { get; set; }
        public Food Food { get; set; } = null!;
        public Ingredient Ingredient { get; set; } = null!;
    }
}
