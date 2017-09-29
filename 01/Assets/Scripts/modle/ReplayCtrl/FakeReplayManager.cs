using UnityEngine;
using projectQ;
using System.Collections.Generic;
using Msg;

public class FakeReplayData
{
    private int tTick = 0;
    public int Tick
    {
        get { return tTick; }
        set { tTick = value; }
    }
    private ProtoMessage pmsg;
    public ProtoMessage ProMsg
    {
        get { return pmsg; }
        set { pmsg = value; }
    }
    private int seatId;
    public int SeatId
    {
        get { return seatId; }
        set { seatId = value; }
    }
    private List<int> mjCodeList;
    public List<int> MjCodeList
    {
        get { return mjCodeList; }
        set { mjCodeList = value; }
    }
}
public class CacheReplayData
{
    public int RoundIndex;
    public Queue<FakeReplayData> MessQueue = new Queue<FakeReplayData>();
};
public partial class FakeReplayManager : SingletonTamplate<FakeReplayManager>
{
    private float fBaseTime = 0f;
    private float fPauseTime = 0f;
    private float fCurrentTime = 0f;
    private int nInitSpeed = 1;
    private int nCurrentSpeed = 1;
    private int _nRoundIndex = -1;
    private int _nDeskId = -1;

    //一桌最多20局 目前只有16局
    const int MAX_ROUND_NUM = 20;
    //每个人最多几张牌？也就14张 同时补牌也就4张吧？
    const int MAX_NUM_TIME_CARD = 4;


    private bool bPause = false;
    public bool Pause
    {
        get { return bPause; }
        set { bPause = value; }
    }
    private bool bDataOver = false;
    public bool DataOver
    {
        get { return bDataOver; }
    }

    private bool bReplayState = false;
    public bool ReplayState
    {
        get { return bReplayState; }
        set { bReplayState = value; }
    }
    private Queue<FakeReplayData> messQueue = new Queue<FakeReplayData>();
    public Queue<FakeReplayData> MessQueue
    {
        get { return messQueue; }
    }

    private Queue<FakeReplayData> replaymessQueue = new Queue<FakeReplayData>();


    public int CurrentSpeed
    {
        get { return nCurrentSpeed; }
        set
        {
            nCurrentSpeed = value;
            Time.timeScale = nCurrentSpeed;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Speed);
        }
    }
    private Dictionary<int, List<int>> _DiskInfo = new Dictionary<int, List<int>>();

    //亮牌
    Dictionary<int, List<CacheReplayData>> dictionaryCache = new Dictionary<int, List<CacheReplayData>>();
    //摸牌 最多缓存3家牌？
    Dictionary<int, MjChangeThreeData> changeThreeCardCache = new Dictionary<int, MjChangeThreeData>();

    //请求某玩家回放数据
    public void RequestReplayFrame(int nDeskId, int nIndex)
    {
        QLoger.ERROR("FakeReplayManager -----> RequestReplayFrame" + nDeskId);

        MjDataManager.Instance.MjData.curUserData.selfUserID = (ulong)MemoryData.UserID;
        MjDataManager.Instance.MjData.curUserData.selfDeskID = nDeskId;
        FakeReplayFilter.Instance.ClearCache();
        StopReplay();
        messQueue.Clear();

        _nDeskId = nDeskId;
        _nRoundIndex = nIndex;
        Queue<FakeReplayData> hasqueue = GetCacheRecord(_nDeskId, nIndex);
        if (hasqueue == null)
        {
            ModelNetWorker.Instance.C2SMessageReplayRoundReq(nDeskId, nIndex);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "");
        }
        else
        {
            foreach (var item in hasqueue)
            {
                messQueue.Enqueue(item);
            }
            InitReplayState();
        }

        //var data = Resources.Load<TextAsset>("proto_test").bytes;
        //var rsp = TcpDataHandler.DeSerializeProtoData(data, typeof(DeskRecordRsp)) as DeskRecordRsp;
        //ParseData(rsp);
    }

    /// <summary>
    /// 缓存亮牌数据
    /// /// <summary>
    void PushInCache(Queue<FakeReplayData> inqueue, int roundindex, int deskid)
    {
        CacheReplayData data = new CacheReplayData();
        data.RoundIndex = roundindex;
        data.MessQueue = new Queue<FakeReplayData>();
        foreach (var vi in inqueue)
        {
            data.MessQueue.Enqueue(vi);
        }
        List<CacheReplayData> oData;
        if (dictionaryCache.TryGetValue(deskid, out oData))
        {
            oData.Add(data);
        }
        else
        {
            //最多20局
            List<CacheReplayData> listData = new List<CacheReplayData>(MAX_ROUND_NUM);
            listData.Add(data);
            dictionaryCache.Add(deskid, listData);
        }
    }
    Queue<FakeReplayData> GetCacheRecord(int deskId, int roundindex)
    {
        List<CacheReplayData> oData;
        if (dictionaryCache.TryGetValue(deskId, out oData))
        {
            for (int i = 0; i < oData.Count; i++)
            {
                if (oData[i].RoundIndex == roundindex)
                    return oData[i].MessQueue;
            }
        }
        return null;
    }

    /// <summary>
    /// 缓存换3张数据
    /// </summary>
    public void PutChangeThreeCardCache(int seatId, List<int> mjCodeList)
    {
        MjChangeThreeData data;
        if (changeThreeCardCache.TryGetValue(seatId, out data))
        {
            data.seatID = seatId;
            data.getCodeList = mjCodeList;
            //changeThreeCardCache[seatId].Enqueue(mjCodeList);
        }
        else
        {
            data = new MjChangeThreeData();
            data.seatID = seatId;
            data.getCodeList = mjCodeList;
            changeThreeCardCache.Add(seatId, data);
        }
    }
    public Dictionary<int, MjChangeThreeData> GetChangeThreeCardCache()
    {
        return changeThreeCardCache;
        //MjChangeThreeData data;
        //changeThreeCardCache.TryGetValue(seatId, out data);
        //return data;
    }

    public void ParseData(DeskRecordRsp rsp)
    {
        List<Msg.DeskRecordFrame> recordList = rsp.FrameList;
        if (recordList != null && recordList.Count > 0)
        {
            for (int i = 0; i < recordList.Count; i++)
            {
                Msg.DeskRecordFrame info = recordList[i];

                if (!FakeReplayFilter.Instance.LegalCheck(info))
                    continue;
                FakeReplayData msg = new FakeReplayData();
                CmdNo no = (CmdNo)info.Xxmsg.MsgID;
                msg.ProMsg = new ProtoMessage(no, info.Xxmsg.MsgBody);
                msg.Tick = info.TickID;
                messQueue.Enqueue(msg);
            }
        }
        if (rsp.IsComplete)
        {
            if (_nDeskId > 0 && _nRoundIndex > 0)
            {
                PushInCache(messQueue, _nRoundIndex, _nDeskId);
            }
            InitReplayState();
            QLoger.ERROR("FakeReplayManager ---- > ParseData");
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
        }
    }
    //TODO: rextest 
    public void ParseRoundData(DeskInfoRsp data)
    {
        //TODO:
        for (int i = 0; i < data.DeskList.Count; ++i)
        {
            _DiskInfo[data.DeskList[i].DeskID] = data.DeskList[i].BoutID;
        }
    }
    public bool CheckReplayData(int nDeskId, int nRoundIndex)
    {
        //TODO:
        if (_DiskInfo == null)
            return false;
        List<int> roundList = null;
        _DiskInfo.TryGetValue(nDeskId, out roundList);

        if (roundList == null)
            return false;

        nRoundIndex++;

        for (int i = 0; i < roundList.Count; ++i)
        {
            if (nRoundIndex == roundList[i])
                return true;
        }
        return false;
    }
    public ProtoMessage GetMessage()
    {
        if (MessQueue.Count <= 0)
        {
            StopReplay();
            //
            //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Over);
        }
        else
        {
            //if (Time.timeSinceLevelLoad > 0)
            {
                fCurrentTime = Time.time - (float)fBaseTime;
                fPauseTime += Time.deltaTime;
                var sd = messQueue.Peek();
                if (fCurrentTime * 1000 > sd.Tick * 50 || fPauseTime > 3)
                {
                    fPauseTime = 0;
                    var retData = messQueue.Dequeue();
                    replaymessQueue.Enqueue(retData);
                    return retData.ProMsg;
                }
            }
        }
        return null;
    }
    public void StopReplay(bool recordDeskId = false)
    {
        //重置動畫速率
        InitSpeed();
        bDataOver = false;
        if (recordDeskId)
            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.DeskViewRecordDeskId = _nDeskId;
    }
    public void PauseReplay(bool bpause)
    {
        bPause  = bpause;
        if (!bPause)
        {
            Time.timeScale = CurrentSpeed;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    public void ChangeSpeed()
    {
        CurrentSpeed = CurrentSpeed == 1 ? 2 : 1;
    }
    public void InitSpeed()
    {
        CurrentSpeed = nInitSpeed;
    }
    public void ReplayAgain()
    {
        AnimPlayManager.Instance.ClearAllAnim();
        DG.Tweening.DOTween.KillAll(true);
        MemoryData.GameStateData.IsMahjongGameIn = false;
        _R.ui.CloseUI("UIMahjongBalanceNew");
        EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongGameStart);
        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_UIReplayCtrl_Play_Init);
        messQueue.Clear();
        Queue<FakeReplayData> hasqueue = GetCacheRecord(_nDeskId, _nRoundIndex);
        if (hasqueue != null)
        {
            foreach (var item in hasqueue)
            {
                messQueue.Enqueue(item);
            }
            InitReplayState();
        }
    }
    void InitReplayState()
    {
        bDataOver = true;
        //初始化计时器
        fBaseTime = Time.time;
        //回放开始
        bReplayState = true;

        bPause = false;

        CurrentSpeed = CurrentSpeed;
        ////初始化回放数据 其他3家玩家摸牌数据
        //timeGetCardCache.Clear();
        //foreach (var item in messQueue)
        //{
        //    if (item.SeatId <= 0)
        //        continue;
        //    PutTimeGetCardCache(item.SeatId, item.MjCodeList);
        //}
    }
    //是否满足回放状态
    public bool CanReplay()
    {
        //非回放状态
        if (!bReplayState)
            return false;

        //接收回放数据未完成
        if (!bDataOver)
            return false;

        ////没有回放数据 点了回放但是没数据?
        //if (MessQueue.Count <= 0)
        //    return false;

        //暂停状态下
        if (bPause)
            return false;

        return true;
    }
}
