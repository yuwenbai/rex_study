using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Msg;
using projectQ;

public abstract class UIModelBase : MonoBehaviour
{
    [HideInInspector]
    public UIViewBase _ui;

    public void RegistEvent()
    {
        //注册网络消息
        GEnum.NamedEvent[] tempCmNo = FocusNetWorkData();
        if (tempCmNo != null && tempCmNo.Length > 0)
        {
            for (int i = 0; i < tempCmNo.Length; ++i)
            {
                EventDispatcher.AddEvent(tempCmNo[i], NetWorkDataCallBack);
            }
        }
    }

    public void UnRegistEvent()
    {
        GEnum.NamedEvent[] tempCmNo = FocusNetWorkData();
        if (tempCmNo != null && tempCmNo.Length > 0)
        {
            for (int i = 0; i < tempCmNo.Length; ++i)
            {
                EventDispatcher.RemoveEvent(tempCmNo[i], NetWorkDataCallBack);
            }
        }
    }



    #region 网络消息
    /// <summary>
    /// 需要注册的网络消息
    /// </summary>
    protected virtual GEnum.NamedEvent[] FocusNetWorkData()
    {
        return null;
    }

    /// <summary>
    /// 网络消息的回掉
    /// </summary>
    /// <param name="msgEnum"></param>
    /// <param name="data"></param>
    protected virtual void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
    {
    }
    #endregion

    #region Event
    private void NetWorkDataCallBack(object[] data)
    {
        if (data != null && data.Length > 0)
        {
            object[] temp = new object[data.Length - 1];
            for (int i = 0; i < temp.Length; ++i)
            {
                temp[i] = data[i + 1];
            }
            OnNetWorkDataCallBack((GEnum.NamedEvent)data[0], temp);
        }
    }

    public virtual void OnEnable()
    {
        this.RegistEvent();
    }

    public virtual void OnDisable()
    {
        this.UnRegistEvent();
    }
    #endregion
}
