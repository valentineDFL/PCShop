using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CharacteristicRealization
    {
        public CharacteristicRealization(Guid id, string value, 
            Guid characteristicPatternId, CharacteristicPattern characteristicPattern)
        {
            Id = id;
            Value = value;
            CharacteristicPatternId = characteristicPatternId;
            CharacteristicPattern = characteristicPattern;
        }

        public Guid Id { get; }

        public string Value { get; }

        public Guid CharacteristicPatternId { get; }

        public CharacteristicPattern CharacteristicPattern { get; }
    }
}
