using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public static class IllegalSymbols
    {
        private static IReadOnlyList<string> IllegalCharacters => new List<string>()
        {
            " ",
            "=",
            "-",
            "_",
            "*",
            "#",            
        };

        public static bool ContainsIllegalCharacter(string value)
        {
            foreach(string str in IllegalCharacters)
            {
                if(value.Contains(str)) 
                    return false;
            }

            return true;
        }
    }
}
