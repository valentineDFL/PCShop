using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Domain.Models;

namespace DataAccessLayer.Mapping
{
    internal class UserMapper
    {
        public UserEntity ModelToEntity(User user)
        {
            if(user == null)
                throw new ArgumentNullException($"User is null {nameof(ModelToEntity)}");

            return new UserEntity()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                WalletBalance = user.WalletBalance,
                BirthDate = user.BirthDate,
                RegistrationDate = user.RegistrationDate,
                Cart = null,
                CartId = user.CartId,
            };
        }

        public User EntityToModel(UserEntity userEntity)
        {
            if (userEntity == null)
                throw new ArgumentNullException($"User entity is null {nameof(EntityToModel)}");

            return new User
                    (
                        userEntity.Id,
                        userEntity.Login,
                        userEntity.Email,
                        userEntity.Password,
                        userEntity.WalletBalance,
                        userEntity.BirthDate,
                        userEntity.RegistrationDate,
                        null,
                        userEntity.CartId
                    );
        }

        public async Task<List<User>> EntitiesToModelsAsync(List<UserEntity> userEntities)
        {
            if (userEntities == null)   
                throw new ArgumentNullException($"Entities is null {nameof(EntitiesToModelsAsync)}");

            List<User> users = new List<User>();

            await Task.Run(() =>
            {
                foreach (UserEntity entity in userEntities)
                {
                    users.Add(EntityToModel(entity));
                }            
            });

            return users;
        }
    }
}