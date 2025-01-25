using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.UserDtos
{
    public record UserDto(Guid Id, string Login, string Email, DateTime BirthDate, DateTime RegistrationDate);
}