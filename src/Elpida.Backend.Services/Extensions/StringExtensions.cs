using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Elpida.Backend.Services.Extensions
{
    public static class StringExtensions
    {
        public static string ToHashString(this string str)
        {
            using var md5 = MD5.Create();
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));

            return md5.ComputeHash(ms).ToHexString();
        }
    }
}