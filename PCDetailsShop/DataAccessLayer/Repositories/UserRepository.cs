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

        public async Task<List<User>> GetAllAsync()
        {
            List<UserEntity> entities = await _dbContext.Users.ToListAsync();

            List<User> users = await _userMapper.EntitiesToModelsAsync(entities);

            return users;
        }

        public async Task<User> CreateAsync(User user)
        {
            if(user == null)
                throw new ArgumentNullException($"User Null {nameof(CreateAsync)}");

            UserEntity entity = _userMapper.ModelToEntity(user);

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException($"User id is Empty {nameof(DeleteAsync)}");

            int countRemovedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            return countRemovedUsers;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            UserEntity user = await _dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == id);

            User result = _userMapper.EntityToModel(user);

            return result;
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            if(string.IsNullOrEmpty(login))
                throw new ArgumentNullException($"User login Null or Empty {nameof(ChangeLoginAsync)}");

            UserEntity userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login);

            User user = _userMapper.EntityToModel(userEntity);

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException($"User Email Null or Empty {nameof(ChangeLoginAsync)}");

            UserEntity userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            User user = _userMapper.EntityToModel(userEntity);

            return user;
        }

        public async Task<int> ChangeLoginAsync(Guid id, string newLogin)
        {
            if (string.IsNullOrEmpty(newLogin))
                throw new ArgumentNullException($"User new login Null or Empty {nameof(ChangeLoginAsync)}");

            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Login, newLogin));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }

        public async Task<int> ChangeEmailAsync(Guid id, string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
                throw new ArgumentNullException($"User new email Null or Empty {nameof(ChangeEmailAsync)}");

            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Email, newEmail));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }

        public async Task<int> ChangePasswordAsync(Guid id, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException($"User new password Null or Empty {nameof(ChangePasswordAsync)}");

            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Password, newPassword));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }

        public async Task<int> AddMoneyToBalance(Guid id, decimal increaseSumm)
        {
            if (increaseSumm <= 0)
                throw new ArgumentNullException($"Increase Summ must be high than 0 {nameof(AddMoneyToBalance)}");

            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.WalletBalance, p => p.WalletBalance + increaseSumm));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }
    }
}