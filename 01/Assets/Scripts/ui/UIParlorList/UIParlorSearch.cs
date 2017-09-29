/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIParlorSearch : MonoBehaviour
    {
        public GameObject ButtonGoBack;
        public System.Action onButtonGoBack;
        public GameObject clientServer;
        #region ScrollView
        public UI.ScrollViewTool.ScrollViewWrapContent SearchScrollView;
        UI.ScrollViewTool.ScrollViewMgr<ParlorItemData> scrollViewMgr = new UI.ScrollViewTool.ScrollViewMgr<ParlorItemData>();

        private void InitScrollView()
        {
            scrollViewMgr.Init(SearchScrollView);
        }
        public void RefreshFriendMJParlor(List<ParlorItemData> searchData)
        {
            gameObject.SetActive(true);
            scrollViewMgr.RefreshScrollView(searchData);
        }
        //麻将列表点击
        public void OnParlorItemClick(ParlorItemData data, GameObject go)
        {
            //打开信息
            _R.ui.OpenUI("UIParlorInfo", data.hall);
        }
        #endregion

        #region Event
        public void OnButtonGoBackClick(GameObject go)
        {
            this.onButtonGoBack();
            gameObject.SetActive(false);
        }
        #endregion

        private void Awake()
        {
            InitScrollView();
            UIEventListener.Get(ButtonGoBack).onClick = OnButtonGoBackClick;
            UIEventListener.Get(clientServer).onClick = OnServerClick;
        }
        void OnServerClick(GameObject go)
        {
            WebSDKParams param = new WebSDKParams("WEB_OPEN_CS_SERVICE");
            param.InsertUrlParams(MemoryData.UserID, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.Name);
            SDKManager.Instance.SDKFunction("WEB_OPEN_CS_SERVICE", param);
        }
    }
}