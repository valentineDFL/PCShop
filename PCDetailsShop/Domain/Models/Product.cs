using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        public Product() { }

        public Product(Guid id, string name, string description, decimal price, float weight, bool stockAvailability, int count, 
            List<Category> categories, List<CharacteristicRealization> characteristicsRealizations)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Weight = weight;
            StockAvailability = stockAvailability;
            Count = count;
            Categories = categories;
            CharacteristicsRealizations = characteristicsRealizations;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public float Weight { get; private set; }

        public bool StockAvailability { get; private set; }

        public int Count { get; private set; }

        // Навигационное свойство
        public List<Category> Categories { get; private set; }

        public List<CharacteristicRealization> CharacteristicsRealizations { get; private set; }
    }
}