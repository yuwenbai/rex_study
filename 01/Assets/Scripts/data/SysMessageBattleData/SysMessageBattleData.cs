/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    /// <summary>
    /// 当前战绩信息状态
    /// 0 未读 , 1 已读 , 2 删除
    /// </summary>
    public enum MessageBattleStateEnum
    {
        /// <summary>
        /// 未读
        /// </summary>
        MESSAGE_BATTLE_UNREAD = 0,
        /// <summary>
        /// 已读
        /// </summary>
        MESSAGE_BATTLE_READ = 1,
        /// <summary>
        /// 删除
        /// </summary>
        MESSAGE_BATTLE_DELETE = 2,
    }

    public class SysMessageBattleData
    {
        /// <summary>
        /// 更新字段数据信息，适用于被删除的时候
        /// ShowType 0 未读 , 1 已读 , 2 删除
        /// </summary>
        public void MessageBattleOnce_Update(Msg.FMjRoomMyRecordOpRsp uInfo)
        {
            if (uInfo.ResultCode == 0)
            {
                foreach (int deskId in uInfo.DeskID)
                {
                    GameResult record = MemoryData.ResultData.get(deskId);

                    if (record != null)
                    {
                        record.showState = uInfo.OpType;
                        if (uInfo.OpType == (int)MessageBattleStateEnum.MESSAGE_BATTLE_DELETE)
                        {
                            MemoryData.ResultData.ResultData.Remove(record);
                        }                        
                    }
                }
            }
            else
            {
                QLoger.ERROR(" 删除失败 ");
            }
        }
    }

    #region 内存数据------------------------------------------------------------------------------

    public partial class MKey
    {
        public const string USER_MESSAGE_BATTLE_DATA = "USER_MESSAGE_BATTLE_DATA";
    }

    public partial class MemoryData
    {
        static public SysMessageBattleData MessageBattleData
        {
            get
            {
                SysMessageBattleData battleData = MemoryData.Get<SysMessageBattleData>(MKey.USER_MESSAGE_BATTLE_DATA);
                if(battleData == null)
                {
                    battleData = new SysMessageBattleData();
                    MemoryData.Set (MKey.USER_MESSAGE_BATTLE_DATA , battleData);
                }
                return battleData;
            }
        }
    }

    #endregion -----------------------------------------------------------------------------------
}