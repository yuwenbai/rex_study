/**
 * 作者：周腾
 * 作用：麻将桌上四个人的位置，各自的信息
 * 日期：2014.4.17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class MahjongPositionPlayerInfo : MonoBehaviour
    {
        public MahJongPlayerInfo[] playerInfoArray;
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="info"></param>
        public void InitPlayerInfo(long[] userIDs, int deskId)
        {
            if (userIDs == null)
                return;

            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            List<PlayerDataModel> players = new List<PlayerDataModel>();
            for (int i = 0; i < userIDs.Length; i++)
            {
                players.Add(MemoryData.PlayerData.get(userIDs[i]));
            }

            List<int> eqe = new List<int>()
            {
                0,1,2,3
            };

            for (int i = 0; i < 4; i++)
            {
                PlayerDataModel data = players.Find(x => x.playerDataMj.seatID - 1 == i);
                if (data != null)
                {
                    int uiseatID = CardHelper.GetMJUIPosByServerPos(data.playerDataMj.seatID, selfSeatID);
                    playerInfoArray[uiseatID].IniPlayerInfo(data.playerDataMj.seatID, (ulong)data.playerDataMj.userID, data.playerDataMj.score, data.PlayerDataBase.Name);
                    playerInfoArray[uiseatID].SetIcon((ulong)data.playerDataMj.userID, data.PlayerDataBase.HeadURL);
                    DebugPro.Log(DebugPro.EnumLog.HeadUrl, "MahjongPositionPlayerInfo__InitPlayerInfo", MemoryData.PlayerData.get(data.playerDataMj.userID).PlayerDataBase.HeadURL);

                    int num = eqe.IndexOf(uiseatID);
                    if (num >= 0)
                        eqe[num] = -1;
                }
            }

            for (int n = 0; n < eqe.Count; n++)
            {
                if (eqe[n] >= 0)
                    playerInfoArray[eqe[n]].ClearPlayerAllMessage();
            }
        }

        #region UI Function

        /// <summary>
        /// 提示“胡”
        /// </summary>
        /// <param name="uiChairID"></param>
        //public void ShowPlayerHuTip(int uiChairID)
        //{
        //    playerInfoArray[uiChairID].ShowTipHu();
        //}
        /// <summary>
        /// 已经准备
        /// </summary>
        /// <param name="uiChairID"></param>
        //public void ShowPlayerReadyTip(int uiChairID)
        //{
        //    playerInfoArray[uiChairID].ShowTipReady();
        //}
        /// <summary>
        /// 房主
        /// </summary>
        /// <param name="uiChairID"></param>
        public void ShowFangZhu(int uiChairID)
        {
            playerInfoArray[uiChairID].ShowFangZhu();
        }
        /// <summary>
        /// 定庄
        /// </summary>
        /// <param name="uiChairID">UI座位id</param>
        /// <param name="showAni">是否播放动画，默认播放</param>
        public void ShowZhuang(int uiChairID, bool showAni)
        {
            for (int i = 0; i < playerInfoArray.Length; i++)
            {
                playerInfoArray[i].ShowZhuang(i == uiChairID, showAni);
            }
        }
        /// <summary>
        /// 定缺
        /// </summary>
        /// <param name="uiChairID">UI座位id</param>
        /// <param name="queType">缺的类型</param>
        /// <param name="showAni">是否播放动画，默认播放</param>
        public void ShowQue(int uiChairID, int queType, bool showAni)
        {
            playerInfoArray[uiChairID].ShowQueType((EnumMjHuaType)queType, showAni);
            //ShowXuanQueResult(uiChairID, (EnumMjHuaType)queType);
        }

        /// <summary>
        /// 显示闹
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="showAni"></param>
        public void ShowNao(int uiChairID, bool showAni)
        {
            playerInfoArray[uiChairID].ShowNao(showAni);
        }

        /// <summary>
        /// 听牌/明楼
        /// </summary>
        /// <param name="uiChairID">UI座位ID</param>
        /// <param name="isTing">true听牌，false明楼</param>
        /// <param name="showAni">是否播放动画，默认播放</param>
        public void ShowTingPai(int uiChairID, string iconName, bool showAni)
        {
            playerInfoArray[uiChairID].MingLou(iconName, showAni);
        }

        /// <summary>
        /// 显示头像中上方标记
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="type"></param>
        /// <param name="showAni"></param>
        public void ShowCenterUp(int uiChairID, EnumMjSpecialCheck type, bool showAni)
        {
            playerInfoArray[uiChairID].ShowCenter(type, showAni);
        }

        /// <summary>
        /// 分数改变
        /// </summary>
        /// <param name="uiChairID">UI座位id</param>
        /// <param name="score">分数</param>
        public void ChangeScore(int uiChairID, int score)
        {
            playerInfoArray[uiChairID].ChangePlayerMoney(score);
        }

        /// <summary>
        /// 显示麻将牌
        /// </summary>
        public void ShowMahjong(int uiSeatID, bool isShow, bool isPlayAni, List<int> mahjongList)
        {
            playerInfoArray[uiSeatID].ShowMahjong(uiSeatID, isShow, isPlayAni, mahjongList);
        }
        /// <summary>
        /// 四家买马显示首次4张牌
        /// </summary>
        public void ShowSiJiaMaiMa(int uiSeatID,List<int> mjPaiList)
        {
            playerInfoArray[uiSeatID].ShowSiJiaMaiMa(uiSeatID, mjPaiList);
        }
        /// <summary>
        /// 显示下飘标记文字
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="values"></param>
        public void ShowMahjongType(int uiSeatID, List<string[,]> values)
        {
            playerInfoArray[uiSeatID].ShowMahjongType(values);
        }

        /// <summary>
        /// 设置人物积分
        /// </summary>
        public void SetScore()
        {
            //int deskId = 
            if (MjDataManager.Instance.MjData.curUserData.selfDeskID == 0)
            {
                return;
            }
            MjDeskInfo info = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            for (int i = 0; i < info.GetAllPlayerID().Length; i++)
            {
                int uiseatID = CardHelper.GetMJUIPosByServerPos(MemoryData.PlayerData.get(info.GetAllPlayerID()[i]).playerDataMj.seatID, selfSeatID);

                //if (i == uiseatID)
                //{
                //QLoger.LOG("++++++++     uiseat" + uiseatID + " score" + MemoryData.PlayerData.get(info.GetAllPlayerID()[i]).playerDataMj.score);
                playerInfoArray[uiseatID].ChangePlayerMoney(MemoryData.PlayerData.get(info.GetAllPlayerID()[i]).playerDataMj.score);
                //}

            }
        }

        /// <summary>
        /// 显示下跑/下炮的数量
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="xiaPao"></param>
        /// <param name="paoNum"></param>
        public void ShowPaoNum(int uiChairID, EnumMjSpecialCheck paoEnum, int paoNum, bool showAni, bool showRight = false)
        {
            playerInfoArray[uiChairID].ShowPaoNum(paoEnum, paoNum, showAni, showRight, uiChairID);
        }

        /// <summary>
        /// 显示断门
        /// </summary>
        /// <param name="uiChairID"></param>
        /// <param name="spName"></param>
        public void ShowDuanMen(int uiChairID,string spName)
        {
            playerInfoArray[uiChairID].ShowDuanMen(uiChairID, spName);
        }

        /// <summary>
        /// 显示中下方补牌标记
        /// </summary>
        public void ShowCenterDown(int uiChairID, string spName, int showNum, bool isShowAni)
        {
            playerInfoArray[uiChairID].ShowCenterDown(uiChairID, spName, showNum, isShowAni);
        }

        public void ShowEffectBySeatID(EnumMjOpenMaType mjType, int uiSeatID, float autoDesTime = 1.0f)
        {
            Transform effObj = CardHelper.GetOpenMaEffeObj(mjType, autoDesTime);

            if (effObj != null)
            {
                playerInfoArray[uiSeatID].ShowIconEffect(effObj.gameObject);
            }
        }

        public Transform GetTransformOfSeatID(int uiSeatID)
        {
            return playerInfoArray[uiSeatID].GetIconTransform();
        }

        /// <summary>
        /// 正在出牌的玩家
        /// </summary>
        /// <param name="uiSeatID"></param>
        /// <param name="showAni"></param>
        public void IsPlaying(int uiSeatID, bool showAni)
        {
            if (uiSeatID == -1)
            {
                for (int i = 0; i < playerInfoArray.Length; i++)
                {
                    playerInfoArray[i].HidePlayAni();
                }
            }
            for (int i = 0; i < playerInfoArray.Length; i++)
            {
                if (uiSeatID == i)
                {
                    playerInfoArray[i].ShowPlayAni();
                }
                else
                {
                    playerInfoArray[i].HidePlayAni();
                }
            }
        }
        #endregion
    }
}