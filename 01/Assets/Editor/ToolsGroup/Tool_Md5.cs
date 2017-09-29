/****************************************************
*
*  Md5值得相关操作
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

class Tool_Md5
{
    #region MD5加密----------------------------

    /// <summary>
    /// 32位 MD5加密
    /// </summary>
    /// <param name="str">加密字符</param>
    /// <returns></returns>
    public static string Md5_Encrypt(string str)
    {
        string cl = str;

        string pwd = "";

        MD5 md5 = MD5.Create();

        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));

        for (int i = 0; i < s.Length; i++)
        {
            pwd = pwd + s[i].ToString("X");
        }

        return pwd;
    }

    #endregion---------------------------------

    #region 获取文件的MD5值--------------------

    /// <summary>
    /// 获取文件的MD5值
    /// filePath 文件的全路径
    /// </summary>
    public static string MD5_File(string filePath)
    {
        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] retVal = md5.ComputeHash(fs);

            fs.Close();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("MD5_File() fail, error:" + ex.Message);
        }
    }

    #endregion---------------------------------
}