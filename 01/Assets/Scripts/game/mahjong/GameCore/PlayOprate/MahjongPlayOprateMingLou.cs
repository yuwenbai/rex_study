/**
* @Author Xin.Wang
*明楼玩法类
*
*/
using System.Collections.Generic;
using MahjongPlayType;


#region 内存存储

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            #region 明楼

            public MahjongPlayOprateMingLou processMinglou
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_MINGLOU.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateMingLou item = new MahjongPlayOprateMingLou();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateMingLou;
                }
            }

            #endregion
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_MINGLOU = "OPRATE_MINGLOU";
    }

    public partial class MjDataManager
    {
        #region 明楼
        public void SetMinglouData(int seatID, List<int> handList, MjTingInfo info)
        {
            MjData.ProcessData.processMinglou.SetPlayerData(seatID, handList, info);
        }


        public List<int> GetMinglouData(int seatID)
        {
            return MjData.ProcessData.processMinglou.GetTingListBySeatID(seatID);
        }

        #endregion
    }

}

#endregion


namespace MahjongPlayType
{
    public class MjMinglouData
    {
        public MjMinglouData(int seatID)
        {
            playerSeatID = seatID;
        }

        //明楼类型枚举
        public enum EnumMinglouType
        {
            Normal = 1,                            //普通明楼   
            CheckTing = 2,                          //查看听口明楼
        }

        public enum EnumMjMinglouNameType
        {
            MinglouShou,                                //明搂
            MinglouMu,                                  //木子边明楼
            Minglao,                                   //明捞
        }


        public int playerSeatID = 0;                    //玩家座位号

        public EnumMinglouType minglouType = EnumMinglouType.Normal;
        public List<int> playerHandList = new List<int>();   //进听手牌 
        public MjTingInfo playerTingInfo = null;


        public void SetMingLouData(List<int> handList, MjTingInfo tingInfo, int showType = 1)
        {
            if (handList != null)
            {
                playerHandList = handList;
            }

            minglouType = (EnumMinglouType)showType;

            if (playerTingInfo != null && tingInfo == null)
            {
                return;
            }

            playerTingInfo = tingInfo;
        }


        public List<int> GetTingList()
        {
            List<int> tingList = new List<int>();

            if (playerTingInfo != null && playerTingInfo.huCode != null)
            {
                tingList = playerTingInfo.huCode;
                tingList.Sort();
            }

            return tingList;
        }

    }

    //明楼网络交互类型
    public class MjMinglouServer
    {
        public int seatID;
        public MjTingInfo tingInfo = null;

    }

    public class MahjongPlayOprateMingLou : MahjongPlayOprateBase
    {
        public Dictionary<int, MjMinglouData> playerDataDic = new Dictionary<int, MjMinglouData>();


        public void SetPlayerData(int seatID, List<int> handList, MjTingInfo info = null, int showType = 1)
        {
            if (!playerDataDic.ContainsKey(seatID))
            {
                MjMinglouData dataItem = new MjMinglouData(seatID);
                playerDataDic.Add(seatID, dataItem);
            }

            MjMinglouData data = playerDataDic[seatID];
            data.SetMingLouData(handList, info, showType);
        }



        public List<int> GetTingListBySeatID(int seatID)
        {
            List<int> tingList = new List<int>();

            if (playerDataDic.ContainsKey(seatID))
            {
                MjMinglouData data = playerDataDic[seatID];
                tingList = data.GetTingList();
            }

            return tingList;

        }


    }

}
