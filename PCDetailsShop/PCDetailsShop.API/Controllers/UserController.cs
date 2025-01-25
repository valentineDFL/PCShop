using Domain.Dto.UserDtos;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Domain.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace PCDetailsShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;

        public UserController(IUserService userService, IValidator<CreateUserDto> createUserValidator, IValidator<UpdateUserDto> updateUserValidator)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<BaseResult<UserDto>>> CreateUserAsync(CreateUserDto dto)
        {
            ValidationResult result = await _createUserValidator.ValidateAsync(dto);

            if (!result.IsValid)
                return BadRequest(result);
            
            BaseResult<UserDto> response = await _userService.CreateUserAsync(dto);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<BaseResult<UserDto>>> DeleteUserByIdAsync(Guid id)
        {
            BaseResult<UserDto> response = await _userService.DeleteUserByIdAsync(id);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        
        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            BaseResult<UserDto> response = await _userService.GetUserByIdAsync(id);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("UpdateUserById")]
        public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto dto)
        {
            ValidationResult result = await _updateUserValidator.ValidateAsync(dto);

            if(!result.IsValid)
                return BadRequest(result);

            BaseResult<UserDto> response = await _userService.UpdateUserAsync(dto);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("UpdateUserWalletBalance")]
        public async Task<ActionResult<UserDto>> UpdateUserWalletBalance(Guid id, decimal increaseSumm)
        {
            BaseResult<UserDto> response = await _userService.IncreaseUserWalletBalanceAsync(id, increaseSumm);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}