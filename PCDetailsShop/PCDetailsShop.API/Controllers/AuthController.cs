using Domain.Dto.UserDto;
using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Interfaces.Mapping;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PCDetailsShop.API.DtoMapping;
using System.Threading.Tasks;


/*
 * Регистрация - создание токена.
 * 
 * (сервис создания токена)
 * 
 * Логин - получение токена
 * 
 * Попытка что-то сделать в классах с атрибутом [Authorize] без токена = ошибка 401
 * 
 * 
 * 
 * 
 */

namespace PCDetailsShop.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IDtoMapper<User, UserDto> _userDtoMapper;

        public AuthController(IUserService userService, IValidator<CreateUserDto> createUserValidator,
            IDtoMapper<User, UserDto> userDtoMapper)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _userDtoMapper = userDtoMapper;
        }

        [HttpPost("Registrate")]
        public async Task<ActionResult<BaseResult<UserDto>>> Registration(CreateUserDto dto)
        {
            ValidationResult validationResult = await _createUserValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            BaseResult<User> response = await _userService.CreateAsync(dto);

            if (response.IsSuccess)
            {
                BaseResult<UserDto> result = _userDtoMapper.FromModelToDtoResult(response.Data);
                return Ok(result);
            }

            return BadRequest(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<BaseResult<string>>> Login([FromBody] LoginUserDto loginInfo)
        {
            var token = await _userService.LoginAsync(loginInfo);

            if (!token.IsSuccess)
                return BadRequest(token);

            // Сохраняем JWT в куки

            HttpContext.Response.Cookies.Append(CookiesCodes.AuthCookie, token.Data);

            return Ok(token);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(CookiesCodes.AuthCookie);

            return NoContent();
        }
    }
}