/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace projectQ
{

    #region 获取模块网络数据

    public partial class UIDefaultModel
    {
        
        public void EGetGameData(GEnum.NamedGameData type, object[] data)
        {

            if (!MemoryData.NamedGameDataCnf.isNeedGetDataFromServer(type))
            {

                EventDispatcher.FireSysEvent(GEnum.NamedEvent.ERCCloseingAndRelushUI);
                QLoger.LOG("当前数据不需要请求网络");
                return;
            }


            switch (type)
            {
                default: break;
                case GEnum.NamedGameData.ENone:
                    {
                        //Do Nothing
                    }
                    break;
                case GEnum.NamedGameData.EC2SRoomCardList:
                    {
                        //桌卡数据获取
                        ModelNetWorker.Instance.C2SRoomCardListReq();
                    }
                    break;
                case GEnum.NamedGameData.EC2SMessageList:
                    {
                        //消息数据获取
                        ModelNetWorker.Instance.MyMailListReq();
                    }
                    break;
                case GEnum.NamedGameData.EC2SMessageBattleList:
                    {
                        //战绩数据获取
                        ModelNetWorker.Instance.C2SMessageBattleListReq();
                        //顺带请求回放数据
						//rextest
                        ModelNetWorker.Instance.C2SMessageReplayDataReq();
                    }
                    break;
                case GEnum.NamedGameData.EC2SMessageGameHistory:
                    {
                        //游戏记录
                        ModelNetWorker.Instance.SaleReq();
                    }
                    break;
                case GEnum.NamedGameData.EC2SMessageRoomDesc:
                    {
                        //麻将馆介绍
                        ModelNetWorker.Instance.FMjRoomDescInfoReq();
                    }
                    break;
                case GEnum.NamedGameData.EC2SMessageRoomSearch:
                    {
                        //麻将馆搜索
                        string searchKey = data[0] as string;
                        int regionID = (int)data[1];
                        Msg.FMjSortType sortType = (Msg.FMjSortType)data[2];
                        ModelNetWorker.Instance.FMjRoomSearchReq(searchKey, regionID, sortType);
                    }
                    break;

                case GEnum.NamedGameData.EC2SMessageRoomPlayerInfo:
                    {
                        //用户麻将馆消息
                        long userId = (long)data[0];
                        ModelNetWorker.Instance.FMjRoomPlayerInfoReq(userId);
                    }
                    break;
                case GEnum.NamedGameData.EC2SMessageAwardConfig:
                    {
                        //抽奖配置
                        Msg.LotteryTypeDef lotteryType = (Msg.LotteryTypeDef)data[0];
                        ModelNetWorker.Instance.C2SAwardConfigReq(lotteryType);
                    }

                    break;
                case GEnum.NamedGameData.EC2SFriendList:
                    {
                        //好友列表数据请求
                        ModelNetWorker.Instance.MyFriendsListReq();
                    }
                    break;
                case GEnum.NamedGameData.EC2SOfficialGroupList:
                    {
                        //官方麻将试玩群请求
                        ModelNetWorker.Instance.C2SOfficialGroupReq();
                    }
                    break;
                case GEnum.NamedGameData.EC2SMahjongOpAction:
                    {
                        ModelNetWorker.Instance.MjOpactionReq_FromEC2S(data);
                    }
                    break;
            }

            MemoryData.NamedGameDataCnf.reflush(type);
        }


    }


    public partial class MKey
    {
        public const string NAMED_GAMEDATA_TIME_COUNT = "NAMED_GAMEDATA_TIME_COUNT";
    }

    public partial class MemoryData
    {


        /// <summary>
        /// 没有配置的数据会每次请求，配置了的会请求判定 //  时间单位 1/10000000.0f
        /// </summary>
        public class NamedGameDataTimeCount
        {
            private Dictionary<string, long> _time_send = new Dictionary<string, long>();
			private Dictionary<string, long> _time_cnf = new Dictionary<string, long>();

            private string make_key(GEnum.NamedGameData type, string sub = "")
            {
                return string.Format("{0}:{1}", (int)type, sub);
            }

			private long getTimeLeft(string type)
            {
                if (_time_cnf.ContainsKey(type))
                {
                    var ttl = _time_cnf[type];
                    if (_time_send.ContainsKey(type))
                    {
						var left = System.DateTime.Now.ToFileTimeUtc() - _time_send[type];
						return ttl - left > 0 ? (ttl - left) : 0;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -1;
                }
            }

            public long getTimeLeft(GEnum.NamedGameData type, string sub = "")
            {
                return getTimeLeft(make_key(type, sub));
            }

            public void reflush(GEnum.NamedGameData type, string sub = "")
            {
                string key = this.make_key(type, sub);
                if (getTimeLeft(key) != -1)
                {
					_time_send [key] = System.DateTime.Now.ToFileTimeUtc ();
                }
            }

            public bool isNeedGetDataFromServer(GEnum.NamedGameData type)
            {
                return this.getTimeLeft(type) <= 0;
            }

            private void setTtl(GEnum.NamedGameData name, float time)
            {
				_time_cnf.Add(this.make_key(name), (long)(time * 10000000));

				/*foreach (var item in _time_cnf) {
					QLoger.ERROR (item.Key + "->" + item.Value);
				}*/
				 
            }

            public void InitNamedGameDataCnf()
            {
                _time_send.Clear();
                _time_cnf.Clear();

                setTtl(GEnum.NamedGameData.ENone, 1);
                setTtl(GEnum.NamedGameData.EC2SMessageBattleList, 180);
                setTtl(GEnum.NamedGameData.EC2SMessageList, 24 * 3600);
                setTtl(GEnum.NamedGameData.EC2SRoomCardList, 60);
                setTtl(GEnum.NamedGameData.EC2SFriendList, 24 * 3600);
                setTtl(GEnum.NamedGameData.EC2SMahjongOpAction, 0.5f);
            }
        }

        static public NamedGameDataTimeCount NamedGameDataCnf
        {
            get
            {
                NamedGameDataTimeCount itemData = MemoryData.Get<NamedGameDataTimeCount>(MKey.NAMED_GAMEDATA_TIME_COUNT);
                if (itemData == null)
                {
                    itemData = new NamedGameDataTimeCount();
                    MemoryData.Set(MKey.NAMED_GAMEDATA_TIME_COUNT, itemData);
                }
                return itemData;
            }
        }

    }
    #endregion

}
