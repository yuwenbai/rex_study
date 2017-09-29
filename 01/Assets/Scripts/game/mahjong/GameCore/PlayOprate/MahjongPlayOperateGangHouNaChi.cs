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
            public MahjongPlayOperateGangHouNaChi processGangHouNaChi
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_GangHouNaChi.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOperateGangHouNaChi item = new MahjongPlayOperateGangHouNaChi();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOperateGangHouNaChi;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_GangHouNaChi = "OPRATE_GangHouNaChi";
    }


    public partial class MjDataManager
    {

    }

}

namespace MahjongPlayType
{
    /// <summary>
    /// 服务器消息通知
    /// </summary>
    public class GangHouNaChiNotifyData
    {
        public List<int> MjCodeList = new List<int>();
        public int RulerID = -1;
        public int SeatID = -1;
        public int SelectSubType = -1;
    }
    /// <summary>
    /// 消息上传
    /// </summary>
    public class GangHouNaChiUploadData
    {
        public int DeskID;
        public int SeatID;
        public int MjCode;
        public int MjIndex;
    }
    
    /// <summary>
    /// 结果返回
    /// </summary>
    public class RspGangHouNaChi
    {
        public int DeskID;
        public int SelectID;
        public int ResultCode;
        public int SeatID;
    }

    public class MahjongPlayOperateGangHouNaChi : MahjongPlayOprateBase
    {
        #region 服务器数据通知类
        private GangHouNaChiNotifyData _GangHouNaChiNotifyData = null;
        public GangHouNaChiNotifyData GangHouNaChiNotifyData
        {
            get { return _GangHouNaChiNotifyData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _GangHouNaChiNotifyData = value;
            }
        }
        #endregion


        #region 服务器消息上传
        private GangHouNaChiUploadData _GangHouNaChiUploadData = null;
        public GangHouNaChiUploadData GangHouNaChiUploadData
        {
            get { return _GangHouNaChiUploadData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _GangHouNaChiUploadData = value;
            }
        }
        #endregion

        #region 服务器赌暗杠消息返回
        private RspGangHouNaChi _RspGangHouNaChi = null;
        public RspGangHouNaChi RspGangHouNaChi
        {
            get { return _RspGangHouNaChi; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _RspGangHouNaChi = value;
                //通知逻辑层数据更新
                EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_RspNotify.ToString());
            }
        }
        #endregion
    }

}
