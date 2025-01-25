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

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        //private readonly IRepository<Cart> _cartRepository;

        private readonly ILogger _logger;
        private readonly IUserValidator _userValidator;

        private readonly IEncrypter _passwordEncrypter;

        public UserService(IRepository<User> userRepository, ILogger logger, IUserValidator userValidator, IEncrypter passwordEncrypter)
        {
            _userRepository = userRepository;
            //_cartRepository = cartRepository;
            _logger = logger;
            _userValidator = userValidator;
            _passwordEncrypter = passwordEncrypter;
        }

        public async Task<BaseResult<UserDto>> GetUserByIdAsync(Guid id)
        {
            try
            {
                ResultCollection<List<User>> users = await _userRepository.GetAllAsync();

                List<User> userssssss = (List<User>)users.Data;

                User user = userssssss.Where(u => u.Id == id)
                    .FirstOrDefault();
                    

                BaseResult validationResult = _userValidator.ValidateOnNull(user);

                if (!validationResult.IsSuccess)
                {
                    return new BaseResult<UserDto>()
                    {
                        ErrorCode = validationResult.ErrorCode,
                        ErrorMessage = validationResult.ErrorMessage,
                    };
                }

                UserDto result = new UserDto
                   (user.Id,
                    user.Login,
                    user.Email,
                    user.BirthDate,
                    user.RegistrationDate);

                return new BaseResult<UserDto>()
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
        }

        public async Task<BaseResult<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                //List<User> users = await _userRepository.GetAllAsync();

                //User user = users.Where(u => u.Login == dto.Login || u.Email == dto.Email)
                //    .FirstOrDefault();

                //BaseResult validateResult = _userValidator.ExistsValidation(user, dto);

                //if (!validateResult.IsSuccess)
                //{
                //    return new BaseResult<UserDto>()
                //    {
                //        ErrorCode = validateResult.ErrorCode,
                //        ErrorMessage = validateResult.ErrorMessage,
                //    };
                //}

                Guid userId = Guid.NewGuid();
                Guid cartId = Guid.NewGuid();

                Cart cart = new Cart
                        (
                             id: cartId,
                             cartTotalPrice: 0,
                             cartTotalWeight: 0,
                             user: null,
                             userId: userId,
                             products: new List<Product>()
                        );

                User user = new User
                        (
                            id: userId,
                            login: dto.Login,
                            email: dto.Email,
                            password: _passwordEncrypter.Encrypt(dto.Password),
                            walletBalance: 0,
                            birthDate: dto.BirthDate,
                            registrationDate: DateTime.UtcNow,
                            cart: cart,
                            cartId: cartId
                        );

                await _userRepository.CreateAsync(user);
                //await _cartRepository.CreateAsync(cart);

                return new BaseResult<UserDto>()
                {
                    Data = new UserDto
                    (
                        user.Id,
                        user.Login,
                        user.Email,
                        user.BirthDate,
                        user.RegistrationDate
                    )
                };
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
        }

        public async Task<BaseResult<UserDto>> DeleteUserByIdAsync(Guid id)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.Error (ex, ex.Message);

                return new BaseResult<UserDto>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }


            throw new NotImplementedException();
        }

        public Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UserDto>> IncreaseUserWalletBalanceAsync(Guid id, decimal increaseSumm)
        {
            throw new NotImplementedException();
        }
    }
}