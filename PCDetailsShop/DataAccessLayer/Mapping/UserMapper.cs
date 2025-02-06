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
        public UserEntity ModelToEntity(User user) // modelToEntityInclude / modelToEntity
        {
            UserEntity userEntity = new UserEntity()
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

            return userEntity;
        }

        public User EntityToModel(UserEntity userEntity)
        {
            if (userEntity == null)
                return null;

            User user = new User
                    (
                        id: userEntity.Id,
                        login: userEntity.Login,
                        email: userEntity.Email,
                        password: userEntity.Password,
                        walletBalance: userEntity.WalletBalance,
                        birthDate: userEntity.BirthDate,
                        registrationDate: userEntity.RegistrationDate,
                        cart: null,
                        cartId: userEntity.CartId
                    );

            return user;
        }

        public async Task<List<User>> EntitiesToModelsAsync(List<UserEntity> userEntities)
        {
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