/****************************************************
*
*  Xml 创建  修改  添加  工具类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using System.IO;
using System.Xml;

public class Tool_XmlOperation
{
    #region lyb XML创建-----------------------------------------

    /// <summary>
    /// 创建Xml
    /// </summary>
    /// <param name="path">Xml路径</param>
    public static void Xml_Creat(string path)
    {
        if (!File.Exists(path))
        {
            //创建最上一层的节点。
            XmlDocument xml = new XmlDocument();

            //创建最上一层的节点。
            XmlElement root = xml.CreateElement("objects");

            xml.AppendChild(root);

            //保存文件
            xml.Save(path);
        }
    }

    #endregion -------------------------------------------------

    #region lyb XML创建一个新的节点-----------------------------

    /// <summary>
    /// 创建一个新的xnl节点
    /// </summary>
    /// <param name="path">Xml路径</param>
    /// <param name="elementName">节点的名字</param>
    public static void Xml_CreatElement(string path, string elementName)
    {
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();

            xml.Load(path);

            XmlNode root = xml.SelectSingleNode("objects");

            XmlElement element = xml.CreateElement("messages");

            element.SetAttribute("id", elementName);

            root.AppendChild(element);

            xml.AppendChild(root);

            xml.Save(path);
        }
    }

    #endregion -------------------------------------------------

    #region lyb XML更新版本号信息-------------------------------

    /// <summary>
    /// 修改xml中的版本信息
    /// 替换的版本信息
    /// </summary>
    public static void Xml_UpdateVersion(string path, string NewVersion)
    {
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();

            xml.Load(path);

            XmlNode node = xml.SelectSingleNode("objects");

            XmlElement ele = (XmlElement)node;

            if (ele != null && ele.Attributes["version"] != null)
            {
                ele.Attributes["version"].Value = NewVersion;
            }

            xml.Save(path);
        }
    }

    #endregion -------------------------------------------------

    #region lyb XML修改-----------------------------------------

    /// <summary>
    /// 修改Xml
    /// </summary>
    /// <param name="path">Xml路径</param>
    /// <param name="uName">要进行更新的名字</param>
    /// <param name="idAfter">修改后的id</param>
    /// <param name="docAfter">修改后的内容</param>
    public static void Xml_Update(string path, string uName, string idAfter, string docAfter)
    {
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            
            XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
            foreach (XmlElement xl1 in xmlNodeList)
            {
                if (!string.IsNullOrEmpty(idAfter))
                {
                    xl1.SetAttribute("id", idAfter);
                }

                foreach (XmlElement xl2 in xl1.ChildNodes)
                {
                    if (xl2.GetAttribute("name") == uName)
                    {
                        xl2.InnerText = docAfter;
                    }
                }
            }
            xml.Save(path);
        }
    }

    #endregion -------------------------------------------------

    #region lyb XML添加-----------------------------------------

    /// <summary>
    /// Xml添加
    /// </summary>
    /// <param name="path">Xml路径</param>
    /// <param name="id">在哪个Id下进行添加</param>
    /// <param name="name">添加的名字</param>
    /// <param name="doc">添加的内容</param>
    /// <param name="type">0-普通配置文件，1-数据表配置文件</param>
    public static void Xml_Add(string path, string id, string name, string doc, int type = 0)
    {
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            XmlNode root = xml.SelectSingleNode("objects");

            XmlElement element = xml.GetElementById(id);
            XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
            foreach (XmlElement xl1 in xmlNodeList)
            {
                if (xl1.GetAttribute("id").Equals(id))
                {
                    element = xl1;
                }
            }

            XmlElement elementChild1;

            if (type == 0)
            {
                elementChild1 = xml.CreateElement("contents");
                elementChild1.SetAttribute("name", name);
            }
            else
            {
                elementChild1 = xml.CreateElement(name);
            }

            elementChild1.InnerText = doc;

            //把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
            element.AppendChild(elementChild1);

            root.AppendChild(element);

            xml.AppendChild(root);

            //保存文件
            xml.Save(path);
        }
    }

    #endregion -------------------------------------------------

    #region lyb XML删除-----------------------------------------

    /// <summary>
    /// Xml删除
    /// </summary>
    /// <param name="path">Xml路径</param>
    /// <param name="nodeId">节点id</param>
    /// <param name="name">删除的名字</param>
    public static void Xml_Delete(string path, string nodeId, string name)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("objects").ChildNodes;
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.GetAttribute("id").Equals(nodeId))
            {
                foreach (XmlElement xl2 in xl1.ChildNodes)
                {
                    if (xl2.GetAttribute("name").Equals(name))
                    {
                        xl2.ParentNode.RemoveChild(xl2);
                        break;
                    }
                }
            }
        }
        xmlDoc.Save(path);
    }

    #endregion -------------------------------------------------

    #region lyb XML查找-----------------------------------------

    /// <summary>
    /// 修改Xml
    /// </summary>
    /// <param name="path">Xml路径</param>
    /// <param name="nodeId">查找的节点Id</param>
    /// <param name="name">查找的节点名字</param>
    public static string Xml_Find(string path, string nodeId, string name)
    {
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
            foreach (XmlElement xl1 in xmlNodeList)
            {
                if (xl1.GetAttribute("id").Equals(nodeId))
                {
                    foreach (XmlElement xl2 in xl1.ChildNodes)
                    {
                        if (xl2.GetAttribute("name").Equals(name))
                        {
                            return xl2.InnerText;
                        }
                    }
                }
            }
        }

        return null;
    }

    #endregion -------------------------------------------------
}