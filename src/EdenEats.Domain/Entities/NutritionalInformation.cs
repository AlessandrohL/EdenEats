using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class NutritionalInformation : BaseEntity
    {
        public int Id { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double SaturatedFat { get; set; }
        public double Carbohydrate { get; set; }
        public double Sugar { get; set; }
        public double Protein { get; set; }
        public double DietaryFiber { get; set; }
        public double Salt { get; set; }
        public Guid FoodId { get; set; }
        public Food Food { get; set; } = null!;
    }
}
