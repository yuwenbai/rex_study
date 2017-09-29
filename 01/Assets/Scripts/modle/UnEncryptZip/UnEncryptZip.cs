/****************************************************
*
*  把下载下来的加密文件进行解密，解压缩
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace projectQ
{
    public class UnEncryptZip : BaseResourceLoad
    {
        void Start()
        {
            if (GameConfig.Instance.IsDown == "1")
            {
                EventDispatcher.FireEvent(EventKey.Bundle_SliderValueShow_Event, BundleStepEnum.Bundle_XmlUnEncrypt);

                UnDES_Encrypt();
            }
            else
            {
                EventDispatcher.FireEvent(EventKey.Bundle_UnEncryZipFinish_Event);
            }
        }

        /// <summary>
        /// 对下载下来的加密文静进行解密
        /// </summary>
        void UnDES_Encrypt()
        {
            if (!Directory.Exists(GameConfig.Instance.XmlUnZip_Path))
            {
                Directory.CreateDirectory(GameConfig.Instance.XmlUnZip_Path);
            }

            Tools_FileOperation.Files_DeleteAll(GameConfig.Instance.XmlUnZip_Path);

            string XmlEncrypt_Path = GameConfig.Instance.XmlLocal_Path;

            DirectoryInfo folderDir = new DirectoryInfo(XmlEncrypt_Path);

            int index = 0;

            foreach (FileInfo file in folderDir.GetFiles())
            {
                string filePath = GameConfig.Instance.XmlLocal_Path + "/" + file.Name;

                EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_XmlUnEncrypt, index, folderDir.GetFiles().Length);

                EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 5, file.Name);

                UnDESEncrypt_Begin(filePath, file.Name);

                index++;
            }

            DebugPro.LogBundle(" #[DES]# - 解密成功 ");

            EventDispatcher.FireEvent(EventKey.Bundle_SliderValueShow_Event, BundleStepEnum.Bundle_XmlUnZip);

            UnZip();
        }

        /// <summary>
        /// DES解密
        /// filesPath - 需要界面的文件路径
        /// fileName - 需要解密的文件名字
        /// </summary>
        void UnDESEncrypt_Begin(string filePath, string fileName)
        {
            fileName = fileName.Replace(".dat", "");

            string outFile = GameConfig.Instance.XmlUnZip_Path + "/" + fileName;

            string password = Tools_CaesarCipher.Caesar_Encryption("OHDI2H7eWkxqqv7i28wa", 25);
            //string password = "123456789";

            //解密文件
            DESHelper.DecryptFile(filePath, outFile, password);

            //删除解密前的文件
            //File.Delete(filesPath);

            //QLoger.LOG( LogType.ELog,"#[DES]# - 解密成功 = " + filesPath);
        }

        /// <summary>
        /// 对已经解密的压缩文件进行解压缩
        /// </summary>
        void UnZip()
        {
            string Xmlzip_Path = GameConfig.Instance.XmlUnZip_Path;

            DirectoryInfo folderDir = new DirectoryInfo(Xmlzip_Path);

            int index = 0;

            foreach (FileInfo file in folderDir.GetFiles())
            {
                string filePath = file.FullName;

                EventDispatcher.FireEvent(EventKey.Bundle_SliderValue_Event,
                        BundleStepEnum.Bundle_XmlUnZip, index, folderDir.GetFiles().Length);

                EventDispatcher.FireEvent(EventKey.Bundle_LoadName_Event, 6, file.Name);

                UnZipFile(filePath, Xmlzip_Path + "/");

                //删除压缩前的文件
                File.Delete(filePath);

                index++;
            }

            DebugPro.LogBundle(" #[Zip]# - 解压缩完成 " );

            EventDispatcher.FireEvent(EventKey.Bundle_UnEncryZipFinish_Event);
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="zipFilePath">解压文件路径</param>
        /// <param name="savePath">解压文件存储路径</param>
        void UnZipFile(string zipFilePath, string savePath)
        {
            if (!File.Exists(zipFilePath))
            {
                DebugPro.LogBundle(" #[Zip]# - 该文件不存在 ");
                return;
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                s.Password = Tools_Md5.Md5_Encrypt("EyGuITypjJ8F");

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
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
            //QLoger.LOG( LogType.ELog,"#[Zip]# - 解压缩完成");
        }
    }
}