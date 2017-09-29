/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg;
namespace projectQ { 
    /// <summary>
    /// 处理馆主麻将馆桌卡销售记录
    /// </summary>
public partial class ModelNetWorker  {

        public void initDefaultHandleOfSale()
        {
            ModelNetWorker.Regiest<FRoomQuerySaleDataRsp>(SaleRsp);
        }
        public void SaleReq()
        {
            var req = new FRoomQuerySaleDataReq();
            req.UserID = MemoryData.UserID;
            req.RoomID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID;
            this.send(req);
        }

        public void SaleRsp(object data)
        {
            var rsp = data as FRoomQuerySaleDataRsp;
            if (rsp.YearSales != null)
            {
                List<Msg.YearSaleData> yearList = rsp.YearSales;
                if (yearList != null && yearList.Count != 0)
                {
                    for (int i = 0; i < yearList.Count; i++)
                    {
                        SaleDataManager.AddYearData(SaleDataManager.ProtoYearToData(yearList[i]));

                        List<Msg.MonthSaleData> monthList = yearList[i].MonthSales;
                        if (monthList != null && monthList.Count != 0)
                        {
                            for (int j = 0; j < monthList.Count; j++)
                            {
                                SaleDataManager.AddMonthData(SaleDataManager.ProtoMonthToData(monthList[j],yearList[i].YearIndex));
                                List<Msg.DaySaleData> dayList = monthList[i].DaySales;
                                if (dayList != null && dayList.Count != 0)
                                {
                                    for (int k = 0; k < dayList.Count; k++)
                                    {
                                        SaleDataManager.AddDayData(SaleDataManager.ProtoToDayData(dayList[k],yearList[i].YearIndex,monthList[j].MonthIndex));
                                    }
                                }
                            }
                        }
                    }
                }
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_QuerySaleData_Rsp);
                //for (int i = 0; i < rsp.YearSales.Count; i++)
                //{
                //    SaleDataManager.AddYearData(SaleDataManager.ProtoYearToData(rsp.YearSales[i]));
                //   // 
                //}
            }
            
        }

    }
}
