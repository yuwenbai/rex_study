/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ {
    
public class UIRedPackgeModel : UIModelBase {
	public UIRedPackge UI { get { return _ui as UIRedPackge; } }

        public void OnSendRedPacketReq(long userId,int ticket,int num,string msg)
        {
            ModelNetWorker.Instance.SendRedPacket(userId, ticket, num, msg);
        }
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.SysData_Other_PushMoney_Rsp,
                //GEnum.NamedEvent.SysData_Other_PushGift_Rsp,
                //GEnum.NamedEvent.SysData_Other_ReceiveGift_Notify,
                //GEnum.NamedEvent.SysData_Other_ReceiveGift_Rsp,
                //GEnum.NamedEvent.SysData_Other_PushGift_Notify,


            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_Other_PushMoney_Rsp:
                    if (data != null)
                    {
                        int resultCode = (int)data[0];
                        if (resultCode == 0)
                        {

                            int bagId = (int)data[1];
                            string msg = data[2] as string;
                            UI.SendRedRsp(bagId, msg);
                            UI.InitMyCardLabel();
                            UI.LoadTip("红包发送成功");
                        }
                    }

                    break;
                //case GEnum.NamedEvent.SysData_Other_PushGift_Rsp:
                //    QLoger.LOG("UIPushModle SysData_Other_Rsp");
                //    //UI.giftPanel.GetComponent<GiftPacketPage>().ShowPushResult((int)data[0]);
                //    break;

                    //case GEnum.NamedEvent.SysData_Other_ReceiveGift_Notify:
                    //    break;

                    //case GEnum.NamedEvent.SysData_Other_ReceiveGift_Rsp:
                    //    break;

                    //case GEnum.NamedEvent.SysData_Other_PushGift_Notify:

                    //    break;

            }
        }

    }
}
