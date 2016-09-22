using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Core.Util
{
    /// <summary>
    ///加 解 密
    /// </summary>
    public class Encryptor
    {
        private static readonly Encoding encoding = Encoding.Default;
        private static byte[] inputByteArray;
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        /// <summary>
        /// MD5加密
        /// </summary>
        public static string MD5(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符） 
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            //32位加密 
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }

        #region 加密 解密，固定密钥
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sourceData">原文</param>
        /// <returns></returns>
        public static string Encrypt(string sourceData)
        {
            //set key and initialization vector values
            try
            {
                //convert data to byte array
                Byte[] sourceDataBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(sourceData);
                //get target memory stream
                MemoryStream tempStream = new MemoryStream();
                //get encryptor and encryption stream
                DESCryptoServiceProvider encryptor = new DESCryptoServiceProvider();
                CryptoStream encryptionStream = new CryptoStream(tempStream, encryptor.CreateEncryptor(key, iv), CryptoStreamMode.Write);

                //encrypt data
                encryptionStream.Write(sourceDataBytes, 0, sourceDataBytes.Length);
                encryptionStream.FlushFinalBlock();

                //put data into byte array
                Byte[] encryptedDataBytes = tempStream.GetBuffer();
                //convert encrypted data into string
                return System.Convert.ToBase64String(encryptedDataBytes, 0, (int)tempStream.Length);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sourceData">密文</param>
        /// <returns></returns>
        public static string Decrypt(string sourceData)
        {
            //set key and initialization vector values
            try
            {
                //convert data to byte array
                Byte[] encryptedDataBytes = System.Convert.FromBase64String(sourceData);
                //get source memory stream and fill it 
                MemoryStream tempStream = new MemoryStream(encryptedDataBytes, 0, encryptedDataBytes.Length);
                //get decryptor and decryption stream 
                DESCryptoServiceProvider decryptor = new DESCryptoServiceProvider();
                CryptoStream decryptionStream = new CryptoStream(tempStream, decryptor.CreateDecryptor(key, iv), CryptoStreamMode.Read);

                //decrypt data 
                StreamReader allDataReader = new StreamReader(decryptionStream);

                return allDataReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region DES加密 解密 固定密钥
        /// <summary>
        /// DES加密方法 固定密钥
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptDES(string encryptString)
        {
            try
            {
                inputByteArray = Encoding.Default.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in mStream.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密方法 固定密钥
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString)
        {
            try
            {
                int len;
                len = decryptString.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(decryptString.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.Default.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion

        #region DES加密 解密 传入密钥
        /// <summary>
        /// DES加密方法  传入密钥
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <param name="encryptIV"></param>
        /// <returns></returns>
        public static string EncryptDES(string encryptString, string encryptKey, string encryptIV)
        {
            try
            {
                key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptKey, "md5").Substring(0, 8));
                iv = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptIV, "md5").Substring(0, 8));
                inputByteArray = Encoding.Default.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in mStream.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密方法 传入密钥
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
        /// <param name="decryptIV"></param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString, string decryptKey, string decryptIV)
        {
            try
            {
                int len;
                len = decryptString.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(decryptString.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(decryptKey, "md5").Substring(0, 8));
                iv = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(decryptIV, "md5").Substring(0, 8));
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.Default.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion

        #region DES加密 解密 传入密钥
        /// <summary>
        /// DES加密方法  传入密钥
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <param name="encryptIV"></param>
        /// <returns></returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] decodedKey = Convert.FromBase64String(encryptKey);
                byte[] keyArr = new Byte[8];
                for (int i = 0; i < keyArr.Length; i++)
                {
                    keyArr[i] = decodedKey[i];
                }
                byte[] dataByteArray = Encoding.UTF8.GetBytes(encryptString);
                des.Mode = System.Security.Cryptography.CipherMode.ECB;
                des.Key = keyArr;
                des.IV = new byte[8];
                string encrypt = "";
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                }
                return encrypt;
            }
        }

        /// <summary>
        /// DES解密方法 传入密钥
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
        /// <param name="decryptIV"></param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            byte[] keys = new byte[8];
            Array.Copy(Convert.FromBase64String(decryptKey), keys, 8);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = keys;
                des.IV = new byte[8]; // ASCIIEncoding.ASCII.GetBytes(sKey);  
                des.Mode = System.Security.Cryptography.CipherMode.ECB;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

        /// <summary>
        /// 哈希序列
        /// </summary>
        /// <param name="public_Key"></param>
        /// <param name="private_Key"></param>
        /// <returns></returns>
        public static string HASH_HMAC(string public_Key, string private_Key)
        {
            string reStr = "";
            try
            {
                using (HMACSHA1 hmacsha1 = new HMACSHA1(encoding.GetBytes(private_Key)))
                {
                    byte[] buff = hmacsha1.ComputeHash(encoding.GetBytes(public_Key));
                    reStr = System.Convert.ToBase64String(buff, 0, (int)buff.Length);
                }
                return reStr;
            }
            catch
            {
                return reStr;
            }
        }

        /// <summary>
        /// hmacSha1算法加密（生成长度40）
        /// </summary>
        /// <param name="encryptText">加密明文</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns></returns>
        public static string hmacSha1(string encryptText, string encryptKey)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.Default.GetBytes(encryptKey));
            byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.Default.GetBytes(encryptText));

            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in RstRes)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            string reStr = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                reStr = Convert.ToBase64String(bytes);
            }
            catch
            {
                reStr = source;
            }
            return reStr;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
    }
}
