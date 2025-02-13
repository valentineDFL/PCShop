using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Characteristic;
using Domain.Models;

namespace DataAccessLayer.Mapping
{
    internal class CharacteristicPatternMapper
    {
        public CharacteristicPattern EntityToModel(CharacteristicPatternEntity entity)
        {
            if (entity == null) 
                throw new ArgumentNullException($"Pattern entity is null {nameof(EntityToModel)}");

            return new CharacteristicPattern(entity.Id, entity.Name);
        }

        public CharacteristicPatternEntity ModelToEntity(CharacteristicPattern model)
        {
            if(model == null)
                throw new ArgumentNullException($"Pattern is null {nameof(ModelToEntity)}");

            return new CharacteristicPatternEntity() { Id = model.Id, Name = model.Name };
        }

        public List<CharacteristicPattern> EntitiesToModels(List<CharacteristicPatternEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException($"Pattern entities is null {nameof(EntitiesToModels)}");

            List<CharacteristicPattern> result = new List<CharacteristicPattern>();

            foreach(CharacteristicPatternEntity entity in entities)
            {
                result.Add(EntityToModel(entity));
            }

            return result;
        }

        public List<CharacteristicPatternEntity> ModelsToEntities(List<CharacteristicPattern> models)
        {
            if (models == null)
                throw new ArgumentNullException($"Patterns is null {nameof(ModelsToEntities)}");

            List<CharacteristicPatternEntity> result = new List<CharacteristicPatternEntity>();

            foreach(CharacteristicPattern model in models)
            {
                result.Add(ModelToEntity(model));
            }

            return result;
        }
    }
}