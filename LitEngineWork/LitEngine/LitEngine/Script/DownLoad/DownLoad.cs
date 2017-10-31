using UnityEngine;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;
namespace LitEngine
{
    public class UnZipFill
    {
        static private UnZipFill sInstance = null;
        static public UnZipFill Getinstance()
        {
            if (sInstance == null)
            {
                sInstance = new UnZipFill();
                ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
            }
            return sInstance;
        }

        public long FileLen
        {
            get;
            private set;
        }
        public void StartUnZipFile(string _filepath, string _exportpath, string _key, System.Action<string,float> _delegate, System.Action<string> _finished)   //解压缩
        {
            using (Stream _stream = File.OpenRead(_filepath))
            {
                StartUnZipFileByStream(_stream, _exportpath, _key, _delegate, _finished);
                _stream.Close();
            }
        }

        public void StartUnZipFileByStream(Stream _stream, string _exportpath, string _key, System.Action<string, float> _delegate, System.Action<string> _finished)
        {


            using (ZipInputStream f = new ZipInputStream(_stream))
            {
                long tunziplen = 0;
                FileLen = _stream.Length;
                string un_dir = _exportpath;

                ZipEntry zp = f.GetNextEntry();

                int tcachelen = 2048;

                byte[] tcachebuffer = new byte[tcachelen];  //每次缓冲 2048 字节
                while (zp != null)
                {
                    tunziplen += f.Length;

                    if (_delegate != null)
                        _delegate(zp.Name, ((float)tunziplen) / ((float)FileLen));
                    string un_tmp2;
                    if (zp.Name.IndexOf("/") >= 0)
                    {
                        int tmp1 = zp.Name.LastIndexOf("/");
                        un_tmp2 = zp.Name.Substring(0, tmp1);
                        if (!Directory.Exists(un_dir + un_tmp2))
                        {
                            Directory.CreateDirectory(un_dir + un_tmp2);
                        }
                    }
                    if (!zp.IsDirectory && zp.Crc != 00000000L) //此“ZipEntry”不是“标记文件”
                    {
                        string tnewfile = un_dir + "/" + zp.Name;

                        if (File.Exists(tnewfile))
                        {
                            File.Delete(tnewfile);
                        }
                        using (FileStream ts = File.Create(tnewfile))
                        {
                            int treadlen = 0;
                            while (true) //持续读取字节，直到一个“ZipEntry”字节读完
                            {
                                treadlen = f.Read(tcachebuffer, 0, tcachebuffer.Length); //读取“ZipEntry”中的字节
                                if (treadlen > 0)
                                {
                                    ts.Write(tcachebuffer, 0, treadlen); //将字节写入新建的文件流

                                }
                                else
                                {
                                    break; //读取的字节为 0 ，跳出循环
                                }
                            }

                            ts.Flush();
                            ts.Close();
                        }

                    }
                    zp = f.GetNextEntry();
                }
                f.Close();
            }

            if (_finished != null)
                _finished(_key);
        }
    }

    public class DownLoadObject
    {
      
    }
}



