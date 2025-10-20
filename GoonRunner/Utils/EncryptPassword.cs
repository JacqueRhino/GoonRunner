using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoonRunner.Utils
{
    public class EncryptPassword
    {
        public static string MD5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
