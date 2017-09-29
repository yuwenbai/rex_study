using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Msg;
using projectQ;
//public interface IFilter
//{
//    bool Check(Msg.ReplayRecord mRecord);
//}
//public class Block : IFilter
//{
//    //全部屏蔽掉
//    private List<CmdNo> BlackMsgList = new List<CmdNo>();
//    public Block()
//    {
//        BlackMsgList.Add(CmdNo.MjCmd_OpAction_Rsp);
//    }
//    public virtual bool Check(Msg.ReplayRecord mRecord)
//    {
//        for (int i = 0; i < BlackMsgList.Count; ++i)
//        {
//            if ((CmdNo)mRecord.Xxmsg.MsgID == BlackMsgList[i])
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//public class Ruler : IFilter
//{
//    //全部屏蔽掉
//    private List<CmdNo> RulerSingleList = new List<CmdNo>();
//    public Ruler()
//    {
//        //过滤名单 过滤掉非当前玩家的消息
//        //加入房间 只需要当前主角的 故过滤掉
//        RulerSingleList.Add(CmdNo.MjCmd_JoinDesk_Rsp);
//        RulerSingleList.Add(CmdNo.MjCmd_SyncPlayerState_Notify);
//        RulerSingleList.Add(CmdNo.MjCmd_OpAction_Rsp);
//        RulerSingleList.Add(CmdNo.MjCmd_ScoreChange_Notify);
//        RulerSingleList.Add(CmdNo.MjCmd_BalanceNewNotify);
//    }
//    public virtual bool Check(Msg.ReplayRecord mRecord)
//    {
//        ulong selfUserId = MjDataManager.Instance.MjData.curUserData.selfUserID;
//        for (int i = 0; i < RulerSingleList.Count; ++i)
//        {
//            if ((mRecord.PlayerID != (long)selfUserId) && ((CmdNo)mRecord.Xxmsg.MsgID == RulerSingleList[i]))
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//public class Cache : IFilter
//{
//    //全部屏蔽掉
//    private Dictionary<CmdNo, List<Msg.ReplayRecord>> RulerCacheList = new Dictionary<CmdNo, List<Msg.ReplayRecord>>();
//    public Cache()
//    {
//        RulerCacheList[CmdNo.MjCmd_DeskUsers_Notify] = new List<Msg.ReplayRecord>(4);
//    }
//    public virtual bool Check(Msg.ReplayRecord mRecord)
//    {
//        CmdNo no = (CmdNo)mRecord.Xxmsg.MsgID;
//        List<Msg.ReplayRecord> tempList = null;
//        RulerCacheList.TryGetValue(no, out tempList);

//        if (tempList == null)
//            return true;

//        for (int i = 0; i < tempList.Count; ++i)
//        {
//            if (tempList[i].PlayerID == mRecord.PlayerID)
//            {
//                return false;
//            }
//        }
//        tempList.Add(mRecord);

//        return true;
//    }
//}
public class FakeReplayFilter : SingletonTamplate<FakeReplayFilter>
{
    private List<CmdNo> RulerSingleList;
    private List<CmdNo> BlackMsgList;
    private List<CmdNo> StateMsgList;

    private Dictionary<CmdNo, List<Msg.DeskRecordFrame>> RulerCacheList;
    public FakeReplayFilter()
    {
        //全部屏蔽掉
        BlackMsgList = new List<CmdNo>();
        BlackMsgList.Add(CmdNo.MjCmd_OpAction_Rsp);
        BlackMsgList.Add(CmdNo.MjCmd_PlayerWarning_Notify);
        //test  !!!
        //BlackMsgList.Add(CmdNo.MjCmd_DeskAction_Notify);
        //BlackMsgList.Add(CmdNo.MjCmd_DeskAction_Rsp);
        //test  !!!

        //过滤名单 过滤掉非当前玩家的消息
        //加入房间 只需要当前主角的 故过滤掉
        RulerSingleList = new List<CmdNo>();
        RulerSingleList.Add(CmdNo.MjCmd_JoinDesk_Rsp);
        RulerSingleList.Add(CmdNo.MjCmd_SyncPlayerState_Notify);
        RulerSingleList.Add(CmdNo.MjCmd_OpAction_Rsp);
        RulerSingleList.Add(CmdNo.MjCmd_ScoreChange_Notify);
        RulerSingleList.Add(CmdNo.MjCmd_BalanceNewNotify);
        RulerSingleList.Add(CmdNo.MjCmd_ChangeThreeNotify);
        RulerSingleList.Add(CmdNo.MjCmd_StandingPlates);
        RulerSingleList.Add(CmdNo.MjCmd_AskReqConfirm);
        RulerSingleList.Add(CmdNo.MjCmd_AskReqChangeThree);
        RulerSingleList.Add(CmdNo.MjCmd_AskReqPao);
        RulerSingleList.Add(CmdNo.MjCmd_DeskUsers_Notify);



        RulerCacheList = new Dictionary<CmdNo, List<Msg.DeskRecordFrame>>();
        RulerCacheList[CmdNo.MjCmd_DeskUsers_Notify] = new List<Msg.DeskRecordFrame>(4);

        StateMsgList = new List<CmdNo>(4);
        StateMsgList.Add(CmdNo.MjCmd_OpAction_Notify);

    }
    public bool LegalCheck(Msg.DeskRecordFrame mRecord)
    {
        bool bCacheCheck = true;
        for (int i = 0; i < BlackMsgList.Count; ++i)
        {
            if((CmdNo)mRecord.Xxmsg.MsgID == BlackMsgList[i])
            {
                bCacheCheck =  false;
                break;
            }
        }
        if (bCacheCheck)
        {
            ulong selfUserId = MjDataManager.Instance.MjData.curUserData.selfUserID;
            for (int i = 0; i < RulerSingleList.Count; ++i)
            {
                if ((mRecord.PlayerID != (long)selfUserId) && ((CmdNo)mRecord.Xxmsg.MsgID == RulerSingleList[i]))
                {
                    bCacheCheck = false;
                    break;
                }
            }
        }

        if (bCacheCheck)
        {
            bCacheCheck = CacheCheck(mRecord);
        }
        if (bCacheCheck)
        {
            bCacheCheck = StateCheck(mRecord);
        }
        return bCacheCheck;
    }
    private bool CacheCheck(Msg.DeskRecordFrame mRecord)
    {
        CmdNo no = (CmdNo)mRecord.Xxmsg.MsgID;
        List<Msg.DeskRecordFrame> tempList = null;
        RulerCacheList.TryGetValue(no, out tempList);

        if (tempList == null)
            return true;

        //for (int i = 0; i < tempList.Count; ++i)
        //{
        //    if (tempList[i].PlayerID == mRecord.PlayerID)
        //    {
        //        return false;
        //    }
        //}
        tempList.Add(mRecord);

        return true;
    }
    //吃碰杠时候 接收到服务器的消息 只处理发起吃碰杠的user
    private bool StateCheck(Msg.DeskRecordFrame mRecord)
    {
        CmdNo no = (CmdNo)mRecord.Xxmsg.MsgID;
        for (int i = 0; i < StateMsgList.Count; ++i)
        {
            if ((CmdNo)mRecord.Xxmsg.MsgID == StateMsgList[i])
            {
                //吃碰杠只保留自己的
                if (mRecord.TouchPlayerID > 0)
                {
                    if (MjDataManager.Instance.MjData.curUserData.selfUserID != (ulong)mRecord.PlayerID)
                        return false;
                }
                else
                {
                    ulong self = MjDataManager.Instance.MjData.curUserData.selfUserID;
                    ulong target = (ulong)mRecord.PlayerID;
                    if (self != target)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public void ClearCache()
    {
        foreach (var vv in RulerCacheList)
        {
            vv.Value.Clear();
        }
    }
}
