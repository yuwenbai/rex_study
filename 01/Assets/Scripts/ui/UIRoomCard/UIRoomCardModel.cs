/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public class UIRoomCardModel : UIModelBase
    {
        public UIRoomCard UI
        {
            get { return _ui as UIRoomCard; }
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
                GEnum.NamedEvent.ERCCloseingAndRelushUI,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_RoomCardList_Succ:
                case GEnum.NamedEvent.ERCCloseingAndRelushUI:
                    UI.StopSendLoading();
                    RoomCard_LoadXml();
                    break;

                case GEnum.NamedEvent.SysUI_RoomCardBuy_Succ:
                    BuyParameterData rParameterData = MemoryData.RoomCardData.RParameterData;
                    if (rParameterData.ResultCode == 0)
                    {
                        WebSDKParams param = new WebSDKParams("WEB_OPEN_PAY_SERVICE");
                        param.InsertUrlParams(GameConfig.Instance.merchant_id,
                                     rParameterData.GoodsID.ToString(),
                                     MemoryData.UserID.ToString(),
                                     MemoryData.UserID.ToString(),
                                     rParameterData.PreOrderID.ToString(),
                                     null,
                                     rParameterData.TotalFee.ToString(),
                                     null,
                                     null);
                        SDKManager.Instance.SDKFunction("WEB_OPEN_PAY_SERVICE", param);
                    }
                    else
                    {
                        this.UI.LoadPop(WindowUIType.SystemPopupWindow, "支付失败", "支付未完成，请重试", new string[] { "确定" }
                        , (index) => { });
                    }

                    break;

                case GEnum.NamedEvent.SysUI_PayFinish_Succ:
                    string goodsId = data[0].ToString();

                    string promptStr = MemoryData.XmlData.XmlBuildDataSole_Get("shop", "GoodsID", goodsId, "BuyInfo");
                    string goodsNun = MemoryData.XmlData.XmlBuildDataSole_Get("shop", "GoodsID", goodsId, "ItemNum");
                    string succStr = string.Format(promptStr, goodsNun);

                    this.UI.LoadPop(WindowUIType.SystemPopupWindow, "支付成功", succStr, new string[] { "确定" }
                            , (index) => { });

                    SDKManager.Instance.HiddenWebView();

                    break;
            }
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb----------------创建桌卡数据------------------------------------------------

        void RoomCardListCreat()
        {
            UITools.CreateChild<shop>(UI.RoomCaardPanelObj.transform, null, RoomCardDataList, (go, cardData) =>
            {
                UIRoomCardList roomCardList = go.GetComponent<UIRoomCardList>();
                roomCardList.RoomCardInit(cardData);
            });
            UI.RoomCaardPanelObj.Reposition();
            UI.ScrollViewObj.ResetPosition();
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

            RoomCardListCreat();
        }

        #endregion -----------------------------------------------------------------------------
    }
}