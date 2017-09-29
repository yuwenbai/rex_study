/****************************************************
*  Copyright(C) 2015 智幻点击
*	版权所有。
*	作者:CEPH
*	创建时间：11/06/2015
*	文件名：  CommonTools.cs @ herocraft151104
*	文件功能描述：
*  创建标识：ceph.11/06/2015
*	创建描述：
*
*  修改标识：yqc.12/09/2015
*  修改描述：增加 md5file 计算文件md5值，float Float(object o) 转换float类型
*
*  修改标识：jeff.1/13/2016
*  修改描述：增加 MakeFileHashCode 计算文件hash值
*  
*  修改标识：xiyongjian.1/13/2016
*  修改描述：增加 FileSize 计算文件大小（字节）
*
*****************************************************/

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text.RegularExpressions;

namespace projectQ
{
    public partial class CommonTools
    {
        #region 文件，文件夹操作

        public static void LOG(LogType t, string p, params object[] v)
        {
            QLoger.LOG(typeof(AnimatorHand_Main), t, p, v);
        }


        /// <summary>
        /// The osha1 object.
        /// </summary>
        static System.Security.Cryptography.SHA1CryptoServiceProvider osha1 = null;
        /// <summary>
        /// Makes the file hash code.
        /// </summary>
        /// <returns>The file hash code.</returns>
        /// <param name="path">Path.</param>
        public static string MakeFileHashCode(string path)
        {
            if (osha1 == null)
            {
                osha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            }

            if (File.Exists(path))
            {
                string hash = "";
                using (System.IO.Stream s = System.IO.File.OpenRead(path))
                {
                    var hs = osha1.ComputeHash(s);
                    hash = Convert.ToBase64String(hs);
                }
                return hash;
            }
            return null;
        }

        public static byte[] ReadByteFile(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static string ReadAllText(string path, Encoding code = null)
        {
            return File.ReadAllText(path, code == null ? Encoding.UTF8 : code);
        }

        public static void WriteAllByte(byte[] data, string file)
        {
            File.WriteAllBytes(file, data);
        }

        public static void WriteAllText(string data, string file, Encoding code = null)
        {
            File.WriteAllText(file, data, code == null ? Encoding.UTF8 : code);
        }

        public static void MakeDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void MakeCleanDir(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        public static string ConverFilePathSeparator(string path)
        {
            string file = path;
            if (file != null)
            {
                if (System.IO.Path.DirectorySeparatorChar == '/')
                {
                    while (file.IndexOf('\\') > 0)
                    {
                        file = file.Replace('\\', System.IO.Path.DirectorySeparatorChar);
                    }
                }
                else if (System.IO.Path.DirectorySeparatorChar == '\\')
                {
                    while (file.IndexOf('/') > 0)
                    {
                        file = file.Replace('/', System.IO.Path.DirectorySeparatorChar);
                    }
                }
                return file;
            }
            else
            {
                return null;
            }
        }

        public static string GetFileName(string file)
        {

            if (file != null)
            {
                file = ConverFilePathSeparator(file);
                return Path.GetFileName(file);
            }

            return null;
        }

        public static string GetFilePath(string file)
        {

            if (file != null)
            {
                file = ConverFilePathSeparator(file);
                return Path.GetFullPath(file);
            }
            return null;
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetDateTimeString(string format = null)
        {
            return System.DateTime.Now.ToString(format == null ? "yyyyMMDDHHmmss" : format);
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="size">字节数</param>
        /// <returns></returns>
        public static string FormatFileSize(long size)
        {
            string result = null;
            double temp = size / 1024d;
            if (temp > 0.1d)
            {
                if (temp / 1024d > 0.1d)
                {
                    result = (temp / 1024d).ToString("0.00") + "MB";
                }
                else
                {
                    result = temp.ToString("0.00") + "KB";
                }
            }
            else
            {
                result = size + "B";
            }
            return result;
        }

        #endregion

        #region 编码转换

        /// <summary>
        /// UTF8 bytes to string .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Utf8BytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// UTF8 string to bytes.
        /// </summary>
        /// <returns>The string to bytes.</returns>
        /// <param name="str">String.</param>
        public static byte[] Utf8StringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        #endregion

        #region 其他

        /// <summary>
        /// Memories the stream to bytes.
        /// </summary>
        /// <returns>The stream to bytes.</returns>
        /// <param name="memStream">Mem stream.</param>
        /// <param name="offset">Offset.</param>
        public static byte[] MemoryStreamToBytes(MemoryStream memStream, int offset)
        {
            memStream.Seek(offset, SeekOrigin.Begin);
            int buffLength = (int)memStream.Length - offset;
            if (buffLength < 0)
                buffLength = 0;

            byte[] bytes = new byte[buffLength];
            memStream.Read(bytes, 0, buffLength);
            memStream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }

        /// <summary>
        ///转换成保留两位数的float类型
        /// </summary>
        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        /// <summary>
        /// 将UNIX时间戳转换成dateTme
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(long timeStamp)
        {
            //这里是时间戳转换 ， long类型时间等服务器数据回传时再处理
            return DateTime.FromFileTime(timeStamp);

        }

        public static string MakeReplaces(string var, string[] vars, int staridx = 0)
        {
            Regex reg = new Regex(@"\{\>(.*?)<}");
            string dest = var;

            MatchCollection c = reg.Matches(var);
            for (int i = 0; i < c.Count && i < vars.Length + staridx; i++)
            {
                dest = dest.Replace(c[i].Value, vars[i + staridx]);
            }
            return dest;
        }


        public delegate bool StringDelegate(string str, int index, out string resultStr);
        public static string MakeReplaces(string var, StringDelegate func)
        {
            Regex reg = new Regex(@"\{\>(.*?)<}");
            System.Text.StringBuilder dest = new System.Text.StringBuilder(var);

            string resultStr;
            int i = 0;
            while (reg.IsMatch(dest.ToString()))
            {

                Match ma = reg.Match(dest.ToString());
                if (ma.Groups.Count > 1 && func(ma.Groups[1].ToString(), i, out resultStr))
                {
                    dest.Replace(ma.Value, resultStr, ma.Index, ma.Length);
                }
                else
                {
                    return "";
                }
                ++i;
            }
            return dest.ToString();
        }


        /// <summary>
        /// 反射
        /// </summary>
        /// <param name="oj"></param>
        /// <returns></returns>
        public static string ReflactionObject(object oj)
        {
            if (oj == null)
            {
                return "null";
            }

#if !UNITY_IPHONE || UNITY_EDITOR
            Type tp = oj.GetType();

            if (tp.IsValueType)
            {
                return oj.ToString();
            }
            else if (tp.Equals(typeof(string)))
            {
                return oj as string;
            }
            else if (tp.IsArray)
            {
                StringBuilder sbuffer = new StringBuilder();
                sbuffer.Append("[");
                foreach (var v in oj as Array)
                {
                    sbuffer.Append(ReflactionObject(v));
                    sbuffer.Append(",");
                }
                if (sbuffer.Length > 1)
                {
                    sbuffer.Remove(sbuffer.Length - 1, 1);
                }
                sbuffer.Append("]");
                return sbuffer.ToString();
            }
            else
            {
                IEnumerable e = oj as IEnumerable;
                StringBuilder sbuffer = new StringBuilder();

                if (e != null)
                {
                    sbuffer.Append("[");
                    foreach (var v in e)
                    {
                        sbuffer.Append(ReflactionObject(v));
                        sbuffer.Append(",");
                    }
                    if (sbuffer.Length > 1)
                    {
                        sbuffer.Remove(sbuffer.Length - 1, 1);
                    }
                    sbuffer.Append("]");
                    return sbuffer.ToString();
                }
            }

            var flag = System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public;

            var fts = tp.GetFields(flag);

            StringBuilder sbufer = new StringBuilder();
            sbufer.Append("{");
            sbufer.Append(tp.Name);
            sbufer.Append(":");
            bool hasValue = false;
            foreach (var v in fts)
            {
                hasValue = true;
                sbufer.Append(v.Name);
                sbufer.Append("=");
                try
                {
                    sbufer.Append(ReflactionObject(v.GetValue(oj)));
                }
                catch (Exception ex)
                {
                    QLoger.ERROR(ex.ToString());
                }
                sbufer.Append(",");
            }

            if (hasValue)
            {
                sbufer.Remove(sbufer.Length - 1, 1);
            }
            sbufer.Append("}");
            return sbufer.ToString();
#else
        return oj.ToString();
#endif
        }

        #endregion

    

        #region -------控制特效开启或者是关闭-------------------------------------

        public static void Tools_SetEffectState(bool isBol)
        {
            GameConfig.Instance.IsEffectOpen = isBol;
            GameDelegateCache.C2CSetEffectStateEvent();
        }

        #endregion ---------------------------------------------------------------

        #region -------正则表达式替换特殊字符-------------------------------------

        public static string Tools_NameRegex(string nameStr)
        {
            string name = Regex.Replace(nameStr, "[ \\[ \\] \\^ \\-_*×――(^)$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", "□").ToUpper();

            return name;
        }

        /// <summary>
        /// 过滤不可显示的内容
        /// </summary>
        /// <param name="nameStr"></param>
        /// <returns></returns>
        public static string Tools_NameRegexPlus(string nameStr, string replaceStr)
        {
            return containsEmoji(nameStr).Trim();
        }

        private static string containsEmoji(string str)
        {
            int len = str.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                if (!isEmojiCharacter(str[i]))
                {
                    sb.Append(str[i]);
                    //return true;
                }
            }
            return sb.ToString();
            //return false;
        }
        private static bool isEmojiCharacter(char codePoint)
        {
            return !((codePoint == 0x0) ||
                    (codePoint == 0x9) ||
                    (codePoint == 0xA) ||
                    (codePoint == 0xD) ||
                    ((codePoint >= 0x20) && (codePoint <= 0xD7FF)) ||
                    ((codePoint >= 0xE000) && (codePoint <= 0xFFFD)) ||
                    ((codePoint >= 0x10000) && (codePoint <= 0x10FFFF)));
        }

        /// <summary>
        /// 剪切字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度文字算2位</param>
        /// <returns></returns>
        public static string Substring(string str, int length)
        {
            if (str == null || str.Length == 0 || length < 0)
            {
                return "";
            }

            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(str);
            int n = 0;  //  表示当前的字节数
            int i = 0;  //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < length; i++)
            {
                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    n++;      //  在UCS2第一个字节时n加1
                }
                else
                {
                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }
            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                if (bytes[i] > 0)
                    i = i - 1;
                //  该UCS2字符是字母或数字，则保留该字符
                else
                    i = i + 1;
            }
            return System.Text.Encoding.Unicode.GetString(bytes, 0, i);
        }
        #endregion ---------------------------------------------------------------

        public static double Distance(double long1, double lat1, double long2, double lat2)
        {
            double a, b, R;
            R = 6378137; // 地球半径  
            lat1 = lat1 * Math.PI / 180.0d;
            lat2 = lat2 * Math.PI / 180.0d;
            a = lat1 - lat2;
            b = (long1 - long2) * Math.PI / 180.0d;
            double d;
            double sa2, sb2;
            sa2 = Math.Sin(a / 2.0d);
            sb2 = Math.Sin(b / 2.0d);
            d = 2
                    * R
                    * Math.Asin(Math.Sqrt(sa2 * sa2 + Math.Cos(lat1)
                            * Math.Cos(lat2) * sb2 * sb2));
            return d;
        }

        /// <summary> 
        /// 反序列化 
        /// </summary> 
        /// <param name="data">数据缓冲区</param> 
        /// <returns>对象</returns> 
        //private static object DeSerialize<T>(byte[] data)
        public static object DeSerializeProto<T>(byte[] data)
        {
            try
            {
                var protodata = TcpDataHandler.DeSerializeProtoData(data, typeof(T));
                LOG(LogType.ELog, "data::" + data + " || " + protodata);
                return protodata;
            }
            catch (Exception e)
            {
                LOG(LogType.EError, e.ToString());
            }
            return null;
        }
    }
}
