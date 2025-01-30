using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Enums
{
    public static class ValidationMessages
    {
        public static string TurnedLoginContainIllegalCharacters => nameof(TurnedLoginContainIllegalCharacters);

        public static string TurnedEmailContainIllegalCharacters => nameof(TurnedEmailContainIllegalCharacters);

        public static string TurnedPasswordContainIllegalCharacters => nameof(TurnedPasswordContainIllegalCharacters);

        public static string TheUserMustBeOver16YearsOfAge => nameof(TheUserMustBeOver16YearsOfAge);

        public static string TurnedNewPasswordContainIllegalCharacters => nameof(TurnedNewPasswordContainIllegalCharacters);
    }
}
