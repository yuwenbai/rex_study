/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using projectQ;

public static class UIChatManager
{
    public static bool isOpenPage = false;
}

public class UIChatModel : UIModelBase
{
    public UIChat UI
    {
        get { return _ui as UIChat; }
    }
    private List<TextConfig> ConfigDataList = new List<TextConfig>();

    public void OnSendReq(string chatMsg)
    {
        ModelNetWorker.Instance.ChatReq(chatMsg);
    }
    /// <summary>
    /// 读取xml
    /// </summary>
    public void Text_LoadXml()
    {
        //XmlTextConfigData data = new XmlTextConfigData();
        //XmlData.XmlInit<XmlTextConfigData>(data);
        //ConfigDataList = XmlTextConfigData.TextConfigList;

        if (ConfigDataList.Count <= 0)
        {
            List<BaseXmlBuild> configList = MemoryData.XmlData.XmlBuildDataDic["TextConfig"];
            foreach (BaseXmlBuild build in configList)
            {
                TextConfig info = (TextConfig)build;
                ConfigDataList.Add(info);
            }
        }
    }

    public TextConfig GetDataById(int id)
    {
        TextConfig data = new TextConfig();
        for (int i = 0; i < ConfigDataList.Count; i++)
        {
            if (id == int.Parse(ConfigDataList[i].ID))
            {
                data = ConfigDataList[i];
            }
        }
        return data;
        //return XmlTextConfigData.GetDataById(id);

    }
    /// <summary>
    /// 通过type获取数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<TextConfig> GetDataByType(string type)
    {
        List<TextConfig> dataList = new List<TextConfig>();
        for (int i = 0; i < ConfigDataList.Count; i++)
        {
            if (type.Equals(ConfigDataList[i].TYPE))
            {
                dataList.Add(ConfigDataList[i]);
            }
        }
        return dataList;
        //return XmlTextConfigData.GetDataListByType(type);
    }
    #region override
    protected override GEnum.NamedEvent[] FocusNetWorkData()
    {
        return new GEnum.NamedEvent[]
        {
            GEnum.NamedEvent.EMjChatNotify
        };
    }

    protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
    {
        switch (msgEnum)
        {
            case GEnum.NamedEvent.EMjChatNotify:

                UI.ShowChat(System.Convert.ToInt64(data[1]), (int)(data[0]), data[2] as string);



                break;
        }
    }
    #endregion
}
