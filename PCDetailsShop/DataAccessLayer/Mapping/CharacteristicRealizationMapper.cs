using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Characteristic;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DataAccessLayer.Mapping
{
    internal class CharacteristicRealizationMapper
    {
        private readonly CharacteristicPatternMapper _characteristicPatternMapper;

        public CharacteristicRealizationMapper(CharacteristicPatternMapper characteristicPatternMapper)
        {
            _characteristicPatternMapper = characteristicPatternMapper;
        }

        public CharacteristicRealization EntityToModel(CharacteristicRealizationEntity entity) 
        {
            if (entity == null)
                throw new ArgumentNullException($"Characteristic Realization model is null {nameof(EntityToModel)}");

            return new CharacteristicRealization
                (
                    entity.Id,
                    entity.Value,
                    entity.CharacteristicPatternId,
                    _characteristicPatternMapper.EntityToModel(entity.CharacteristicPattern)
                );
        }

        public CharacteristicRealizationEntity ModelToEntity(CharacteristicRealization model)
        {
            if (model == null)
                throw new ArgumentNullException($"Characteristic Realization entity is null {nameof(ModelToEntity)}");

            return new CharacteristicRealizationEntity()
            {
                Id = model.Id,
                Value = model.Value,
                CharacteristicPatternId = model.CharacteristicPatternId,
                CharacteristicPattern = _characteristicPatternMapper.ModelToEntity(model.CharacteristicPattern)
            };
        }

        public List<CharacteristicRealization> EntitiesToModels(List<CharacteristicRealizationEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException($"Characteristic Realization entities is null {nameof(EntitiesToModels)}");

            List<CharacteristicRealization> result = new List<CharacteristicRealization>();

            foreach(CharacteristicRealizationEntity realization in entities)
            {
                result.Add(EntityToModel(realization));
            }

            return result;
        }

        public List<CharacteristicRealizationEntity> ModelsToEntities(List<CharacteristicRealization> models)
        {
            if (models == null)
                throw new ArgumentNullException($"Characteristic Realization models is null {nameof(EntitiesToModels)}");

            List<CharacteristicRealizationEntity> result = new List<CharacteristicRealizationEntity>();

            foreach (CharacteristicRealization realization in models)
            {
                result.Add(ModelToEntity(realization));
            }

            return result;
        }
    }
}
