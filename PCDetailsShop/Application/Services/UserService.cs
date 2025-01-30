using Domain.Dto.UserDtos;
using Domain.Interfaces.Services;
using Domain.Result;
using FluentValidation;
using Serilog;
using Domain.Interfaces.Repositories;
using Domain.Enums;
using Domain.Interfaces.Validators;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Encrypt;
using Domain.Models;
using DataAccessLayer.Entities;
using Domain.Interfaces.MappingW;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Cart> _cartRepository;

        private readonly ILogger _logger;
        private readonly IUserValidator _userValidator;

        private readonly IEncrypter _passwordEncrypter;

        public UserService(IRepository<User> userRepository, IRepository<Cart> cartRepository, ILogger logger, IUserValidator userValidator, IEncrypter passwordEncrypter)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _logger = logger;
            _userValidator = userValidator;
            _passwordEncrypter = passwordEncrypter;
        }

        public async Task<BaseResult<User>> GetUserByIdAsync(Guid id)
        {
            try
            {
                BaseResult<User> user = await _userRepository.GetByIdAsync(id);

                return user;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<CollectionResult<User>> GetAllUsersAsync()
        {
            try
            {
                CollectionResult<User> users = await _userRepository.GetAllAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new CollectionResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<User>> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                CollectionResult<User> users = await _userRepository.GetAllAsync();

                if (users.IsSuccess)
                {
                    User findedUser = users.Data.FirstOrDefault(u => u.Login == dto.Login || u.Email == dto.Email);
                    BaseResult<User> existsValidationResult = _userValidator.ExistsValidation(findedUser, dto);

                    if (!existsValidationResult.IsSuccess)
                    return existsValidationResult;
                }

                return await CreateUserAndCart(dto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        private async Task<BaseResult<User>> CreateUserAndCart(CreateUserDto dto)
        {
            Guid userId = Guid.NewGuid();
            Guid CartId = Guid.NewGuid();


            string password = _passwordEncrypter.Encrypt(dto.Password);
            User user = new User(userId, dto.Login, dto.Email, password, 0, dto.BirthDate, DateTime.UtcNow, null, CartId);
            BaseResult<User> result = await _userRepository.CreateAsync(user);

            Cart userCart = new Cart(CartId, 0, 0, null, userId, new List<Product>());
            await _cartRepository.CreateAsync(userCart);

            return result;
        }

        public async Task<BaseResult<Guid>> DeleteUserByIdAsync(Guid id)
        {
            try
            {
                BaseResult<Guid> deletedUser = await _userRepository.DeleteAsync(id);

                return deletedUser;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<User>> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                if(dto.summForAdd < 0)
                {
                    return new BaseResult<User>()
                    {
                        ErrorCode = (int)ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero,
                        ErrorMessage = ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero.ToString(),
                    };
                }

                BaseResult<User> userForUpdate = await _userRepository.GetByIdAsync(dto.Id);

                if (!userForUpdate.IsSuccess)
                    return userForUpdate;

                User user = _userRepository.GetAllAsync()
                    .Result
                    .Data
                    .FirstOrDefault(u => u.Login == dto.Login || u.Email == dto.Email);

                BaseResult<User> existValidateResult = _userValidator.ExistsValidation(user, dto);

                if(!existValidateResult.IsSuccess)
                    return existValidateResult;

                string oldPassword = _passwordEncrypter.Encrypt(dto.OldPassword);
                BaseResult<User> passwordValidationResult = _userValidator.PasswordValidation(userForUpdate.Data.Password, oldPassword);

                if (!passwordValidationResult.IsSuccess)
                    return passwordValidationResult;

                return await UpdateUser(dto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
        }

        private async Task<BaseResult<User>> UpdateUser(UpdateUserDto dto)
        {
            string newPassword = _passwordEncrypter.Encrypt(dto.NewPassword);

            User updateData = new User(dto.Id, dto.Login, dto.Email, newPassword, dto.summForAdd, DateTime.MinValue, DateTime.MinValue, null, Guid.Empty);

            BaseResult<User> updateResult = await _userRepository.UpdateAsync(updateData);

            return updateResult;
        }
    }
}