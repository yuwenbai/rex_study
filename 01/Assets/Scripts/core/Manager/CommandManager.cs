using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Msg;

namespace projectQ
{
    //public class CommandData{
    //    public string key = null;
	   // private object[] _data = null;
    //    public object[] Data {
    //        set { _data = value; }
    //        get { return _data; }
    //    }
        
    //    public T GetData<T>(int index)
	   // {
		  //  if(_data != null && _data.Length > index && _data[index] != null)
			 //   return (T)_data[index];
		  //  else
			 //   return default(T);
	   // }

	   // public void Clear()
	   // {
		  //  _data = null;
	   // }
    //}
    //public class CommandManager : BaseManager
    //{
    //    public enum CommandType
    //    {
    //        Normal = 0,
    //        NetWork,
    //        UI,
    //    }

    //    public delegate void OnEventCallBack(CommandData data);

	   // private Dictionary<string,CommandData> dataDic = new Dictionary<string, CommandData>();
	   // private Dictionary<string,OnEventCallBack> eventDic = new Dictionary<string, OnEventCallBack>();

	   // private const string Prefix_CmdNo = "cmdno_";
    //    private const string Prefix_UI = "ui_";
    //    private System.Text.StringBuilder tempStr;


    //    #region 普通消息
    //    //注册消息
    //    public void RegEvent(string key, CommandType type, OnEventCallBack func)
	   // {
    //        key = GetKey(key, type);

    //        if (!eventDic.ContainsKey(key))
		  //  {
			 //   eventDic.Add(key, null);
		  //  }
		  //  if(eventDic[key] != null)
			 //   eventDic[key] -= func;

		  //  eventDic[key] += func;
	   // }
	   // //反注册
	   // public void UnRegEvent(string key, CommandType type, OnEventCallBack func)
	   // {
    //        key = GetKey(key, type);

    //        if (eventDic.ContainsKey(key) && eventDic[key] != null)
		  //  {
			 //   eventDic[key] -= func;
		  //  }
	   // }
	   // //发送消息
	   // public void SendEvent(string key, CommandType type,params object[] data)
	   // {
    //        String ComKey = GetKey(key, type);
    //        CommandData cData = new CommandData();
    //        cData.key = key;
    //        cData.Data = data;
    //        if (eventDic.ContainsKey(ComKey) && eventDic[ComKey] != null)
    //        {
    //            eventDic[ComKey](cData);
    //        }
    //    }

    //    public void SendEvent(string key,CommandData data)
    //    {
    //        if (eventDic.ContainsKey(key) && eventDic[key] != null)
    //        {
    //            eventDic[key](data);
    //        }
    //    }


    //    #endregion

    //    #region 数据消息
    //    //设置数据 并发送消息
    //    public void SetData(string key, CommandType type, object[] data)
	   // {
    //        String ComKey = GetKey(key, type);
    //        CommandData cData = new CommandData();
    //        cData.key = key;
    //        cData.Data = data;

    //        if (dataDic.ContainsKey(ComKey))
			 //   dataDic[ComKey] = cData;
		  //  else
			 //   dataDic.Add(ComKey, cData);

		  //  SendEvent(ComKey, cData);
	   // }

	   // public CommandData GetData(string key, CommandType type,bool isDel = false)
	   // {
    //        key = GetKey(key, type);
    //        CommandData result = null;
    //        if (dataDic.ContainsKey(key))
    //        {
    //            result = dataDic[key];
    //            if (isDel)
    //            {
    //                dataDic.Remove(key);
    //            }
    //        }
		  //  return result;
	   // }
	   // #endregion

	   // #region 网络消息
	   // public void RegNetEvent(CmdNo net,OnEventCallBack func)
	   // {		
		  //  RegEvent(net.ToString(),CommandType.NetWork, func);
	   // }

	   // public void UnRegNetEvent(CmdNo net, OnEventCallBack func)
	   // {
		  //  UnRegEvent(net.ToString(),CommandType.NetWork,func);
	   // }

	   // public void SendNetEvent(CmdNo net, params object[] data)
	   // {
		  //  SendEvent(net.ToString(), CommandType.NetWork, data);
	   // }

    //    #endregion

    //    #region UI消息
    //    public void RegUIEvent(string uiName, OnEventCallBack func)
    //    {
    //        RegEvent(uiName, CommandType.UI, func);
    //    }

    //    public void UnRegUIEvent(string uiName, OnEventCallBack func)
    //    {
    //        UnRegEvent(uiName, CommandType.UI, func);
    //    }

    //    public void SendUIEvent(string uiName, params object[] data)
    //    {
    //        SendEvent(uiName, CommandType.UI, data);
    //    }
    //    #endregion

    //    private string GetKey(string key, CommandType type)
    //    {
    //        if (tempStr == null)
    //            tempStr = new System.Text.StringBuilder();
    //        tempStr.Length = 0;
    //        switch (type)
    //        {
    //            case CommandType.NetWork:
    //                tempStr.Append(Prefix_CmdNo).Append(key);
    //                break;
    //            case CommandType.UI:
    //                tempStr.Append(Prefix_UI).Append(key);
    //                break;
    //            default:
    //                return key;
    //        }

    //        return tempStr.ToString();
    //    }

    //    #region override
    //    public override void Init()
    //    {

    //    }

    //    public override void Dispose()
    //    {

    //    }
    //    #endregion
    //}
}
