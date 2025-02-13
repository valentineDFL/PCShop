using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Interfaces.Mapping;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace PCDetailsShop.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IDtoMapper<User, UserDto> _userDtoMapper;

        public UsersController(IUserService userService, IValidator<CreateUserDto> createUserValidator,
            IDtoMapper<User, UserDto> userDtoMapper)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _userDtoMapper = userDtoMapper;
        }

        [HttpPost("")]
        public async Task<ActionResult<BaseResult<UserDto>>> CreateUserAsync(CreateUserDto dto)
        {
            ValidationResult validationResult = await _createUserValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            BaseResult<User> response = await _userService.CreateUserAsync(dto);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDtoResult(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<BaseResult<Guid>>> DeleteUserByIdAsync(Guid id)
        {
            BaseResult<Guid> response = await _userService.DeleteUserByIdAsync(id);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BaseResult<UserDto>>> GetUserById(Guid id)
        {
            BaseResult<User> response = await _userService.GetUserByIdAsync(id);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDtoResult(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpGet]
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

        [HttpGet("{name}")]
        public async Task<ActionResult<CollectionResult<UserDto>>> GetUserByNameAsync(string name)
        {
            BaseResult<User> response = await _userService.GetUserByNameAsync(name);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDtoResult(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpPut("{id:guid}/change-login")]
        public async Task<ActionResult<BaseResult<string>>> ChangeUserLoginByIdAsync(Guid id, string newLogin)
        {
            BaseResult<string> response = await _userService.ChangeUserLoginAsync(id, newLogin);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{id:guid}/change-email")]
        public async Task<ActionResult<BaseResult<string>>> ChangeUserEmailByIdAsync(Guid id, string newEmail)
        {
            BaseResult<string> response = await _userService.ChangeUserEmailAsync(id, newEmail);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{id:guid}/change-password")]
        public async Task<ActionResult<BaseResult<string>>> ChangeUserPasswordByIdAsync(Guid id, string oldPassword, string newPassword)
        {
            BaseResult<string> response = await _userService.ChangeUserPasswordAsync(id, oldPassword, newPassword);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{id:guid}/balance/sum")]
        public async Task<ActionResult<BaseResult<decimal>>> IncreaseWalletBalance(Guid id, decimal sum)
        {
            if(!decimal.TryParse(sum.ToString(), out decimal res))
            {
                BaseResult<decimal> increaseWalletBalanceErrorResult = new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.TheAmountToBeCreditedMustBeANumber,
                    ErrorMessage = ErrorCodes.TheAmountToBeCreditedMustBeANumber.ToString()
                };

                return BadRequest(increaseWalletBalanceErrorResult);
            }

            BaseResult<decimal> response = await _userService.AddMoneyToBalanceAsync(id, sum);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}