using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Characteristic;

namespace DataAccessLayer.Entities
{
    internal class ProductEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public float Weight { get; set; }

        public bool StockAvailability => Amount > 0; 

        public int Amount { get; set; }

        // Навигационное свойство
        public List<CategoryEntity> Categories { get; set; }

        public List<CharacteristicRealizationEntity> CharacteristicsRealization { get; set; }
    }
}