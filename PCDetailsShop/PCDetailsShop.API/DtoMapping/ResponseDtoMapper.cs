﻿using Domain.Dto.UserDtos;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace PCDetailsShop.API.DtoMapping
{
    public class ResponseDtoMapper
    {
        public BaseResult<UserDto> FromModelToDto(User user)
        {
            return new BaseResult<UserDto>()
            {
                Data = FromUserToDto(user),
            };
        }

        private UserDto FromUserToDto(User user)
        {
            return new UserDto(user.Id, user.Login, user.Email, user.BirthDate, user.RegistrationDate);
        }

        public async Task<CollectionResult<UserDto>> FromModelsToDtoAsync(List<User> users)
        {
            List<UserDto> dtos = new List<UserDto>();
            
            await Task.Run(() => 
            {
                foreach (User user in users)
                {
                    dtos.Add(FromUserToDto(user));
                }
            });

            return new CollectionResult<UserDto>()
            {
                Count = dtos.Count,
                Data = dtos
            };
        }
    }
}
