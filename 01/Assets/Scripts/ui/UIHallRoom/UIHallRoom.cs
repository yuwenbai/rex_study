/**
* 作者：周腾\GarFey
* 作用：
* 日期：
*/
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class UIHallRoom : UIViewBase
    {
        public UIHallRoomModel Model { get { return _model as UIHallRoomModel; } }
        public UILabel cardNumLab;
        public UILabel caluNumLab;
        public GameObject closeButtonObj;

        #region YearScrollView Content
        public UIScrollView yearScrollView;
        public GameObject yearContent;
        public GameObject yearItem;
        private List<GameObject> yearObjList = new List<GameObject>();
        #endregion
        #region MonthScrollView Content
        public UIScrollView monthScrollView;
        public GameObject monthContent;
        public GameObject monthItem;
        private List<GameObject> monthObjList = new List<GameObject>();
        #endregion
        #region DayScrollView
        public GameObject dayContent;
        public GameObject dayItem;
        private List<GameObject> dayObjList = new List<GameObject>();
        public GameObject noDayData;
        public UI.ScrollViewTool.ScrollViewWrapContent dayScrollView;
        UI.ScrollViewTool.ScrollViewMgr<DayItemData> dayscrollViewManager = new UI.ScrollViewTool.ScrollViewMgr<DayItemData>();
        #endregion
        private bool first = true;
        public UILabel mjRoomNameLabel;
        private int createYear;
        private int createMonth;
        private int createDay;
        /// <summary>
        /// 关闭按钮监听
        /// </summary>
        /// <param name="go"></param>
        void OnCloseBtn(GameObject go)
        {
            Model.ClearInfo();
            ClearYearItem();
            ClearMonthItem();
            ClearDayItem();
            //this.Close();
            this.Hide();
        }


        /// <summary>
        /// 计算总计
        /// </summary>
        void RefreshCusome(int selectYear, int selectMonth)
        {
            cardNumLab.text = Model.GetCardByYearMonth(selectYear, selectMonth).ToString();
            caluNumLab.text = Model.GetCaluByYearMonth(selectYear, selectMonth).ToString();
        }
        #region override
        public override void Init()
        {
            yearObjList = new List<GameObject>();
            monthObjList = new List<GameObject>();
            dayObjList = new List<GameObject>();
            UIEventListener.Get(closeButtonObj).onClick = OnCloseBtn;

        }

        public override void OnShow()
        {
            Model.GetCurrentTime();
            Model.SendReq();
            InitDayScrollView();
            //InitUI();

        }

        public override void OnHide()
        {

        }
        #endregion

        #region Year
        /// <summary>
        /// 初始化年列表的Content的Center委托
        /// </summary>
        void InitYearYearWrapDele()
        {
            UICenterOnChild year = yearContent.GetComponent<UICenterOnChild>();
            if (year == null)
            {
                yearContent.AddComponent<UICenterOnChild>();
            }
            yearContent.GetComponent<UICenterOnChild>().onCenter = YearWrapDeleGate;
        }
        /// <summary>
        /// 年列表Content委托的回调
        /// </summary>
        void YearWrapDeleGate(GameObject centerGo)
        {

            Model.selectYear = centerGo.GetComponent<YearItem>().year;
            InitMonthScrollView(Model.selectYear, first);
            first = false;

        }

        /// <summary>
        /// 初始化年ScrollView
        /// </summary>
        void InitYearScrollView()
        {
            //ClearYearItem();
            //for (int i = 0; i < Model.GetYears().Count; i++)
            //{
            //    GameObject go = Instantiate(yearItem) as GameObject;
            //    go.transform.SetParent(yearContent.transform);
            //    go.transform.localEulerAngles = Vector3.zero;
            //    go.transform.localPosition = Vector3.zero;
            //    go.transform.localScale = Vector3.one;
            //    go.GetComponent<YearItem>().InitYearItem(Model.GetYears()[i]);
            //    go.gameObject.name = Model.GetYears()[i].ToString();
            //    //go.GetComponent<UICenterOnClick>().OnClick = OnYearItemCenterClick;
            //    go.SetActive(true);
            //    yearObjList.Add(go);
            //}
            for (int i = 0; i < Model.GetLocalYears(createYear).Count; i++)
            {
                GameObject go = Instantiate(yearItem) as GameObject;
                go.transform.SetParent(yearContent.transform);
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                int year = Model.GetLocalYears(createYear)[i];
                go.GetComponent<YearItem>().InitYearItem(year);
                go.gameObject.name = year.ToString();
                //go.GetComponent<UICenterOnClick>().OnClick = OnYearItemCenterClick;
                go.SetActive(true);
                yearObjList.Add(go);
            }
            yearContent.GetComponent<UIGrid>().Reposition();
            yearScrollView.GetComponent<UIScrollView>().ResetPosition();
            InitYearYearWrapDele();
            YearScrollOffset();
        }

        void YearScrollOffset()
        {
            for (int i = 0; i < yearObjList.Count; i++)
            {
                if (Model.selectYear == yearObjList[i].GetComponent<YearItem>().year)
                {
                    yearObjList[i].GetComponent<UICenterOnClick>().OnClick();
                    yearScrollView.MoveRelative(new Vector3(0, (yearObjList.Count) * 60, 0));
                }
            }
        }
        void OnYearItemCenterClick(GameObject go)
        {
            QLoger.LOG("on year center click" + go.name);
        }
        #endregion
        #region Month
        /// <summary>
        /// 通过年份初始化月的数据列表
        /// </summary>
        /// <param name="selectYear"></param>
        void InitMonthScrollView(int selectYear, bool first)
        {
            ClearMonthItem();
            //for (int i = Model.GetMonthDataByYear(selectYear)[0].month; i <= Model.GetMonthDataByYear(selectYear)[Model.GetMonthDataByYear(selectYear).Count - 1].month; i++)
            //{
            //    GameObject go = Instantiate(monthItem) as GameObject;
            //    go.transform.SetParent(monthContent.transform);
            //    go.transform.localEulerAngles = Vector3.zero;
            //    go.transform.localPosition = Vector3.zero;
            //    go.transform.localScale = Vector3.one;
            //    go.GetComponent<MonthItem>().InitLabel(i, selectYear);
            //    go.gameObject.name = i.ToString();
            //    go.SetActive(true);
            //    monthObjList.Add(go);
            //}

            for (int i = 0; i < Model.GetLocalMonthByYear(selectYear, createYear, createMonth).Count; i++)
            {
                GameObject go = Instantiate(monthItem) as GameObject;
                go.transform.SetParent(monthContent.transform);
                GameObjectHelper.NormalizationTransform(go.transform);
                int month = Model.GetLocalMonthByYear(selectYear, createYear, createMonth)[i];
                go.GetComponent<MonthItem>().InitLabel(month, selectYear);
                go.gameObject.name = month.ToString();
                go.SetActive(true);
                monthObjList.Add(go);
            }
            monthContent.GetComponent<UIGrid>().Reposition();
            monthScrollView.GetComponent<UIScrollView>().ResetPosition();
            InitMonthDele();
            if (first)
            {
                ScrollOffset(Model.selectMonth);
            }
            else
            {
                monthObjList[0].GetComponent<UICenterOnClick>().OnClick();
                ScrollOffset(1);
            }
        }

        void ScrollOffset(int month)
        {
            switch (month)
            {
                case 1:
                    monthScrollView.MoveRelative(new Vector3(0, (Model.selectMonth) * 60 - 220, 0));
                    break;
                case 2:
                    monthScrollView.MoveRelative(new Vector3(0, (Model.selectMonth) * 60 - 280, 0));
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    monthScrollView.MoveRelative(new Vector3(0, (Model.selectMonth) * 60 - 90, 0));
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    monthScrollView.MoveRelative(new Vector3(0, (Model.selectMonth) * 60 - 20, 0));
                    break;
            }
        }
        void InitMonthDele()
        {
            UICenterOnChild month = monthContent.GetComponent<UICenterOnChild>();
            if (month == null)
            {
                monthContent.AddComponent<UICenterOnChild>();
            }
            monthContent.GetComponent<UICenterOnChild>().onCenter = MonthDele;
        }

        void MonthDele(GameObject go)
        {
            QLoger.LOG("monthdele " + go.name);
            Model.selectMonth = go.GetComponent<MonthItem>().month;

            InitDayScrollViewData(Model.selectYear, Model.selectMonth);
        }
        #endregion

        #region Day

        void InitDayScrollView()
        {
            dayscrollViewManager.Init(dayScrollView);
        }
        /// <summary>
        /// 初始化每一天的数据
        /// </summary>
        void InitDayScrollViewData(int year, int month)
        {
            //if (Model.GetDayDatasByYearMonth(year, month).Count > 0)
            //{
            //    dayContent.SetActive(true);
            //    noDayData.SetActive(false);
            //    dayscrollViewManager.RefreshScrollView(Model.GetDayDatasByYearMonth(year, month));
            //}
            //else
            //{
            //    noDayData.SetActive(true);
            //    dayContent.SetActive(false);
            //}
            if (Model.GetDaysByYearMonth(year, month, createDay,createYear,createMonth).Count > 0)
            {
                dayContent.SetActive(true);
                noDayData.SetActive(false);
                dayscrollViewManager.RefreshScrollView(Model.GetDaysByYearMonth(year, month, createDay,createYear,createMonth));
            }
            else
            {
                noDayData.SetActive(true);
                dayContent.SetActive(false);
            }
            for (int i = 0; i < yearObjList.Count; i++)
            {
                if (year == yearObjList[i].GetComponent<YearItem>().year)
                {
                    yearObjList[i].GetComponent<YearItem>().SetCenter();
                }
                else
                {
                    yearObjList[i].GetComponent<YearItem>().SetNoCenter();
                }
            }
            for (int i = 0; i < monthObjList.Count; i++)
            {
                if (month == monthObjList[i].GetComponent<MonthItem>().month)
                {
                    monthObjList[i].GetComponent<MonthItem>().SetCenter();
                }
                else
                {
                    monthObjList[i].GetComponent<MonthItem>().SetNoCenter();
                }
            }
            RefreshCusome(year, month);
        }

        #endregion
        public void InitUI()
        {
            int roomID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID;
            MjRoom room = MemoryData.MjHallData.GetMjHallById(roomID);
            mjRoomNameLabel.text = room.RoomName;
            GetCreateRoomTime();
            //mjRoomNameLabel.text = MemoryData.PlayerData.MyPlayerModel.m;
            InitYearScrollView();
        }
        void GetCreateRoomTime()
        {
            int roomID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID;
            MjRoom room = MemoryData.MjHallData.GetMjHallById(roomID);
            string createTime = room.CreateTime.Substring(0, 10);
            string[] cTime = createTime.Split('-');
            createYear = int.Parse(cTime[0]);
            createMonth = int.Parse(cTime[1]);
            createDay = int.Parse(cTime[2]);


            //string createTime = 
        }
        void ClearDayItem()
        {
            //for (int i = 0; i < dayContent.GetComponent<UIGrid>().GetChildList().Count; i++)
            //{
            //    dayContent.GetComponent<UIGrid>().RemoveChild(dayContent.GetComponent<UIGrid>().GetChildList()[i]);
            //}
            for (int i = 0; i < dayObjList.Count; i++)
            {
                Destroy(dayObjList[i]);
            }
            dayObjList.Clear();
        }

        void ClearMonthItem()
        {
            //for (int i = 0; i < monthContent.GetComponent<UIGrid>().GetChildList().Count; i++)
            //{
            //    monthContent.GetComponent<UIGrid>().RemoveChild(monthContent.GetComponent<UIGrid>().GetChildList()[i]);
            //}
            for (int i = 0; i < monthObjList.Count; i++)
            {
                Destroy(monthObjList[i]);
            }
            monthObjList.Clear();
        }

        void ClearYearItem()
        {
            //for (int i = 0; i < yearContent.GetComponent<UIGrid>().GetChildList().Count; i++)
            //{
            //    yearContent.GetComponent<UIGrid>().RemoveChild(yearContent.GetComponent<UIGrid>().GetChildList()[i]);
            //}
            for (int i = 0; i < yearObjList.Count; i++)
            {
                Destroy(yearObjList[i]);
            }
            yearObjList.Clear();
        }
    }

}
