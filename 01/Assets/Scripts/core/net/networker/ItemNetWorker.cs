/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Msg ;

namespace projectQ
{
    //注册数据处理 好友网络数据处理
    public partial class ModelNetWorker{

        public void initDefaultHandleOfItem()
        {
            ModelNetWorker.Regiest<MyBagItemListRsp>(MyBagItemListRsp);
            ModelNetWorker.Regiest<SyncMyBagItemNotify>(SyncMyBagItemNotify);
        }

        #region Get Message

        /// <summary>
        /// 获得我的道具列表
        /// </summary>
        /// <param name="rsp">Rsp.</param>
        public void MyBagItemListRsp(object rsp)
        {
            var prsp = rsp as MyBagItemListRsp;
            for (int i = 0; i < prsp.ItemList.Count; i++) 
            {
                var p = prsp.ItemList [i];
                var op = new ItemData ();
                op.id = p.ItemID;
                op.count = p.ItemCount;
                op.guid = p.ItemGUID;

                MemoryData.ItemData [op.guid] = op;
            }
            //FIRE_EVENT 获取到好友信息
        }

        public void SyncMyBagItemNotify(object rsp)
        {
            var prsp = rsp as SyncMyBagItemNotify;
            for (int i = 0; i < prsp.ItemList.Count; i++) 
            {
                var p = prsp.ItemList [i];
                var op = new ItemData ();
                op.id = p.ItemID;
                op.count = p.ItemCount;
                op.guid = p.ItemGUID;

                MemoryData.ItemData [op.guid] = op;
            }
            //FIRE_EVENT 获取到好友信息
        }

        #endregion

        #region Send Message

        /// <summary>
        /// 请求背包
        /// </summary>
        public void MyBagItemListReq()
        {
            var msg = new MyBagItemListReq();
            msg.UserID = MemoryData.UserID ;
            this.send (msg);
        }
        #endregion

    }
}