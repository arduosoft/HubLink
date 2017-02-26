using System;
using System.Security.Cryptography;
using System.Text;

namespace Wlog.Library.BLL.Utils
{
    public static class PasswordManagement
    {
        public static string EncodePassword(string password)
        {
            using (SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(sha.ComputeHash(Encoding.ASCII.GetBytes(password)));
            }
        }
    }
}
