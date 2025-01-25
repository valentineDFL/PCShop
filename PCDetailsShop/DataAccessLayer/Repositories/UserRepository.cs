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

        public async Task<ResultCollection<List<User>>> GetAllAsync()
        {
            List<UserEntity> entities = await _dbContext.Users.ToListAsync();
            List<User> users = await _userMapper.EntitiesToModelsAsync(entities);

            if(users.Count == 0)
            {
                return new ResultCollection<List<User>>()
                {
                    Count = 0,
                    ErrorCode = (int)ErrorCodes.UsersNotFound,
                    ErrorMessage = ErrorCodes.UsersNotFound.ToString(),
                };
            }

            BaseResult<List<User>> baseResult = new BaseResult<List<User>>() { Data = users };

            return new ResultCollection<List<User>>()
            {
                Count = users.Count,
                Data = (IEnumerable<List<User>>)baseResult,
            };
        }

        public async Task<BaseResult<User>> CreateAsync(User user)
        {
            if(user == null)
                throw new ArgumentNullException($"User Null {nameof(CreateAsync)}");

            UserEntity entity = _userMapper.ModelToEntity(user);

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new BaseResult<User>()
            {
                Data = user,
            };
        }

        public async Task<BaseResult<Guid>> DeleteAsync(Guid id)
        {
            if(id == Guid.Empty) 
                throw new ArgumentException("User id is Empty");

            int countUsersToRemove = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            if(countUsersToRemove == 0)
            {
                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            return new BaseResult<Guid>()
            {
                Data = id,
            };
        }

        public async Task<BaseResult<User>> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException($"User Null {nameof(UpdateAsync)}");

            UserEntity entity = _userMapper.ModelToEntity(user);

            int updatedPropertyCount = await _dbContext.Users
                .Where(p => p.Id == entity.Id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Login, entity.Login)
                .SetProperty(p => p.Email, entity.Email)
                .SetProperty(p => p.Password, entity.Password)
                .SetProperty(p => p.WalletBalance, entity.WalletBalance)
                .SetProperty(p => p.BirthDate, entity.BirthDate));

            await _dbContext.SaveChangesAsync();

            if(updatedPropertyCount == 0)
            {
                return new BaseResult<User>()
                {
                    
                };
            }

            return new BaseResult<User>()
            { 
                Data = user
            };
        }
    }
}