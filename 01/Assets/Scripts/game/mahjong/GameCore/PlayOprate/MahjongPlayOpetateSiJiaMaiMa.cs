/**
 * @Author HaiLong.Zhang
 *广东麻将玩法：四家买马
 *
 */

using System.Collections.Generic;
using MahjongPlayType;
using projectQ;

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOpetateSiJiaMaiMa processSiJiaMaiMa
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_SiJiaMaiMa.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOpetateSiJiaMaiMa item = new MahjongPlayOpetateSiJiaMaiMa();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOpetateSiJiaMaiMa;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_SiJiaMaiMa = "OPRATE_SiJiaMaiMa";
    }


    /// <summary>
    /// 管理
    /// </summary>
    public partial class MjDataManager
    {
        public void InitSiJiaMaiMaData()
        {
            EventDispatcher.FireEvent(MJEnum.MjSiJiaMaiMaEvent.MJSJMM_LogicNotify.ToString());
        }
        public void SiJiaMaiMaGetData(out List<int> mjCode,out int seatID)
        {
            mjCode = new List<int>();
            seatID = -1;
            MjDataManager.Instance.MjData.ProcessData.processSiJiaMaiMa.GetSiJiaMaiMaData(out seatID, out mjCode);
        }
    }
}

namespace MahjongPlayType
{
    /// <summary>
    /// 服务器通知下发消息
    /// </summary>
    
    public class SiJiaMaiMaData
    {
        public List<SiJiaMaiMaCellData> sjmm_subData = new List<SiJiaMaiMaCellData>();
        public int rulerId = -1;    //玩法ID
        public int selectType = -1; //阶段检测
        public SiJiaMaiMaData(Msg.MjSiJiaMaiMaNotify notify)
        {
            this.rulerId = notify.nRulerID;
            this.selectType = notify.nSelectSubType;
        }
       
    }
    public class SiJiaMaiMaCellData
    {
        public bool OpSet = false;
        public int seatID = -1;                           //座位号
        public List<int> buyInSeatID = new List<int>();  //买中的玩家
        public List<int> mjCode = new List<int>();      //摸的4张牌
        public List<EnumMjBuyhorseStateType> buyInType = new List<EnumMjBuyhorseStateType>();   //是否中(1未中，2买中赢家，3买中输家)
        public string headUrl = null;                  //头像
        public SiJiaMaiMaCellData(Msg.MjParseCommon msgCommon,Msg.MjParseSiJiaMaiMaData msgConfirm) 
        {
            this.OpSet = msgCommon.bOpSet;
            this.seatID = msgCommon.SeatID;
            this.buyInSeatID = msgConfirm.nHitSeatID;
            this.mjCode = msgConfirm.nMjCode;
            if (msgConfirm.nType != null && msgConfirm.nType.Count > 0)
            {
                for (int i = 0; i < msgConfirm.nType.Count; i++)
                {
                    this.buyInType.Add((EnumMjBuyhorseStateType)msgConfirm.nType[i]);
                }
            }
        }
    }
    public class MahjongPlayOpetateSiJiaMaiMa : MahjongPlayOprateBase
    {
        #region 服务器四家买马数据通知类
        private SiJiaMaiMaData _siJiaMaiMaData = null;
        public SiJiaMaiMaData SiJiaMaiMaData
        {
            get { return _siJiaMaiMaData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _siJiaMaiMaData = value;
            }
        }
        public void GetSiJiaMaiMaData(out int seatID,out List<int> mjCode)
        {
            seatID = -1;
            mjCode = new List<int>();
            for(int i=0;i<_siJiaMaiMaData.sjmm_subData.Count;i++)
            {
                seatID = _siJiaMaiMaData.sjmm_subData[i].seatID;
                mjCode = _siJiaMaiMaData.sjmm_subData[i].mjCode;
            }
        }
        #endregion
    }
}
