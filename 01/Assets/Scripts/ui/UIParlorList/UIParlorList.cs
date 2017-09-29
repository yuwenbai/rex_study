

using System;
/**
* @Author JEFF
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{

    public class UIParlorList : UIViewBase
    {
        private UIParlorListModel Model
        {
            get { return _model as UIParlorListModel; }
        }
        public GameObject labelNoData;
        public GameObject MainPage;
        public GameObject ButtonGoBack;
        public GameObject ButtonSearch;
        //public GameObject ButtonArea;
        public GameObject buttonService;
        //public GameObject servicePanel;
        public UIInput InputSearch;

        public UIParlorSearch PanelSearchScript;
        public SortPanel sortCity;
        public bool firstEnter = true;
        private float firstEnterTimer;
        public float clickTimer;
        public bool cantClick;
        #region Event
        /// <summary>
        /// 打开搜索结果
        /// </summary>
        public void OnOpenSearch()
        {
            PanelSearchScript.RefreshFriendMJParlor(Model.searchPriendDataList);
            MainPage.SetActive(false);
        }

        //临时添加的检测城市选单有没有麻将馆的方法，调用比较混乱，可能有风险
        public bool CheckIsHaveRoom(int RegionID)
        {
            return Model.CheckIsHaveRoom(RegionID);
        }

        private void OnSearchGoBackClick()
        {
            MainPage.SetActive(true);
        }
        private void OnButtonGoBackClick(GameObject go)
        {
            //this.GoBack();
            this.Close();
        }

        //搜索按钮点击
        private void OnButtonSearchClick(GameObject go)
        {
            string value = InputSearch.value;
            value = value.Trim();
            if (value.Length == 0)
            {
                LoadTip("请输入棋牌室ID或名字");
                return;
            }
            if (firstEnter || cantClick || sortCity.canClick)
            {
                return;
            }
            else
            {
                Model.OnSendSearch(InputSearch.value);
                ButtonSearch.GetComponent<BoxCollider>().enabled = false;
                //cantClick = true;
                InputSearch.value = "";
            }

        }

        private void Update()
        {
            if (firstEnter)
            {
                firstEnterTimer += Time.deltaTime;
                if (firstEnterTimer > 3.5)
                {
                    firstEnter = false;
                    firstEnterTimer = 0;

                }
            }
            if (cantClick)
            {
                clickTimer += Time.deltaTime;
                if (clickTimer > 3.5)
                {
                    cantClick = false;
                    clickTimer = 0;
                }
            }
        }
        public void OnCityIdSet(int cityId)
        {
            Model.SetCityId(cityId);
        }
        public void OnPopupListSelect(string key)
        {
            Model.SetSortType(key);

            scrollViewMgr.RefreshScrollView(Model.SortByType());
        }

        private void OnButtonAreaClick(GameObject go)
        {
            LoadUIMain("UIMap");
        }

        public void OnSortClick()
        {
            Model.OnSendSearch();
        }

        #endregion

        #region ScrollView
        public UI.ScrollViewTool.ScrollViewWrapContent ParlorScrollView;
        UI.ScrollViewTool.ScrollViewMgr<ParlorItemData> scrollViewMgr = new UI.ScrollViewTool.ScrollViewMgr<ParlorItemData>();

        private void InitScrollView()
        {
            scrollViewMgr.Init(ParlorScrollView);
        }
        public void RefreshFriendMJParlor()
        {
            MainPage.SetActive(true);
            scrollViewMgr.RefreshScrollView(Model.priendDataList);
            if (Model.priendDataList.Count > 0)
            {
                labelNoData.SetActive(false);
            }
            else
            {
                labelNoData.SetActive(true);
            }
        }
        //麻将列表点击
        public void OnParlorItemClick(ParlorItemData data, GameObject go)
        {
            //打开信息
            LoadUIMain("UIParlorInfo", data.hall);
        }
        #endregion

        #region override
        public override void Init()
        {
            //this.ShowMask();
            //InitCityLabel();
            InitScrollView();
            UIEventListener.Get(ButtonGoBack).onClick = OnButtonGoBackClick;
            UIEventListener.Get(ButtonSearch).onClick = OnButtonSearchClick;
            //UIEventListener.Get(ButtonArea).onClick = OnButtonAreaClick;
            UIEventListener.Get(buttonService).onClick = OnButtonServiceClick;

            //popupList.OnPopupListSelect = OnPopupListSelect;
            EventDispatcher.AddEvent(GEnum.NamedEvent.SysData_MjHall_BindUpdate, RoomBindCallBack);
        }

        void RoomBindCallBack(object[] objs)
        {
            if(objs.Length >= 2)
            {
                bool isOk = (int)objs[1] == 0;
                if (isOk)
                    this.Close();
            }
        }

        void OnButtonServiceClick(GameObject go)
        {
            //LoadUIMain("UIConnectService");
            WebSDKParams param = new WebSDKParams("WEB_OPEN_CS_SERVICE");
            param.InsertUrlParams(MemoryData.UserID, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name);
            SDKManager.Instance.SDKFunction("WEB_OPEN_CS_SERVICE",param);
            //servicePanel.SetActive(true);
            //SDKManager.Instance.SDKFunction("SERVICE_INFO");
        }
        void InitCityLabel()
        {
            sortCity.cityData.CityTableData_LoadXml();
            sortCity.showLabel.text = "全国";
            //sortCity.InitItem();
        }
        public override void OnShow()
        {
            Model.cityId = 0;
            InitCityLabel();
            Model.OnSendSearch("");

            //RefreshFriendMJParlor();
            PanelSearchScript.onButtonGoBack = OnSearchGoBackClick;
        }
        private int cityId = 0;
        private string sortType = "";
        /// <summary>
        /// 根据选择的城市筛选麻将馆
        /// </summary>
        /// <param name="cityId"></param>
        //public void SortByCity(int cityId)
        //{
        //    this.cityId = cityId;
        //    //Model.SortByCitySort(cityId, sortType);
        //    List<ParlorItemData> datas = Model.SortByCitySort(cityId, sortType);
        //    if (cityId == 0)
        //    {
        //        labelNoData.SetActive(false);
        //    }
        //    else
        //    {
        //        if (datas == null || datas.Count == 0)
        //        {
        //            labelNoData.SetActive(true);
        //        }
        //        else
        //        {
        //            labelNoData.SetActive(false);
        //        }
        //    }
        //    scrollViewMgr.RefreshScrollView(datas);
        //}
        /// <summary>
        /// 根据排序类型对麻将馆排序
        /// </summary>
        /// <param name="sortType"></param>
        //public void SortByType(string sortType)
        //{
        //    this.sortType = sortType;
        //    List<ParlorItemData> datas = Model.SortByCitySort(cityId, sortType);
        //    if (cityId == 0)
        //    {
        //        labelNoData.SetActive(false);
        //    }
        //    else
        //    {
        //        if (datas == null || datas.Count == 0)
        //        {
        //            labelNoData.SetActive(true);
        //        }
        //        else
        //        {
        //            labelNoData.SetActive(false);
        //        }
        //    }

        //    scrollViewMgr.RefreshScrollView(datas);

        //}
        public override void OnHide()
        {
        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.SysData_MjHall_BindUpdate, RoomBindCallBack);
        }

        public override void GoBack()
        {
            if(MainPage.activeSelf)
            {
                this.OnButtonGoBackClick(null);
            }
            else
            {
                PanelSearchScript.OnButtonGoBackClick(null);
            }
        }
        #endregion

    }
}
