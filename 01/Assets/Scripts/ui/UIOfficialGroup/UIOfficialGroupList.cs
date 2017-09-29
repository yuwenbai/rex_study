/**
 * @Author lyb
 * 官方麻将试玩群
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIOfficialGroupList : MonoBehaviour
    {
        public UILabel GroupName;

        /// <summary>
        /// 官方推荐群数据保存
        /// </summary>
        private officialconfig officeInfo;
        /// <summary>
        /// url地址
        /// </summary>
        private string groupUrl;

        void Start() { }

        /// <summary>
        /// 初始化
        /// </summary>
        public void OfficialGroupListInit(officialconfig oInfo)
        {
            officeInfo = oInfo;
            GroupName.text = oInfo.PlayName;
            groupUrl = oInfo.UrlRes;
        }

        void OnClick()
        {
            _R.ui.OpenUI("UIOfficialBox", officeInfo);

            //WebSDKParams param = new WebSDKParams("WEB_OPEN_URL");
            //param.InsertUrlParams(groupUrl);
            //SDKManager.Instance.SDKFunction("WEB_OPEN_URL", param);
        }
    }
}
