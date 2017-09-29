/**
* @Author lyb
* 活动
*
*/

using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIActivityModel : UIModelBase
    {
        private UIActivity UI
        {
            get { return _ui as UIActivity; }
        }

        #region override--------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysData_Other_Activity_Rsp,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysData_Other_Activity_Rsp:
                    S2CActivityInit();
                    break;
            }
        }

        #endregion---------------------------------------------------

        #region 跟服务器请求消息 ------------------------------------

        public void C2SActivityDataSend(Msg.LotteryTypeDef type)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageAwardConfig, type);
        }

        #endregion --------------------------------------------------

        #region 初始化活动页签 --------------------------------------

        public void S2CActivityInit()
        {
            List<ActivityData> dataList = MemoryData.SysActivityData.GetActivityList();

            int index = 0;
            GameObject obj = null;

            UITools.CreateChild<ActivityData>(UI.GridLeft.transform, null, dataList, (go, aData) =>
            {
                if (index == 0)
                {
                    obj = go;
                }
                else
                {
                    go.GetComponent<UIDefinedButton>().SoundSelect = GEnum.SoundEnum.btn_select3;
                }

                UIActivityList activityList = go.GetComponent<UIActivityList>();

                activityList.ActivityListInit(aData, OnActivityClickCallBack);

                index++;
            });

            UI.GridLeft.Reposition();

            if (obj != null)
            {
                obj.GetComponent<UIButton>().gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);

                obj.GetComponent<UIDefinedButton>().SoundSelect = GEnum.SoundEnum.btn_select3;
            }
        }

        #endregion --------------------------------------------------

        #region Common ----------------------------------------------

        /// <summary>
        /// 活动页签按钮点击回调
        /// </summary>
        public void OnActivityClickCallBack(ActivityData aData)
        {
            DownHeadTexture.Instance.Activity_TextureGet(aData.ResUrl, TextureSetCallBack);
        }

        /// <summary>
        /// 获取活动图片资源回调
        /// </summary>
        void TextureSetCallBack(Texture2D tex2D, string headName)
        {
            UI.ActivityTex.mainTexture = tex2D;
        }

        #endregion --------------------------------------------------
    }
}