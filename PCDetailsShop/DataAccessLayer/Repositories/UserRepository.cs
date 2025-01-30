using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    internal class UserRepository : IRepository<User>
    {
        private readonly PcShopDbContext _dbContext;
        private readonly UserMapper _userMapper;

        public UserRepository(PcShopDbContext dbContext, UserMapper userMapper)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
        }

        public async Task<CollectionResult<User>> GetAllAsync()
        {
            List<UserEntity> entities = await _dbContext.Users.ToListAsync();

            if (entities.Count == 0)
            {
                return new CollectionResult<User>()
                {
                    Count = 0,
                    ErrorCode = (int)ErrorCodes.UsersNotFound,
                    ErrorMessage = ErrorCodes.UsersNotFound.ToString(),
                };
            }

            List<User> users = await _userMapper.EntitiesToModelsAsync(entities);

            return new CollectionResult<User>()
            {
                Count = users.Count,
                Data = users,
            };
        }

        public async Task<BaseResult<User>> CreateAsync(User user)
        {
            if(user == null)
                throw new ArgumentNullException($"User Null {nameof(CreateAsync)}");

            UserEntity entity = _userMapper.ModelToEntity(user);

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new BaseResult<User>() { Data = user };
        }

        public async Task<BaseResult<Guid>> DeleteAsync(Guid id)
        {
            if(id == Guid.Empty) 
                throw new ArgumentException($"User id is Empty {nameof(DeleteAsync)}");

            int countRemovedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            if(countRemovedUsers == 0)
            {
                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<Guid>() { Data = id };
        }

        public async Task<BaseResult<User>> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException($"User Null {nameof(UpdateAsync)}");

            UserEntity entity = _userMapper.ModelToEntity(user);

            int updatedUsersCount = await _dbContext.Users
                .Where(p => p.Id == entity.Id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Login, entity.Login)
                .SetProperty(p => p.Email, entity.Email)
                .SetProperty(p => p.Password, entity.Password)
                .SetProperty(p => p.WalletBalance, p => p.WalletBalance + entity.WalletBalance)
                .SetProperty(p => p.BirthDate, entity.BirthDate));

            if(updatedUsersCount == 0)
            {
                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<User>() { Data = user };
        }

        public async Task<BaseResult<User>> GetByIdAsync(Guid id)
        {
            UserEntity user = await _dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == id);

            if(user == null)
            {
                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            User result = await _userMapper.EntityToModelAsync(user);

            return new BaseResult<User>() { Data = result };
        }
    }
}