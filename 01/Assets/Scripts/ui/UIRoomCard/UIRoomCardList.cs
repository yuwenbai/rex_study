/**
 * @Author lyb
 *  单个桌卡信息
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIRoomCardList : MonoBehaviour
    {
        /// <summary>
        /// 桌卡ID
        /// </summary>
        public string RoomCardID;
        /// <summary>
        /// 桌卡图像
        /// </summary>
        public UISprite RoomCardSpr;
        /// <summary>
        /// 桌卡名字
        /// </summary>
        public UILabel RoomCardName;        
        /// <summary>
        /// 桌卡價格str
        /// </summary>
        public UILabel RoomCardPriceStr;

        /// <summary>
        /// 桌卡價格
        /// </summary>
        private string RoomCardPrice;

        void Start(){}

        void OnDestroy()
        {
            RoomCardSpr = null;
            RoomCardName = null;
            RoomCardPrice = null;
            RoomCardPriceStr = null;
        }

        /// <summary>
        /// 初始化单个桌卡信息
        /// </summary>
        public void RoomCardInit(shop data)
        {
            RoomCardID = data.GoodsID;
            RoomCardSpr.spriteName = data.ImageRes;
            RoomCardSpr.MakePixelPerfect();
            RoomCardName.text = string.Format(data.Notes, data.ItemNum);
            RoomCardPriceStr.text = string.Format(data.ButtonDesc , data.MoneyNum);
        }

        /// <summary>
        /// 点击桌卡按钮
        /// </summary>
        void OnClick()
        {
            ModelNetWorker.Instance.C2SRoomCardBuyReq(int.Parse(RoomCardID));
        }
    }
}


