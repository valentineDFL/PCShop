using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CharacteristicRealization
    {
        public CharacteristicRealization() { }

        public CharacteristicRealization(Guid id, string value, Guid characteristicPatternId, 
            CharacteristicPattern characteristicPattern, Guid productId, Product product)
        {
            Id = id;
            Value = value;
            CharacteristicPatternId = characteristicPatternId;
            CharacteristicPattern = characteristicPattern;
            ProductId = productId;
            Product = product;
        }

        public Guid Id { get; private set; }

        public string Value { get; private set; }

        // ссылка
        public Guid CharacteristicPatternId { get; private set; }

        // навигационное свойство
        public CharacteristicPattern CharacteristicPattern { get; private set; }

        public Guid ProductId { get; private set; }

        public Product Product { get; private set; }
    }
}
