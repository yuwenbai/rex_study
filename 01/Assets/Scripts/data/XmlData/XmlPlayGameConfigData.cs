/**
* @Author YQC
*
*
*/
using System;
using Msg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Reflection;

namespace projectQ
{
    namespace projectQ.XmlData
    {
        public class PlayGameConfigData
        {
            private List<GameConfig> gameList;

            public List<GameConfig> GameList
            {
                get
                {
                    if (gameList == null)
                        gameList = new List<GameConfig>();
                    return gameList;
                }

                set { gameList = value; }
            }

        }
        public class GameConfig
        {
            private int mjType;
            private int configID;
            private int gameID;
            private string gameName;
            private int regionID;
            private int cityID;
            private int sortID;
            private List<RulerConfig> rulerList;

            public int MjType
            {
                get { return mjType; }

                set { mjType = value; }
            }

            public int GameID
            {
                get { return gameID; }

                set { gameID = value; }
            }

            public int ConfigID
            {
                set { configID = value; }
                get { return configID; }
            }

            public string GameName
            {
                get { return gameName; }
                set { gameName = value; }
            }

            public int RegionID
            {
                get { return regionID; }
                set { regionID = value; }
            }

            public int CityID
            {
                get { return cityID; }
                set { cityID = value; }
            }

            public int SortID
            {
                get { return sortID; }
                set { sortID = value; }
            }

            public List<RulerConfig> RulerList
            {
                get {
                    if (rulerList == null) rulerList = new List<RulerConfig>();
                    return rulerList;
                }

                set { rulerList = value; }
            }
        }

        public class RulerConfig
        {
            private int rulerType;
            private string rulerTypeName;
            private string rulerTypeDesc;
            private int groupTypeDef;
            private int rulerSort;
            private int rulerTypeSort;
            private List<RulerData> rulerData;

            public int RulerType
            {
                get { return rulerType; }

                set { rulerType = value; }
            }

            public string RulerTypeName
            {
                get { return rulerTypeName; }

                set { rulerTypeName = value; }
            }

            public string RulerTypeDesc
            {
                get { return rulerTypeDesc; }

                set { rulerTypeDesc = value; }
            }

            public int GroupTypeDef
            {
                get { return groupTypeDef; }
                set { groupTypeDef = value; }
            }

            public int RulerSort
            {
                get { return rulerSort; }
                set { rulerSort = value; }
            }
            public int RulerTypeSort
            {
                get { return rulerTypeSort; }
                set { rulerTypeSort = value; }
            }
            

            public List<RulerData> RulerData
            {
                get
                {
                    if (rulerData == null)
                        rulerData = new List<RulerData>();
                    return rulerData;
                }

                set { rulerData = value; }
            }
        }

        public class RulerData
        {
            //private int rulerID;
            private string rulerName;
            private int normalSet;

            //2017/07/10 新增
            //唯一ID
            private int rulerConfigID;
            //原始ID
            private int rulerTypeID;
            //选项类型
            private int paramShowType;
            private string mutexID;
            private string front;
            private int sort;
            private int paramType;
            private int param1;
            private int param2;

            private int alignType;
            private int row;
            private int column;
            //private string param3;

            /*  public int RulerID
              {
                  get { return rulerID; }
                  set { rulerID = value; }
              }
              */
            public string RulerName
            {
                get { return rulerName; }
                set { rulerName = value; }
            }

            public int NormalSet
            {
                get { return normalSet; }
                set { normalSet = value; }
            }

            public int RulerConfigID
            {
                get { return rulerConfigID; }

                set { rulerConfigID = value; }
            }

            public int RulerTypeID
            {
                get { return rulerTypeID; }

                set { rulerTypeID = value; }
            }

            public int ParamShowType
            {
                get { return paramShowType; }

                set { paramShowType = value; }
            }

            public string MutexID
            {
                get { return mutexID; }

                set { mutexID = value; }
            }

            public string Front
            {
                get { return front; }

                set { front = value; }
            }

            public int Sort
            {
                get { return sort; }

                set { sort = value; }
            }
            
            public int ParamType
            {
                get { return paramType; }

                set { paramType = value; }
            }

            public int Param1
            {
                get { return param1; }

                set { param1 = value; }
            }

            public int Param2
            {
                get { return param2; }

                set { param2 = value; }
            }

            public int AlignType
            {
                get
                {
                    return alignType;
                }

                set
                {
                    alignType = value;
                }
            }

            public int Row
            {
                get
                {
                    return row;
                }

                set
                {
                    row = value;
                }
            }

            public int Column
            {
                get
                {
                    return column;
                }

                set
                {
                    column = value;
                }
            }

            //public string Param3
            //{
            //    get { return param3; }

            //    set { param3 = value; }
            //}
        }
    }
    public class XmlPlayGameConfigData : BaseXmlData
    {
        public Dictionary<string, Type> PlayGameXmlTypeMap = new Dictionary<string, Type>();

        public override void Init()
        {
            MemoryData.Set(MKey.MJ_PLAY_DATA, null);
            MemoryData.Set(MKey.MJ_PLAY_DATA_XML, null);

            PlayGameXmlTypeMap = new Dictionary<string, Type>();
            PlayGameXmlTypeMap.Add("GameConfig", typeof(projectQ.XmlData.GameConfig));
            PlayGameXmlTypeMap.Add("RulerConfig", typeof(projectQ.XmlData.RulerConfig));
            PlayGameXmlTypeMap.Add("RulerData", typeof(projectQ.XmlData.RulerData));

            LoadXml(XmlDataEnum.XML_GAMECONFIG_FASHION);
        }

        public void LoadXml(XmlDataEnum xmlEnum)
        {
            XmlDocument xml = XmlInit(xmlEnum);

            XmlNodeList xmlNodeList = xml.SelectSingleNode("GameList").ChildNodes;

            NodeListTo(MemoryData.MahjongPlayDataXML.XMLPlayData, xmlNodeList, "GameList");
        }

        private void NodeListTo(object obj, XmlNodeList xmlNodeList,string fild = null)
        {
            foreach (XmlNode item in xmlNodeList)
            {
                NoteTo(item, obj , fild);
            }
        }

        //如果填入字段不会有返回值
        private void NoteTo(XmlNode note, object obj, string listName = null)
        {
            if (note.ChildNodes.Count == 0) return ;

            //纯粹的单个值
            if(note.ChildNodes.Count == 1 && note.ChildNodes[0].NodeType == XmlNodeType.Text)
            {
                if (obj != null)
                {
                    XmlToProperty(obj, note);
                }
            }
            else
            {
                if(IsClass(note))
                {
                    object result = XmlToNewClass(note);

                    if(obj != null && result != null)
                    {
                        SetValueByStrAttribute(obj, string.IsNullOrEmpty(listName)?note.Name: listName, result);

                        if (note.ChildNodes.Count > 0)
                        {
                            NodeListTo(result, note.ChildNodes);
                        }
                    }
                }
                else
                {
                    if (note.ChildNodes.Count > 0)
                    {
                        NodeListTo(obj, note.ChildNodes, note.Name);
                    }
                }
            }
        }

        private bool IsClass(XmlNode note)
        {
            return PlayGameXmlTypeMap.ContainsKey(note.Name) ;
        }

        private object XmlToNewClass(XmlNode note)
        {
            return CreateNewClass(note.Name);
        }

        private void XmlToProperty(object obj, XmlNode note)
        {
            SetValueByStrAttribute(obj, note.Name, note.InnerText);
        }

        private object CreateNewClass(string className)
        {
            if (PlayGameXmlTypeMap.ContainsKey(className))
            {
                ConstructorInfo con = PlayGameXmlTypeMap[className].GetConstructor(new Type[] { });
                return con.Invoke(new Type[] { });
            }
            return null;
        }

        private void SetValueByStrAttribute(object obj, string strAttribute, object value)
        {
            if (value == null || string.IsNullOrEmpty(strAttribute) || obj == null) return;
            PropertyInfo propertyInfoName = (obj.GetType()).GetProperty(strAttribute);

            if (propertyInfoName == null) return;

            if (propertyInfoName.PropertyType.IsGenericType)
            {
                object list = propertyInfoName.GetValue(obj, null);
                Type objType = list.GetType();
                MethodInfo addMethod= objType.GetMethod("Add");
                addMethod.Invoke(list, new object[] { value });
            }
            else
            {
                string strValue = value as string;

                if (string.IsNullOrEmpty(strValue)) return;
				try {
					switch (propertyInfoName.PropertyType.ToString())
					{
					case "System.Int32":
						propertyInfoName.SetValue(obj, int.Parse(strValue), null);
						break;
					case "System.String":
						propertyInfoName.SetValue(obj, value, null);
						break;
					}

				} catch (Exception ex) {
					QLoger.ERROR (strValue + ex );
				}
            }
        }

    }
}
