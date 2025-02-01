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
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Cart> _cartRepository;

        private readonly ILogger _logger;
        private readonly IUserValidator _userValidator;

        private readonly IEncrypter _passwordEncrypter;

        public UserService(IUserRepository userRepository, IRepository<Cart> cartRepository, ILogger logger, IUserValidator userValidator, IEncrypter passwordEncrypter)
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

        public async Task<BaseResult<User>> GetUserByNameAsync(string name)
        {
            try
            {
                BaseResult<User> user = await _userRepository.GetByLoginAsync(name);

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
                    BaseResult<User> existsValidationResult = _userValidator.ValidateOnExists(findedUser, dto);

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

        public async Task<BaseResult<string>> ChangeUserLoginAsync(Guid id, string newLogin)
        {
            try
            {
                BaseResult<User> user = await _userRepository.GetByIdAsync(id);

                if (user.IsSuccess)
                {
                    return new BaseResult<string>()
                    {
                        ErrorCode = user.ErrorCode,
                        ErrorMessage = user.ErrorMessage,
                    };
                }

                BaseResult<string> loginRepeatValidationResult = _userValidator.ValidateOnLoginRepeat(user.Data.Login, newLogin);
                
                if(!loginRepeatValidationResult.IsSuccess)
                    return loginRepeatValidationResult;

                BaseResult<User> userWithTurnedLogin = await _userRepository.GetByLoginAsync(newLogin);

                BaseResult<string> loginExistsValidationResult = _userValidator.ValidateOnLoginExists(userWithTurnedLogin.Data, newLogin);

                if(!loginExistsValidationResult.IsSuccess)
                    return loginExistsValidationResult;

                return await _userRepository.ChangeLoginAsync(id, newLogin);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<string>> ChangeUserEmailAsync(Guid id, string newEmail)
        {
            try
            {
                BaseResult<User> user = await _userRepository.GetByIdAsync(id);

                if (!user.IsSuccess)
                {
                    return new BaseResult<string>()
                    {
                        ErrorCode = user.ErrorCode,
                        ErrorMessage = user.ErrorMessage,
                    };
                }

                BaseResult<string> emailRepeatValidationResult = _userValidator.ValidateOnEmailRepeat(user.Data.Email, newEmail);

                if (!emailRepeatValidationResult.IsSuccess)
                    return emailRepeatValidationResult;

                CollectionResult<User> users = await _userRepository.GetAllAsync();

                User userWithTurnedEmail = users.Data.FirstOrDefault(u => u.Email == newEmail);

                BaseResult<string> emailExistsValidationResult = _userValidator.ValidateOnEmailExists(userWithTurnedEmail, newEmail);

                if(!emailExistsValidationResult.IsSuccess)
                    return emailExistsValidationResult;

                return await _userRepository.ChangeEmailAsync(id, newEmail);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<string>> ChangeUserPasswordAsync(Guid id, string oldPassword, string newPassword)
        {
            try
            {
                BaseResult<User> user = await _userRepository.GetByIdAsync(id);

                if (!user.IsSuccess)
                {
                    return new BaseResult<string>()
                    {
                        ErrorCode = user.ErrorCode,
                        ErrorMessage = user.ErrorMessage,
                    };
                }

                string userPassword = user.Data.Password;

                oldPassword = _passwordEncrypter.Encrypt(oldPassword);
                newPassword = _passwordEncrypter.Encrypt(newPassword);

                BaseResult<string> passwordRepeatValidationResult = _userValidator.ValidateOnPasswordRepeat(userPassword, newPassword);

                if (!passwordRepeatValidationResult.IsSuccess)
                    return passwordRepeatValidationResult;

                return await _userRepository.ChangePasswordAsync(id, newPassword);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<decimal>> AddMoneyToBalanceAsync(Guid id, decimal increaseSumm)
        {
            try
            {
                BaseResult<decimal> creditValidationResult = _userValidator.ValidateOnCredit(increaseSumm);

                if (!creditValidationResult.IsSuccess)
                    return creditValidationResult;

                BaseResult<User> user = await _userRepository.GetByIdAsync(id);

                if (!user.IsSuccess)
                {
                    return new BaseResult<decimal>()
                    {
                        ErrorCode = user.ErrorCode,
                        ErrorMessage = user.ErrorMessage,
                    };
                }

                return await _userRepository.AddMoneyToBalance(id, increaseSumm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }
    }
}