using projectQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace projectQ
{
    public class MahjongData : AnimatorData
    {

        //&nplayercount 玩家数量
        //$strposcfg  动画位置配置文件
        //
        public MahjongData(int nplayercount, string strposcfg)
        {
            this.PlayerCount = nplayercount;
            this.AnimatorHandPosCfg = strposcfg;
            mUserHandInfoList = new List<User_hand_info>(nplayercount);
            Init();
        }

        public MahjongData()
        {
        }

        public override void Init()
        {
            //初始化位置信息
            this.StData = Resources.Load<AnimatorHandPos>(this.AnimatorHandPosCfg);

            if (this.PlayerCount > 0)
            {
                for (int i = 0; i < this.PlayerCount; ++i)
                {
                    long uid = MjDataManager.Instance.GetCurrentUserIDBySeatID(i + 1);
                    var vInfo = new User_hand_info(uid, true);
                    var u = MemoryData.PlayerData.CheckModel(uid);
                    if (u == null)
                    {
                        QLoger.LOG(LogType.EError, "初始化手数据配置错误 无法找到用户{0},默认使用男手", uid);
                    }

                    vInfo.sex = (u == null || u.PlayerDataBase.Sex == 1) ? SexEnum.Man : SexEnum.Woman;
                    mUserHandInfoList.Add(vInfo);
                }
            }
        }
        public override void Clear()
        {
            EventDispatcher.FireEvent(MJEnum.HandModelEvent.HME_Destroy.ToString());
        }
    }
}
