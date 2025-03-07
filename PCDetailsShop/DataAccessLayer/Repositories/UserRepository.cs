using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly PcShopDbContext _dbContext;
        public UserRepository(PcShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllAsync()
        {
            List<User> entities = await _dbContext.Users.ToListAsync();

            if (entities.Count == 0)
                return new List<User>();

            return entities;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
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
            User userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == id);

            if(userEntity == null)
                return (null, ErrorCodes.UserNotFound);

            return (userEntity, ErrorCodes.None);
        }

        public async Task<(User User, ErrorCodes ErrorCode)> GetByLoginAsync(string login)
        {
            User userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login);

            if(userEntity == null)
                return (null, ErrorCodes.UserNotFound);

            return (userEntity, ErrorCodes.None);
        }

        public async Task<(User User, ErrorCodes ErrorCode)> GetByEmailAsync(string email)
        {
            User userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if(userEntity == null)
                return (null, ErrorCodes.UserNotFound);

            return (userEntity, ErrorCodes.None);
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

        public async Task<List<Domain.Enums.Permissions>> GetPermissionsAsync(Guid userId)
        {
            var roles = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Where(u => u.Id == userId)
                .Select(u => u.Roles)
                .ToListAsync();

            Console.WriteLine(roles.Count);

            return roles
                .SelectMany(r => r)
                .SelectMany(r => r.Permissions)
                .Select(p => (Permissions)p.Id)
                .ToList();
        }
    }
}