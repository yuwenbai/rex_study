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
    public class UIUserInfoRightCenterControll : MonoBehaviour
    {
        public UIUserInfo userInfo;
        public GameObject areaItem;
        public UIScrollView scrollView;
        public UIGrid grid;
        public RightCenterAreaData areaData;
        private List<GameObject> areaObjList = new List<GameObject>();

        public delegate void OnRightItemClick(int playId);
        public OnRightItemClick dele_OnRigth;
        public void InitAllItem()
        {
           
            for (int i = 0; i < MemoryData.MahjongPlayData.LocalRegionAndFashionPlayList.Count; i++)
            {
                GameObject go = Instantiate(areaItem) as GameObject;
                go.transform.SetParent(grid.transform);
                GameObjectHelper.NormalizationTransform(grid.transform);
                UIUserAreaItem item=  go.GetComponent<UIUserAreaItem>();
                item.InitAreaItem(MemoryData.MahjongPlayData.LocalRegionAndFashionPlayList[i].ConfigId, MemoryData.MahjongPlayData.LocalRegionAndFashionPlayList[i].Name);
                item.dele_ItemClick = OnItemClick;
                areaObjList.Add(go);
            }
            //for (int i = 0; i < areaData.GetAreaName().Count; i++)
            //{
            //    GameObject go = Instantiate(areaItem) as GameObject;
            //    go.transform.SetParent(grid.transform);
            //    go.transform.localPosition = Vector3.zero;
            //    go.transform.localScale = Vector3.one;
            //    go.transform.localEulerAngles = Vector3.zero;
            //    go.GetComponent<UIUserAreaItem>().InitAreaItem(areaData.GetAreaId()[i], areaData.GetAreaName()[i]);
            //    go.GetComponent<UIUserAreaItem>().dele_ItemClick = OnItemClick;
            //    areaObjList.Add(go);
            //}
            grid.Reposition();
            scrollView.ResetPosition();
            
        }
        /// <summary>
        /// 此处应根据playId刷新面板显示
        /// </summary>
        /// <param name="playId"></param>
       public void OnItemClick(int playId)
        {
            for (int i = 0; i < MemoryData.MahjongPlayData.LocalRegionAndFashionPlayList.Count; i++)
            {
                if (playId == MemoryData.MahjongPlayData.LocalRegionAndFashionPlayList[i].ConfigId)
                {
                    areaObjList[i].GetComponent<UIUserAreaItem>().ShowSelect();
                    //userInfo.RefreshUI();
                    if (dele_OnRigth != null)
                    {
                        dele_OnRigth(playId);
                    }
                }
                else
                {
                    areaObjList[i].GetComponent<UIUserAreaItem>().ShowNoSelect();
                }
                
            }
        }


        private void Awake()
        {
            //InitArea();
            //InitAllItem();
         
        }
    }
}
