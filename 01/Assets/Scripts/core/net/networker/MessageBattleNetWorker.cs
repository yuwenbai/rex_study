/**
 * @Author lyb
 * 战绩面板数据跟服务器交互
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg;

namespace projectQ
{
    //注册数据处理 战绩数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfMessageBattle()
        {
            ModelNetWorker.Regiest<FMjRoomMyRecordRsp>(S2CMessageBattleListRsp);
            ModelNetWorker.Regiest<FMjRoomMyRecordOpRsp>(S2CMessageBattleDeleteRsp);
            ModelNetWorker.Regiest<FMjRoomMyRecordNotify>(S2CMessageBattleNewRsp);
        }

        #region Get Message --------------------------------------------------------------

        public List<Msg.MjDeskRecord> mBattleDataList;

        /// <summary>
        /// 获得自己的战绩记录消息列表
        /// </summary>
        public void S2CMessageBattleListRsp(object rsp)
        {
            if (mBattleDataList == null)
            {
                mBattleDataList = new List<MjDeskRecord>();
            }

            var prsp = rsp as FMjRoomMyRecordRsp;

            foreach (Msg.MjDeskRecord recordData in prsp.RecordList)
            {
                if (recordData.MaxBouts != 0
                    && recordData.BoutsRecord.Count > 0
                    && recordData.State != (int)MessageBattleStateEnum.MESSAGE_BATTLE_DELETE)
                {
                    mBattleDataList.Add(recordData);
                }
            }

            if (prsp.IsFinish)
            {
                MessageBattleInfoListFill(mBattleDataList);

                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_MessageBattle_Succ);
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "MyMessageBattleList");

                mBattleDataList = null;
            }
        }

        /// <summary>
        /// 如果有新的战绩产生服务器会推送过来新的数据
        /// </summary>
        public void S2CMessageBattleNewRsp(object rsp)
        {
            var prsp = rsp as FMjRoomMyRecordNotify;

            MessageBattleInfoListFill(prsp.RecordList);
        }

        /// <summary>
        /// 服务器返回战绩操作消息
        /// </summary>
        public void S2CMessageBattleDeleteRsp(object rsp)
        {
            var prsp = rsp as FMjRoomMyRecordOpRsp;

            MemoryData.MessageBattleData.MessageBattleOnce_Update(prsp);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_MessageBattleDelete_Succ, prsp.ResultCode);
        }

        #endregion -----------------------------------------------------------------------

        #region Send Message -------------------------------------------------------------

        /// <summary>
        /// 跟服务器请求战绩列表
        /// </summary>
        public void C2SMessageBattleListReq()
        {
            var msg = new FMjRoomMyRecordReq();
            msg.UserID = MemoryData.UserID;
            this.send(msg);
        }

        /// <summary>
        /// 跟服务器发送删除战绩列表消息
        /// state 当前执行的操作 --- OpType 0 未读 , 1 已读 , 2 删除
        /// </summary>
        public void C2SMessageBattleDeleteReq(MessageBattleStateEnum state, List<int> deskList)
        {
            var msg = new FMjRoomMyRecordOpReq();
            msg.UserID = MemoryData.UserID;
            msg.OpType = (int)state;
            foreach (int deskId in deskList)
            {
                msg.DeskID.Add(deskId);
            }
            this.send(msg);
        }

        #endregion -------------------------------------------------------------------------

        #region 解析服务器传过来的数据存储到本地 -------------------------------------------

        /// <summary>
        /// 战绩面板数据填充，调用战绩面板的时候需要填充一下数据
        /// </summary>
        public void MessageBattleInfoListFill(List<Msg.MjDeskRecord> mBattleDataList)
        {
            foreach (Msg.MjDeskRecord battleData in mBattleDataList)
            {
                MessageBattleInfoFill(battleData);
            }

            MemoryData.ResultData.ResultData.Sort(MessageBattleTimeCompare);
        }

        /// <summary>
        /// 单个数据填充
        /// </summary>
        public void MessageBattleInfoFill(Msg.MjDeskRecord mBattleData)
        {
            GameResult gameResult = MessageBattleFill(mBattleData);

            List<GameResultCostData> resultCostList = GetResultCostDataList(mBattleData.URInfo);
            int resultType = mBattleData.DeskType;

            MemoryData.ResultData.AddOrUpdateData(gameResult);
            MemoryData.ResultData.SetResultCostData(mBattleData.DeskID, resultCostList, resultType);
        }

        private GameResult MessageBattleFill(Msg.MjDeskRecord battleData)
        {
            int gameSub = battleData.ConfigID;
            GameResult gameResult = new GameResult(battleData.DeskID, battleData.MjGameType, gameSub,
                    battleData.OddsLimit, battleData.ShowType, GetMjBureauDetialInfo(battleData.BoutsRecord),
                    GetMjTitleInfoList(battleData.TitleInfo), SelfSeatIdGet(battleData.PlayerList));

            gameResult.ownerUserID = battleData.OwnerUserID;
            gameResult.recordTime = battleData.RecordTime;
            gameResult.maxBouts = battleData.MaxBouts;
            gameResult.showState = battleData.State;
            gameResult.SetPlayerInfo(GameResultPlayerListGet(battleData.PlayerList), MemoryData.UserID);

            return gameResult;
        }

        private int SelfSeatIdGet(List<Msg.MjDeskPlayerData> playerList)
        {
            int selfSeatID = 0;
            foreach (Msg.MjDeskPlayerData palyer in playerList)
            {
                if (palyer.UserID == MemoryData.UserID)
                {
                    selfSeatID = palyer.SeatID;
                    break;
                }
            }
            return selfSeatID;
        }

        private List<GameResultPlayer> GameResultPlayerListGet(List<Msg.MjDeskPlayerData> pList)
        {
            List<GameResultPlayer> resultPlayerList = new List<GameResultPlayer>();

            foreach (Msg.MjDeskPlayerData playerInfo in pList)
            {
                GameResultPlayer player = new GameResultPlayer(playerInfo.UserID, playerInfo.Name, playerInfo.HeadUrl);

                player.SetResultData(playerInfo.SeatID, playerInfo.Score, playerInfo.WinBouts);

                resultPlayerList.Add(player);
            }

            return resultPlayerList;
        }

        #endregion -------------------------------------------------------------------------

        #region 战绩数据排序 ---------------------------------------------------------------

        /// <summary>
        /// 战绩时间排序
        /// </summary>
        public int MessageBattleTimeCompare(object arg0, object arg1)
        {
            GameResult f0 = (GameResult)arg0;
            GameResult f1 = (GameResult)arg1;

            int flag0 = f0.recordTime.CompareTo(f1.recordTime);
            if (flag0 != 0)
                return -flag0;
            return 0;
        }

        #endregion -------------------------------------------------------------------------
    }
}