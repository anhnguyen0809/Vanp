using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Vanp.DAL.Utils
{
    public static class Sercurity
    {
        public static string CreateHashMD5(string strInput)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] input = Encoding.Default.GetBytes(strInput);
            byte[] output = md5.ComputeHash(input);
            string result = BitConverter.ToString(input).Replace("-", "");
            return result;
        }
    }
}