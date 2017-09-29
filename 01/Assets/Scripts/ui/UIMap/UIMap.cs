/**
* @Author lyb
* 地区选择
*
*/

using UnityEngine;
using System.Collections.Generic;

namespace projectQ
{
    public class UIMap : UIViewBase
    {
        private UIMapModel Model
        {
            get
            {
                return this._model as UIMapModel;
            }
        }

        public GameObject CloseBtn;
        public GameObject NoAreaObj;
        public List<UIMapList> MapAreaList;

        #region override --------------------------------------

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = OnButtonGoBackClick;
            UIEventListener.Get(NoAreaObj).onClick = OnNoAreaClick;
        }

        public override void OnHide() { }

        public override void OnShow()
        {
            Model.MapAreaListInit();

            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID <= 0)
            {
                // 没有进行过区域选择。屏蔽返回按钮
                CloseBtn.SetActive(false);
            }
            else
            {
                CloseBtn.SetActive(true);
            }
        }

        #endregion --------------------------------------------

        #region 点击按钮回调-----------------------------------

        /// <summary>
        /// 关闭按钮点击回调
        /// </summary>
        private void OnButtonGoBackClick(GameObject go)
        {
            this.Close();
        }

        /// <summary>
        /// 点击没有区域的地方提示回调
        /// </summary>
        private void OnNoAreaClick(GameObject go)
        {
            this.LoadPop(WindowUIType.SystemPopupWindow, "选择玩法", "当前区域未开放\n\n更多地区玩法即将开放", new string[] { "确定" }
            , (index) => { });
        }

        #endregion --------------------------------------------
    }
}
