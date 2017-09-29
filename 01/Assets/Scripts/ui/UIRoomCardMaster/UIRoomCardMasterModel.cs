/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace projectQ
{
    public class UIRoomCardMasterModel : UIModelBase
    {
        public UIRoomCardMaster UI
        {
            get { return _ui as UIRoomCardMaster; }
        }

        private List<shop> RoomCardDataList = new List<shop>();

        #region override----------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysUI_RoomCardList_Succ,
                GEnum.NamedEvent.SysUI_RoomCardBuy_Succ,
                GEnum.NamedEvent.SysUI_PayFinish_Succ,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_RoomCardList_Succ:
                    RoomCard_LoadXml();
                    break;

                case GEnum.NamedEvent.SysUI_RoomCardBuy_Succ:

                    BuyParameterData rParameterData = MemoryData.RoomCardData.RParameterData;
                    if (rParameterData.ResultCode == 0)
                    {
                        WebSDKParams param = new WebSDKParams("WEB_OPEN_PAY_SERVICE");
                        param.InsertUrlParams(GameConfig.Instance.merchant_id,
                            rParameterData.GoodsID.ToString(),
                            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.MyAgentID.ToString(),
                            MemoryData.UserID.ToString(),
                            rParameterData.PreOrderID.ToString(),
                            "Purchase",
                            rParameterData.TotalFee.ToString(),
                            null,
                            null);

                        SDKManager.Instance.SDKFunction("WEB_OPEN_PAY_SERVICE", param);
                    }
                    else
                    {
                        this.UI.LoadPop(WindowUIType.SystemPopupWindow, "购买桌卡", "支付未完成，请重试", new string[] { "确定" }
                        , (index) => { });
                    }

                    break;

                case GEnum.NamedEvent.SysUI_PayFinish_Succ:

                    this.UI.LoadPop(WindowUIType.SystemPopupWindow, "进货成功", "进货成功，请在后台查看", new string[] { "确定" }
                            , (index) => { });

                    SDKManager.Instance.HiddenWebView();

                    break;
            }
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb----------------初始化桌卡数据------------------------------------------------

        public void RoomCardListInit()
        {
            shop shopData = RoomCardDataList[0];
            UI.RoomCardData = shopData;
            UI.RoomCardID = int.Parse(shopData.GoodsID);

            UI.RoomCardNum = 1;
            UI.RoomCardValueSet();
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb----------------读取本地Xml文件解析数据存储在本地-----------------------------

        public void RoomCard_LoadXml()
        {
            RoomCardDataList = new List<shop>();

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["shop"];

            foreach (BaseXmlBuild build in buildList)
            {
                shop info = (shop)build;

                foreach (GoodsData gData in MemoryData.RoomCardData.RGoodsData)
                {
                    if (info.GoodsID.Equals(gData.GoodsID.ToString()))
                    {
                        RoomCardDataList.Add(info);
                    }
                }
            }

            RoomCardListInit();
        }

        #endregion -----------------------------------------------------------------------------
    }
}


