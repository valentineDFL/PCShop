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
    internal class UserRepository : IUserRepository
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

            User result = _userMapper.EntityToModel(user);

            return new BaseResult<User>() { Data = result };
        }

        public async Task<BaseResult<User>> GetByLoginAsync(string login)
        {
            if(string.IsNullOrEmpty(login))
                throw new ArgumentNullException($"User login Null or Empty {nameof(ChangeLoginAsync)}");

            UserEntity userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login);

            if(userEntity == null)
            {
                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            User user = _userMapper.EntityToModel(userEntity);

            return new BaseResult<User>() { Data = user };
        }

        public async Task<BaseResult<string>> ChangeLoginAsync(Guid id, string newLogin)
        {
            if (string.IsNullOrEmpty(newLogin))
                throw new ArgumentNullException($"User new login Null or Empty {nameof(ChangeLoginAsync)}");

            int updatedProperties = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Login, newLogin));

            if(updatedProperties == 0)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<string>() { Data = newLogin };
        }

        public async Task<BaseResult<string>> ChangeEmailAsync(Guid id, string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
                throw new ArgumentNullException($"User new email Null or Empty {nameof(ChangeEmailAsync)}");

            int updatedProperties = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Email, newEmail));

            if (updatedProperties == 0)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<string>() { Data = newEmail };
        }

        public async Task<BaseResult<string>> ChangePasswordAsync(Guid id, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException($"User new password Null or Empty {nameof(ChangePasswordAsync)}");

            int updatedProperties = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Password, newPassword));

            if (updatedProperties == 0)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<string>() { Data = newPassword };
        }

        public async Task<BaseResult<decimal>> AddMoneyToBalance(Guid id, decimal increaseSumm)
        {
            if (increaseSumm <= 0)
                throw new ArgumentNullException($"Increase Summ must be high than 0 {nameof(AddMoneyToBalance)}");

            int updatedProperties = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.WalletBalance, p => p.WalletBalance + increaseSumm));

            if (updatedProperties == 0)
            {
                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<decimal>() { Data = increaseSumm };
        }
    }
}