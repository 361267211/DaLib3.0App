using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager.Adapters.Util
{
    public class Base64Crypt
    {
        private string S;
        private string K;
        private List<char> T;
        public Base64Crypt(string codeTable)
        {
            T = new List<char>();
            K = codeTable;
        }

        public string Token
        {
            get
            {
                return S == null ? K : S;
            }
            set
            {
                T.Clear();
                S = value;
                if (S == null)
                {
                    foreach (var item in K)
                    {
                        T.Add(item);
                    }
                }
                else if (S.Length < 16)
                {
                    foreach (var item in S)
                    {
                        T.Add(item);
                    }
                    for (int i = 0; i < 64 - S.Length; i++)
                    {
                        T.Add(K[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < 16; i++)
                    {
                        T.Add(S[i]);
                    }
                }
            }
        }

        public string Encode(string x)
        {
            if (string.IsNullOrWhiteSpace(K))
            {
                throw new Exception("码表不能为空");
            }
            return string.IsNullOrEmpty(x) ? x : InternalEncode(Encoding.UTF8.GetBytes(x));
        }
        public string Decode(string x)
        {
            if (string.IsNullOrWhiteSpace(K))
            {
                throw new Exception("码表不能为空");
            }
            return string.IsNullOrEmpty(x) ? x : Encoding.UTF8.GetString(InternalDecode(x));
        }

        public byte[] Encode(byte[] x)
        {
            return x == null ? null : Encoding.UTF8.GetBytes(InternalEncode(x));
        }
        public byte[] Decode(byte[] x)
        {
            return x == null ? null : InternalDecode(Encoding.UTF8.GetString(x));
        }
        private void CheckToken()
        {
            if (T.Count != 16)
            {
                Token = K;
            }
        }
        private byte[] InternalDecode(string str)
        {
            CheckToken();
            int k = 0;
            int strLength = str.Length;

            byte[] data = new byte[strLength / 2];
            for (int i = 0, j = 0; i < data.Length; i++, j++)
            {
                byte s = 0;
                int index1 = Token.ToList().IndexOf(str[j]);
                j += 1;
                int index2 = Token.ToList().IndexOf(str[j]);
                s = (byte)(s ^ index1);
                s = (byte)(s << 4);
                s = (byte)(s ^ index2);
                data[k] = s;
                k++;
            }
            return data;
        }
        private string InternalEncode(byte[] x)
        {
            CheckToken();
            StringBuilder strEn = new StringBuilder();
            System.Collections.ArrayList arr = new System.Collections.ArrayList(x);
            for (int i = 0; i < arr.Count; i++)
            {
                byte data = (byte)arr[i];
                int v1 = data >> 4;
                strEn.Append(Token[v1]);
                int v2 = ((data & 0x0f) << 4) >> 4;
                strEn.Append(Token[v2]);
            }
            return strEn.ToString();
        }
    }
}
