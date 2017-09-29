/**
* @Author GarFey
* 检查更新上传管理类
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;
using PlugProject;

namespace BuildUpdateEditor
{
    /// <summary>
    /// 上传资源类型Type
    /// </summary>
    public enum UpdateType
    {
        Xml,
        Texture,
        AssetBundle
    }
    public class CheckUpdateControl
    {
        public static CheckUpdateControl Instan;
        public static CheckUpdateControl GetInstance
        {
            get { return Instan == null ? Instan = new CheckUpdateControl() : Instan; }
        }

        public string UserID = "";
        public string PassWord = "";
        private float barProgress = 0;
        private string barShwo = "";
        public float SidebarWidth
        {
            get {return EditorWindow.GetWindow(typeof(BuildUpEditor)).position.width / 3; }
        }
        /// <summary>
        /// 当前选择上传类型
        /// </summary>
        public static UpdateType mUpDateType = UpdateType.Xml;

        /// <summary>
        /// fileInfoDic 待删除队列
        /// </summary>
        public Queue<string> removeDicNameQueue = new Queue<string>();
        /// <summary>
        /// UpLoading 初始化成功回调
        /// </summary>
        /// <param name="isFi"></param>
        public void IsInitFinish(bool isFi)
        {
            if (!isFi)
            {
                ToolsEditorState.toolsEidtorCurState = TEditorState.NULL;
            }
            else
            {
                CheckForUpdateView.GetInstance.Init();
                ToolsEditorState.toolsEidtorCurState = TEditorState.CHECKUPDATE;
            }
        }

        /// <summary>
        /// 检查本地服务器md5
        /// </summary>
        public void CheckUpdateMd5()
        {
            CheckUpDate.GetInstance.ClearFileInfoDic();
            checkFile(); 
        }
        private void checkFile()
        {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            switch(mUpDateType)
            {
                case UpdateType.Xml:
                    fileInfoList = UpLoadingData.UpLoadingFileList;
                    break;
                case UpdateType.Texture:
                    fileInfoList = UpLoadingData.UpLoadingTextureFileList;
                    break;
                case UpdateType.AssetBundle:
                    break;
            }
            if (fileInfoList.Count <= 0)
            {
                EditorUtility.DisplayDialog("提示", "本地没有可上传文件，请检查！", "确定");
                return;
            }
            
            FileInfo tmpInfo;
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                tmpInfo = fileInfoList[i];
                setCheckInfo(tmpInfo);
            }
        }
        /// <summary>
        /// 包装数据到本地
        /// </summary>
        /// <param name="fInfo"></param>
        void setCheckInfo(FileInfo fInfo)
        {
            string tmpFtpMd5 = "";
            string tmpLocMd5 = "";
            string tmpFileVersion = "";
            string md5fileName = "";

            CheckListInfo clInfo = new CheckListInfo();
            clInfo._FileName = fInfo.Name;
            clInfo._LocalFilePath = fInfo.FullName.Replace("\\", "/");
            string[] nameArr = clInfo._FileName.Split('.');
            md5fileName = nameArr[0];
            tmpFtpMd5 = UpLoadingFind.UpLoading_Md5Get(md5fileName);
            tmpLocMd5 = UpLoadingFind.UpLoading_Md5LocalGet((XmlMessageTypeEnum)GetTypeIdex(mUpDateType, 0), md5fileName);
            tmpFileVersion = UpLoadingFind.UpLoading_VersionGet(md5fileName);
            if (tmpFtpMd5 != "")
            {
                if (tmpFtpMd5 == tmpLocMd5)
                {
                    clInfo._VersionTitle = clInfo._FileVersion = tmpFileVersion != "" ? tmpFileVersion : "1001";
                    clInfo.mfileState = CheckListInfo.FileState.Null;
                }
                else
                {
                    if (tmpFileVersion != "")
                    {
                        if (tmpFileVersion != "-1")
                        {
                            int tmpVers = int.Parse(tmpFileVersion);
                            tmpVers += 1;
                            clInfo._VersionTitle = tmpFileVersion + " → " + tmpVers;
                            clInfo._FileVersion = "" + tmpVers;
                        }
                        else
                        {
                            clInfo._VersionTitle = clInfo._FileVersion = tmpFileVersion;
                        }
                    }
                    else
                    {
                        clInfo._VersionTitle = clInfo._FileVersion = "1001";
                    }
                    clInfo.mfileState = CheckListInfo.FileState.Difc;
                }
                clInfo._FileMd5 = tmpLocMd5;
            }
            else
            {
                clInfo._VersionTitle = clInfo._FileVersion = "1001";
                clInfo._FileMd5 = tmpLocMd5;
                clInfo.mfileState = CheckListInfo.FileState.Add;
            }
            CheckUpDate.GetInstance.AddOrReviseFileInfoDic(fInfo.Name, clInfo);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="serVersion"></param>
        public void UpdateFileInfoDic(string serVersion)
        {
            if(serVersion== "")
            {
                EditorUtility.DisplayDialog("提示", "服务器Version不能为空！", "确定");
                return;
            }
           
            if(EditorUtility.DisplayDialog("提示", "确认上传至 【 " + ToolsBar.GetInstance.GetSelect()+" 】", "确定", "取消"))
            {
                string mlocalPath = "";
                string fileName = "";

                //Tool_UpLoadingXml.UpLoadingMd5Xml_Init();
                int Totalcount = CheckUpDate.GetInstance.getFileInfoDic.Count;
                int i = 0;
                foreach (var tmp in CheckUpDate.GetInstance.getFileInfoDic)
                {
                    i++;
                    barProgress = (float)i / Totalcount;
                    barProgress = (float)Math.Round((double)barProgress, 2);
                    mlocalPath = tmp.Value._LocalFilePath;
                    string[] nameArr = tmp.Key.Split('.');
                    fileName = nameArr[0];
                    upLoadFiles(mlocalPath, UpLoadingPath.UpLoading_PathGet((PathServerEnum)GetTypeIdex(mUpDateType, 1), 1, fileName), upLoadFilesCallBack);
                }
                UpVersion(serVersion);
                UpMd5();

                if (CheckUpDate.GetInstance.getFileInfoDic.Count == removeDicNameQueue.Count)
                {
                    CheckUpDate.GetInstance.ClearFileInfoDic();
                    EditorUtility.DisplayDialog("提示", "所有文件上传成功！", "确定");
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "有文件上传失败，查看Error！", "确定");
                }
            }
        }
        void upLoadFilesCallBack(string fileName,bool bo)
        {
            if(bo)
            {
                if (CheckUpDate.GetInstance.getFileInfoDic.ContainsKey(fileName))
                {
                    string[] arr = fileName.Split('.');
                    string xmlFileName = arr[0];
                    CheckListInfo tmpi = CheckUpDate.GetInstance.getFileInfoDic[fileName];
                    UpLoadingUpdate.UpLoading_DataUpdate((XmlMessageTypeEnum)GetTypeIdex(mUpDateType, 0), xmlFileName, int.Parse(tmpi._FileVersion));
                    RemoveFileInfo(fileName);
                }
                Debug.Log("文件【" + fileName + "】上传成功！<br/>");
            }
            else
            {
                Debug.LogError("文件【" + fileName + "】上传失败！<br/>");
            }
        }
        void UpVersionCallBack(string fileName, bool bo)
        {
            if(bo)
            {
                EditorUtility.DisplayDialog("提示", fileName+"上传成功", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("提示", fileName + "上传失败！", "确定");
            }
        }
        void UpMd5CallBack(string fileName, bool bo)
        {
            if (bo)
            {
                EditorUtility.DisplayDialog("提示", fileName + "上传成功", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("提示", fileName + "上传失败！", "确定");
            }
        }

        /// <summary>  
        /// 上传文件  
        /// </summary>  
        /// <param name="localFile">要上传到FTP服务器的本地文件</param>  
        /// <param name="ftpPath">FTP地址</param>  
        /// <param name="isBar">是否显示进度条</param>
        /// <param name="isBigFile">是否 是 大文件</param>
        public void upLoadFiles(string localFile, string ftpFilePath, System.Action<string,bool> callBack, bool isBar = true,bool  isBigFile = false)
        {
            if (!File.Exists(localFile))
            {
                EditorUtility.DisplayDialog("错误", "本地文件" + localFile + "不存在", "确定");
                Debug.LogError("文件：“" + localFile + "” 不存在！");
                callBack(localFile,false);
                return;
            }
            string fileName = getFileName(localFile);
            if (fileName.EndsWith(".meta"))
            {
                //meta文件不上传
                return;
            }

            string local = localFile.Substring(localFile.LastIndexOf('.') + 1);
            string build = ftpFilePath.Substring(ftpFilePath.LastIndexOf('.') + 1);
            if (!string.Equals(local, build))
            {
                callBack(fileName, false);
                Debug.Log("本地类型跟上传类型不符 local:: "+ local+" ||  ftp:: " + build);
                return;
            }

            if (fileCheckExist(ftpFilePath))
            {
                //删除
                Debug.Log("文件<" + fileName + ">存在，将删除");
                deleteFile(ftpFilePath);
            }
            try
            {
                FileInfo fileInf = new FileInfo(localFile);
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(ftpFilePath);// 根据uri创建FtpWebRequest对象   
                reqFTP.Credentials = new NetworkCredential(UserID, PassWord);// ftp用户名和密码  
                reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行  
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令  

                reqFTP.UseBinary = true;// 指定数据传输类型  
                reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小  
                int buffLength = 2048;// 缓冲大小设置为2kb  
                byte[] buff = new byte[buffLength];
                int contentLen;
                int curStrmLen = 0;

                // 打开一个文件流 (System.IO.FileStream) 去读上传的文件  
                FileStream fs = fileInf.OpenRead();

                Stream strm = reqFTP.GetRequestStream();// 把上传的文件写入流  
                contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的2kb  

                while (contentLen != 0)// 流内容没有结束  
                {
                    curStrmLen += contentLen;
                    // 把内容从file stream 写入 upload stream  
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                   if (isBigFile)
                   {
                       ShowProgressBar(fileName, curStrmLen, fs.Length);
                   }
                }
                // 关闭两个流  
                strm.Close();
                fs.Close();

                if (isBar)
                {
                    barShwo = fileName + "上传中 ... " + barProgress * 100 + "%";
                    loadProgressBar(barShwo, barProgress, "文件上传中...");
                }
                callBack(fileName,true);
            }
            catch (Exception ex)
            {
                callBack(fileName,false);
                Debug.LogError("上传文件【" + fileName + " || " + ftpFilePath + "】时，发生错误：" + ex.Message + "<br/>");
            }
        }
        // 大文件显示bar
        private void ShowProgressBar(string fileName,int curPro,long totalPro)
        {
            barProgress = (float)curPro / totalPro;
            barProgress = (float)Math.Round((double)barProgress, 2);
            barShwo = fileName + "上传中 ... " + barProgress * 100 + "%";
            loadProgressBar(barShwo, barProgress, "文件上传中...");
        }

        /// <summary>
        /// 上传Version
        /// </summary>
        public void UpVersion(string vers,bool isBar =true)
        {
            if (vers == "")
            {
                EditorUtility.DisplayDialog("提示", "服务器Version不能为空！", "确定");
                return;
            }
            UpLoadingUpdate.UpLoading_VersionUpdate(vers);
            upLoadFiles(UpLoadingData.UpLoadingFilePath, UpLoadingPath.UpLoading_PathGet(PathServerEnum.Path_VersionXml, 1), UpVersionCallBack, isBar);
        }
        /// <summary>
        /// 上传MD5
        /// </summary>
        public void UpMd5()
        {
            upLoadFiles(UpLoadingData.UpLoadingMd5Path, UpLoadingPath.UpLoading_PathGet(PathServerEnum.Path_Md5Xml, 1), UpMd5CallBack);
        }
        
        /// <summary>
        /// 修改文件Version
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Vers"></param>
        public void CheckUpReviseVersion(string Name,string Vers)
        {
            if(CheckUpDate.GetInstance.getFileInfoDic.ContainsKey(Name))
            {
                CheckListInfo tmpInfo = CheckUpDate.GetInstance.getFileInfoDic[Name];
                if(Vers != tmpInfo._FileVersion)
                {
                    tmpInfo._VersionTitle = Vers;
                    tmpInfo._FileVersion = Vers;
                    tmpInfo.mfileState = CheckListInfo.FileState.Revise;
                }
            }
        }

        /// <summary>
        /// fileInfodic 排序
        /// </summary>
        public void OrderByFileInfoDic()
        {
            CheckUpDate.GetInstance.OrderByFileInfoDic();
        }

        /// <summary>  
        /// 文件存在检查  
        /// </summary>  
        /// <param name="ftpPath"></param>  
        /// <returns></returns>  
        private bool fileCheckExist(string filePath)
        {
            string _fileName = getFileName(filePath);
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(filePath));
                ftpWebRequest.Credentials = new NetworkCredential(UserID, PassWord);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == _fileName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
            return success;
        }

        /// <summary>
        /// 根据当前类型获取数据库类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pathType"> 0 XmlMessageTypeEnum 1 PathServerEnum  </param>
        /// <returns></returns>
        public int GetTypeIdex(UpdateType type,int pathType)
        {
            int mType = -1;
            switch (type)
            {
                case UpdateType.Xml:
                    mType = pathType == 0 ? (int)XmlMessageTypeEnum.xmlUrl : (int)PathServerEnum.Path_AssetsXml;
                    break;
                case UpdateType.Texture:
                    mType = pathType == 0 ? (int)XmlMessageTypeEnum.textureUrl : (int)PathServerEnum.Path_AssetsTexture;
                    break;
            }
            return mType;
        }

        /// <summary>
        /// 删除fileInfo 添加到删除队列
        /// </summary>
        /// <param name="fileName"></param>
        public void RemoveFileInfo(string fileName)
        {
            if (!removeDicNameQueue.Contains(fileName))
            {
                removeDicNameQueue.Enqueue(fileName);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        private void deleteFile(string ftpFilePath)
        {
            string _fileName = "";
            try
            {
                _fileName = getFileName(ftpFilePath);
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpFilePath));

                reqFTP.Credentials = new NetworkCredential(UserID, PassWord);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                sr.Close();
                datastream.Close();
                response.Close();
                Debug.Log("文件<" + _fileName + ">删除成功");
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog("错误", "删除文件" + _fileName + "错误", "确定");
                Debug.LogError("删除失败 --> " + ex.Message + "  文件名:" + _fileName);
            }
        }

        /// <summary>
        /// 根据资源路径截取资源名称  例 f/ass/d/c.txt  最后得到 c.txt
        /// </summary>
        /// <param name="path"></param>
        private string getFileName(string path)
        {
            string fileName = "";
            if (path != "" && path.Contains("/"))
            {
                int idex = path.LastIndexOf('/');
                fileName = path.Substring(idex + 1);
            }
            return fileName;
        }

        /// <summary>
        /// 刷新进度条
        /// </summary>
        /// <param name="showTi">提示问本</param>
        /// <param name="progres">进度百分比 0~1 之间</param>
        ///  <param name="title">标题</param>
        private void loadProgressBar(string showTi, float progres, string title = "Simple Progress Bar")
        {
            if (progres >= 1)
            {
                EditorUtility.ClearProgressBar(); //清空进度条
            }
            else
            {
                EditorUtility.DisplayProgressBar(title, showTi, progres);
            }
        }

        public void OnDestory()
        {
            Instan = null;
        }
    }
}
