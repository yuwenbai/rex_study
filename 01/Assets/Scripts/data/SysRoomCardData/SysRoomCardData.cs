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
    /// 获取列表成功参数
    /// </summary>
    public class GoodsData
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public int GoodsID;
        /// <summary>
        /// ID
        /// </summary>
        public int TagID;
    }

    /// <summary>
    /// 购买成功参数
    /// </summary>
    public class BuyParameterData
    {
        /// <summary>
        /// 0 代表购买成功。 非0 代表购买失败
        /// </summary>
        public int ResultCode;
        /// <summary>
        /// 商品Id
        /// </summary>
        public int GoodsID;
        /// <summary>
        /// 预充值单ID
        /// </summary>
        public long PreOrderID;
        /// <summary>
        /// 数量
        /// </summary>
        public int TotalFee;
    }

    public class SysRoomCardData
    {
        private List<GoodsData> _RGoodsData = new List<GoodsData>();
        public List<GoodsData> RGoodsData
        {
            get{
                return _RGoodsData;
            }
        }

        private BuyParameterData _RParameterData = new BuyParameterData();
        public BuyParameterData RParameterData
        {
            get {
                return _RParameterData;
            }
        }

        /// <summary>
        /// 商品列表填充
        /// </summary>
        public void GoodsData_Update(List<Msg.GoodsData> bDataList)
        {
            _RGoodsData = new List<GoodsData>();
            foreach (Msg.GoodsData goodList in bDataList)
            {
                GoodsData gData = new GoodsData();
                gData.GoodsID = goodList.GoodsID;
                gData.TagID = goodList.TagID;

                _RGoodsData.Add(gData);
            }
        }

        /// <summary>
        /// 更新 赋值数据信息
        /// </summary>
        public void BuyParameterData_Update(Msg.BuyGoodsRsp bInfo)
        {
            _RParameterData = new BuyParameterData();
            _RParameterData.ResultCode = bInfo.ResultCode;
            _RParameterData.GoodsID = bInfo.GoodsID;
            _RParameterData.PreOrderID = bInfo.PreOrderID;
            _RParameterData.TotalFee = bInfo.TotalFee;            
        }
    }

    #region 内存数据------------------------------------------------------------------------------

    public partial class MKey
    {
        public const string USER_ROOM_CARD_DATA = "USER_ROOM_CARD_DATA";
    }

    public partial class MemoryData
    {
        static public SysRoomCardData RoomCardData
        {
            get
            {
                SysRoomCardData roomCardData = MemoryData.Get<SysRoomCardData>(MKey.USER_ROOM_CARD_DATA);
                if(roomCardData == null)
                {
                    roomCardData = new SysRoomCardData();
                    MemoryData.Set (MKey.USER_ROOM_CARD_DATA, roomCardData);
                }
                return roomCardData;
            }
        }
    }

    #endregion -----------------------------------------------------------------------------------
}