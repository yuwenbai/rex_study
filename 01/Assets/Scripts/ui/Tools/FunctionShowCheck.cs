/**
 * @Author lyb
 * 根据数据表来判定当前功能按钮是否开启
 *
 */

using UnityEngine;

namespace projectQ
{
    public class FunctionShowCheck : MonoBehaviour
    {
        [Tooltip("是否自动运行监测")]
        public bool IsAutoRun = false;
        public GEnum.FunctionIconEnum FunctionId;
        public UIGrid ParentGrid;
        /// <summary>
        /// 提示小红点
        /// </summary>
        public GameObject PromptObj;

        void OnEnable()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.SysTools_FunctionShow, FunctionShowInit);
            EventDispatcher.AddEvent(GEnum.NamedEvent.SysTools_FunctionPromptRefresh, FunctionPromptCheck);

            FunctionPromptCheck(null);

            if (IsAutoRun)
            {
                FunctionShowInit(null);
            }
        }

        void OnDisable()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.SysTools_FunctionShow, FunctionShowInit);
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.SysTools_FunctionPromptRefresh, FunctionPromptCheck);
        }

        /// <summary>
        /// 初始化按钮是否隐藏显示
        /// </summary>
        void FunctionShowInit(object[] data)
        {
            bool isShow = IsFunctionShow();

            if (isShow == gameObject.activeInHierarchy)
            {
                return;
            }

            gameObject.SetActive(isShow);

            if (ParentGrid != null)
            {
                ParentGrid.Reposition();
            }
        }

        /// <summary>
        /// 获取当前功能按钮是否要开启
        /// </summary>
        private bool IsFunctionShow()
        {
            string shStr = "";

#if UNITY_ANDROID
            shStr = "IsShow";
#elif UNITY_IOS
            shStr = "IsIOSShow";
#endif

#if __BUNDLE_IOS_SERVER
            shStr = "IsIosJudgeShow";
#endif

            string showStr = MemoryData.XmlData.XmlBuildDataSole_Get("FunctionIcon", "FunctionID", ((int)FunctionId).ToString(), shStr);

            bool isShow = true;
            if (!string.IsNullOrEmpty(showStr))
            {
                isShow = showStr.Equals("0") ? true : false;
            }

            return isShow;
        }


        #region 功能按钮上的红点显示-------------------------

        public void FunctionPromptCheck(object[] data)
        {
            if (PromptObj == null)
            {
                return;
            }

            switch (FunctionId)
            {
                case GEnum.FunctionIconEnum.Function_2001:
                    //幸运转盘
                    if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes == 0)
                    {
                        //可以抽奖
                        PromptObj.SetActive(true);
                    }
                    else
                    {
                        //不可以抽奖
                        PromptObj.SetActive(false);
                    }
                    break;
            }
        }

        #endregion-------------------------------------------

    }
}