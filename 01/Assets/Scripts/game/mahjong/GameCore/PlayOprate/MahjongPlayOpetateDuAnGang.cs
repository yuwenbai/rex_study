/**
 * @Author Xin.Wang
 *麻将特殊玩法： 胡飘 杠飘 飘素自摸  飘金钓鱼
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
            public MahjongPlayOprateDuAnGang processDuAnGang
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_DuAnGang.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOprateDuAnGang item = new MahjongPlayOprateDuAnGang();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOprateDuAnGang;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_DuAnGang = "OPRATE_DuAnGang";
    }


    public partial class MjDataManager
    {

    }

}

namespace MahjongPlayType
{
    /// <summary>
    /// 服务器通知下发消息
    /// </summary>
    public class NotifyDuAnGangData
    {
        public bool State = false;
        public int SeatID = -1;
        public List<int> MjCodeList = new List<int>();
    }
    /// <summary>
    /// 上传结果
    /// </summary>
    public class UploadDuAnGangData
    {
        public int MjCode;
        public int SeatID;
        public int DeskID;
        public int Type;
    }
    /// <summary>
    /// 结果返回
    /// </summary>
    public class RspDuAnGangData
    {
        public int ResultCode;//1.成功 2过，3失败
        public int SeatID;
        public int DeskID;
    }

    public class MahjongPlayOprateDuAnGang : MahjongPlayOprateBase
    {
        #region 服务器赌暗杠数据通知类
        private NotifyDuAnGangData _DuAnGangData = null;
        public NotifyDuAnGangData NotifyDuAnGangData
        {
            get { return _DuAnGangData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _DuAnGangData = value;
            }
        }
        #endregion

            
        #region 服务器赌暗杠消息上传
        private UploadDuAnGangData _UploadDuAnGangData = null;
        public UploadDuAnGangData UploadDuAnGangData
        {
            get { return _UploadDuAnGangData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _UploadDuAnGangData = value;
            }
        }
        #endregion

        #region 服务器赌暗杠消息返回
        private RspDuAnGangData _RspDuAnGangData = null;
        public RspDuAnGangData RspDuAnGangData
        {
            get { return _RspDuAnGangData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _RspDuAnGangData = value;

            }
        }
        #endregion
    }

}
