/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public enum BgType
    {
        City,
        Sort
    }
    public class SortPanel : MonoBehaviour
    {
        public UIParlorList parlorList;
        public SortPanelCityData cityData;
        public GameObject bg;
        public GameObject item;
        public UILabel showLabel;
        public GameObject clickObj;
        public UIScrollView scrollView;
        public UIGrid grid;
        public BgType type;
        private int cityID;
        private List<string> sortList = new List<string>();
        private List<GameObject> goList = new List<GameObject>();
        private bool initOver = false;
        public UI.ScrollViewTool.ScrollViewWrapContent cityScrollView;
        UI.ScrollViewTool.ScrollViewMgr<SortItemData> scrollViewMgr = new UI.ScrollViewTool.ScrollViewMgr<SortItemData>();

        void InitScrollVIew()
        {
            scrollViewMgr.Init(cityScrollView);
        }
        void RefreshScrollView()
        {
            scrollViewMgr.RefreshScrollView(cityData.cityItemDataList);
        }
        private void SetDefaultSelect()
        {
            //ItemClick(0, sortList[0]);
            goList[0].GetComponent<SortItem>().ShowSelect();
        }
        public void InitItem()
        {
            switch (type)
            {
                case BgType.City:

                    break;
                case BgType.Sort:
                    for (int i = 0; i < sortList.Count; i++)
                    {
                        GameObject go = Instantiate(item) as GameObject;
                        go.transform.SetParent(grid.transform);
                        GameObjectHelper.NormalizationTransform(go.transform);
                        go.GetComponent<SortItem>().InitItem(i, sortList[i]);
                        go.GetComponent<SortItem>().dele_ItemClick = ItemClick;
                        goList.Add(go);
                    }
                    SetDefaultSelect();
                    break;
            }

        }

        private void Awake()
        {
            sortList.Add("人气排行");
            sortList.Add("热度排行");
            sortList.Add("推荐排行");
            UIEventListener.Get(clickObj).onClick = OnLabelClick;
        }

        private void Start()
        {
            InitItem();
            InitScrollVIew();
        }
        public bool canClick;
        private float clickTimer;
        private void Update()
        {
            if (canClick)
            {
                clickTimer += Time.deltaTime;
                if (clickTimer >= 3.5)
                {
                    canClick = false;
                    clickTimer = 0;
                    return;
                }
            }
        }
        void OnLabelClick(GameObject go)
        {
            if (bg.activeSelf)
            {
                switch (type)
                {
                    case BgType.City:
                        bg.SetActive(false);
                        cityScrollView.gameObject.SetActive(false);
                        break;
                    case BgType.Sort:
                        bg.SetActive(false);
                        break;
                    default:
                        break;
                }

            }
            else
            {
                switch (type)
                {
                    case BgType.City:
                        bg.SetActive(true);
                        cityScrollView.gameObject.SetActive(true);
                        break;
                    case BgType.Sort:
                        bg.SetActive(true);
                        break;
                    default:
                        break;
                }

                if (bg.activeSelf && type == BgType.City)
                {


                    //if (!initOver)
                    //{
                    if (cityID == 0|| !(cityID == MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID))
                    {

                        cityData.InitTypeAreaDict();
                        cityData.InitAreaType();
                        cityID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID;
                        cityData.XMLToData(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID);

                        cityData.cityItemDataList[0].IsSelect = true;
                        RefreshScrollView();
                    }
                }
            }
        }
        private int cityId;
        private string sortType;
        /// <summary>
        /// 城市筛选
        /// </summary>
        /// <param name="data"></param>
        /// <param name="go"></param>
        public void OnItemClick(SortItemData data, GameObject go)
        {

            if (bg != null && cityScrollView != null)
            {
                bg.SetActive(false);
                cityScrollView.gameObject.SetActive(false);
            }
            //if (parlorList.firstEnter || parlorList.cantClick)//|| canClick 
            //{
            //    return;
            //}
            //else
            //{
                this.cityId = data.cityId;
                showLabel.text = data.cityName;
                for (int i = 0; i < cityData.cityItemDataList.Count; i++)
                {
                    if (data.cityId == cityData.cityItemDataList[i].cityId)
                    {
                        canClick = true;
                        data.IsSelect = true;
                        parlorList.OnCityIdSet(data.cityId);
                        parlorList.OnSortClick();
                    }
                    else
                    {
                        cityData.cityItemDataList[i].IsSelect = false;
                    }
                //}

            }
            scrollViewMgr.RefreshData();

        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="index"></param>
        /// <param name="text"></param>
        void ItemClick(int index, string text)
        {
            bg.SetActive(false);

            if (parlorList.firstEnter || canClick || parlorList.cantClick)
            {
                return;
            }
            else
            {
                for (int i = 0; i < goList.Count; i++)
                {
                    if (index == i)
                    {
                        canClick = true;
                        goList[i].GetComponent<SortItem>().ShowSelect();
                        parlorList.OnPopupListSelect(text);
                        parlorList.OnSortClick();
                    }
                    else
                    {
                        goList[i].GetComponent<SortItem>().ShowNoSelect();
                    }
                }
                this.sortType = text;
                showLabel.text = text;
            }
        }
    }
}