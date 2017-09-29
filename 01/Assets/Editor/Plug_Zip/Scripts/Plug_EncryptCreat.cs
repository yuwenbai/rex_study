/****************************************************
*
*  文件压缩，文件加密
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;

namespace PlugProject
{
    public class Plug_EncryptCreat
    {
        #region 文件压缩 --------------------------------------------

        public static void Encrypt_ZipCreat()
        {
            string XmlFile_Path = Application.dataPath + "/Resources/ClientConfig/XmlCreatFile";
            string XmlZip_Path = Application.dataPath + "/Resources/ClientConfig/XmlZip";

            Tool_FileOperation.Files_DeleteAll(XmlZip_Path);

            DirectoryInfo TheFolder = new DirectoryInfo(XmlFile_Path);

            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if (NextFile.Extension != ".meta")
                {
                    string fileToZip = NextFile.FullName;

                    string zipedFile = XmlZip_Path + "/" + NextFile.Name.Replace(NextFile.Extension, ".zip");

                    CreateZipFile(fileToZip, zipedFile);
                }
            }

            AssetDatabase.Refresh();
        }

        #endregion --------------------------------------------------

        #region 文件加密 --------------------------------------------

        public static void Encrypt_DESCreat()
        {
            string XmlZip_Path = Application.dataPath + "/Resources/ClientConfig/XmlZip";

            DirectoryInfo TheFolder = new DirectoryInfo(XmlZip_Path);

            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if (NextFile.Extension != ".meta")
                {
                    string fileToZip = NextFile.FullName;

                    DESEncrypt_Begin(fileToZip);
                }
            }

            AssetDatabase.Refresh();
        }

        #endregion --------------------------------------------------

        #region 文件压缩 , 解压缩 -----------------------------------

        /// <summary>
        /// 创建压缩文件
        /// </summary>
        /// <param name="filesPath">需要压缩的文件路径或者是文件夹均可</param>
        /// <param name="zipFilePath">压缩成的文件存放路径</param>
        private static void CreateZipFile(string filesPath, string zipFilePath)
        {
            if (!File.Exists(filesPath))
            {
                PlugMenuItemMain.PlugLog_Show(" #[Zip]# - 该文件不存在 ");
                return;
            }

            try
            {
                string[] filenames = Directory.GetFiles(filesPath);
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                {
                    s.SetLevel(9); // 压缩级别 0-9

                    s.Password = Tool_Md5.Md5_Encrypt("EyGuITypjJ8F");
                    //s.Password = "123"; //Zip压缩文件密码

                    byte[] buffer = new byte[4096]; //缓冲区大小

                    foreach (string file in filenames)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            }
                            while (sourceBytes > 0);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during processing {0}", ex);
            }

            PlugMenuItemMain.PlugLog_Show(" #[Zip]# - 压缩完成 = " + filesPath);
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="zipFilePath">解压文件路径</param>
        /// <param name="savePath">解压文件存储路径</param>
        private static void UnZipFile(string zipFilePath, string savePath)
        {
            if (!File.Exists(zipFilePath))
            {
                PlugMenuItemMain.PlugLog_Show(" #[Zip]# - 该文件不存在 ");
                return;
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                s.Password = Tool_Md5.Md5_Encrypt("EyGuITypjJ8F");

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(savePath + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            PlugMenuItemMain.PlugLog_Show("#[Zip]# - 解压缩完成");
        }

        #endregion --------------------------------------------------

        #region 文件加密 , 解密 -------------------------------------

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="filesPath"></param>
        private static void DESEncrypt_Begin(string filesPath)
        {
            string outFile = filesPath + ".dat";

            string password = Tool_CaesarCipher.Caesar_Encryption("OHDI2H7eWkxqqv7i28wa", 25);

            //string password = "123456789";

            //加密文件
            DESFileHelper.EncryptFile(filesPath, outFile, password);

            //删除加密前的文件                                                             
            File.Delete(filesPath);

            PlugMenuItemMain.PlugLog_Show(" #[DES]# - 加密成功 = " + filesPath);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="filesPath"></param>
        private static void UnDESEncrypt_Begin(string filesPath)
        {
            string outFile = filesPath.Substring(0, filesPath.Length - 4);

            string password = Tool_CaesarCipher.Caesar_Encryption("OHDI2H7eWkxqqv7i28wa", 25);

            //解密文件
            DESFileHelper.DecryptFile(filesPath, outFile, password);

            //删除解密前的文件                                                             
            File.Delete(filesPath);

            PlugMenuItemMain.PlugLog_Show("#[DES]# - 解密成功");
        }

        #endregion --------------------------------------------------
    }
}