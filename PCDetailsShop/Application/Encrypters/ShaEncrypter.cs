using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Encrypt;

namespace Application.Encrypters
{
    internal class ShaEncrypter : IEncrypter
    {
        public string Encrypt(string toEncrypt)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);

            byte[] hashBytes = sha256Hash.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
