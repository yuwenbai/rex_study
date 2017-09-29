/**
 * 作者：周腾\GarFey
 * 作用：
 * 日期：
 */

using System.Collections.Generic;
using UnityEngine;
using System;
namespace projectQ
{
    public static class SaleDataManager
    {
        private static List<YearData> yearDataList = new List<YearData>();
        private static List<MonthItemData> monthItemData = new List<MonthItemData>();
        //private static List<DayItemData> dayItemData = new List<DayItemData>();
        /// <summary>
        /// 年的协议转为数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static YearData ProtoYearToData(Msg.YearSaleData data)
        {
            List<MonthItemData> monthList = new List<MonthItemData>();
            YearData result = new YearData();
            result.year = data.YearIndex;
            result.income = data.Income;
            result.tickets = data.Tickets;
            for (int i = 0; i < data.MonthSales.Count; i++)
            {
                monthList.Add(ProtoMonthToData(data.MonthSales[i], data.YearIndex));
            }
            result.monthSaleData = monthList;
            return result;
        }
        /// <summary>
        /// 添加年数据
        /// </summary>
        /// <param name="data"></param>
        public static void AddYearData(YearData data)
        {
            if (yearDataList == null)
            {
                yearDataList = new List<YearData>();
            }
            yearDataList.Add(data);
        }
        /// <summary>
        /// 获取年数据列表
        /// </summary>
        /// <returns></returns>
        public static List<YearData> GetYearData()
        {
            return yearDataList;
        }

        public static MonthItemData ProtoMonthToData(Msg.MonthSaleData data, int year)
        {

            MonthItemData result = new MonthItemData();
            List<DayItemData> dayDatas = new List<DayItemData>();
            result.month = data.MonthIndex;
            result.income = data.Income;
            result.tickets = data.Tickets;
            result.year = year;
            for (int i = 0; i < data.DaySales.Count; i++)
            {

                dayDatas.Add(ProtoToDayData(data.DaySales[i], year, data.MonthIndex));

            }
            result.daySales = dayDatas;
            return result;
        }

        public static void AddMonthData(MonthItemData data)
        {
            if (monthItemData == null)
            {
                monthItemData = new List<MonthItemData>();
            }
            monthItemData.Add(data);
        }

        public static List<MonthItemData> GetMonthData()
        {
            return monthItemData;
        }

        public static DayItemData ProtoToDayData(Msg.DaySaleData data, int year, int month)
        {
            DayItemData result = new DayItemData();
            result.calcu = data.Income;
            result.card = data.Tickets;
            result.day = data.DayIndex;
            result.year = year;
            result.month = month;
            return result;
        }
        public static void AddDayData(DayItemData data)
        {
            List<DayItemData> dayItemData = null;
            if (dayItemData == null)
            {
                dayItemData = new List<DayItemData>();
            }
            dayItemData.Add(data);
        }


        /// <summary>
        /// 通过年，月获取这一年，这一月里每天的数据
        /// </summary>
        /// <param name="selectYear"></param>
        /// <param name="selectMonth"></param>
        /// <returns></returns>
        public static List<DayItemData> GetSelectYearMonthDayDatasByYearMonth(int selectYear, int selectMonth)
        {
            List<DayItemData> dayItemList = new List<DayItemData>();
            for (int i = 0; i < yearDataList.Count; i++)
            {
                if (selectYear == yearDataList[i].year)
                {
                    for (int j = 0; j < yearDataList[i].monthSaleData.Count; j++)
                    {
                        if (selectMonth == yearDataList[i].monthSaleData[j].month)
                        {
                            dayItemList = yearDataList[i].monthSaleData[j].daySales;
                            //return dayItemList;
                        }

                    }
                }

            }

            return dayItemList;
        }

        public static void ClearInfo()
        {
            yearDataList.Clear();
            monthItemData.Clear();
            //dayItemData.Clear();
        }
    }

    public class UIHallRoomModel : UIModelBase
    {
        private UIHallRoom UI { get { return _ui as UIHallRoom; } }
        [HideInInspector]
        public int selectYear;
        [HideInInspector]
        public int currYear;
        [HideInInspector]
        public int selectMonth;
        [HideInInspector]
        public int currMonth;

        private List<YearData> yearList = new List<YearData>();
        private List<MonthItemData> monthList = new List<MonthItemData>();
        /// <summary>
        /// 获取当前日期
        /// </summary>
        public void GetCurrentTime()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            selectYear = currentTime.Year;
            currYear = currentTime.Year;
            selectMonth = currentTime.Month;
            currMonth = currentTime.Month;
        }
        /// <summary>
        /// 通过月份获得这个月有多少天
        /// </summary>
        /// <param name="month"></param>
        public int GetDaysByMonth(int year, int month)
        {
            DateTime d1 = new DateTime(year, month, 1);//当前月第一天
            DateTime d2 = d1.AddMonths(1).AddDays(-1);
            return d2.Day;
        }
        public void ClearInfo()
        {
            SaleDataManager.ClearInfo();
        }


        /// <summary>
        /// 获取年的数据列表
        /// </summary>
        /// <returns></returns>
        public List<YearData> GetYearsData()
        {
            return SaleDataManager.GetYearData();
        }

        /// <summary>
        /// 获取都有哪一年的数据的int列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetYears()
        {
            List<int> year = new List<int>();
            for (int i = 0; i < SaleDataManager.GetYearData().Count; i++)
            {
                year.Add(SaleDataManager.GetYearData()[i].year);
            }
            return year;
            // return SaleDataManager.GetYearData();
        }

        /// <summary>
        /// 通过年，月获取天的数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<DayItemData> GetDayDatasByYearMonth(int year, int month)
        {

            return SaleDataManager.GetSelectYearMonthDayDatasByYearMonth(year, month);
        }

        public List<int> GetMonthByYear(int year)
        {
            List<int> month = new List<int>();

            for (int i = 0; i < GetMonthDataByYear(year).Count; i++)
            {
                month.Add(GetMonthDataByYear(year)[i].month);
            }
            return month;
        }
        /// <summary>
        /// 通过年获取月的数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<MonthItemData> GetMonthDataByYear(int year)
        {
            List<MonthItemData> monthItemData = new List<MonthItemData>();
            for (int i = 0; i < SaleDataManager.GetYearData().Count; i++)
            {
                if (year == SaleDataManager.GetYearData()[i].year)
                {
                    monthItemData = SaleDataManager.GetYearData()[i].monthSaleData;
                }
            }
            return monthItemData;
        }

        public int GetCardByYearMonth(int year, int month)
        {
            int card = 0;
            for (int i = 0; i < SaleDataManager.GetYearData().Count; i++)
            {
                if (year == SaleDataManager.GetYearData()[i].year)
                {
                    for (int j = 0; j < SaleDataManager.GetYearData()[i].monthSaleData.Count; j++)
                    {
                        if (month == SaleDataManager.GetYearData()[i].monthSaleData[j].month)
                        {
                            card = SaleDataManager.GetYearData()[i].monthSaleData[j].tickets;
                        }
                    }
                }
            }
            return card;
        }

        public int GetCaluByYearMonth(int year, int month)
        {
            int calu = 0;
            for (int i = 0; i < SaleDataManager.GetYearData().Count; i++)
            {
                if (year == SaleDataManager.GetYearData()[i].year)
                {
                    for (int j = 0; j < SaleDataManager.GetYearData()[i].monthSaleData.Count; j++)
                    {
                        if (month == SaleDataManager.GetYearData()[i].monthSaleData[j].month)
                        {
                            calu = SaleDataManager.GetYearData()[i].monthSaleData[j].income;
                        }
                    }
                }
            }
            return calu;
        }
        /// <summary>
        /// 伪造的天数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<DayItemData> GetDaysByYearMonth(int year, int month, int createDay,int createYear,int createMonth)
        {
            List<DayItemData> localDayData = new List<DayItemData>();
            List<DayItemData> serverData = GetDayDatasByYearMonth(year, month);
            int days = DateTime.DaysInMonth(year, month);
            if (year == createYear && month == createMonth)
            {
                for (int i = createDay; i <= days; i++)
                {
                    DayItemData data = new DayItemData();
                    data.calcu = 0;
                    data.card = 0;
                    data.day = i;
                    data.month = month;
                    data.year = year;
                    localDayData.Add(data);
                }
            }
            else
            {
                for (int i = 1; i <= days; i++)
                {
                    DayItemData data = new DayItemData();
                    data.calcu = 0;
                    data.card = 0;
                    data.day = i;
                    data.month = month;
                    data.year = year;
                    localDayData.Add(data);
                }
            }
            for (int j = 0; j < localDayData.Count; j++)
            {
                for (int k = 0; k < serverData.Count; k++)
                {
                    if (localDayData[j].day == serverData[k].day)
                    {
                        localDayData[j] = serverData[k];
                    }
                }
            }
            return localDayData;
        }

        /// <summary>
        /// 伪造的年数据
        /// </summary>
        /// <param name="createYear"></param>
        /// <returns></returns>
        public List<int> GetLocalYears(int createYear)
        {
            List<int> yearList = new List<int>();
            for (int i = createYear; i <= currYear; i++)
            {
                yearList.Add(i);
            }
            return yearList;
        }
        /// <summary>
        /// 伪造的月数据
        /// </summary>
        /// <param name="selectYear"></param>
        /// <param name="createYear"></param>
        /// <param name="createMonth"></param>
        /// <returns></returns>
        public List<int> GetLocalMonthByYear(int selectYear, int createYear, int createMonth)
        {
            List<int> monthList = new List<int>();
            if (selectYear == createYear && selectYear == currYear)
            {
                if (createMonth == currMonth)
                {
                    monthList.Add(currMonth);
                }
                else
                {
                    for (int i = createMonth; i <= currMonth; i++)
                    {
                        monthList.Add(i);
                    }
                }

            }
            else
            {
                if (selectYear == createYear && selectYear != currYear)
                {
                    for (int i = createMonth; i <= 12; i++)
                    {
                        monthList.Add(i);
                    }
                }
                if (selectYear == currYear && selectYear != createYear)
                {
                    for (int i = 1; i <= currMonth; i++)
                    {
                        monthList.Add(i);
                    }
                }
                if (selectYear != currYear && selectYear != createYear)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        monthList.Add(i);
                    }
                }
            }

            return monthList;
        }
        /// <summary>
        /// 请求数据
        /// </summary>    
        public void SendReq()
        {
            UI.LoadSendLoading();
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageGameHistory);
            //ModleNetWorker.Instance.SaleReq();
        }
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysData_Other_QuerySaleData_Rsp,
                GEnum.NamedEvent.ERCCloseingAndRelushUI,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_Other_QuerySaleData_Rsp:

                    UI.StopSendLoading();
                    UI.InitUI();
                    //UI.OnInitUI();
                    break;
                case GEnum.NamedEvent.ERCCloseingAndRelushUI:
                    UI.StopSendLoading();
                    UI.InitUI();
                    break;
            }
        }



    }
}
