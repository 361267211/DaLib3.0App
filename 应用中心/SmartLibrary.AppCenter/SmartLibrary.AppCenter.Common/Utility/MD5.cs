using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.Utility
{
    public static class MD5
    {
        /// <summary>
        /// 计算输入字符串的md5哈希值
        /// </summary>
        /// <param name="str">要计算的字符串</param>
        /// <returns>使用Base64编码后的字符串</returns>
        public static string ComputeHash(string str)
        {
            byte[] data = Encoding.Default.GetBytes(str);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //md5.
            data = md5.ComputeHash(data);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 基于16位的md5哈希值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeHash16(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }
        /// <summary>
        /// TripleDES解密
        /// </summary>
        public static string TripleDESDecrypting(string Source)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
                byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };   //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateDecryptor();
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader strd = new StreamReader(cs, Encoding.Default);
                return strd.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
            }
        }

        /// <summary>
        /// 字符串md5加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        public static string Encode(string str, Encoding encoding = null)
        {
            encoding = encoding == null ? Encoding.ASCII : encoding;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(encoding.GetBytes(str));
            var strResult = BitConverter.ToString(result);
            return strResult.Replace("-", "");
        }

        /// <summary>
        /// 计算输入字符串的md5哈希值32位
        /// </summary>
        /// <param name="str">要计算的字符串</param>
        /// <returns>使用Base64编码后的字符串</returns>
        public static string ComputeHash32(string str)
        {
            byte[] data = Encoding.Default.GetBytes(str);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //md5.
            data = md5.ComputeHash(data);
            string byte2String = null;
            for (int i = 0; i < data.Length; i++)
            {
                byte2String += data[i].ToString("x");
            }
            return byte2String;
        }
    }
}
