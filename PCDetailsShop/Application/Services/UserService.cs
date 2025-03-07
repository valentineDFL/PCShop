using Domain.Dto.UserDtos;
using Domain.Interfaces.Services;
using Domain.Result;
using Domain.Interfaces.Repositories;
using Domain.Enums;
using Domain.Interfaces.Encrypt;
using Domain.Models;
using Domain.Dto.UserDto;
using Domain.Interfaces.Auth;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;

        private readonly IEncrypter _passwordEncrypter;

        private readonly IJwtProvider _jwtProvider;

        public UserService(IUserRepository userRepository,
                           ICartRepository cartRepository,
                           IEncrypter passwordEncrypter,
                           IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _passwordEncrypter = passwordEncrypter;
            _jwtProvider = jwtProvider;
        }

        public async Task<BaseResult<User>> GetByIdAsync(Guid id)
        {
            (User User, ErrorCodes errorCode) user = await _userRepository.GetByIdAsync(id);

            if(user.errorCode != ErrorCodes.None)
            {
                return new BaseResult<User>()
                {
                    ErrorCode = (int)user.errorCode,
                    ErrorMessage = user.errorCode.ToString(),
                };
            }

            return new BaseResult<User>() { Data = user.User };
        }

        public async Task<BaseResult<User>> GetByNameAsync(string name)
        {
            (User User, ErrorCodes errorCode) user = await _userRepository.GetByLoginAsync(name);

            if(user.errorCode != ErrorCodes.None)
            {
                return new BaseResult<User>()
                {
                    ErrorCode = (int)user.errorCode,
                    ErrorMessage = user.errorCode.ToString(),
                };
            }

            return new BaseResult<User>() { Data = user.User };
        }

        public async Task<CollectionResult<User>> GetAllAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();

            if (users.Count == 0)
            {
                return new CollectionResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.UsersNotFound,
                    ErrorMessage = ErrorCodes.UsersNotFound.ToString(),
                };
            }

            return new CollectionResult<User>() { Count = users.Count, Data = users };
        }

        public async Task<BaseResult<User>> CreateAsync(CreateUserDto dto)
        {
            (User User, ErrorCodes errorCode) userWithTurnedLogin = await _userRepository.GetByLoginAsync(dto.Login);
            (User User, ErrorCodes errorCode) userWithTurnedEmail = await _userRepository.GetByEmailAsync(dto.Email);

            if (userWithTurnedLogin.errorCode != ErrorCodes.UserNotFound)
            {
                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.UserWithTurnedLoginAlreadyExists,
                    ErrorMessage = ErrorCodes.UserWithTurnedLoginAlreadyExists.ToString(),
                };
            }

            if (userWithTurnedEmail.errorCode != ErrorCodes.UserNotFound)
            {
                return new BaseResult<User>()
                {
                    ErrorCode = (int)ErrorCodes.UserWithTurnedEmailAlreadyExists,
                    ErrorMessage = ErrorCodes.UserWithTurnedEmailAlreadyExists.ToString(),
                };
            }

            return await CreateUserAndCartAsync(dto);
        }

        private async Task<BaseResult<User>> CreateUserAndCartAsync(CreateUserDto dto)
        {
            Guid userId = Guid.NewGuid();
            Guid CartId = Guid.NewGuid();

            string password = _passwordEncrypter.Encrypt(dto.Password);
            User userForCreate = new User(userId, dto.Login, dto.Email, password, 0, dto.BirthDate, DateTime.UtcNow, null, CartId);

            User createdUser = await _userRepository.CreateAsync(userForCreate);

            Cart userCart = new Cart(CartId, 0, 0, createdUser, userId, new List<Product>());
            await _cartRepository.CreateAsync(userCart);

            return new BaseResult<User>() { Data = createdUser };
        }

        public async Task<BaseResult<string>> LoginAsync(LoginUserDto loginInfo)
        {
            var user = await _userRepository.GetByEmailAsync(loginInfo.Email);

            if(user.ErrorCode != ErrorCodes.None)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)user.ErrorCode,
                    ErrorMessage = user.ErrorCode.ToString()
                };
            }

            if(user.User.Password != _passwordEncrypter.Encrypt(loginInfo.Password))
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserPasswordDoesNotMatch,
                    ErrorMessage = ErrorCodes.UserPasswordDoesNotMatch.ToString(),
                };
            }

            var token = _jwtProvider.GenerateToken(user.User);

            return new BaseResult<string>() { Data = token };            
        }

        public async Task<BaseResult<Guid>> DeleteByIdAsync(Guid id)
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

        public async Task<BaseResult<string>> ChangeLoginAsync(Guid id, string newLogin)
        {
            (User User, ErrorCodes errorCode) userWithTurnedNewLogin = await _userRepository.GetByLoginAsync(newLogin);

            if(userWithTurnedNewLogin.errorCode != ErrorCodes.UserNotFound)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserWithTurnedLoginAlreadyExists,
                    ErrorMessage = ErrorCodes.UserWithTurnedLoginAlreadyExists.ToString(),
                };
            }

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

        public async Task<BaseResult<string>> ChangeEmailAsync(Guid id, string newEmail)
        {
            (User User, ErrorCodes errorCode) userWithTurnedNewEmail = await _userRepository.GetByEmailAsync(newEmail);

            if (userWithTurnedNewEmail.errorCode != ErrorCodes.UserNotFound)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserWithTurnedEmailAlreadyExists,
                    ErrorMessage = ErrorCodes.UserWithTurnedEmailAlreadyExists.ToString(),
                };
            }

            int updatedUsers = await _userRepository.ChangeEmailAsync(id, newEmail);

            if (updatedUsers == 0)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString()
                };
            }

            return new BaseResult<string>() { Data = newEmail };
        }

        public async Task<BaseResult<string>> ChangePasswordAsync(Guid id, string oldPassword, string newPassword)
        {
            (User User, ErrorCodes errorCode) user = await _userRepository.GetByIdAsync(id);

            if (user.errorCode != ErrorCodes.None)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)user.errorCode,
                    ErrorMessage = user.errorCode.ToString(),
                };
            }

            string userPassword = user.User.Password;

            oldPassword = _passwordEncrypter.Encrypt(oldPassword);
            newPassword = _passwordEncrypter.Encrypt(newPassword);

            if(userPassword != oldPassword)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.TheOldPasswordDoesNotMatchThePassword,
                    ErrorMessage = ErrorCodes.TheOldPasswordDoesNotMatchThePassword.ToString()
                };
            }

            await _userRepository.ChangePasswordAsync(id, newPassword);

            return new BaseResult<string>() { Data = "Password Changed" };
        }

        public async Task<BaseResult<decimal>> AddMoneyToBalanceAsync(Guid id, decimal increaseSumm)
        {
            if(increaseSumm <= 0)
            {
                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero,
                    ErrorMessage = ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero.ToString(),
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