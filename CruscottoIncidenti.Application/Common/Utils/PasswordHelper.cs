using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CruscottoIncidenti.Application.Common.Utils
{
    public static class PasswordHelper
    {
        public static string EncryptPassword(string plainPassword)
        {
            string encrypted = string.Empty;
            using (SHA256 hash = SHA256.Create())
            {
                encrypted = string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(plainPassword))
                    .Select(item => item.ToString("x2")));
            }
            return encrypted;
        }
    }
}
