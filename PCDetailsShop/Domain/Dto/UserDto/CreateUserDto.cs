using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.UserDtos
{
    public record CreateUserDto(string Login, string Email, string Password, DateTime BirthDate);
}