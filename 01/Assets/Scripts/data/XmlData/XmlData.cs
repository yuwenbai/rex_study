/**
 * @Author lyb
 * Xml数据类
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.IO;

namespace projectQ
{
    public enum XmlDataEnum
    {
        /// <summary>
        /// 资源加载的服务器路径
        /// </summary>
        XML_BUNDLE_CONFIG = 0,
        /// <summary>
        /// 地区玩法列表
        /// </summary>
        XML_GAMECONFIG_REGION = 1,
        /// <summary>
        /// 流行玩法列表
        /// </summary>
        XML_GAMECONFIG_FASHION = 2,
    }

    public class XmlData
    {
        private Dictionary<string, List<BaseXmlBuild>> _XmlBuildDataDic = new Dictionary<string, List<BaseXmlBuild>>();
        public Dictionary<string, List<BaseXmlBuild>> XmlBuildDataDic
        {
            get
            {
                return _XmlBuildDataDic;
            }
        }

        #region 本地Xml数据泛型读取--------------------------------------------------------

        public static T XmlInit<T>(T t) where T : BaseXmlData
        {
            t.Init();
            return default(T);
        }

        #endregion ------------------------------------------------------------------------

        #region Xml数据初始化，游戏资源加载后调用------------------------------------------

        /// <summary>
        /// 游戏开始的时候调用。。初始化所有的xml数据存储下来
        /// </summary>
        public void Init()
        {
            XmlPlayGameConfigData playData = new XmlPlayGameConfigData();
            XmlData.XmlInit<XmlPlayGameConfigData>(playData);

            XmlDataCtrl ctrl = new XmlDataCtrl();
            ctrl.XmlDataInit();
        }

        #endregion ------------------------------------------------------------------------

        #region Xml数据查询接口------------------------------------------------------------

        /// <summary>
        /// 查询某一个表的字段
        /// table             表名字
        /// lineName          字段名字
        /// cValueList        比较的数据
        /// </summary>
        public List<BaseXmlBuild> XmlBuildData_Get(string table, string lineName, List<string> cValueList)
        {
            List<BaseXmlBuild> build_get = new List<BaseXmlBuild>();

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic[table];
            foreach (BaseXmlBuild bData in buildList)
            {
                foreach (string str in cValueList)
                {
                    if (str.Equals(bData.XmlBuildDic[lineName]))
                    {
                        //符合要求的数据
                        build_get.Add(bData);
                    }
                }
            }

            return build_get;
        }

        /// <summary>
        /// 查询某一个表的唯一值
        /// table             表名字
        /// lineName          字段名字
        /// lineValue         字段比较数据
        /// rowName           查询的列的名字
        /// </summary>
        public string XmlBuildDataSole_Get(string table, string lineName, string lineValue, string rowName)
        {
            string soleValue = "";

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic[table];
            foreach (BaseXmlBuild bData in buildList)
            {
                if (lineValue.Equals(bData.XmlBuildDic[lineName]))
                {
                    //符合要求的数据

                    if (bData.XmlBuildDic.ContainsKey(rowName))
                    {
                        //存在本数据，返回数值
                        return bData.XmlBuildDic[rowName];
                    }
                }
            }

            return soleValue;
        }

        /// <summary>
        /// 针对于结算数据表2 扩展一个查询接口
        /// </summary>
        public string XmlBuildDataDetailSheet_Get(string lineValue, string rowName)
        {
            string soleValue = "";

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["DetailSheet"];

            foreach (BaseXmlBuild bData in buildList)
            {
                DetailSheet detailData = (DetailSheet)bData;

                if (lineValue.Equals(detailData.ID))
                {
                    //符合要求的数据
                    if (string.IsNullOrEmpty(detailData.other))
                    {
                        soleValue = detailData.biaozhun;
                    }
                    else
                    {
                        string[] values = detailData.other.Split(new char[] { ';' });
                        
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                string[] dData = values[i].Split(new char[] { '=' });

                                if (dData[0].Equals(rowName))
                                {
                                    soleValue = dData[1];
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(soleValue))
                        {
                            soleValue = detailData.biaozhun;
                        }
                    }
                }
            }

            return soleValue;
        }

        /// <summary>
        /// 查询TextConfig表中的一个数据
        /// textId            文字ID
        /// </summary>
        public string XmlBuildDataText_Get(GTextsEnum textId)
        {
            string textStr = "";

            if (MemoryData.XmlData.XmlBuildDataDic.ContainsKey("TextConfig"))
            {
                List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["TextConfig"];
                foreach (BaseXmlBuild bData in buildList)
                {
                    TextConfig textConfig = (TextConfig)bData;
                    if (textConfig.ID.Equals(((int)textId).ToString()))
                    {
                        return textConfig.Value;
                    }
                }
            }

            return textStr;
        }

        /// <summary>
        /// 获取当前的地区
        /// </summary>
        public string XmlBuildDataRegion_Get(int regionId = -1)
        {
            List<string> sList = new List<string>();
            if (regionId == -1)
            {
                sList.Add(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID.ToString());
            }
            else
            {
                sList.Add(regionId.ToString());
            }

            List<BaseXmlBuild> buildList = XmlBuildData_Get("SelectRegion", "RegionID", sList);
            if (buildList.Count > 0)
            {
                SelectRegion region = (SelectRegion)buildList[0];
                return region.RegionName;
            }
            return "";
        }

        /// <summary>
        /// 查询MusicSound表中的一个数据
        /// textId            文字ID
        /// </summary>
        public string XmlBuildDataSound_Get(GEnum.SoundEnum soundId)
        {
            string soundName = "";

            if (MemoryData.XmlData.XmlBuildDataDic.ContainsKey("MusicSound"))
            {
                List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["MusicSound"];
                foreach (BaseXmlBuild bData in buildList)
                {
                    MusicSound musicSound = (MusicSound)bData;
                    if (musicSound.ID.Equals(((int)soundId).ToString()))
                    {
                        return musicSound.Name;
                    }
                }
            }

            return soundName;
        }

        /// <summary>
        /// 查询PrefabConfig表中的一个数据
        /// id            查询ID
        /// </summary>
        public PrefabConfig FindPrefabConfigById(string id)
        {
            List<BaseXmlBuild> list = MemoryData.XmlData.XmlBuildDataDic["PrefabConfig"];
            if (list == null)
            {
                DebugPro.DebugError("List<PrefabConfig> is null");
                return null;
            }
            for (int i = 0; i < list.Count; i++)
            {
                PrefabConfig config = list[i] as PrefabConfig;
                if (config == null)
                {
                    return null;
                }
                if (config.ID.Equals(id))
                {
                    return config;
                }
            }
            return null;
        }

        #endregion ------------------------------------------------------------------------
    }

    public class XmlDataCtrl : BaseXmlData
    {
        public override void Init() { }

        /// <summary>
        /// xml数据初始化
        /// </summary>
        public void XmlDataInit()
        {
            BaseXmlBuild.XmlTypeMaper = new Dictionary<string, Type>();

            BaseXmlBuild.Init();

            foreach (KeyValuePair<string, System.Type> kv in BaseXmlBuild.XmlTypeMaper)
            {
                XmlInit(kv.Key);

                List<BaseXmlBuild> typeList = new List<BaseXmlBuild>();

                foreach (List<XmlCommonData> cData in XmlDataDic.Values)
                {
                    ConstructorInfo con = kv.Value.GetConstructor(new Type[] { });
                    var info = con.Invoke(new Type[] { });

                    BaseXmlBuild build = info as BaseXmlBuild;

                    for (int i = 0; i < cData.Count; i++)
                    {
                        build.XmlBuildDic.Add(cData[i].XmlName, cData[i].XmlValue);
                    }
                    typeList.Add(build);
                }

                MemoryData.XmlData.XmlBuildDataDic.Add(kv.Key, typeList);
            }

            QLoger.LOG(" Xml数据初始化完毕 ");

            //xxx();

            if (Directory.Exists(GameConfig.Instance.ResourceLocal_Path))
            {
                LocalXmlFileDelete();
            }
        }

        /// <summary>
        /// 数据初始化完毕 把解压到本地的xml文件删除
        /// </summary>
        void LocalXmlFileDelete()
        {
            Tools_FileOperation.Files_DeleteAll(GameConfig.Instance.XmlUnZip_Path);
        }

        /*
        void xxx()
        {
            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["regiontable"];

            foreach (BaseXmlBuild build in buildList)
            {
                regiontable award = (regiontable)build;
                string str1 = award.RegionID;
                string str2 = award.RegionName;
                QLoger.LOG(" regiontable Data = " + str1 + str2);
            }
        }
        */
    }

    #region 内存数据-------------------------------------------------------------------

    public partial class MKey
    {
        public const string USER_XML_DATA = "USER_XML_DATA";
    }

    public partial class MemoryData
    {
        static public XmlData XmlData
        {
            get
            {
                XmlData xmlData = MemoryData.Get<XmlData>(MKey.USER_XML_DATA);
                if (xmlData == null)
                {
                    xmlData = new XmlData();
                    MemoryData.Set(MKey.USER_XML_DATA, xmlData);
                }
                return xmlData;
            }
        }
    }

    #endregion ------------------------------------------------------------------------
}