using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
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

            if (entities.Count == 0)
                return new List<User>();

            List<User> users = await _userMapper.EntitiesToModelsAsync(entities);

            return users;
        }

        public async Task<User> CreateAsync(User user)
        {
            UserEntity entity = _userMapper.ModelToEntity(user);

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            int countRemovedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            return countRemovedUsers;
        }

        public async Task<(User User, ErrorCodes ErrorCode)> GetByIdAsync(Guid id)
        {
            UserEntity userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == id);

            if(userEntity == null)
                return (null, ErrorCodes.UserNotFound);

            User result = _userMapper.EntityToModel(userEntity);

            return (result, ErrorCodes.None);
        }

        public async Task<(User User, ErrorCodes ErrorCode)> GetByLoginAsync(string login)
        {
            UserEntity userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login);

            if(userEntity == null)
                return (null, ErrorCodes.UserNotFound);

            User result = _userMapper.EntityToModel(userEntity);

            return (result, ErrorCodes.None);
        }

        public async Task<(User User, ErrorCodes ErrorCode)> GetByEmailAsync(string email)
        {
            UserEntity userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if(userEntity == null)
                return (null, ErrorCodes.UserNotFound);

            User result = _userMapper.EntityToModel(userEntity);

            return (result, ErrorCodes.None);
        }

        public async Task<int> ChangeLoginAsync(Guid id, string newLogin)
        {
            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Login, newLogin));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }

        public async Task<int> ChangeEmailAsync(Guid id, string newEmail)
        {
            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Email, newEmail));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }

        public async Task<int> ChangePasswordAsync(Guid id, string newPassword)
        {
            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Password, newPassword));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }

        public async Task<int> AddMoneyToBalance(Guid id, decimal increaseSumm)
        {
            int updatedUsers = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.WalletBalance, p => p.WalletBalance + increaseSumm));

            await _dbContext.SaveChangesAsync();

            return updatedUsers;
        }
    }
}