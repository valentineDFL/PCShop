using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Characteristic
{
    internal class CharacteristicRealizationEntity
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        // ссылка
        public Guid CharacteristicPatternId { get; set; }

        // навигационное свойство
        public CharacteristicPatternEntity CharacteristicPattern { get; set; }
    }
}
