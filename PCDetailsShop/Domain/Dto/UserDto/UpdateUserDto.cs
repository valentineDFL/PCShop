using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.UserDtos
{
    public record class UpdateUserDto(Guid Id, string Login, string Email, string OldPassword, string NewPassword);
}