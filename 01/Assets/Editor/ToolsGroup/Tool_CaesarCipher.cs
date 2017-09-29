/****************************************************
*
*  凯撒密码计算
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.Linq;

public class Tool_CaesarCipher
{
    //private static int key = 25;
    //private static string str = "OHDI2H7eWkxqqv7i28wa";

    /// <summary>
    /// 凯撒密码加密算法
    /// encryStr 原始密码 ，encryKey 加密key
    /// </summary>
    public static string Caesar_Encryption(string encryStr, int encryKey)
    {
        string strCaesar = "";

        char[] ch = encryStr.ToArray();

        for (int i = 0; i < encryStr.Length; i++)
        {
            string sou = ch[i].ToString();
            string tar = "";
            bool isChar = "abcdefghijklmnopqrstuvwxyz".Contains(sou.ToLower());
            bool isToUpperChar = isChar && (sou.ToUpper() == sou);
            sou = sou.ToLower();
            if (isChar)
            {
                int offset = (AscII(sou) + encryKey - AscII("a")) % (AscII("z") - AscII("a") + 1);
                tar = Convert.ToChar(offset + AscII("a")).ToString();
                if (isToUpperChar)
                {
                    tar = tar.ToUpper();
                }
            }
            else
            {
                tar = sou;
            }
            strCaesar += tar;
        }

        return strCaesar;
    }

    /// <summary>
    /// 凯撒密码解密算法
    /// encryStr 加密后的密码 ， encryKey 加密时候的key
    /// </summary>
    public static string Caesar_UnEncryption(string encryStr, int encryKey)
    {
        string strCaesar = "";

        char[] ch = encryStr.ToArray();

        for (int i = 0; i < encryStr.Length; i++)
        {
            string sou = ch[i].ToString();
            string tar = "";
            bool isChar = "abcdefghijklmnopqrstuvwxyz".Contains(sou.ToLower());
            bool isToUpperChar = isChar && (sou.ToUpper() == sou);
            sou = sou.ToLower();
            if (isChar)
            {
                int offset = (AscII("z") + encryKey - AscII(sou)) % (AscII("z") - AscII("a") + 1);
                tar = Convert.ToChar(AscII("z") - offset).ToString();
                if (isToUpperChar)
                {
                    tar = tar.ToUpper();
                }
            }
            else
            {
                tar = sou;
            }
            strCaesar += tar;
        }

        return strCaesar;
    }

    private static int AscII(string str)
    {
        byte[] array = new byte[1];
        array = System.Text.Encoding.ASCII.GetBytes(str);
        int asciicode = (short)(array[0]);
        return asciicode;
    }
}