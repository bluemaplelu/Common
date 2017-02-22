using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace DotNet.Utilities
{
    /// <summary>
    /// 字符串加密组件
    /// </summary>
    public class Encrypt
    {
        #region 另一种，报错
        //#region "定义加密字串变量"
        //private static SymmetricAlgorithm mCSP;  //声明对称算法变量
        //private const string CIV = "Mi9l/+7Zujhy12se6Yjy111A";  //初始化向量
        //private const string CKEY = "jkHuIy9D/9i="; //密钥（常量）
        //#endregion

        ///// <summary>
        ///// 实例化
        ///// </summary>
        //public Encrypt()
        //{
        //    mCSP = new DESCryptoServiceProvider();  //定义访问数据加密标准 (DES) 算法的加密服务提供程序 (CSP) 版本的包装对象,此类是SymmetricAlgorithm的派生类
        //}

        ///// <summary>
        ///// 加密字符串
        ///// </summary>
        ///// <param name="Value">需加密的字符串</param>
        ///// <returns></returns>
        //public static string EncryptString(string Value)
        //{
        //    ICryptoTransform ct; //定义基本的加密转换运算
        //    MemoryStream ms; //定义内存流
        //    CryptoStream cs; //定义将内存流链接到加密转换的流
        //    byte[] byt;

        //    //CreateEncryptor创建(对称数据)加密对象
        //    ct = mCSP.CreateEncryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV)); //用指定的密钥和初始化向量创建对称数据加密标准

        //    byt = Encoding.UTF8.GetBytes(Value); //将Value字符转换为UTF-8编码的字节序列

        //    ms = new MemoryStream(); //创建内存流
        //    cs = new CryptoStream(ms, ct, CryptoStreamMode.Write); //将内存流链接到加密转换的流
        //    cs.Write(byt, 0, byt.Length); //写入内存流
        //    cs.FlushFinalBlock(); //将缓冲区中的数据写入内存流，并清除缓冲区
        //    cs.Close(); //释放内存流

        //    return Convert.ToBase64String(ms.ToArray()); //将内存流转写入字节数组并转换为string字符
        //}

        ///// <summary>
        ///// 解密字符串
        ///// </summary>
        ///// <param name="Value">要解密的字符串</param>
        ///// <returns>string</returns>
        //public static string DecryptString(string Value)
        //{
        //    ICryptoTransform ct; //定义基本的加密转换运算
        //    MemoryStream ms; //定义内存流
        //    CryptoStream cs; //定义将数据流链接到加密转换的流
        //    byte[] byt;

        //    ct = mCSP.CreateDecryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV)); //用指定的密钥和初始化向量创建对称数据解密标准
        //    byt = Convert.FromBase64String(Value); //将Value(Base 64)字符转换成字节数组

        //    ms = new MemoryStream();
        //    cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
        //    cs.Write(byt, 0, byt.Length);
        //    cs.FlushFinalBlock();
        //    cs.Close();

        //    return Encoding.UTF8.GetString(ms.ToArray()); //将字节数组中的所有字符解码为一个字符串
        //}

        //默认密钥向量
        #endregion
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串 </returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));//转换为字节
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();//实例化数据加密标准
                MemoryStream mStream = new MemoryStream();//实例化内存流
                //将数据流链接到加密转换的流
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}
