using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Characteristic;

namespace DataAccessLayer.Entities
{
    internal class CategoryEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ProductEntity> Products { get; set; } // при маппинге делать налл категорий
        //  после маппить категории, а потом делать наоборот.

        public List<CharacteristicPatternEntity> CharacteristicPatterns { get; set; }
    }
}