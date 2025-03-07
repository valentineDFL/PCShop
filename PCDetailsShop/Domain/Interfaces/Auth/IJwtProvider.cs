using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
