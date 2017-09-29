/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public enum WXOpenParaEnum
    {
        /// <summary>
        /// Null
        /// </summary>
        None = -1,
        /// <summary>
        /// 邀请微信好友
        /// </summary>
        SHARE_INVITE_FRIEND = 1,
        /// <summary>
        /// 邀请微信用户打牌   //参数 0 桌子号6位
        /// </summary>
        SHARE_INVITE_PLAY = 2,
        /// <summary>
        /// 邀请好友关联麻将馆
        /// </summary>
        SHARE_INVITE_FRIEND_GO_MYMUSEUM = 3,
        /// <summary>
        /// 邀请好友有礼
        /// </summary>
        SHARE_INVITE_FRIEND_TO_GET_REWARD = 4,
    }

    public class SysInitData
    {
        public WXOpenParaEnum initKey = WXOpenParaEnum.None;
        public string[] initValue = null;

        //public WXOpenParaEnum initKey = WXOpenParaEnum.SHARE_INVITE_PLAY;
        //public string[] initValue =  new string[] { "313640" };  

        public void Clear()
        {
            initKey = WXOpenParaEnum.None;
            initValue = null;
        }

        /// <summary>
        /// 游戏进入赋值
        /// </summary>
        public void SetDataValue(WXOpenParaEnum dKey, string[] dValue)
        {
            initKey = dKey;
            initValue = new string[dValue.Length];

            for (int i = 0; i < initValue.Length; i++)
            {
                initValue[i] = dValue[i];
            }
        }

        //取得Key 并且已经做好了验证措施 保证不需要再次判断
        public WXOpenParaEnum GetKey()
        {
            switch (initKey)
            {
                case WXOpenParaEnum.SHARE_INVITE_FRIEND:
                    break;
                case WXOpenParaEnum.SHARE_INVITE_PLAY:
                    if (initValue != null && initValue.Length > 0 && initValue[0].Length == 6)
                    {
						//8.16 加入当前牌桌检测 如果微信分享与当前所在牌桌相同则清除微信分享进桌数据
                        if(int.Parse(initValue[0]) == MjDataManager.Instance.MjData.curUserData.selfDeskID)
                        {
                            this.Clear();
                            break;
                        }
                        else
                        {
                            return initKey;
                        }
                    }
                    break;
                case WXOpenParaEnum.SHARE_INVITE_FRIEND_GO_MYMUSEUM:
                    if (initValue != null && initValue.Length > 0)
                        return initKey;
                    break;
                case WXOpenParaEnum.SHARE_INVITE_FRIEND_TO_GET_REWARD:
                    if (initValue != null && initValue.Length > 0)
                        return initKey;
                    break;
            }
            return WXOpenParaEnum.None;
        }

        /// <summary>
        /// 根据不同的Excute 判断
        /// </summary>
        public void Execute()
        {
            switch (initKey)
            {
                case WXOpenParaEnum.SHARE_INVITE_FRIEND:
                    break;
                case WXOpenParaEnum.SHARE_INVITE_PLAY:
                    {
                        MemoryData.GameStateData.IsWxShareInvitePlay = true;
                        MemoryData.GameStateData.IsExternalLinkJoinDesk = true;
                        MahjongLogicNew.Instance.SendMjJoinDesk((ulong)MemoryData.UserID, int.Parse(initValue[0]));
                        //Clear();
                    }
                    break;
                case WXOpenParaEnum.SHARE_INVITE_FRIEND_GO_MYMUSEUM:
                    {
                        //拿到传回来的RoomId 调用接口 查到当前麻将馆 呼出关联面板
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_ParlorWindow_Rsp, initValue[0]);
                        Clear();
                    }
                    break;
                case WXOpenParaEnum.SHARE_INVITE_FRIEND_TO_GET_REWARD:
                    {
                        ModelNetWorker.Instance.C2SInviteCodeGetAwardReq(initValue[0]);
                        Clear();
                    }
                    break;
            }
        }
    }

    #region 内存数据

    public partial class MKey
    {
        public const string SYS_INIT_DATA = "SYS_INIT_DATA";
    }

    public partial class MemoryData
    {
        static public SysInitData InitData
        {
            get
            {
                SysInitData itemData = MemoryData.Get<SysInitData>(MKey.SYS_INIT_DATA);
                if (itemData == null)
                {
                    itemData = new SysInitData();
                    MemoryData.Set(MKey.SYS_INIT_DATA, itemData);
                }
                return itemData;
            }
        }
    }

    #endregion
}