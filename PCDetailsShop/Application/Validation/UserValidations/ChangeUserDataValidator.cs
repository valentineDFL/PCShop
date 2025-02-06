using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Validation.UserValidations
{
    internal class ChangeUserDataValidator : IUserValidator
    {
        public BaseResult<User> ValidateOnNull(User user)
        {
            BaseResult<User> userResult = new BaseResult<User>();

            if(user == null)
            {
                userResult.ErrorCode = (int)ErrorCodes.UserNotFound;
                userResult.ErrorMessage = ErrorCodes.UserNotFound.ToString();

                return userResult;
            }

            userResult.Data = user;

            return userResult;
        }

        public CollectionResult<User> ValidateOnNull(List<User> users)
        {
            CollectionResult<User> usersResult = new CollectionResult<User>();

            if (users == null)
            {
                usersResult.ErrorCode = (int)ErrorCodes.UsersNotFound;
                usersResult.ErrorMessage = ErrorCodes.UsersNotFound.ToString();

                return usersResult;
            }

            usersResult.Data = users;
            usersResult.Count = users.Count;

            return usersResult;
        }

        public BaseResult<User> ValidateOnExists(User user, CreateUserDto dto)
        {
            if (user != null)
            {
                int errorCode = 0;

                if (user.Login == dto.Login)
                    errorCode = (int)ErrorCodes.UserWithTurnedLoginAlreadyExists;

                else if (user.Email == dto.Email)
                    errorCode = (int)ErrorCodes.UserWithTurnedEmailAlreadyExists;

                return new BaseResult<User>()
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ErrorCodes.UserAlreadyExists.ToString()
                };
            }

            return new BaseResult<User>();
        }

        public BaseResult<string> ValidateOnLoginExists(User user, string newLogin)
        {
            if (user.Login == newLogin)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.UserWithTurnedLoginAlreadyExists,
                    ErrorMessage = ErrorCodes.UserAlreadyExists.ToString()
                };
            }
            
            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnEmailExists(User user, string newEmail)
        {
            if (user != null)
            {
                if (user.Login == newEmail)
                {
                    return new BaseResult<string>()
                    {
                        ErrorCode = (int)ErrorCodes.UserWithTurnedEmailAlreadyExists,
                        ErrorMessage = ErrorCodes.UserAlreadyExists.ToString()
                    };
                }
            }

            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnLoginRepeat(string oldLogin, string newLogin)
        {
            if (oldLogin == newLogin)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.TheOldLoginDoesNotMatchTheLogin,
                    ErrorMessage = ErrorCodes.TheOldLoginDoesNotMatchTheLogin.ToString()
                };
            }

            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnEmailRepeat(string oldEmail, string newEmail)
        {
            if(oldEmail == newEmail)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.TheOldEmailDoesNotMatchThePassword,
                    ErrorMessage = ErrorCodes.TheOldEmailDoesNotMatchThePassword.ToString()
                };
            }

            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnPasswordRepeat(string userPassword, string oldPassword)
        {
            if (userPassword == oldPassword)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.TheOldPasswordDoesNotMatchThePassword,
                    ErrorMessage = ErrorCodes.TheOldPasswordDoesNotMatchThePassword.ToString(),
                };
            }

            return new BaseResult<string>();
        }

        public BaseResult<decimal> ValidateOnCredit(decimal creditToAmount)
        {
            if(creditToAmount < 0)
            {
                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero,
                    ErrorMessage = ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero.ToString()
                };
            }

            return new BaseResult<decimal>();
        }
    }
}
