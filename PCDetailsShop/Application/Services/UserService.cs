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
        private readonly ICartRepository _cartRepository;

        private readonly IUserValidator _userValidator;

        private readonly IEncrypter _passwordEncrypter;

        public UserService(IUserRepository userRepository, ICartRepository cartRepository, IUserValidator userValidator, IEncrypter passwordEncrypter)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _userValidator = userValidator;
            _passwordEncrypter = passwordEncrypter;
        }

        public async Task<BaseResult<User>> GetUserByIdAsync(Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);

            BaseResult<User> userValidationResult = new BaseResult<User>();

            return userValidationResult;
        }

        public async Task<BaseResult<User>> GetUserByNameAsync(string name)
        {
            User user = await _userRepository.GetByLoginAsync(name);

            BaseResult<User> userValidationResult = new BaseResult<User>();

            return userValidationResult;
        }

        public async Task<CollectionResult<User>> GetAllUsersAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();

            CollectionResult<User> usersValidationResult = _userValidator.ValidateOnNull(users);

            return usersValidationResult;
        }

        public async Task<BaseResult<User>> CreateUserAsync(CreateUserDto dto)
        {
            User userWithTurnedLogin = await _userRepository.GetByLoginAsync(dto.Login);
            User userWithTurnedEmail = await _userRepository.GetByLoginAsync(dto.Email);

            if(userWithTurnedLogin != null)
            {
                BaseResult<User> existsLoginValidationResult = _userValidator.ValidateOnExists(userWithTurnedLogin, dto);

                if (!existsLoginValidationResult.IsSuccess)
                    return existsLoginValidationResult;
            }

            if(userWithTurnedLogin != null)
            {
                BaseResult<User> existsEmailValidationResult = _userValidator.ValidateOnExists(userWithTurnedEmail, dto);

                if(!existsEmailValidationResult.IsSuccess)
                return existsEmailValidationResult;
            }

            return await CreateUserAndCart(dto);
        }

        private async Task<BaseResult<User>> CreateUserAndCart(CreateUserDto dto)
        {
            Guid userId = Guid.NewGuid();
            Guid CartId = Guid.NewGuid();

            string password = _passwordEncrypter.Encrypt(dto.Password);
            User userForCreated = new User(userId, dto.Login, dto.Email, password, 0, dto.BirthDate, DateTime.UtcNow, null, CartId);
            User createdUser = await _userRepository.CreateAsync(userForCreated);

            Cart userCart = new Cart(CartId, 0, 0, null, userId, new List<Product>());
            await _cartRepository.CreateAsync(userCart);

            return new BaseResult<User>() { Data = createdUser };
        }

        public async Task<BaseResult<Guid>> DeleteUserByIdAsync(Guid id)
        {
            int deletedUser = await _userRepository.DeleteAsync(id);

            if(deletedUser == 0)
            {
                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            return new BaseResult<Guid>() { Data = id };
        }

        public async Task<BaseResult<string>> ChangeUserLoginAsync(Guid id, string newLogin)
        {
            User user = await _userRepository.GetByIdAsync(id);

            BaseResult<User> userFoundValidationResult = _userValidator.ValidateOnNull(user);

            if (!userFoundValidationResult.IsSuccess)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = userFoundValidationResult.ErrorCode,
                    ErrorMessage = userFoundValidationResult.ErrorMessage,
                };
            }

            BaseResult<string> changeLoginValidatioResult = await ChangeLoginValidation(user.Login, newLogin);

            if (!changeLoginValidatioResult.IsSuccess)
                return changeLoginValidatioResult;

            int updatedUsers = await _userRepository.ChangeLoginAsync(id, newLogin);

            if(updatedUsers == 0)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString()
                };
            }

            return new BaseResult<string>() { Data = newLogin };
        }

        private async Task<BaseResult<string>> ChangeLoginValidation(string oldLogin, string newLogin)
        {
            BaseResult<string> loginRepeatValidationResult = _userValidator.ValidateOnLoginRepeat(oldLogin, newLogin);

            if (!loginRepeatValidationResult.IsSuccess)
                return loginRepeatValidationResult;

            User userWithTurnedLogin = await _userRepository.GetByLoginAsync(newLogin);

            BaseResult<string> loginExistsValidationResult = _userValidator.ValidateOnLoginExists(userWithTurnedLogin, newLogin);

            if (!loginExistsValidationResult.IsSuccess)
                return loginExistsValidationResult;

            return new BaseResult<string>();
        }

        public async Task<BaseResult<string>> ChangeUserEmailAsync(Guid id, string newEmail)
        {
            User user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            BaseResult<string> emailChangeValidation = await ChangeEmailValidation(user.Email, newEmail);

            int updatedUsers = await _userRepository.ChangeEmailAsync(id, newEmail);

            if(updatedUsers == 0)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            return new BaseResult<string>() { Data = newEmail };
        }

        private async Task<BaseResult<string>> ChangeEmailValidation(string oldEmail, string newEmail)
        {
            BaseResult<string> emailRepeatValidationResult = _userValidator.ValidateOnEmailRepeat(oldEmail, newEmail);

            if (!emailRepeatValidationResult.IsSuccess)
                return emailRepeatValidationResult;

            User userWithTurnedEmail = await _userRepository.GetByEmailAsync(newEmail);

            BaseResult<string> emailExistsValidationResult = _userValidator.ValidateOnEmailExists(userWithTurnedEmail, newEmail);

            if (!emailExistsValidationResult.IsSuccess)
                return emailExistsValidationResult;

            return new BaseResult<string>();
        }

        public async Task<BaseResult<string>> ChangeUserPasswordAsync(Guid id, string oldPassword, string newPassword)
        {
            User user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            string userPassword = user.Password;

            oldPassword = _passwordEncrypter.Encrypt(oldPassword);
            newPassword = _passwordEncrypter.Encrypt(newPassword);

            BaseResult<string> passwordRepeatValidationResult = _userValidator.ValidateOnPasswordRepeat(userPassword, newPassword);

            if (!passwordRepeatValidationResult.IsSuccess)
                return passwordRepeatValidationResult;

            int updatedUsers = await _userRepository.ChangePasswordAsync(id, newPassword);

            if(updatedUsers == 0)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            return new BaseResult<string>() { Data = "Password Changed" };
        }

        public async Task<BaseResult<decimal>> AddMoneyToBalanceAsync(Guid id, decimal increaseSumm)
        {
            BaseResult<decimal> creditValidationResult = _userValidator.ValidateOnCredit(increaseSumm);

            if (!creditValidationResult.IsSuccess)
                return creditValidationResult;

            User user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            int updatedUser = await _userRepository.AddMoneyToBalance(id, increaseSumm);

            if(updatedUser == 0)
            {
                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            return new BaseResult<decimal>() { Data = increaseSumm };
        }
    }
}