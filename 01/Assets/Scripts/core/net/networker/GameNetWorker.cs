/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg;

namespace projectQ
{

    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfGame()
        {
            ModelNetWorker.Regiest<SyncMoney>(SyncMoney);
            ModelNetWorker.Regiest<RoomApplyFinishNotify>(RoomApplyFinishNotify);
            //修改帐号
            ModelNetWorker.Regiest<ModifyAccountRsp>(OnModifyAccountRsp);
            
        }
        //临时写的+++++++++++++++++++++++++++++++++++
        public void AreaReq(object data)
        {
            AreaRsp(null);
        }
        public void AreaRsp(object data)
        {
            List<SysArea> areaList = new List<SysArea>();
            int ids = 1;
            SysArea area = new SysArea(ids++, "河南",0, 0);
            areaList.Add(area);
            area = new SysArea(ids++, "郑州", 1, ids - 1);
            areaList.Add(area);

            area = new SysArea(ids++, "安徽", 0, 0);
            areaList.Add(area);
            area = new SysArea(ids++, "洛阳", 1, ids - 1);
            areaList.Add(area);

            area = new SysArea(ids++, "河北", 0, 0);
            areaList.Add(area);
            area = new SysArea(ids++, "开封", 1, ids - 1);
            areaList.Add(area);
            area = new SysArea(ids++, "石家庄", 1, ids - 2);
            areaList.Add(area);

            area = new SysArea(ids++, "山东", 0, 0);
            areaList.Add(area);
            area = new SysArea(ids++, "济南", 1, ids - 1);
            areaList.Add(area);
            area = new SysArea(ids++, "烟台", 1, ids - 2);
            areaList.Add(area);

            area = new SysArea(ids++, "山西", 0, 0);
            areaList.Add(area);
            area = new SysArea(ids++, "大同", 1, ids - 1);
            areaList.Add(area);

            area = new SysArea(ids++, "广东", 0, 0);
            areaList.Add(area);
            area = new SysArea(ids++, "广州", 1, ids - 1);
            areaList.Add(area);

            area = new SysArea(ids++, "四川", 0, 0);
            areaList.Add(area);
            area = new SysArea(ids++, "成都", 1, ids - 1);
            areaList.Add(area);
            MemoryData.AreaData.AreaListInterpreter(areaList);
        }




        /// <summary>
        /// 货币接收
        /// </summary>
        /// <param name="data"></param>
        public void SyncMoney(object data)
        {
            var rsp = data as SyncMoney;
            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Money = rsp.money;
            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Diamond = rsp.diamond;
            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindTickets = rsp.BindTickets;
            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Tickets = rsp.Tickets;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_User_RoomCard_Update);
        }

        /// <summary>
        ///  馆长申请完成通知客户端
        /// </summary>
        public void RoomApplyFinishNotify(object data)
        {
            var rsp = data as RoomApplyFinishNotify;
            QLoger.LOG("收到馆长申请完成 RoomApplyFinishNotify_" + rsp.ResultCode + "_" + rsp.UserID);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_RoomApplyFinishNotify, rsp.ResultCode, rsp.UserID);
        }

        private void OnModifyAccountRsp(object data)
        {
            var rsp = data as ModifyAccountRsp;
            if(rsp.ResultCode == (int) ErrorCode.ErrCode_Success)//成功
            {
                MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Account = rsp.Account;
               
            }
           
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EModificationAccountResult, rsp.ResultCode);
        }
        public void ModifyAccountReq(string accountInfo)
        {
            ModifyAccountReq req = new Msg.ModifyAccountReq();
            req.Account = accountInfo;
            req.UserID = MemoryData.UserID;
            this.send(req);
        }
    }
}
