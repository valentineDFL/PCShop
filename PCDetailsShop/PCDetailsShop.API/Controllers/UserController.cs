using Domain.Dto.UserDtos;
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
        private readonly IValidator<UpdateUserDto> _updateUserValidator;
        private readonly ResponseDtoMapper _userDtoMapper;

        public UserController(IUserService userService, IValidator<CreateUserDto> createUserValidator, 
            IValidator<UpdateUserDto> updateUserValidator, ResponseDtoMapper userDtoMapper)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
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

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        
        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            BaseResult<User> response = await _userService.GetUserByIdAsync(id);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDto(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpPut("UpdateUserById")]
        public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto dto)
        {
            ValidationResult result = await _updateUserValidator.ValidateAsync(dto);

            if(!result.IsValid)
                return BadRequest(result);

            BaseResult<User> response = await _userService.UpdateUserAsync(dto);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}