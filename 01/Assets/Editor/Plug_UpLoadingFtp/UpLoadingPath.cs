/****************************************************
*
*  获取服务器上的各个功能模块的路径
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

namespace PlugProject
{
    public class UpLoadingPath : BaseUpLoading
    {
        private static string xxx = "http://192.168.221.48/ResPub/GarFeyTestXmlAssets/";

        #region 查询服务器上相关资源路径 -----------------------------

        /// <summary>
        /// 查询服务器上相关资源路径
        /// xName 文件名字
        /// </summary>
        public static string UpLoading_PathGet(PathServerEnum sPathEnum, int index = 0, string xName = "")
        {
            string sPath = "";

            string sSite = UpLoadingData.ServerSiteUrl;
            //string sSite = xxx;
            if (index == 1)
            {
                sSite = sSite.Replace("http", "ftp");
                sSite = sSite.Replace("/ResPub", "");
            }

            switch (sPathEnum)
            {
                case PathServerEnum.Path_VersionXml:
                    sPath = sSite + "version.xml";
                    break;
                case PathServerEnum.Path_Md5Xml:
                    sPath = sSite + "UpLoadingMd5.xml";
                    break;
                case PathServerEnum.Path_MusicXml:
                    sPath = sSite + "MusicXml.xml";
                    break;
                case PathServerEnum.Path_AssetsXml:
                case PathServerEnum.Path_AssetsTexture:
                    sPath = sSite + UpLoading_AssetsXmlPathGet(sPathEnum, xName);
                    break;
                case PathServerEnum.Path_AssetsMusicBg:
                case PathServerEnum.Path_AssetsMusicSound:
                case PathServerEnum.Path_AssetsMusicPuTong:
                case PathServerEnum.Path_AssetsMusicHeNan:
                    UpLoading_ErrorBox(" #[UpLoadingPath]# - 功能未开发 = " + sPathEnum);
                    break;
            }

            return sPath;
        }

        /// <summary>
        /// 获取Xml的上传全路径
        /// </summary>
        private static string UpLoading_AssetsXmlPathGet(PathServerEnum sPathEnum, string xName)
        {
            string xPath = "";

            foreach (string sValue in UpLoadingData.XmlList_Server)
            {
                string[] values = sValue.Split(new char[] { '@' });
                string[] verValue = values[1].Split(new char[] { '?' });

                if (values[0].Equals(xName))
                {
                    xPath = verValue[0];
                    break;
                }
            }

            if (string.IsNullOrEmpty(xPath))
            {
                if (sPathEnum == PathServerEnum.Path_AssetsXml)
                {
                    xPath = string.Format("DownXmlAssets/{0}.zip.dat", xName);
                }
                else if (sPathEnum == PathServerEnum.Path_AssetsTexture)
                {
                    xPath = string.Format("DownTextureAssets/{0}.png", xName);
                }
            }

            return xPath;
        }

        #endregion ----------------------------------------------------
    }
}