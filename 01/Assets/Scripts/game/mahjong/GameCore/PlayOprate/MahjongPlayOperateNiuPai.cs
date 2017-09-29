
using System.Collections.Generic;
using MahjongPlayType;
using projectQ;

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOperateNiuPai processNiuPai
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_NiuPai.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOperateNiuPai item = new MahjongPlayOperateNiuPai();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOperateNiuPai;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_NiuPai = "OPRATE_NiuPai";
    }


    public partial class MjDataManager
    {

    }

}

namespace MahjongPlayType
{
    public enum NiuPaiGroupType
    {//这个枚举值不能修改，可以追加
        None=0,//无
        Peacock=1,//孔雀东南飞
        ZFB=2,//中发白
        DNXB=3,//中南西北
    }
    public class NiuPaiGroupItem
    {
        public List<int> MjCodes = new List<int>();
        public NiuPaiGroupType GroupType = NiuPaiGroupType.None;
    }
    /// <summary>
    /// 服务器通知下发消息
    /// </summary>
    public class NotifyNiuPai
    {
        public int DeskID = -1;
        public int SeatID = -1;
        public int RuleID = -1;
        public bool IsNiuPai = true;
        public List<NiuPaiGroupItem> Data = new List<NiuPaiGroupItem>();
    }
    /// <summary>
    /// 上传结果
    /// </summary>
    public class RequestNiuPai
    {
        public bool IsNiuPai = true;
        public int DeskID = -1;
        public int SeatID = -1;
        public int SelectedID = -1; //1扭 2过
        public List<NiuPaiGroupItem> Data = new List<NiuPaiGroupItem>();
    }
    /// <summary>
    /// 结果返回
    /// </summary>
    public class ResponseNiuPai
    {
        public bool IsNiuPai = true;
        public int DeskID = -1;
        public int SeatID = -1;
        public int SelectedID = -1; //1扭 2过
        public List<NiuPaiGroupItem> Data = new List<NiuPaiGroupItem>();
    }
    /// <summary>
    /// 历史扭牌记录
    /// </summary>
    public class NiuPaiHistoryRecords
    {
        public int DeskID = -1;
        public int SeatID = -1;
        public List<NiuPaiGroupItem> NiuPaiData = new List<NiuPaiGroupItem>();
        public List<NiuPaiGroupItem> BuNiuData = new List<NiuPaiGroupItem>();
        public List<int> NiuPaiCountRecords = new List<int>();
        public List<int> BuNiuPaiCountRecords = new List<int>();
    }


    public class MahjongPlayOperateNiuPai : MahjongPlayOprateBase
    {
        #region 数据通知类
        private NotifyNiuPai _NiuPaiData = null;
        public NotifyNiuPai NotifyNiuPaiData
        {
            get { return _NiuPaiData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _NiuPaiData = value;
            }
        }
        #endregion

            
        #region 消息上传
        private RequestNiuPai _RequestNiuPai = null;
        public RequestNiuPai RequestNiuPai
        {
            get { return _RequestNiuPai; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _RequestNiuPai = value;
            }
        }
        #endregion

        #region 服务器赌暗杠消息返回
        private ResponseNiuPai _ResponseNiuPai = null;
        public ResponseNiuPai ResponseNiuPai
        {
            get { return _ResponseNiuPai; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _ResponseNiuPai = value;
            }
        }
        #endregion

        #region 历史数据
        private List<NiuPaiHistoryRecords>  _HistroyRecords = new List<NiuPaiHistoryRecords>();
        public List<NiuPaiHistoryRecords> HistoryRecords
        {
            get { return _HistroyRecords; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _HistroyRecords = value;
            }
        } 
        #endregion

    }

}

