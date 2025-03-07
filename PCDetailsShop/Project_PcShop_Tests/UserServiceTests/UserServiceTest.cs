using Application.Services;
using Domain.Dto.UserDtos;
using Domain.Interfaces.Encrypt;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Result;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Project_PcShop_Tests.UserServiceTests
{
    public class UserServiceTest
    {
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private Mock<ICartRepository> _cartRepositoryMock = new Mock<ICartRepository>();

        private Mock<IUserValidator> _userValidatorMock = new Mock<IUserValidator>();

        private Mock<IEncrypter> _passwordEncrypter = new Mock<IEncrypter>();

        //[Fact]
        //public async void CreateUserTest() // Test ������� �� 3 ����� 1) ������������� ������, 
        //{
        //    UserService userService = new UserService(_userRepositoryMock.Object, _cartRepositoryMock.Object, _passwordEncrypter.Object);

        //    CreateUserDto dtoCreate = new CreateUserDto("ValentinLoginTest", "ValentinGmail", "zalupaPenisHer", DateTime.UtcNow);

        //    BaseResult<User> createdUser = await userService.CreateAsync(dtoCreate);

        //    bool createdUserNotNull = createdUser.IsSuccess;

        //    Assert.True(createdUserNotNull, "User Must Be not null");
        //}
    }
}