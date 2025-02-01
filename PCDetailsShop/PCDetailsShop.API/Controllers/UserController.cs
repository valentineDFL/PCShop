using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Interfaces.MappingW;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PCDetailsShop.API.DtoMapping;

namespace PCDetailsShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IBaseMapper<User, UserDto> _userDtoMapper;

        public UserController(IUserService userService, IValidator<CreateUserDto> createUserValidator,
            IBaseMapper<User, UserDto> userDtoMapper)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _userDtoMapper = userDtoMapper;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<BaseResult<UserDto>>> CreateUserAsync(CreateUserDto dto)
        {
            ValidationResult validationResult = await _createUserValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            BaseResult<User> response = await _userService.CreateUserAsync(dto);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDto(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<BaseResult<UserDto>>> DeleteUserByIdAsync(Guid id)
        {
            BaseResult<Guid> response = await _userService.DeleteUserByIdAsync(id);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<BaseResult<UserDto>>> GetUserById(Guid id)
        {
            BaseResult<User> response = await _userService.GetUserByIdAsync(id);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDto(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<CollectionResult<UserDto>>> GetAllUsersAsync()
        {
            CollectionResult<User> response = await _userService.GetAllUsersAsync();

            if (response.IsSuccess)
            {
                CollectionResult<UserDto> result = await _userDtoMapper.FromModelsToDtosAsync((List<User>)response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpGet("GetUserByName")]
        public async Task<ActionResult<CollectionResult<UserDto>>> GetUserByNameAsync(string name)
        {
            BaseResult<User> response = await _userService.GetUserByNameAsync(name);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDto(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpPut("ChangeUserLoginById")]
        public async Task<ActionResult<BaseResult<string>>> ChangeUserLoginByIdAsync(Guid userId, string newLogin)
        {
            if (!IllegalSymbols.NotContainsIllegalCharacter(newLogin))
            {
                BaseResult<string> newLoginErrorResult = new BaseResult<string>
                {
                    ErrorCode = (int)ErrorCodes.TurnedLoginContainIllegalCharacters,
                    ErrorMessage = ErrorCodes.TurnedLoginContainIllegalCharacters.ToString()
                };

                return BadRequest(newLoginErrorResult);
            }

            BaseResult<string> response = await _userService.ChangeUserLoginAsync(userId, newLogin);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("ChangeUserEmailById")]
        public async Task<ActionResult<BaseResult<string>>> ChangeUserEmailByIdAsync(Guid userId, string newEmail)
        {
            if (!IllegalSymbols.NotContainsIllegalCharacter(newEmail))
            {
                BaseResult<string> newEmailErrorResult = new BaseResult<string>
                {
                    ErrorCode = (int)ErrorCodes.TurnedEmailContainIllegalCharacters,
                    ErrorMessage = ErrorCodes.TurnedEmailContainIllegalCharacters.ToString()
                };

                return BadRequest(newEmailErrorResult);
            }

            BaseResult<string> response = await _userService.ChangeUserEmailAsync(userId, newEmail);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("ChangeUserPasswordById")]
        public async Task<ActionResult<BaseResult<string>>> ChangeUserPasswordByIdAsync(Guid userId, string oldPassword, string newPassword)
        {
            if (!IllegalSymbols.NotContainsIllegalCharacter(oldPassword))
            {
                BaseResult<string> oldPasswordErrorResult = new BaseResult<string>
                {
                    ErrorCode = (int)ErrorCodes.TurnedOldPasswordContainIllegalCharacters,
                    ErrorMessage = ErrorCodes.TurnedOldPasswordContainIllegalCharacters.ToString()
                };

                return BadRequest(oldPasswordErrorResult);
            }
            else if (!IllegalSymbols.NotContainsIllegalCharacter(newPassword))
            {
                BaseResult<string> newPasswordErrorResult = new BaseResult<string>
                {
                    ErrorCode = (int)ErrorCodes.TurnedOldPasswordContainIllegalCharacters,
                    ErrorMessage = ErrorCodes.TurnedOldPasswordContainIllegalCharacters.ToString()
                };

                return BadRequest(newPasswordErrorResult);
            }

            BaseResult<string> response = await _userService.ChangeUserPasswordAsync(userId, oldPassword, newPassword);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("IncreaseWalletBalance")]
        public async Task<ActionResult<BaseResult<decimal>>> IncreaseWalletBalance(Guid userId, decimal increaseSumm)
        {
            if(!decimal.TryParse(increaseSumm.ToString(), out decimal res))
            {
                BaseResult<decimal> increaseWalletBalanceErrorResult = new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.TheAmountToBeCreditedMustBeANumber,
                    ErrorMessage = ErrorCodes.TheAmountToBeCreditedMustBeANumber.ToString()
                };

                return BadRequest(increaseWalletBalanceErrorResult);
            }


            BaseResult<decimal> response = await _userService.AddMoneyToBalanceAsync(userId, increaseSumm);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}