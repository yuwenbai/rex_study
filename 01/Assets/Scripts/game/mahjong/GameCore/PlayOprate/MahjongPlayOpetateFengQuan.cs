/**
 * @Author HaiLong.Zhang
 *广东麻将玩法：风圈
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
            public MahjongPlayOpetateFengQuan processFengQuan
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_FengQuan.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOpetateFengQuan item = new MahjongPlayOpetateFengQuan();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOpetateFengQuan;
                }
            }
        }
    }


    public partial class MjPlayOprateType
    {
        public const string OPRATE_FengQuan = "OPRATE_FengQuan";
    }


    /// <summary>
    /// 管理
    /// </summary>
    public partial class MjDataManager
    {
        public void InitFengQuanData()
        {
            EventDispatcher.FireEvent(MJEnum.MjFengQuanEvent.MJFQ_LogicNotify.ToString());
        }
        public void FengQuanGetData(out int type, out int deskID)
        {
            type = -1;
            deskID = -1;
            MjData.ProcessData.processFengQuan.GetFengQuanData(out type, out deskID);
        }
    }
}

namespace MahjongPlayType
{
    /// <summary>
    /// 服务器通知下发消息
    /// </summary>

    public class FengQuanData
    {
        public int deskID = -1;       //桌号
        public int type = -1;         //风圈标识
        public FengQuanData(int deskID, int identify)
        {
            this.deskID = deskID;
            this.type = identify;
        }
    }

    public class MahjongPlayOpetateFengQuan : MahjongPlayOprateBase
    {
        #region 服务器风圈数据通知类
        private FengQuanData _fengQuanData = null;
        public FengQuanData FengQuanData
        {
            get { return _fengQuanData; }
            set
            {
                if (NullHelper.IsObjectIsNull(value))
                {
                    return;
                }
                _fengQuanData = value;
            }
        }

        public void GetFengQuanData(out int type, out int deskID)
        {
            type = -1;
            deskID = -1;
            if (_fengQuanData == null)
            {
                return;
            }
            type = FengQuanData.type;
            deskID = FengQuanData.deskID;
        }
        #endregion
    }
}
