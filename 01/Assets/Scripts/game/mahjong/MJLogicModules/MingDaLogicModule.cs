using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;

public class MingDaLogicModule : SpecialLogicModule
{
    public override void AddEvents()
    {
        m_EventHelper.AddEvent(MJEnum.MingDaEvents.MD_LogicNotify.ToString(), NotifyMingDa);
        m_EventHelper.AddEvent(MJEnum.MingDaEvents.MD_RspNotify.ToString(), RspNotifyMingDa);
        m_EventHelper.AddEvent(MJEnum.MingDaEvents.MD_LogicUploadData.ToString(), LogicUploadData);
    }

    private void NotifyMingDa(object[] param)
    {
        int subType = (int)param[0];
        EnumMjSelectSubType curSubType = (EnumMjSelectSubType)subType;
        if (curSubType == EnumMjSelectSubType.MjSelectSubType_NoOperation)
            return;

        bool isUpdate = true;

        if (curSubType == EnumMjSelectSubType.MjSelectSubType_WAIT_Select)
        {
            //每个人的选择
            if (param.Length > 2)
            {
                int seatID = (int)param[1];
                List<int> rulerID = (List<int>)param[2];
                List<int> valueList = (List<int>)param[3];

                LogicMingDaRspData(seatID, rulerID, valueList);
            }
            else
            {
                //重连
                LogicMjMingDaData();
            }
        }
        else
        {
            //通知开始或者结束
            LogicMjMingDaData();

            if (curSubType == EnumMjSelectSubType.MjSelectSubType_RESULT_Select)
                isUpdate = false;
        }

        if (isUpdate)
        {
            List<int> list = MjDataManager.Instance.GetAllSeatIDCurDesk(true);
            for (int i = 0; i < list.Count; i++)
            {
                EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_ShowRefresh.ToString(), list[i]);
            }
        }
    }

    private void RspNotifyMingDa(object[] param)
    {
        int subType = (int)param[0];
        if (param.Length < 2)
        {
            EnumMjSelectSubType curSubType = (EnumMjSelectSubType)subType;
            if (curSubType == EnumMjSelectSubType.MjSelectSubType_WAIT_Select)
            {
                //每个人的选择
                int seatID = (int)param[1];

            }
            else
            {
                //通知开始或者结束

            }
        }
        else
        {
            int seatID = (int)param[1];


        }
    }

    private void LogicUploadData(object[] param)
    {
        if (MjDataManager.Instance.isOutMingDa)
        {
            MjMingDaData data = MjDataManager.Instance.GetMainPlayer();
            if (data != null)
            {
                EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_LogicNotify.ToString(), (int)data.SelectState);
            }
        }
    }

    public override void ClearUp()
    {
    }

    /// <summary>
    /// 明打个人（桌子内的某人）数据更新
    /// </summary>
    /// <param name="seatID"></param>
    /// <param name="curType"></param>
    /// <param name="curValue"></param>
    public void LogicMingDaRspData(int seatID, List<int> curType, List<int> curValue)
    {
        if (curType != null && curType.Count > 0 && curValue != null && curValue.Count == curType.Count)
        {
            for (int i = 0; i < curType.Count; i++)
            {
                MjDataManager.Instance.UpdateMingDaData(seatID, curType[i], curValue[i]);
            }

            EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_ShowRefresh.ToString(), seatID);
        }
    }

    /// <summary>
    /// 明打（所有相关）数据获得时回调
    /// </summary>
    /// <param name="dataList"></param>
    public void LogicMjMingDaData()
    {
        EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_ShowUI.ToString());
    }
}
